using System.Text.Json.Serialization;

namespace TechChallengeFase1.Domain.DTOs.AuthDto
{
    public class TokenResponseDto
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = null!;
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
