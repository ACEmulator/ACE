using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Orchestration;

public sealed class LauncherStateMachine
{
    public LauncherState State { get; private set; } = LauncherState.NotConfigured;
    public string Message { get; private set; } = "Complete setup to begin.";
    public event Action<LauncherStateChanged>? Changed;

    public void Set(LauncherState state, string message)
    {
        State = state;
        Message = message;
        Changed?.Invoke(new LauncherStateChanged(state, message));
    }
}
