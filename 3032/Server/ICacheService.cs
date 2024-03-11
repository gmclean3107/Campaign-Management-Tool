public interface ICacheService
{
    T? Get<T>(string key, CancellationToken cancellationToken = default);

    void Set<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default);

    void Remove(string key, CancellationToken cancellationToken = default);
}