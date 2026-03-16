using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechChallengeFase1.Domain.Entities;

[Table("LibraryGame")]
public class LibraryGame : SoftDelete
{
    [Key]
    public int LibraryGameId { get; set; }

    public int LibraryId { get; set; }

    public int GameId { get; set; }

    public int? AcquiredFromOrderId { get; set; }

    public DateTime? AcquiredAt { get; set; }

    public int PlaytimeMinutes { get; set; }

    public DateTime? LastPlayedAt { get; set; }

    public Library? Library { get; set; }

    public Game? Game { get; set; }

    public LibraryGame(int libraryId, int gameId, int? acquiredFromOrderId = null, DateTime? acquiredAt = null)
    {
        LibraryId = libraryId;
        GameId = gameId;
        AcquiredFromOrderId = acquiredFromOrderId;
        AcquiredAt = acquiredAt ?? DateTime.UtcNow;
    }

    public void Update(int libraryId, int gameId, int? acquiredFromOrderId, DateTime? acquiredAt, int playtimeMinutes, DateTime? lastPlayedAt)
    {
        LibraryId = libraryId;
        GameId = gameId;
        AcquiredFromOrderId = acquiredFromOrderId;
        AcquiredAt = acquiredAt;
        PlaytimeMinutes = playtimeMinutes;
        LastPlayedAt = lastPlayedAt;
    }
}
