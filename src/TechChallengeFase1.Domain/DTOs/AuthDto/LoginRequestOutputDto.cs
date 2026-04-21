using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechChallengeFase1.Domain.DTOs.AuthDto
{
    public class LoginRequestOutputDto
    {
        [Required]
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = null!;

        [Required]
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [Required]
        [JsonPropertyName("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }

        [Required]
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = null!;

        [Required]
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = null!;

        [JsonPropertyName("not-before-policy")]
        public int NotBeforePolicy { get; set; }

        [JsonPropertyName("session_state")]
        public string SessionState { get; set; } = null!;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = null!;
    }
}
