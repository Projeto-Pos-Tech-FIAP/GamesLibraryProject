using Microsoft.EntityFrameworkCore;
using System.Data;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.Entities;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Infrastructure.Data.Audit;

namespace TechChallengeFase1.Infrastructure.Data.Context;

public sealed class MyDbContext : DbContext
{
    private readonly IAuditService? _auditService;

    public DbSet<Game> Games { get; set; }
    public DbSet<GameGenre> GameGenres { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibraryGame> LibraryGames { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options, IAuditService? auditService = null)
        : base(options)
    {
        _auditService = auditService;
    }

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditEntries = CaptureAuditEntries();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (_auditService != null && auditEntries.Count > 0)
        {
            var logs = auditEntries.Select(e => e.ToAuditLog());
            await _auditService.SaveAuditLogsAsync(logs, cancellationToken);
        }

        return result;
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

                method?.Invoke(null, [modelBuilder]);
            }
        }
    }

    private static void ConfigureSoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : class, ISoftDelete
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
    }

    private List<AuditEntryCapture> CaptureAuditEntries()
    {
        if (_auditService is null)
            return [];

        ChangeTracker.DetectChanges();

        var entries = new List<AuditEntryCapture>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State is EntityState.Detached or EntityState.Unchanged)
                continue;

            entries.Add(new AuditEntryCapture(entry));
        }

        return entries;
    }
}
