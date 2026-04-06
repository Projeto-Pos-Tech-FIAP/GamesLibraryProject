using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Application.Interfaces;

public interface IGameService
{
    Task<Game> CreateAsync(GameInputDto dto);
}
