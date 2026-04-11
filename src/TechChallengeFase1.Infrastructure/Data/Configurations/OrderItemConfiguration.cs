using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItem");

        builder.HasKey(oi => oi.OrderItemId);

        builder.Property(oi => oi.Price)
            .HasColumnType("decimal(10,2)");

        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oi => oi.Game)
            .WithMany(g => g.OrderItems)
            .HasForeignKey(oi => oi.GameId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
