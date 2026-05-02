
namespace TechChallengeFase1.Application.DTOs;

public class LibraryGameOutputDto
{
    public int LibraryGameId { get; set; }
    public int LibraryId { get; set; }
    public int GameId { get; set; }
    public DateTime? AcquiredAt { get; set; }
}