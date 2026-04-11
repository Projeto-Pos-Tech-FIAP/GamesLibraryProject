using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TechChallengeFase1.Infrastructure.Cache.Abstractions;

namespace TechChallengeFase1.Infrastructure.Cache;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan expiration, CancellationToken ct)
    {
        var cached = await _cache.GetStringAsync(key, ct);

        if (cached is not null)
            return JsonConvert.DeserializeObject<T>(cached)!;

        var value = await factory(ct);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), options, ct);

        return value;
    }

    public async Task RemoveInvalidateCacheAsync(string key, CancellationToken ct)
    {
        await _cache.RemoveAsync(key, ct);
    }

    public async Task RemoveInvalidateCacheAsync(IEnumerable<string> keys, CancellationToken ct)
    {
        var tasks = keys.Select(key => _cache.RemoveAsync(key, ct));
        await Task.WhenAll(tasks);
    }
}
