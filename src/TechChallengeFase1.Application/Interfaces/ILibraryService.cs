using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace TechChallengeFase1.Application.Interfaces;

public interface ILibraryService
{
    Task<Library?> GetByUserGuidAsync(Guid userGuid);
    Task<LibraryGameOutputDto> AcquireGameAsync(LibraryGameInputDto dto);
}
