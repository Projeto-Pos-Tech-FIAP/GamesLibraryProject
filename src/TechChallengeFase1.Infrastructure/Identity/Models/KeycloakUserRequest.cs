namespace TechChallengeFase1.Infrastructure.Identity.Models
{
    public class KeycloakUserRequest
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool Enabled { get; set; }

        public List<KeycloakCredential> Credentials { get; set; } = new();
        public Dictionary<string, List<string>> Attributes { get; set; } = new();
    }

    public class KeycloakCredential
    {
        public string Type { get; set; } = "password";
        public string Value { get; set; } = null!;
        public bool Temporary { get; set; } = false;
    }
}

