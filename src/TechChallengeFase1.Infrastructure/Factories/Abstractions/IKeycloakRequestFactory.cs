using TechChallengeFase1.Infrastructure.Identity.Models;

namespace TechChallengeFase1.Infrastructure.Factories.Abstractions
{
    public interface IKeycloakRequestFactory
    {
        HttpRequestMessage CreateTokenRequest(string username, string password);
        HttpRequestMessage CreateRefreshTokenRequest(string refreshToken);
        HttpRequestMessage GetUsersRequest(string token, string? username = null, string? email = null);
        HttpRequestMessage CreateUserRequest(KeycloakUserRequest bodyRequest, string token);
        HttpRequestMessage AddUserToGroupRequest(string userId, string groupId, string token);
        HttpRequestMessage GetGroupsRequest(string token);
        HttpRequestMessage SetUserEnabledRequest(string userId, bool enable, string token);
        HttpRequestMessage GetAllUsersRequest(string token, int first = 0, int max = 100);
        HttpRequestMessage UpdateUserRequest(string userId, KeycloakUpdateUserRequest bodyRequest, string token);
    }
}
