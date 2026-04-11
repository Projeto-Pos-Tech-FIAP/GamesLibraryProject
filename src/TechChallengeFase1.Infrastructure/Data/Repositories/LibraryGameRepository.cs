using Microsoft.EntityFrameworkCore;
using TechChallengeFase1.Domain.Entities;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Infrastructure.Data.Context;

namespace TechChallengeFase1.Infrastructure.Data.Repositories;

public class LibraryGameRepository : ILibraryGameRepository
{
    private readonly MyDbContext _context;

    public LibraryGameRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(int libraryId, int gameId)
    {
        return await _context.LibraryGames.AnyAsync(lg => lg.LibraryId == libraryId && lg.GameId == gameId);
    }

    public async Task<LibraryGame> AddAsync(LibraryGame libraryGame)
    {
        await _context.LibraryGames.AddAsync(libraryGame);
        await _context.SaveChangesAsync();
        return libraryGame;
    }
}
