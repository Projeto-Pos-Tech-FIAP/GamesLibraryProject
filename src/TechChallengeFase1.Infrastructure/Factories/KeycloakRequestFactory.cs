namespace TechChallengeFase1.Infrastructure.Factories
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using TechChallengeFase1.Infrastructure.Factories.Abstractions;
    using TechChallengeFase1.Infrastructure.Identity.Models;
    using TechChallengeFase1.Infrastructure.Options;
    using TechChallengeFase1.Infrastructure.Providers.Keycloak.Abstractions;

    public class KeycloakRequestFactory : IKeycloakRequestFactory
    {
        private readonly KeycloakSettings _settings;
        private readonly IKeycloakTokenProvider _tokenProvider;

        public KeycloakRequestFactory(IKeycloakConfigProvider provider, IKeycloakTokenProvider tokenProvider)
        {
            _settings = provider.Get();
            _tokenProvider = tokenProvider;
        }

        public HttpRequestMessage CreateTokenRequest(string username, string password)
        {
            var url = $"{_settings.BaseUrl}realms/{_settings.Realm}/protocol/openid-connect/token";

            return new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("client_id", _settings.ClientId),
                new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                })
            };
        }

        public HttpRequestMessage CreateRefreshTokenRequest(string refreshToken)
        {
            var url = $"{_settings.BaseUrl}realms/{_settings.Realm}/protocol/openid-connect/token";
            return new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("client_id", _settings.ClientId),
                new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
            })
            };
        }
        public HttpRequestMessage GetUsersRequest(string token, string? username = null, string? email = null)
        {
            var url = $"{_settings.BaseUrl}admin/realms/{_settings.Realm}/users";

            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(username))
                queryParams.Add($"username={username}");

            if (!string.IsNullOrEmpty(email))
                queryParams.Add($"email={email}");

            if (queryParams.Any())
                url += "?" + string.Join("&", queryParams);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return request;
        }

        public HttpRequestMessage CreateUserRequest(KeycloakUserRequest bodyRequest, string token)
        {
            var url = $"{_settings.BaseUrl}admin/realms/{_settings.Realm}/users";

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(bodyRequest, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return request;
        }
        public HttpRequestMessage AddUserToGroupRequest(string userId, string groupId, string token)
        {
            var url = $"{_settings.BaseUrl}admin/realms/{_settings.Realm}/users/{userId}/groups/{groupId}";

            var request = new HttpRequestMessage(HttpMethod.Put, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return request;
        }
        public HttpRequestMessage GetGroupsRequest(string token)
        {
            var url = $"{_settings.BaseUrl}admin/realms/{_settings.Realm}/groups";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return request;
        }
        public HttpRequestMessage SetUserEnabledRequest(string userId, bool enable, string token)
        {
            var url = $"{_settings.BaseUrl}admin/realms/{_settings.Realm}/users/{userId}";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            var json = JsonSerializer.Serialize(new
            {
                enabled = enable
            });
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return request;
        }
        public HttpRequestMessage GetAllUsersRequest(string token, int first = 0, int max = 100)
        {
            var url = $"{_settings.BaseUrl}admin/realms/{_settings.Realm}/users?first={first}&max={max}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return request;
        }
        public HttpRequestMessage UpdateUserRequest(string userId, KeycloakUpdateUserRequest bodyRequest, string token)
        {
            var url = $"{_settings.BaseUrl}admin/realms/{_settings.Realm}/users/{userId}";

            var request = new HttpRequestMessage(HttpMethod.Put, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(bodyRequest, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return request;
        }
    }
}
