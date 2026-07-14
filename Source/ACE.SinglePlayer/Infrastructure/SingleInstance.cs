namespace ACE.SinglePlayer.Infrastructure;

public sealed class SingleInstance : IDisposable
{
    private readonly Mutex mutex;

    public SingleInstance(string name)
    {
        mutex = new Mutex(initiallyOwned: true, name, out var createdNew);
        IsPrimary = createdNew;
    }

    public bool IsPrimary { get; }

    public void Dispose()
    {
        if (IsPrimary)
            mutex.ReleaseMutex();
        mutex.Dispose();
    }
}
