using AutoMapper;
using System.Net;
using System.Text.Json;
using TechChallengeFase1.Domain.DTOs.AuthDto;
using TechChallengeFase1.Domain.Exceptions;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Infrastructure.Factories.Abstractions;
using TechChallengeFase1.Infrastructure.Identity.Models;
using TechChallengeFase1.Infrastructure.Providers.Keycloak.Abstractions;

namespace TechChallengeFase1.Infrastructure.Identity.Services
{
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _http;
        private readonly IKeycloakRequestFactory _factory;
        private readonly IKeycloakTokenProvider _tokenProvider;
        private readonly IMapper _mapper;

        public KeycloakService(
            HttpClient http,
            IKeycloakRequestFactory factory,
            IKeycloakTokenProvider tokenProvider,
            IMapper mapper
            )
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<LoginRequestOutputDto> GetTokenAsync(string username, string password)
        {
            var request = _factory.CreateTokenRequest(username, password);

            var response = await _http.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedException("Invalid username or password");

                throw new ExternalServiceException($"Error authenticating with Keycloak: {content}");
            }

            var token = JsonSerializer.Deserialize<LoginRequestOutputDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (token == null)
                throw new ExternalServiceException("Error deserializing Keycloak response");

            return token;
        }

        public async Task<LoginRequestOutputDto> RefreshTokenAsync(string refreshToken)
        {
            var request = _factory.CreateRefreshTokenRequest(refreshToken);
            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedException("Invalid refresh token");

            return JsonSerializer.Deserialize<LoginRequestOutputDto>(content)!;
        }
        public async Task<List<KeycloakUserResponseDto>> GetAllUsersAsync()
        {

            var token = await _tokenProvider.GetAdminTokenAsync();

            var request = _factory.GetAllUsersRequest(token);

            var response = await _http.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ExternalServiceException($"Erro: {content}");

            return JsonSerializer.Deserialize<List<KeycloakUserResponseDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<KeycloakUserResponseDto>();
        }
        public async Task<List<KeycloakUserResponseDto>> GetUsersAsync(string? username = null, string? email = null)
        {

            var token = await _tokenProvider.GetAdminTokenAsync();

            var request = _factory.GetUsersRequest(token, username, email);

            var response = await _http.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ExternalServiceException($"Erro: {content}");

            return JsonSerializer.Deserialize<List<KeycloakUserResponseDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<KeycloakUserResponseDto>();
        }
        public async Task<string> CreateUserAsync(CreateUserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var userCreate = _mapper.Map<KeycloakUserRequest>(model);
            var token = await _tokenProvider.GetAdminTokenAsync();
            var request = _factory.CreateUserRequest(userCreate, token);
            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ExternalServiceException($"Error creating user: {content}");

            var location = response.Headers.Location;
            if (location == null)
                throw new ExternalServiceException("User created but Location header not found.");

            var userId = location.Segments.Last();

            return userId;
        }
        public async Task AssignUserToGroupAsync(string userGuid, string groupGuid)
        {
            if (string.IsNullOrEmpty(userGuid))
                throw new ArgumentNullException(nameof(userGuid));

            if (string.IsNullOrEmpty(groupGuid))
                throw new ArgumentNullException(nameof(groupGuid));

            var token = await _tokenProvider.GetAdminTokenAsync();
            var request = _factory.AddUserToGroupRequest(userGuid, groupGuid, token);
            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ExternalServiceException($"Error assigning user to group: {content}");

        }
        public async Task<List<GroupsDto>> GetAllGroupsByToken()
        {

            var token = await _tokenProvider.GetAdminTokenAsync();
            var request = _factory.GetGroupsRequest(token);
            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ExternalServiceException($"Error getting all groups: {content}");

            return JsonSerializer.Deserialize<List<GroupsDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<GroupsDto>();
        }
        public async Task SetUserEnabledAsync(string userId, bool enable)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var token = await _tokenProvider.GetAdminTokenAsync();
            var request = _factory.SetUserEnabledRequest(userId, enable, token);
            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ExternalServiceException($"Error setting user enabled state to {enable} for user {userId}: {content}");
        }
        public async Task UpdateUserAsync(string userId, CreateUserModel model)
        {
            var token = await _tokenProvider.GetAdminTokenAsync();
            var userCreate = _mapper.Map<KeycloakUpdateUserRequest>(model);
            var request = _factory.UpdateUserRequest(userId, userCreate, token);
            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ExternalServiceException(
                    $"Failed to update user '{userId}'. StatusCode: {(int)response.StatusCode} ({response.StatusCode}). Response: {content}"
                );
        }
    }
}