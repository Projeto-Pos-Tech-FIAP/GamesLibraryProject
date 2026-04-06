using AutoMapper;
using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Entities;
using TechChallengeFase1.Domain.Interfaces;

namespace TechChallengeFase1.Application.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IMapper _mapper;

    public GameService(IGameRepository gameRepository, IMapper mapper)
    {
        _gameRepository = gameRepository;
        _mapper = mapper;
    }

    public async Task<GameOutputDto> CreateAsync(GameInputDto dto)
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

        var savedGame = await _gameRepository.AddAsync(game);
        return _mapper.Map<GameOutputDto>(savedGame);
    }
}
