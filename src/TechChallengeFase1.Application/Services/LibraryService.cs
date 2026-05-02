using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Entities;
using TechChallengeFase1.Domain.Exceptions;
using TechChallengeFase1.Domain.Interfaces;

namespace TechChallengeFase1.Application.Services;

public class LibraryService : ILibraryService
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILibraryGameRepository _libraryGameRepository;
    private readonly IGameRepository _gameRepository;

    public LibraryService(
        ILibraryRepository libraryRepository,
        ILibraryGameRepository libraryGameRepository,
        IGameRepository gameRepository)
    {
        _libraryRepository = libraryRepository;
        _libraryGameRepository = libraryGameRepository;
        _gameRepository = gameRepository;
    }

    public async Task<Library?> GetByUserGuidAsync(Guid userGuid)
    {
        return await _libraryRepository.GetByUserGuidAsync(userGuid);
    }

    public async Task<LibraryGameOutputDto> AcquireGameAsync(LibraryGameInputDto dto)
    {
      var library = await _libraryRepository.GetByIdAsync(dto.LibraryId)
          ?? throw new NotFoundException($"Biblioteca {dto.LibraryId} não encontrada.");

      var game = await _gameRepository.GetByIdAsync(dto.GameId)
          ?? throw new NotFoundException($"Jogo {dto.GameId} não encontrado.");

      var alreadyOwned = await _libraryGameRepository.ExistsAsync(dto.LibraryId, dto.GameId);
      if (alreadyOwned)
          throw new InvalidOperationException($"O jogo '{game.Title}' já está na biblioteca.");

      var libraryGame = new LibraryGame(dto.LibraryId, dto.GameId, dto.AcquiredFromOrderId, dto.AcquiredAt);

      var result = await _libraryGameRepository.AddAsync(libraryGame);

      return new LibraryGameOutputDto
      {
        LibraryGameId = result.LibraryGameId,
        LibraryId = result.LibraryId,
        GameId = result.GameId,
        AcquiredAt = result.AcquiredAt
      };
  }
}
