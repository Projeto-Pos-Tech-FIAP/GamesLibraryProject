using Microsoft.Extensions.Options;
using System.Text.Json;
using TechChallengeFase1.Domain.DTOs.AuthDto;
using TechChallengeFase1.Domain.Exceptions;
using TechChallengeFase1.Infrastructure.Options;
using TechChallengeFase1.Infrastructure.Providers.Keycloak.Abstractions;

namespace TechChallengeFase1.Infrastructure.Providers.Keycloak
{
    public class KeycloakTokenProvider : IKeycloakTokenProvider
    {
        private readonly HttpClient _http;
        private readonly KeycloakSettings _settings;

        private string? _cachedToken;
        private DateTime _expiresAt;

        public KeycloakTokenProvider(HttpClient http, IOptions<KeycloakSettings> options)
        {
            _http = http;
            _settings = options.Value;
        }

        public async Task<string> GetAdminTokenAsync()
        {
           
            if (!string.IsNullOrEmpty(_cachedToken) && _expiresAt > DateTime.UtcNow)
                return _cachedToken;

            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("client_id", _settings.ClientId),
            new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

            var response = await _http.PostAsync(
                $"{_settings.BaseUrl}realms/{_settings.Realm}/protocol/openid-connect/token",
                content);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new ExternalServiceException($"Erro ao obter token: {json}");

            var tokenResponse = JsonSerializer.Deserialize<TokenResponseDto>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _cachedToken = tokenResponse!.AccessToken;
            _expiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 30);

            return _cachedToken!;
        }
    }
}
