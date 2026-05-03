using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Application.DTOs.Shared;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.DTOs;

namespace TechChallengeFase1.Api.Controllers;

[Authorize]
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
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(GameOutputDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionOutputDto), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GameOutputDto>> CreateAsync([FromBody] GameInputDto dto)
    {
        var output = await _gameService.CreateAsync(dto);

        return Created($"api/Game/{output.GameId}", output);
    }
}
