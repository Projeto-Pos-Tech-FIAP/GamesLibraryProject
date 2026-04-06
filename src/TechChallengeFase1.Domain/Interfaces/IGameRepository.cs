using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Domain.Interfaces;

public interface IGameRepository
{
    Task<Game?> GetByIdAsync(int gameId);
    Task<Game> AddAsync(Game game);
}
