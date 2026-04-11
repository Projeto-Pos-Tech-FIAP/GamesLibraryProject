using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Infrastructure.Data.Configurations;

public class LibraryGameConfiguration : IEntityTypeConfiguration<LibraryGame>
{
    public void Configure(EntityTypeBuilder<LibraryGame> builder)
    {
        builder.ToTable("LibraryGame");

        builder.HasKey(lg => lg.LibraryGameId);

        builder.HasOne(lg => lg.Library)
            .WithMany(l => l.LibraryGames)
            .HasForeignKey(lg => lg.LibraryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lg => lg.Game)
            .WithMany(g => g.LibraryGames)
            .HasForeignKey(lg => lg.GameId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
