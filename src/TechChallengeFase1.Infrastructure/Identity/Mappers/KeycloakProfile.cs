using AutoMapper;
using TechChallengeFase1.Domain.DTOs.AuthDto;
using TechChallengeFase1.Infrastructure.Identity.Models;

namespace TechChallengeFase1.Infrastructure.Identity.Mappers
{
    public class KeycloakProfile : Profile
    {
        public KeycloakProfile()
        {
            CreateMap<CreateUserModel, KeycloakUserRequest>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.Credentials, opt => opt.MapFrom(src =>
                    new List<KeycloakCredential>
                    {
                        new KeycloakCredential
                        {
                            Value = src.Password
                        }
                    }
                ))
                .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src =>
                    new Dictionary<string, List<string>>
                    {
                        { "DateOfBirth", new List<string> { src.DateOfBirth.ToString("yyyy-MM-dd") } },
                        { "Gender", new List<string> { src.Gender } }
                    }
                ));
            CreateMap<CreateUserModel, KeycloakUpdateUserRequest>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src =>
                    new Dictionary<string, List<string>>
                    {
                        { "DateOfBirth", new List<string> { src.DateOfBirth.ToString("yyyy-MM-dd") } },
                        { "Gender", new List<string> { src.Gender } }
                    }
                ));
        }
    }
}