using Microsoft.Extensions.DependencyInjection;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Application.Services;

namespace TechChallengeFase1.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<ILibraryService, LibraryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUsersServices, UsersService>();
        services.AddAutoMapper(typeof(ApplicationServiceExtensions).Assembly);

        return services;
    }
}
