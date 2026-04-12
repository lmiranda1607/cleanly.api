using Cleanly.Domain.Entities;
using Cleanly.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cleanly.Infrastructure.Persistence.Configurations;

public class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.ToTable("service_requests");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.CustomerId).HasColumnName("customer_id").IsRequired();
        builder.Property(x => x.AddressId).HasColumnName("address_id").IsRequired();
        builder.Property(x => x.RequestedDate).HasColumnName("requested_date").IsRequired();
        builder.Property(x => x.DurationHours).HasColumnName("duration_hours").IsRequired();
        builder.Property(x => x.Notes).HasColumnName("notes");
        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(30)
            .HasConversion(
                x => x switch
                {
                    ServiceRequestStatus.Created => "created",
                    ServiceRequestStatus.SearchingCleaner => "searching_cleaner",
                    ServiceRequestStatus.Matched => "matched",
                    ServiceRequestStatus.CleanerOnTheWay => "cleaner_on_the_way",
                    ServiceRequestStatus.Arrived => "arrived",
                    ServiceRequestStatus.InProgress => "in_progress",
                    ServiceRequestStatus.Completed => "completed",
                    ServiceRequestStatus.Cancelled => "cancelled",
                    _ => throw new InvalidOperationException($"Unknown status: {x}")
                },
                x => x switch
                {
                    "created" => ServiceRequestStatus.Created,
                    "searching_cleaner" => ServiceRequestStatus.SearchingCleaner,
                    "matched" => ServiceRequestStatus.Matched,
                    "cleaner_on_the_way" => ServiceRequestStatus.CleanerOnTheWay,
                    "arrived" => ServiceRequestStatus.Arrived,
                    "in_progress" => ServiceRequestStatus.InProgress,
                    "completed" => ServiceRequestStatus.Completed,
                    "cancelled" => ServiceRequestStatus.Cancelled,
                    _ => throw new InvalidOperationException($"Unknown status: {x}")
                })
            .IsRequired();

        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
