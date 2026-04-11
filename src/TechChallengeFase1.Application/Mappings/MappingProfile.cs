using AutoMapper;
using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Game, GameOutputDto>();
        CreateMap<GameInputDto, Game>();
        CreateMap<Order, OrderOutputDto>();
        CreateMap<OrderItem, OrderItemOutputDto>();
        CreateMap<Currency, CurrencyOutputDto>();
    }
}
