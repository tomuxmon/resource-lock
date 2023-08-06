var sum = 0;
var threads = new List<Thread>(1000);
var cts = new CancellationTokenSource();
var rng = new Random();

async Task do_it_1() {
    for (int j = 0; j < 1000; j++) {
        await Task.Delay(rng.Next(2));
        sum++;
    }
}

async Task do_it_2() {
    for (int j = 0; j < 1000; j++) {
        await Task.Delay(rng.Next(2));
        using var _ = await typeof(Marker)
            .GetLock()
            .LockAsync(cts.Token)
            .ConfigureAwait(false);
        sum++;
    }
}

for (int i = 0; i < 1000; i++) {
    var t = new Thread(() => do_it_1().GetAwaiter().GetResult());
    threads.Add(t);
    t.Start();
};

Console.WriteLine("Lets wait fot do it 1!");
threads.AsParallel().ForAll(t => t.Join());
threads.Clear();
Console.WriteLine(sum);
sum = 0;

for (int i = 0; i < 1000; i++) {
    var t = new Thread(() => do_it_2().GetAwaiter().GetResult());
    threads.Add(t);
    t.Start();
};

Console.WriteLine("Lets wait fot do it 2!");
threads.AsParallel().ForAll(t => t.Join());
threads.Clear();
Console.WriteLine(sum);
sum = 0;
