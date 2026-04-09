using Microsoft.EntityFrameworkCore;
using TechChallengeFase1.Domain.Entities;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Infrastructure.Data.Context;

namespace TechChallengeFase1.Infrastructure.Data.Repositories;

public class GameRepository : IGameRepository
{
    private readonly MyDbContext _context;

    public GameRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Game> AddAsync(Game game)
    {
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();
        return game;
    }

    public async Task<Game?> GetByIdAsync(int gameId)
    {
        return await _context.Games
            .Include(g => g.GameGenres)
            .Include(g => g.LibraryGames)
            .FirstOrDefaultAsync(g => g.GameId == gameId);
    }

    public async Task<List<Game>> GetAllAsync()
    {
        return await _context.Games
            .Include(g => g.GameGenres)
            .Include(g => g.LibraryGames)
            .AsNoTracking()
            .ToListAsync();
    }
}
