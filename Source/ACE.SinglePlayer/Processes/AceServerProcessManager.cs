using System.Diagnostics;

using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Processes;

public sealed class AceServerProcessManager : IDisposable
{
    private readonly LauncherLog log;
    private Process? process;
    private ProcessOwnershipRecord? ownership;
    private bool stopping;
    private bool standardInputAvailable;
    private string? ownershipPath;

    public AceServerProcessManager(LauncherLog log)
    {
        this.log = log;
    }

    public bool IsRunning => process is { HasExited: false };
    public Process? Process => process;
    public event Action<int>? UnexpectedExit;

    public async Task<Process?> TryAttachLauncherOwnedAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        if (IsRunning)
            return process;

        var record = await ProcessOwnershipRecord.LoadAsync(settings.OwnershipPath, cancellationToken);
        if (record is null)
            return null;
        try
        {
            var candidate = System.Diagnostics.Process.GetProcessById(record.ProcessId);
            if (candidate.HasExited || !record.Matches(candidate.Id, candidate.StartTime.ToUniversalTime(), settings.ServerExePath))
            {
                candidate.Dispose();
                return null;
            }

            process = candidate;
            ownership = record;
            ownershipPath = settings.OwnershipPath;
            standardInputAvailable = false;
            process.EnableRaisingEvents = true;
            process.Exited += OnExited;
            log.Write($"Attached to launcher-owned ACE.Server process {process.Id}.");
            return process;
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public async Task<Process> StartAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        if (IsRunning)
            return process!;

        Directory.CreateDirectory(settings.RuntimeDirectory);
        if (File.Exists(settings.ReadyFilePath))
            File.Delete(settings.ReadyFilePath);

        var startInfo = new ProcessStartInfo
        {
            FileName = settings.ServerExePath,
            WorkingDirectory = Path.GetDirectoryName(settings.ServerExePath)!,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            CreateNoWindow = true
        };
        startInfo.ArgumentList.Add("--config");
        startInfo.ArgumentList.Add(settings.ConfigPath);
        startInfo.ArgumentList.Add("--ready-file");
        startInfo.ArgumentList.Add(settings.ReadyFilePath);

        process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
        process.OutputDataReceived += (_, args) => { if (args.Data is not null) log.Write("[ACE.Server] " + args.Data); };
        process.ErrorDataReceived += (_, args) => { if (args.Data is not null) log.Write("[ACE.Server error] " + args.Data); };
        process.Exited += OnExited;
        if (!process.Start())
            throw new InvalidOperationException("Windows did not start ACE.Server.");

        standardInputAvailable = true;
        ownershipPath = settings.OwnershipPath;
        ownership = ProcessOwnershipRecord.FromProcess(process, settings.ServerExePath);
        await ownership.SaveAsync(ownershipPath, cancellationToken);
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        log.Write($"Started launcher-owned ACE.Server process {process.Id}.");
        return process;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (process is null || process.HasExited)
        {
            DeleteOwnershipRecord();
            return;
        }

        stopping = true;
        try
        {
            if (standardInputAvailable)
            {
                log.Write("Requesting graceful ACE.Server shutdown with its stop-now console command.");
                await process.StandardInput.WriteLineAsync("stop-now");
                await process.StandardInput.FlushAsync(cancellationToken);
            }

            try
            {
                await process.WaitForExitAsync(cancellationToken).WaitAsync(TimeSpan.FromSeconds(20), cancellationToken);
            }
            catch (TimeoutException)
            {
                if (IsStillOwnedProcess())
                {
                    log.Write("ACE.Server did not stop within 20 seconds; forcing only the verified launcher-owned process to exit.");
                    process.Kill(entireProcessTree: true);
                    await process.WaitForExitAsync(cancellationToken);
                }
            }
        }
        finally
        {
            DeleteOwnershipRecord();
            stopping = false;
        }
    }

    private bool IsStillOwnedProcess()
    {
        return process is { HasExited: false } && ownership is not null &&
               ownership.Matches(process.Id, process.StartTime.ToUniversalTime(), ownership.ExecutablePath);
    }

    private void OnExited(object? sender, EventArgs e)
    {
        if (sender is not Process exited)
            return;
        log.Write($"ACE.Server process {exited.Id} exited with code {exited.ExitCode}.");
        DeleteOwnershipRecord();
        if (!stopping)
            UnexpectedExit?.Invoke(exited.ExitCode);
    }

    private void DeleteOwnershipRecord()
    {
        if (ownershipPath is null || !File.Exists(ownershipPath))
            return;
        try
        {
            File.Delete(ownershipPath);
        }
        catch (IOException)
        {
            // A later startup will validate the PID and start time before trusting the record.
        }
    }

    public void Dispose() => process?.Dispose();
}
