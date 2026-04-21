using TechChallengeFase1.Application.DTOs.UsersDto;
using TechChallengeFase1.Domain.DTOs.AuthDto;

namespace TechChallengeFase1.Application.Interfaces
{
    public interface IUsersServices
    {
        Task CreateUserAsync(CreateUserInputDto command);
        Task SetUserEnabledAsync(string userEmail, bool enable);
        Task<List<KeycloakUserResponseDto>> GetAllUsersAsync();
        Task<KeycloakUserResponseDto> GetByUsernameOrEmailAsync(string? username, string? email);
        Task UpdateUserAsync(EditUserDto command, string userEmail);
    }
}
