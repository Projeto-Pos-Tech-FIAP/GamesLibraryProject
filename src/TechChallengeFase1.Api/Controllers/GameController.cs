using Microsoft.AspNetCore.Mvc;
using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Application.DTOs.Shared;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.DTOs;

namespace TechChallengeFase1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Adiciona um novo jogo ao sistema.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(GameOutputDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionOutputDto), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GameOutputDto>> CreateAsync([FromBody] GameInputDto dto)
    {
        var game = await _gameService.CreateAsync(dto);

        var output = new GameOutputDto
        {
            GameId = game.GameId,
            Title = game.Title,
            Description = game.Description,
            DeveloperId = game.DeveloperId,
            PublisherId = game.PublisherId,
            ReleaseDate = game.ReleaseDate,
            BasePrice = game.BasePrice,
            IsActive = game.IsActive,
            CreatedAt = game.CreatedAt,
            CreatedBy = game.CreatedBy
        };

        return CreatedAtAction(nameof(CreateAsync), new { id = output.GameId }, output);
    }
}
