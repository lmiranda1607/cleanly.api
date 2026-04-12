using Cleanly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleanly.Infrastructure.Persistence.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("ratings");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.RequestId).HasColumnName("request_id").IsRequired();
        builder.Property(x => x.CustomerId).HasColumnName("customer_id").IsRequired();
        builder.Property(x => x.CleanerId).HasColumnName("cleaner_id").IsRequired();
        builder.Property(x => x.Score).HasColumnName("score").IsRequired();
        builder.Property(x => x.Comment).HasColumnName("comment");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasIndex(x => x.RequestId).IsUnique();

        builder.HasOne(x => x.Request)
            .WithOne(x => x.Rating)
            .HasForeignKey<Rating>(x => x.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Cleaner)
            .WithMany()
            .HasForeignKey(x => x.CleanerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
