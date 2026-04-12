using Cleanly.Application.Common.Interfaces;
using Cleanly.Application.Common.Interfaces.Services;
using Cleanly.Application.Common.Models;
using Cleanly.Application.DTOs.Requests;
using Cleanly.Domain.Entities;
using Cleanly.Domain.Enums;
using Cleanly.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Cleanly.Application.Services;

public sealed class RequestService(IApplicationDbContext dbContext) : IRequestService
{
    public async Task<Result<ServiceRequestDto>> CreateAsync(long customerId, CreateServiceRequestDto dto, CancellationToken cancellationToken = default)
    {
        var address = new Address(
            customerId,
            dto.Address.Label,
            dto.Address.Street,
            dto.Address.Number,
            dto.Address.Neighborhood,
            dto.Address.City,
            dto.Address.State,
            dto.Address.ZipCode,
            dto.Address.Latitude,
            dto.Address.Longitude);

        dbContext.Addresses.Add(address);
        await dbContext.SaveChangesAsync(cancellationToken);

        var request = new ServiceRequest(customerId, address.Id, dto.RequestedDate, dto.DurationHours, dto.Notes);
        request.MoveTo(ServiceRequestStatus.SearchingCleaner);

        dbContext.ServiceRequests.Add(request);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<ServiceRequestDto>.Ok(Map(request, null));
    }

    public async Task<ServiceRequestDto?> GetByIdAsync(long requestId, long userId, CancellationToken cancellationToken = default)
    {
        var request = await dbContext.ServiceRequests
            .AsNoTracking()
            .Include(x => x.Match)
            .FirstOrDefaultAsync(x => x.Id == requestId, cancellationToken);

        if (request is null)
        {
            return null;
        }

        var isCustomer = request.CustomerId == userId;
        var isAssignedCleaner = request.Match?.CleanerId == userId;
        if (!isCustomer && !isAssignedCleaner)
        {
            return null;
        }

        return Map(request, request.Match);
    }

    public async Task<IReadOnlyCollection<ServiceRequestDto>> GetMyAsync(long customerId, CancellationToken cancellationToken = default)
    {
        var requests = await dbContext.ServiceRequests
            .AsNoTracking()
            .Include(x => x.Match)
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return requests.Select(x => Map(x, x.Match)).ToArray();
    }

    public async Task<Result<ServiceRequestDto>> CancelAsync(long requestId, long customerId, CancellationToken cancellationToken = default)
    {
        var request = await dbContext.ServiceRequests.Include(x => x.Match)
            .FirstOrDefaultAsync(x => x.Id == requestId && x.CustomerId == customerId, cancellationToken);
        if (request is null)
        {
            return Result<ServiceRequestDto>.Fail("Request not found.");
        }

        try
        {
            request.MoveTo(ServiceRequestStatus.Cancelled);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<ServiceRequestDto>.Ok(Map(request, request.Match));
        }
        catch (InvalidServiceRequestTransitionException ex)
        {
            return Result<ServiceRequestDto>.Fail(ex.Message);
        }
    }

    public async Task<Result<ServiceRequestDto>> AcceptAsync(long requestId, long cleanerId, CancellationToken cancellationToken = default)
    {
        var request = await dbContext.ServiceRequests.Include(x => x.Match)
            .FirstOrDefaultAsync(x => x.Id == requestId, cancellationToken);
        if (request is null)
        {
            return Result<ServiceRequestDto>.Fail("Request not found.");
        }

        if (request.Match is not null)
        {
            return Result<ServiceRequestDto>.Fail("Request already matched.");
        }

        try
        {
            request.MoveTo(ServiceRequestStatus.Matched);
            request.MoveTo(ServiceRequestStatus.CleanerOnTheWay);
        }
        catch (InvalidServiceRequestTransitionException ex)
        {
            return Result<ServiceRequestDto>.Fail(ex.Message);
        }

        var match = new ServiceMatch(request.Id, cleanerId);
        dbContext.ServiceMatches.Add(match);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<ServiceRequestDto>.Ok(Map(request, match));
    }

    public async Task<Result<ServiceRequestDto>> ArriveAsync(long requestId, long cleanerId, CancellationToken cancellationToken = default)
        => await MoveForCleanerAsync(requestId, cleanerId, ServiceRequestStatus.Arrived, markStarted: false, markFinished: false, cancellationToken);

    public async Task<Result<ServiceRequestDto>> StartAsync(long requestId, long cleanerId, CancellationToken cancellationToken = default)
        => await MoveForCleanerAsync(requestId, cleanerId, ServiceRequestStatus.InProgress, markStarted: true, markFinished: false, cancellationToken);

    public async Task<Result<ServiceRequestDto>> CompleteAsync(long requestId, long cleanerId, CancellationToken cancellationToken = default)
        => await MoveForCleanerAsync(requestId, cleanerId, ServiceRequestStatus.Completed, markStarted: false, markFinished: true, cancellationToken);

    public async Task<IReadOnlyCollection<ServiceRequestDto>> GetAvailableForCleanerAsync(CancellationToken cancellationToken = default)
    {
        var requests = await dbContext.ServiceRequests
            .AsNoTracking()
            .Where(x => x.Status == ServiceRequestStatus.SearchingCleaner)
            .OrderBy(x => x.RequestedDate)
            .Take(100)
            .ToListAsync(cancellationToken);

        return requests.Select(x => Map(x, null)).ToArray();
    }

    private async Task<Result<ServiceRequestDto>> MoveForCleanerAsync(
        long requestId,
        long cleanerId,
        ServiceRequestStatus target,
        bool markStarted,
        bool markFinished,
        CancellationToken cancellationToken)
    {
        var request = await dbContext.ServiceRequests.Include(x => x.Match)
            .FirstOrDefaultAsync(x => x.Id == requestId, cancellationToken);

        if (request?.Match is null || request.Match.CleanerId != cleanerId)
        {
            return Result<ServiceRequestDto>.Fail("Request not found for cleaner.");
        }

        try
        {
            request.MoveTo(target);
            if (markStarted)
            {
                request.Match.MarkStarted();
            }

            if (markFinished)
            {
                request.Match.MarkFinished();
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<ServiceRequestDto>.Ok(Map(request, request.Match));
        }
        catch (InvalidServiceRequestTransitionException ex)
        {
            return Result<ServiceRequestDto>.Fail(ex.Message);
        }
    }

    private static ServiceRequestDto Map(ServiceRequest request, ServiceMatch? match)
        => new(
            request.Id,
            request.CustomerId,
            request.AddressId,
            request.RequestedDate,
            request.DurationHours,
            request.Notes,
            request.Status,
            request.CreatedAt,
            request.UpdatedAt,
            match?.CleanerId,
            match?.AcceptedAt,
            match?.StartedAt,
            match?.FinishedAt);
}
