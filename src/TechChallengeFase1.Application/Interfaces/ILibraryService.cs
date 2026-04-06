using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Application.Interfaces;

public interface ILibraryService
{
    Task<Library?> GetByUserGuidAsync(Guid userGuid);
    Task<LibraryGame> AcquireGameAsync(LibraryGameInputDto dto);
}
