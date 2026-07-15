using System.Diagnostics;

using ACE.SinglePlayer.ClientLaunch;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Processes;

public sealed class ClientProcessManager : IDisposable
{
    private readonly IReadOnlyDictionary<ClientLaunchMode, IClientLaunchProvider> providers;
    private readonly LauncherLog log;
    private Process? process;
    private bool stopping;

    public ClientProcessManager(IEnumerable<IClientLaunchProvider> providers, LauncherLog log)
    {
        this.providers = providers.ToDictionary(provider => provider.Mode);
        this.log = log;
    }

    public bool IsRunning => process is { HasExited: false };
    public event Action<int>? ClientExited;

    public async Task<Process> StartAsync(LauncherSettings settings, string accountPassword, CancellationToken cancellationToken)
    {
        if (IsRunning)
            throw new InvalidOperationException("The game is already running.");
        if (!providers.TryGetValue(settings.ClientLaunchMode, out var provider))
            throw new NotSupportedException($"Client launch mode '{settings.ClientLaunchMode}' is not implemented.");
        if (!provider.IsAvailable(out var reason))
            throw new InvalidOperationException(reason);

        var workingDirectory = Path.GetDirectoryName(Path.GetFullPath(settings.ClientExePath))!;
        log.Write($"Launching client executable '{Path.GetFullPath(settings.ClientExePath)}'.");
        log.Write($"Client working/DAT directory: '{workingDirectory}'. ACE.Server DAT directory: '{Path.GetFullPath(settings.DatFilesDirectory)}'.");
        process = await provider.LaunchAsync(new ClientLaunchRequest(settings, accountPassword), cancellationToken);
        process.EnableRaisingEvents = true;
        process.Exited += OnExited;
        log.Write($"Started acclient.exe process {process.Id} using {provider.DisplayName} mode.");
        return process;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (process is null || process.HasExited)
            return;

        stopping = true;
        try
        {
            process.CloseMainWindow();
            try
            {
                await process.WaitForExitAsync(cancellationToken).WaitAsync(TimeSpan.FromSeconds(10), cancellationToken);
            }
            catch (TimeoutException)
            {
                process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync(cancellationToken);
            }
        }
        finally
        {
            stopping = false;
        }
    }

    private void OnExited(object? sender, EventArgs e)
    {
        if (sender is not Process exited)
            return;
        log.Write($"acclient.exe process {exited.Id} exited with code {exited.ExitCode}.");
        if (exited.ExitCode != 0)
            log.Write("Client startup failed. Verify that the complete AC client, including all client DAT files, is beside acclient.exe in a writable folder and is not blocked by Windows Security.");
        if (!stopping)
            ClientExited?.Invoke(exited.ExitCode);
    }

    public void Dispose() => process?.Dispose();
}
