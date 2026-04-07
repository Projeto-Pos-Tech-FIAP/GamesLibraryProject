namespace TechChallengeFase1.Infrastructure.Cache.Abstractions;

public interface ICacheService
{
    Task<T> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan expiration, CancellationToken ct);
    Task RemoveInvalidateCacheAsync(string key, CancellationToken ct);
    Task RemoveInvalidateCacheAsync(IEnumerable<string> keys, CancellationToken ct);
}

