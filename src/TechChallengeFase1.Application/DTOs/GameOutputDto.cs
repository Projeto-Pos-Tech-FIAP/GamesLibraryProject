using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Application.DTOs;

public class GameOutputDto
{
    public int GameId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int DeveloperId { get; set; }
    public int PublisherId { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }

    public GameGenre Genre { get; set; } = null!;
}
