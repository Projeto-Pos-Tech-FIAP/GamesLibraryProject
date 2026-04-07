using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Infrastructure.Data.Context;
using TechChallengeFase1.Infrastructure.Data.Repositories;
using TechChallengeFase1.Infrastructure.Data.Services;
using TechChallengeFase1.Infrastructure.Options;

namespace TechChallengeFase1.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(
            configuration.GetSection(MongoDbSettings.SectionName));

        services.AddScoped<IAuditService, MongoAuditService>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Domain repositories
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<ILibraryRepository, LibraryRepository>();
        services.AddScoped<ILibraryGameRepository, LibraryGameRepository>();

        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        var redisConnectionString = configuration.GetConnectionString("Redis");

        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        return services;
    }
}
