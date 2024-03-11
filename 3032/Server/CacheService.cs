using Microsoft.Extensions.Caching.Memory;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T? Get<T>(string key, CancellationToken cancellationToken = default)
    {
        return _cache.Get<T>(key);
    }

    public void Set<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions();
        if (expiration.HasValue)
        {
            cacheEntryOptions.AbsoluteExpirationRelativeToNow = expiration;
        }
        _cache.Set(key, value, cacheEntryOptions);
    }

    public void Remove(string key, CancellationToken cancellationToken = default)
    {
        _cache.Remove(key);
    }
}