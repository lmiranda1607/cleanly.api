using Cleanly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleanly.Infrastructure.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.Label).HasColumnName("label").HasMaxLength(60).IsRequired();
        builder.Property(x => x.Street).HasColumnName("street").HasMaxLength(180).IsRequired();
        builder.Property(x => x.Number).HasColumnName("number").HasMaxLength(20);
        builder.Property(x => x.Neighborhood).HasColumnName("neighborhood").HasMaxLength(100);
        builder.Property(x => x.City).HasColumnName("city").HasMaxLength(100).IsRequired();
        builder.Property(x => x.State).HasColumnName("state").HasMaxLength(80).IsRequired();
        builder.Property(x => x.ZipCode).HasColumnName("zip_code").HasMaxLength(20);
        builder.Property(x => x.Latitude).HasColumnName("latitude").HasPrecision(10, 7);
        builder.Property(x => x.Longitude).HasColumnName("longitude").HasPrecision(10, 7);
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
