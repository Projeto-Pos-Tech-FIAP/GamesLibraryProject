using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using TechChallengeFase1.Api.Configurations;
using TechChallengeFase1.Api.GraphQL.Queries;
using TechChallengeFase1.Api.GraphQL.Types;
using TechChallengeFase1.Api.Middlewares;
using TechChallengeFase1.Application.Extensions;
using TechChallengeFase1.Infrastructure.Extensions;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Scope} {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger, dispose: true);

        builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var listaErros = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .Select(e => new
                    {
                        Campo = e.Key,
                        Mensagem = e.Value!.Errors.First().ErrorMessage
                    })
                    .ToList<dynamic>();

                return new ObjectResult(new
                {
                    CodigoStatus = 400,
                    Mensagem = "Erro de validação nos campos enviados.",
                    ListaErros = listaErros
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            };
        });
        builder.Services
        .AddGraphQLServer()
        .AddQueryType(d => d.Name("Query"))
        .AddTypeExtension<GameQueries>()
        .AddType<GameType>()
        .AddFiltering()
        .AddSorting()
        .AddProjections();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TechChallenge API",
                Version = "v1",
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando Bearer. Ex: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
            });
        });

        await builder.Services.AddKeycloakAuthentication(builder.Configuration);

        var app = builder.Build();

        app.MapGraphQL("/graphql");

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}