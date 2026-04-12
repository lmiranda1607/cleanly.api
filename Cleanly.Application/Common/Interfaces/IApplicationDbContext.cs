using Cleanly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cleanly.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<CustomerProfile> CustomerProfiles { get; }
    DbSet<CleanerProfile> CleanerProfiles { get; }
    DbSet<Address> Addresses { get; }
    DbSet<ServiceRequest> ServiceRequests { get; }
    DbSet<ServiceMatch> ServiceMatches { get; }
    DbSet<CleanerLocation> CleanerLocations { get; }
    DbSet<Rating> Ratings { get; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
