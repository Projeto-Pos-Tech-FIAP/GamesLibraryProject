using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.DTOs;

namespace TechChallengeFase1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class LibraryController : ControllerBase
{
  private readonly ILibraryService _libraryService;

  public LibraryController(ILibraryService libraryService)
  {
    _libraryService = libraryService ?? throw new ArgumentNullException(nameof(libraryService));
  }

  /// <summary>
  /// Retornar a biblioteca usando o userGuid
  /// </summary>
  [HttpGet("{userGuid:guid}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetByUserGuidAsync(Guid userGuid)
  {
    var library = await _libraryService.GetByUserGuidAsync(userGuid);

    if (library == null)
      return NotFound(new { Message = $"Biblioteca do usuário {userGuid} não encontrada." });

    return Ok(library);
  }

  /// <summary>
  /// Adicioanr jogo na biblioteca
  /// </summary>
  [HttpPost("acquire")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> AcquireGameAsync([FromBody] LibraryGameInputDto dto)
  {
    try
    {
      var libraryGame = await _libraryService.AcquireGameAsync(dto);
      var response = new
      {
        Message = "Jogo adicionado à biblioteca com sucesso.",
        Data = libraryGame
      };
      return Ok(response);
    }
    catch (System.Collections.Generic.KeyNotFoundException ex)
    {
      return NotFound(new { ex.Message });
    }
    catch (InvalidOperationException ex)
    {
      return BadRequest(new { ex.Message });
    }
  }
}