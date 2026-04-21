using AutoMapper;
using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Application.DTOs.UsersDto;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.DTOs.AuthDto;
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
        CreateMap<CreateUserInputDto, CreateUserModel>();
        CreateMap<EditUserDto, CreateUserModel>();
        CreateMap<KeycloakUserResponseDto, CreateUserModel>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Level, opt => opt.Ignore())
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src =>
                src.Attributes != null && src.Attributes.ContainsKey("Gender")
                    ? src.Attributes["Gender"].FirstOrDefault()
                    : null))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src =>
                src.Attributes != null && src.Attributes.ContainsKey("DateOfBirth")
                    ? DateTime.Parse(src.Attributes["DateOfBirth"].First())
                    : default));
    }
}
