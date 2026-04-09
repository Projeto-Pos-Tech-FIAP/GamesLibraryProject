using TechChallengeFase1.Application.DTOs;

namespace TechChallengeFase1.Api.GraphQL.Types;

public class GameType : ObjectType<GameOutputDto>
{
    protected override void Configure(IObjectTypeDescriptor<GameOutputDto> descriptor)
    {
        descriptor.Field(g => g.GameId).Type<NonNullType<IntType>>();
        descriptor.Field(g => g.BasePrice).Description("Preço base em reais");
        descriptor.Field(g => g.Description).Description("Descrição do jogo");
    }
}
