using Cleanly.Application.Common.Interfaces;
using Cleanly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cleanly.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();
    public DbSet<CleanerProfile> CleanerProfiles => Set<CleanerProfile>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
    public DbSet<ServiceMatch> ServiceMatches => Set<ServiceMatch>();
    public DbSet<CleanerLocation> CleanerLocations => Set<CleanerLocation>();
    public DbSet<Rating> Ratings => Set<Rating>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
