using Cleanly.Application.Common.Interfaces;
using Cleanly.Application.Common.Interfaces.Services;
using Cleanly.Application.Common.Models;
using Cleanly.Application.Common.Security;
using Cleanly.Application.DTOs.Auth;
using Cleanly.Domain.Entities;
using Cleanly.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cleanly.Application.Services;

public sealed class AuthService(IApplicationDbContext dbContext, IPasswordHasher passwordHasher) : IAuthService
{
    public async Task<Result<AuthUserDto>> RegisterCustomerAsync(RegisterCustomerDto dto, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.Users.AnyAsync(x => x.Email == dto.Email || x.Phone == dto.Phone, cancellationToken);
        if (exists)
        {
            return Result<AuthUserDto>.Fail("Email or phone already exists.");
        }

        var user = new User(dto.FullName.Trim(), dto.Email.Trim().ToLowerInvariant(), dto.Phone.Trim(), UserRole.Customer, passwordHasher.Hash(dto.Password));

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var customerProfile = new CustomerProfile(user.Id, dto.PreferredPaymentMethod);
        dbContext.CustomerProfiles.Add(customerProfile);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<AuthUserDto>.Ok(Map(user));
    }

    public async Task<Result<AuthUserDto>> RegisterCleanerAsync(RegisterCleanerDto dto, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.Users.AnyAsync(x => x.Email == dto.Email || x.Phone == dto.Phone, cancellationToken);
        if (exists)
        {
            return Result<AuthUserDto>.Fail("Email or phone already exists.");
        }

        var user = new User(dto.FullName.Trim(), dto.Email.Trim().ToLowerInvariant(), dto.Phone.Trim(), UserRole.Cleaner, passwordHasher.Hash(dto.Password));
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var cleanerProfile = new CleanerProfile(user.Id, dto.Bio, dto.ServiceRadiusKm <= 0 ? 10 : dto.ServiceRadiusKm);
        dbContext.CleanerProfiles.Add(cleanerProfile);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<AuthUserDto>.Ok(Map(user));
    }

    public async Task<Result<AuthUserDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (user is null || !passwordHasher.Verify(dto.Password, user.PasswordHash))
        {
            return Result<AuthUserDto>.Fail("Invalid credentials.");
        }

        return Result<AuthUserDto>.Ok(Map(user));
    }

    public async Task<AuthUserDto?> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        return user is null ? null : Map(user);
    }

    private static AuthUserDto Map(User user) => new(user.Id, user.FullName, user.Email, user.Phone, user.Role);
}
