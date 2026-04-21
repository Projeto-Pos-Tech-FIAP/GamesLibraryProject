using System.Text.Json.Serialization;

namespace TechChallengeFase1.Domain.DTOs.AuthDto
{
    public class KeycloakUserResponseDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;

        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = null!;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = null!;

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonPropertyName("attributes")]
        public Dictionary<string, List<string>>? Attributes { get; set; }
    }
}