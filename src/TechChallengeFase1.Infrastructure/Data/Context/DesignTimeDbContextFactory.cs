using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TechChallengeFase1.Infrastructure.Data.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var apiAppSettingsPath = Path.Combine(basePath, "..", "..", "..", "src", "TechChallengeFase1.Api", "appsettings.json");

        if (!File.Exists(apiAppSettingsPath))
        {
            apiAppSettingsPath = Path.Combine(basePath, "appsettings.json");
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(apiAppSettingsPath) ?? basePath)
            .AddJsonFile(Path.GetFileName(apiAppSettingsPath), optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("DefaultConnection is not configured for design-time DbContext creation.");

        var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new MyDbContext(optionsBuilder.Options);
    }
}
