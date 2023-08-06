using System.Collections.Concurrent;

internal static class ResourceLock {
    private static readonly ConcurrentDictionary<Type, AsyncLock> _locks = new();
    internal static AsyncLock GetLock(this Type type)
        => _locks.GetOrAdd(type, t => new AsyncLock());
}
