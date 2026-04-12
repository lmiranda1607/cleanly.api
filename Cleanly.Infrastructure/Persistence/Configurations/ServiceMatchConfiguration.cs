using Cleanly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleanly.Infrastructure.Persistence.Configurations;

public class ServiceMatchConfiguration : IEntityTypeConfiguration<ServiceMatch>
{
    public void Configure(EntityTypeBuilder<ServiceMatch> builder)
    {
        builder.ToTable("service_matches");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.RequestId).HasColumnName("request_id").IsRequired();
        builder.Property(x => x.CleanerId).HasColumnName("cleaner_id").IsRequired();
        builder.Property(x => x.AcceptedAt).HasColumnName("accepted_at").IsRequired();
        builder.Property(x => x.StartedAt).HasColumnName("started_at");
        builder.Property(x => x.FinishedAt).HasColumnName("finished_at");

        builder.HasIndex(x => x.RequestId).IsUnique();

        builder.HasOne(x => x.Request)
            .WithOne(x => x.Match)
            .HasForeignKey<ServiceMatch>(x => x.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Cleaner)
            .WithMany()
            .HasForeignKey(x => x.CleanerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
