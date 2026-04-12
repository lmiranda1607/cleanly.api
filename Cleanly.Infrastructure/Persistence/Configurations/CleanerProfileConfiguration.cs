using Cleanly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleanly.Infrastructure.Persistence.Configurations;

public class CleanerProfileConfiguration : IEntityTypeConfiguration<CleanerProfile>
{
    public void Configure(EntityTypeBuilder<CleanerProfile> builder)
    {
        builder.ToTable("cleaner_profiles");

        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.Bio).HasColumnName("bio");
        builder.Property(x => x.Active).HasColumnName("active").IsRequired();
        builder.Property(x => x.ServiceRadiusKm).HasColumnName("service_radius_km").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasOne(x => x.CurrentLocation)
            .WithOne(x => x.CleanerProfile)
            .HasForeignKey<CleanerLocation>(x => x.CleanerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
