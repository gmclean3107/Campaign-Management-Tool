using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Service for caching data using an in-memory cache implementation.
/// </summary>
public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class.
    /// </summary>
    /// <param name="cache">The memory cache implementation.</param>
    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves the cached item with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="key">The key of the cached item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The cached item, or <c>null</c> if the key does not exist.</returns>
    public T? Get<T>(string key, CancellationToken cancellationToken = default)
    {
        return _cache.Get<T>(key);
    }

    /// <summary>
    /// Sets the specified value into the cache with the given key.
    /// </summary>
    /// <typeparam name="T">The type of the value to cache.</typeparam>
    /// <param name="key">The key to associate with the cached value.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="expiration">The optional expiration time for the cached item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public void Set<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions();
        if (expiration.HasValue)
        {
            cacheEntryOptions.AbsoluteExpirationRelativeToNow = expiration;
        }
        _cache.Set(key, value, cacheEntryOptions);
    }

    /// <summary>
    /// Removes the cached item with the specified key.
    /// </summary>
    /// <param name="key">The key of the item to remove from the cache.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public void Remove(string key, CancellationToken cancellationToken = default)
    {
        _cache.Remove(key);
    }
}