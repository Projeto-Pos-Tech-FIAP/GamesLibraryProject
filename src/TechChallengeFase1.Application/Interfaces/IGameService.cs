using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Domain.DTOs;

namespace TechChallengeFase1.Application.Interfaces;

public interface IGameService
{
    Task<GameOutputDto> CreateAsync(GameInputDto dto);
}
