using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Infrastructure.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Game");

        builder.HasKey(g => g.GameId);

        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Description)
            .HasColumnType("text");

        builder.Property(g => g.BasePrice)
            .HasColumnType("decimal(10,2)");

        builder.Property(g => g.CreatedAt)
            .IsRequired();

        builder.Property(g => g.CreatedBy)
            .IsRequired();
    }
}
