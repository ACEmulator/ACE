using System.Diagnostics;

using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Database;

public sealed class ManagedMariaDbRuntime : IDatabaseRuntime
{
    private readonly ExternalMariaDbRuntime validator;
    private readonly LauncherLog log;
    private Process? process;

    public ManagedMariaDbRuntime(ExternalMariaDbRuntime validator, LauncherLog log)
    {
        this.validator = validator;
        this.log = log;
    }

    public bool IsManaged => true;

    public async Task StartAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        if (process is { HasExited: false })
            return;
        if (!File.Exists(settings.ManagedDatabaseExePath))
            throw new FileNotFoundException("The configured mariadbd.exe was not found.", settings.ManagedDatabaseExePath);

        var dataDirectory = Path.Combine(settings.RuntimeDirectory, "Database");
        if (!Directory.Exists(Path.Combine(dataDirectory, "mysql")))
            throw new InvalidOperationException(
                "Managed MariaDB is experimental and its data directory is not initialized. Initialize this exact MariaDB distribution's data directory first, then retry. No database binaries are downloaded by the launcher.");

        var startInfo = new ProcessStartInfo
        {
            FileName = settings.ManagedDatabaseExePath,
            WorkingDirectory = Path.GetDirectoryName(settings.ManagedDatabaseExePath)!,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        startInfo.ArgumentList.Add($"--datadir={dataDirectory}");
        startInfo.ArgumentList.Add($"--port={settings.DatabasePort}");
        startInfo.ArgumentList.Add("--bind-address=127.0.0.1");
        startInfo.ArgumentList.Add("--console");

        process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
        process.OutputDataReceived += (_, args) => { if (args.Data is not null) log.Write("[MariaDB] " + args.Data); };
        process.ErrorDataReceived += (_, args) => { if (args.Data is not null) log.Write("[MariaDB] " + args.Data); };
        if (!process.Start())
            throw new InvalidOperationException("MariaDB did not start.");
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        var deadline = DateTime.UtcNow.AddSeconds(45);
        DatabaseValidationResult result;
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (process.HasExited)
                throw new InvalidOperationException($"Managed MariaDB exited unexpectedly (exit code {process.ExitCode}).");
            result = await validator.ValidateAsync(settings, cancellationToken);
            if (result.IsValid)
                return;
            await Task.Delay(500, cancellationToken);
        } while (DateTime.UtcNow < deadline);

        throw new TimeoutException("Managed MariaDB started but did not become ready within 45 seconds. " + result.Message);
    }

    public Task<DatabaseValidationResult> ValidateAsync(LauncherSettings settings, CancellationToken cancellationToken) =>
        validator.ValidateAsync(settings, cancellationToken);

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (process is null || process.HasExited)
            return;

        process.CloseMainWindow();
        try
        {
            await process.WaitForExitAsync(cancellationToken).WaitAsync(TimeSpan.FromSeconds(10), cancellationToken);
        }
        catch (TimeoutException)
        {
            log.Write("Managed MariaDB did not accept a graceful close; stopping the launcher-owned process.");
            process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync(cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync(CancellationToken.None);
        process?.Dispose();
    }
}
