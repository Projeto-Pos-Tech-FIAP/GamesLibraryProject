using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Entities;
using TechChallengeFase1.Domain.Interfaces;

namespace TechChallengeFase1.Application.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<Game> CreateAsync(GameInputDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("O título do jogo é obrigatório.", nameof(dto.Title));

        if (dto.BasePrice < 0)
            throw new ArgumentException("O preço base não pode ser negativo.", nameof(dto.BasePrice));

        var game = new Game(
            dto.Title,
            dto.DeveloperId,
            dto.PublisherId,
            dto.ReleaseDate,
            dto.BasePrice,
            dto.CreatedBy,
            dto.Description,
            dto.IsActive);

        return await _gameRepository.AddAsync(game);
    }
}
