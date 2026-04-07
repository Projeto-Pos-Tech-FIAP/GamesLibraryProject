using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;
using TechChallengeFase1.Api.Middlewares;
using TechChallengeFase1.Application.DTOs.Shared;
using TechChallengeFase1.Application.Extensions;
using TechChallengeFase1.Infrastructure.Data.Context;
using TechChallengeFase1.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

        var erro = new ExceptionOutputDto
        {
            CodigoStatus = HttpStatusCode.BadRequest,
            Mensagem = "Erro de validação nos campos enviados.",
            ListaErros = listaErros
        };

        return new ObjectResult(erro)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    };
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Scope} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger, dispose: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
