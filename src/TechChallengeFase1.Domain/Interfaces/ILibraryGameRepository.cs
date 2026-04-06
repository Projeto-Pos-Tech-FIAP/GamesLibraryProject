using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Domain.Interfaces;

public interface ILibraryGameRepository
{
    Task<bool> ExistsAsync(int libraryId, int gameId);
    Task<LibraryGame> AddAsync(LibraryGame libraryGame);
}
