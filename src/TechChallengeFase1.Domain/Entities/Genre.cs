using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechChallengeFase1.Domain.Entities;

[Table("Genre")]
public class Genre : SoftDelete
{
    [Key]
    public int GenreId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Description { get; set; } = null!;

    public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

    public Genre(string description)
    {
        Description = description;
    }

    public void Update(string description)
    {
        Description = description;
    }
}
