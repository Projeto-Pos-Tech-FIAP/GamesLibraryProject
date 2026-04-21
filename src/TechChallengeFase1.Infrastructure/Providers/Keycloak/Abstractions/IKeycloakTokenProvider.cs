namespace TechChallengeFase1.Infrastructure.Providers.Keycloak.Abstractions
{
    public interface IKeycloakTokenProvider
    {
        Task<string> GetAdminTokenAsync();
    }
}
