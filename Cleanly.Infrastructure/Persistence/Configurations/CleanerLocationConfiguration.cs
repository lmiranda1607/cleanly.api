using Cleanly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleanly.Infrastructure.Persistence.Configurations;

public class CleanerLocationConfiguration : IEntityTypeConfiguration<CleanerLocation>
{
    public void Configure(EntityTypeBuilder<CleanerLocation> builder)
    {
        builder.ToTable("cleaner_locations");

        builder.HasKey(x => x.CleanerId);
        builder.Property(x => x.CleanerId).HasColumnName("cleaner_id");
        builder.Property(x => x.Latitude).HasColumnName("latitude").HasPrecision(10, 7).IsRequired();
        builder.Property(x => x.Longitude).HasColumnName("longitude").HasPrecision(10, 7).IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
    }
}
