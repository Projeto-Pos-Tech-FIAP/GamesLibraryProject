using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechChallengeFase1.Domain.Entities;

[Table("Game")]
public class Game : SoftDelete
{
    [Key]
    public int GameId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int DeveloperId { get; set; }

    public int PublisherId { get; set; }

    public DateTime ReleaseDate { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal BasePrice { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

    public ICollection<LibraryGame> LibraryGames { get; set; } = new List<LibraryGame>();

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public Game(string title, int developerId, int publisherId, DateTime releaseDate, decimal basePrice, Guid createdBy, string? description = null, bool isActive = true)
    {
        Title = title;
        Description = description;
        DeveloperId = developerId;
        PublisherId = publisherId;
        ReleaseDate = releaseDate;
        BasePrice = basePrice;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
        IsActive = isActive;
    }

    public void Update(string title, int developerId, int publisherId, DateTime releaseDate, decimal basePrice, Guid updatedBy, string? description = null, bool isActive = true)
    {
        Title = title;
        Description = description;
        DeveloperId = developerId;
        PublisherId = publisherId;
        ReleaseDate = releaseDate;
        BasePrice = basePrice;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
