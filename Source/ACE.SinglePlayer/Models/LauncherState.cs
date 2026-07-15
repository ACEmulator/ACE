namespace ACE.SinglePlayer.Models;

public enum LauncherState
{
    NotConfigured,
    PreparingData,
    CheckingDatabase,
    StartingDatabase,
    StartingServer,
    WaitingForWorld,
    Ready,
    GameRunning,
    Error
}

public sealed record LauncherStateChanged(LauncherState State, string Message);
