using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TechChallengeFase1.Domain.Entities;
using TechChallengeFase1.Domain.Interfaces;

namespace TechChallengeFase1.Infrastructure.Data.Context;

public sealed class MyDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<GameGenre> GemeGenres { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibraryGame> LibraryGames { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItens { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }

    public void SetConnectionString(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string nao pode ser vazia.", nameof(connectionString));

        var connection = Database.GetDbConnection();

        if (connection.State == ConnectionState.Open)
            connection.Close();

        Database.SetConnectionString(connectionString);
        ChangeTracker.Clear();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(MyDbContext)
                    .GetMethod(nameof(ConfigureSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);

                method?.Invoke(null, new object[] { modelBuilder });
            }
        }
    }

    private static void ConfigureSoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : class, ISoftDelete
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
    }
}
