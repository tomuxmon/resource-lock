public class AsyncLock {

    private readonly SemaphoreSlim _semaphore;

    public AsyncLock() =>
        _semaphore = new SemaphoreSlim(1, 1);

    public ValueTask<IDisposable> LockAsync() =>
        LockAsync(CancellationToken.None);

    public async ValueTask<IDisposable> LockAsync(CancellationToken ct) {
        await _semaphore.WaitAsync(ct).ConfigureAwait(false);
        return new Releaser(this);
    }

    internal struct Releaser : IDisposable {
        private readonly AsyncLock _asyncLock;
        private bool _isDisposed;

        internal Releaser(AsyncLock asyncLock) {
            _asyncLock = asyncLock;
            _isDisposed = false;
        }

        public void Dispose() {
            if (_isDisposed) {
                return;
            }

            _asyncLock._semaphore.Release();
            _isDisposed = true;
        }
    }
}
