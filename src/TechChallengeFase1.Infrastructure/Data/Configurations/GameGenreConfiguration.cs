using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Infrastructure.Data.Configurations;

public class GameGenreConfiguration : IEntityTypeConfiguration<GameGenre>
{
    public void Configure(EntityTypeBuilder<GameGenre> builder)
    {
        builder.ToTable("GameGenre");

        builder.HasKey(gg => gg.GameGenreId);

        builder.HasOne(gg => gg.Game)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(gg => gg.Genre)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GenreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
