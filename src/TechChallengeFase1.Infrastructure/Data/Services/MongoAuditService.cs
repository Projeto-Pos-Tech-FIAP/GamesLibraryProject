using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.Entities;
using TechChallengeFase1.Infrastructure.Options;

namespace TechChallengeFase1.Infrastructure.Data.Services;

public class MongoAuditService : IAuditService
{
    private readonly IMongoCollection<AuditLog> _collection;

    static MongoAuditService()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(AuditLog)))
        {
            BsonClassMap.RegisterClassMap<AuditLog>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                  .SetIdGenerator(StringObjectIdGenerator.Instance)
                  .SetSerializer(new MongoDB.Bson.Serialization.Serializers.StringSerializer(BsonType.ObjectId));
            });
        }
    }

    public MongoAuditService(IOptions<MongoDbSettings> settings)
    {
        var config = settings.Value;
        var client = new MongoClient(config.ConnectionString);
        var database = client.GetDatabase(config.DatabaseName);
        _collection = database.GetCollection<AuditLog>(config.CollectionName);
    }

    public async Task SaveAuditLogsAsync(IEnumerable<AuditLog> logs, CancellationToken cancellationToken = default)
    {
        var logList = logs.ToList();
        if (logList.Count == 0)
            return;

        await _collection.InsertManyAsync(logList, cancellationToken: cancellationToken);
    }
}
