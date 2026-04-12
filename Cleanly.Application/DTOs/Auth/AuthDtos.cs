using Cleanly.Domain.Enums;

namespace Cleanly.Application.DTOs.Auth;

public sealed record RegisterCustomerDto(string FullName, string Email, string Phone, string Password, string? PreferredPaymentMethod);

public sealed record RegisterCleanerDto(string FullName, string Email, string Phone, string Password, string? Bio, int ServiceRadiusKm);

public sealed record LoginDto(string Email, string Password);

public sealed record AuthUserDto(long Id, string FullName, string Email, string Phone, UserRole Role);
