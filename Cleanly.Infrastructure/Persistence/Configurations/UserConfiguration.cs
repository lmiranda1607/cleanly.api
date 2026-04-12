using Cleanly.Domain.Entities;
using Cleanly.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleanly.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(120).IsRequired();
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(180).IsRequired();
        builder.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(20).IsRequired();
        builder.Property(x => x.Role)
            .HasColumnName("role")
            .HasMaxLength(20)
            .HasConversion(
                x => x switch
                {
                    UserRole.Customer => "customer",
                    UserRole.Cleaner => "cleaner",
                    UserRole.Admin => "admin",
                    _ => throw new InvalidOperationException($"Unknown role: {x}")
                },
                x => x switch
                {
                    "customer" => UserRole.Customer,
                    "cleaner" => UserRole.Cleaner,
                    "admin" => UserRole.Admin,
                    _ => throw new InvalidOperationException($"Unknown role: {x}")
                })
            .IsRequired();

        builder.Property(x => x.ReputationScore).HasColumnName("reputation_score").HasPrecision(4, 2).IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Phone).IsUnique();

        builder.HasOne(x => x.CustomerProfile)
            .WithOne(x => x.User)
            .HasForeignKey<CustomerProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CleanerProfile)
            .WithOne(x => x.User)
            .HasForeignKey<CleanerProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata.FindNavigation(nameof(User.Addresses))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
