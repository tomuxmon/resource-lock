internal class Program {
    private static void Main(string[] args) {
        var sum = 0;
        var tasks = new List<Task>();

        for (int i = 0; i < 1000; i++) {
            var t = Task.Factory.StartNew(async () => {
                for (int j = 0; j < 1000; j++) {
                    using var _ = await (await typeof(Marker).GetLockAsync()).LockAsync().ConfigureAwait(false);
                    sum++;
                }
            });
            tasks.Add(t);
        }
        Task.WaitAll(tasks.ToArray());
        
        Console.WriteLine(sum);
    }

}
