namespace TechChallengeFase1.Api.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
