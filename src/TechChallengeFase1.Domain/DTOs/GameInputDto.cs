using System;

namespace TechChallengeFase1.Domain.DTOs;

public class GameInputDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int DeveloperId { get; set; }
    public int PublisherId { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal BasePrice { get; set; }
    public Guid CreatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}
