using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Infrastructure.Data.Context;
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

        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
