using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Domain.Interfaces;

public interface ILibraryRepository
{
    Task<Library?> GetByUserGuidAsync(Guid userGuid);
    Task<Library?> GetByIdAsync(int libraryId);
}
