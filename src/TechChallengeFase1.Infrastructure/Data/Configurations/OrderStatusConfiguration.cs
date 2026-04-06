using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Infrastructure.Data.Configurations;

public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("OrderStatus");

        builder.HasKey(os => os.OrderStatusId);

        builder.Property(os => os.Description)
            .IsRequired()
            .HasMaxLength(50);
    }
}
