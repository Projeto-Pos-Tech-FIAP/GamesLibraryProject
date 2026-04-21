namespace TechChallengeFase1.Infrastructure.Identity.Models
{
    public class KeycloakUpdateUserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool Enabled { get; set; } = true;
        public Dictionary<string, List<string>>? Attributes { get; set; }
    }
}
