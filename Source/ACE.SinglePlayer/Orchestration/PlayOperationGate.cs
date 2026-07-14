namespace ACE.SinglePlayer.Orchestration;

public sealed class PlayOperationGate
{
    private int active;

    public bool IsActive => Volatile.Read(ref active) != 0;

    public bool TryEnter() => Interlocked.CompareExchange(ref active, 1, 0) == 0;

    public void Exit() => Volatile.Write(ref active, 0);
}
