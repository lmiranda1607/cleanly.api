using Cleanly.Application.Common.Interfaces;
using Cleanly.Application.Common.Interfaces.Services;
using Cleanly.Application.Common.Models;
using Cleanly.Application.DTOs.Cleaners;
using Cleanly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cleanly.Application.Services;

public sealed class CleanerService(IApplicationDbContext dbContext) : ICleanerService
{
    public async Task<Result<CleanerStatusDto>> SetOnlineAsync(long cleanerId, CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.CleanerProfiles.FirstOrDefaultAsync(x => x.UserId == cleanerId, cancellationToken);
        if (profile is null)
        {
            return Result<CleanerStatusDto>.Fail("Cleaner profile not found.");
        }

        profile.Activate();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<CleanerStatusDto>.Ok(new CleanerStatusDto(cleanerId, profile.Active));
    }

    public async Task<Result<CleanerStatusDto>> SetOfflineAsync(long cleanerId, CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.CleanerProfiles.FirstOrDefaultAsync(x => x.UserId == cleanerId, cancellationToken);
        if (profile is null)
        {
            return Result<CleanerStatusDto>.Fail("Cleaner profile not found.");
        }

        profile.Deactivate();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<CleanerStatusDto>.Ok(new CleanerStatusDto(cleanerId, profile.Active));
    }

    public async Task<Result<CleanerLocationDto>> UpdateLocationAsync(long cleanerId, UpdateCleanerLocationDto dto, CancellationToken cancellationToken = default)
    {
        var profileExists = await dbContext.CleanerProfiles.AnyAsync(x => x.UserId == cleanerId, cancellationToken);
        if (!profileExists)
        {
            return Result<CleanerLocationDto>.Fail("Cleaner profile not found.");
        }

        var location = await dbContext.CleanerLocations.FirstOrDefaultAsync(x => x.CleanerId == cleanerId, cancellationToken);
        if (location is null)
        {
            location = new CleanerLocation(cleanerId, dto.Latitude, dto.Longitude);
            dbContext.CleanerLocations.Add(location);
        }
        else
        {
            location.Update(dto.Latitude, dto.Longitude);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<CleanerLocationDto>.Ok(new CleanerLocationDto(cleanerId, location.Latitude, location.Longitude, location.UpdatedAt));
    }
}
