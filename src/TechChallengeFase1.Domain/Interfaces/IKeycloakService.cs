using TechChallengeFase1.Domain.DTOs.AuthDto;

namespace TechChallengeFase1.Domain.Interfaces
{
    public interface IKeycloakService
    {
        Task<LoginRequestOutputDto> GetTokenAsync(string username, string password);
        Task<LoginRequestOutputDto> RefreshTokenAsync(string refreshToken);
        Task<List<KeycloakUserResponseDto>> GetUsersAsync(string? username = null, string? email = null);
        Task<List<KeycloakUserResponseDto>> GetAllUsersAsync();
        Task<List<GroupsDto>> GetAllGroupsByToken();
        Task<string> CreateUserAsync(CreateUserModel createUserInputDto);
        Task AssignUserToGroupAsync(string userId, string groupName);
        Task SetUserEnabledAsync(string userId, bool enable);
        Task UpdateUserAsync(string userId, CreateUserModel model);
    }
}