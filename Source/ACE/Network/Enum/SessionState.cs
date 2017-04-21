namespace ACE.Network.Enum
{
    public enum SessionState
    {
        Idle,
        AuthConnecting,
        AuthConnected,
        WorldConnecting,
        WorldConnected,
        Terminating,
        Terminated
    }
}