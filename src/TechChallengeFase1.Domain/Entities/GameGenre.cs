using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechChallengeFase1.Domain.Entities;

[Table("GameGenre")]
public class GameGenre : SoftDelete
{
    [Key]
    public int GameGenreId { get; set; }

    public int GenreId { get; set; }

    public int GameId { get; set; }

    public Genre? Genre { get; set; }

    public Game? Game { get; set; }

    public GameGenre(int genreId, int gameId)
    {
        GenreId = genreId;
        GameId = gameId;
    }

    public void Update(int genreId, int gameId)
    {
        GenreId = genreId;
        GameId = gameId;
    }
}
