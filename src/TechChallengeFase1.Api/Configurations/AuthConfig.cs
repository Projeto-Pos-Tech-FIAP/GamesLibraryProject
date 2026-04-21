using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace TechChallengeFase1.Api.Configurations;

public static class AuthConfig
{
    public static async Task<IServiceCollection> AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["Keycloak:Authority"];
        var audience = configuration["Keycloak:Audience"];

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,

                    ValidateAudience = false,
                    ValidAudience = audience,

                    ValidateLifetime = true
                };


                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var identity = context.Principal?.Identity as ClaimsIdentity;

                       
                        var realmAccess = context.Principal?.FindFirst("realm_access")?.Value;

                        if (!string.IsNullOrEmpty(realmAccess))
                        {
                            var roles = JsonSerializer.Deserialize<JsonElement>(realmAccess)
                                .GetProperty("roles")
                                .EnumerateArray()
                                .Select(r => r.GetString());

                            foreach (var role in roles)
                            {
                                if (!string.IsNullOrEmpty(role))
                                    identity?.AddClaim(new Claim(ClaimTypes.Role, role));
                            }
                        }

                       
                        var resourceAccess = context.Principal?.FindFirst("resource_access")?.Value;

                        if (!string.IsNullOrEmpty(resourceAccess))
                        {
                            var json = JsonSerializer.Deserialize<JsonElement>(resourceAccess);

                            if (json.TryGetProperty("tech-challenge", out var client))
                            {
                                var roles = client.GetProperty("roles")
                                    .EnumerateArray()
                                    .Select(r => r.GetString());

                                foreach (var role in roles)
                                {
                                    if (!string.IsNullOrEmpty(role))
                                        identity?.AddClaim(new Claim(ClaimTypes.Role, role));
                                }
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();
        return services;
    }
}

