using Microsoft.EntityFrameworkCore;
using TechChallengeFase1.Domain.Entities;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Infrastructure.Data.Context;

namespace TechChallengeFase1.Infrastructure.Data.Repositories;

public class LibraryRepository : ILibraryRepository
{
    private readonly MyDbContext _context;

    public LibraryRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Library?> GetByIdAsync(int libraryId)
    {
        return await _context.Libraries
            .Include(l => l.LibraryGames)
            .FirstOrDefaultAsync(l => l.LibraryId == libraryId);
    }

    public async Task<Library?> GetByUserGuidAsync(Guid userGuid)
    {
        return await _context.Libraries
            .Include(l => l.LibraryGames)
            .FirstOrDefaultAsync(l => l.UserGuid == userGuid);
    }
}
