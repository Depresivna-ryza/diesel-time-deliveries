using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Delivery.Domain.Models;

namespace Delivery.Infrastructure.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName(nameof(PlanId))
            .ValueGeneratedNever()
            .HasConversion(
                planId => planId.Value,
                plan => PlanId.Create(plan)
            );
    }
}