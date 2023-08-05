using System.Collections.Concurrent;

internal static class ResourceLock {
    private static readonly ConcurrentDictionary<Type, AsyncLock> _locks = new();
    private static readonly AsyncLock _lock = new();
    internal static async Task<AsyncLock> GetLockAsync(this Type type) {
        using var _ = await _lock.LockAsync();
        return _locks.GetOrAdd(type, new AsyncLock());
    }
    internal static AsyncLock GetLock(this Type type)
        => _locks.GetOrAdd(type, new AsyncLock());
}
