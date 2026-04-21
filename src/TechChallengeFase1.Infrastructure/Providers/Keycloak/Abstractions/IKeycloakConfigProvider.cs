using TechChallengeFase1.Infrastructure.Options;

namespace TechChallengeFase1.Infrastructure.Providers.Keycloak.Abstractions
{
    public interface IKeycloakConfigProvider
    {
        KeycloakSettings Get();
    }
}
