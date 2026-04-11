using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Application.Interfaces;

namespace TechChallengeFase1.Api.GraphQL.Queries;

[ExtendObjectType("Query")]
public class GameQueries
{
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<GameOutputDto>> GetGames(
        [Service] IGameService gameService)
        => await gameService.GetAllAsync();

    public async Task<GameOutputDto?> GetGameById(
        [Service] IGameService gameService, int gameId)
        => await gameService.GetGameByIdAsync(gameId);
}
