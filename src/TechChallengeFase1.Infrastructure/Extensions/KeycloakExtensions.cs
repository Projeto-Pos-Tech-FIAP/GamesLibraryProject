using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Infrastructure.Factories;
using TechChallengeFase1.Infrastructure.Factories.Abstractions;
using TechChallengeFase1.Infrastructure.Identity.Services;
using TechChallengeFase1.Infrastructure.Options;
using TechChallengeFase1.Infrastructure.Providers.Keycloak;
using TechChallengeFase1.Infrastructure.Providers.Keycloak.Abstractions;

namespace TechChallengeFase1.Infrastructure.Extensions
{
    public static class KeycloakExtensions
    {
        public static IServiceCollection AddKeycloak(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KeycloakSettings>(
                configuration.GetSection("Keycloak"));

            services.AddSingleton<IKeycloakConfigProvider, KeycloakConfigProvider>();
            services.AddSingleton<IKeycloakTokenProvider, KeycloakTokenProvider>();

            services.AddScoped<IKeycloakRequestFactory, KeycloakRequestFactory>();

            services.AddHttpClient<IKeycloakService, KeycloakService>();
            

            return services;
        }
    }
}
