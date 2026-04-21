using Microsoft.Extensions.Options;
using TechChallengeFase1.Infrastructure.Options;
using TechChallengeFase1.Infrastructure.Providers.Keycloak.Abstractions;

namespace TechChallengeFase1.Infrastructure.Providers.Keycloak
{
    public class KeycloakConfigProvider : IKeycloakConfigProvider
    {
        private readonly KeycloakSettings _settings;

        public KeycloakConfigProvider(IOptions<KeycloakSettings> options)
        {
            _settings = options.Value;
        }

        public KeycloakSettings Get()
        {
            return _settings;
        }
    }
}
