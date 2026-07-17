namespace ACE.SinglePlayer.Infrastructure;

public sealed class LauncherLog : IDisposable
{
    private readonly object sync = new();
    private readonly StreamWriter writer;

    public LauncherLog(string logsDirectory)
    {
        Directory.CreateDirectory(logsDirectory);
        LogPath = Path.Combine(logsDirectory, "OpenDereth.log");
        writer = new StreamWriter(new FileStream(LogPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
        {
            AutoFlush = true
        };
    }

    public string LogPath { get; }
    public event Action<string>? MessageWritten;

    public void Write(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        var line = $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss zzz} {message}";
        lock (sync)
            writer.WriteLine(line);
        MessageWritten?.Invoke(line);
    }

    public void Dispose() => writer.Dispose();
}
