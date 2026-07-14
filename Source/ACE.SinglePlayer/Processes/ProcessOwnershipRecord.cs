using System.Diagnostics;
using System.Text.Json;

namespace ACE.SinglePlayer.Processes;

public sealed class ProcessOwnershipRecord
{
    public int ProcessId { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public string ExecutablePath { get; set; } = string.Empty;

    public bool Matches(int processId, DateTime startTimeUtc, string executablePath)
    {
        return ProcessId == processId &&
               Math.Abs((StartTimeUtc - startTimeUtc).TotalSeconds) < 1 &&
               string.Equals(Path.GetFullPath(ExecutablePath), Path.GetFullPath(executablePath), StringComparison.OrdinalIgnoreCase);
    }

    public static ProcessOwnershipRecord FromProcess(Process process, string executablePath) => new()
    {
        ProcessId = process.Id,
        StartTimeUtc = process.StartTime.ToUniversalTime(),
        ExecutablePath = Path.GetFullPath(executablePath)
    };

    public static async Task<ProcessOwnershipRecord?> LoadAsync(string path, CancellationToken cancellationToken)
    {
        if (!File.Exists(path))
            return null;
        await using var stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<ProcessOwnershipRecord>(stream, cancellationToken: cancellationToken);
    }

    public async Task SaveAsync(string path, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var temporaryPath = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            await using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                await JsonSerializer.SerializeAsync(stream, this, cancellationToken: cancellationToken);
            File.Move(temporaryPath, path, true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }
}
