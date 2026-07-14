using System.Net;

using ACE.SinglePlayer.Configuration;
using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;
using ACE.SinglePlayer.Networking;
using ACE.SinglePlayer.Processes;

namespace ACE.SinglePlayer.Orchestration;

public sealed class LauncherController : IAsyncDisposable
{
    private readonly AceConfigurationWriter configurationWriter;
    private readonly DatabaseRuntimeFactory databaseRuntimeFactory;
    private readonly DatabaseBootstrapper databaseBootstrapper;
    private readonly SettingsStore settingsStore;
    private readonly AceServerProcessManager serverManager;
    private readonly ClientProcessManager clientManager;
    private readonly ReadyFileMonitor readyFileMonitor;
    private readonly ISecretProtector secretProtector;
    private readonly LauncherLog log;
    private readonly PlayOperationGate playGate = new();
    private CancellationTokenSource? runCancellation;
    private IDatabaseRuntime? databaseRuntime;

    public LauncherController(LauncherSettings settings, AceConfigurationWriter configurationWriter,
        DatabaseRuntimeFactory databaseRuntimeFactory, DatabaseBootstrapper databaseBootstrapper,
        SettingsStore settingsStore, AceServerProcessManager serverManager,
        ClientProcessManager clientManager, ReadyFileMonitor readyFileMonitor,
        ISecretProtector secretProtector, LauncherLog log)
    {
        Settings = settings;
        this.configurationWriter = configurationWriter;
        this.databaseRuntimeFactory = databaseRuntimeFactory;
        this.databaseBootstrapper = databaseBootstrapper;
        this.settingsStore = settingsStore;
        this.serverManager = serverManager;
        this.clientManager = clientManager;
        this.readyFileMonitor = readyFileMonitor;
        this.secretProtector = secretProtector;
        this.log = log;
        State = new LauncherStateMachine();

        serverManager.UnexpectedExit += exitCode => State.Set(LauncherState.Error,
            $"ACE.Server stopped unexpectedly (exit code {exitCode}). Open Logs for details.");
        clientManager.ClientExited += exitCode => { _ = HandleClientExitAsync(); };
    }

    public LauncherSettings Settings { get; private set; }
    public LauncherStateMachine State { get; }
    public bool IsServerRunning => serverManager.IsRunning;
    public bool IsClientRunning => clientManager.IsRunning;

    public void UpdateSettings(LauncherSettings settings)
    {
        Settings = settings;
        State.Set(LauncherState.Ready, "Configuration saved. Click Play.");
    }

    public async Task<bool> PlayAsync(CancellationToken cancellationToken = default)
    {
        if (!playGate.TryEnter())
        {
            log.Write("Ignored a duplicate Play request while startup is already in progress.");
            return false;
        }

        try
        {
            if (clientManager.IsRunning)
            {
                log.Write("Ignored Play because the game is already running.");
                return false;
            }

            var validation = SetupValidator.Validate(Settings);
            if (!validation.IsValid)
            {
                State.Set(LauncherState.NotConfigured, validation.Message);
                return false;
            }

            Directory.CreateDirectory(Settings.RuntimeDirectory);
            Directory.CreateDirectory(Settings.ModsDirectory);
            runCancellation?.Dispose();
            runCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var token = runCancellation.Token;

            if (!serverManager.IsRunning)
            {
                databaseRuntime = databaseRuntimeFactory.Create(Settings);
                State.Set(databaseRuntime.IsManaged ? LauncherState.StartingDatabase : LauncherState.CheckingDatabase,
                    databaseRuntime.IsManaged ? "Preparing your private local database..." : "Checking MariaDB/MySQL and ACE databases...");
                await databaseRuntime.StartAsync(Settings, token);

                if (databaseRuntime.IsManaged)
                {
                    await databaseBootstrapper.BootstrapAsync(Settings, Settings.WorldDatabaseSqlPath, token);
                    var databaseValidation = await databaseRuntime.ValidateAsync(Settings, token);
                    if (!databaseValidation.IsValid)
                        throw new InvalidOperationException(databaseValidation.Message);
                    await settingsStore.SaveAsync(Settings, token);
                }

                await configurationWriter.WriteAsync(Settings, token);
                State.Set(LauncherState.StartingServer, "Starting the local ACE world...");
                var server = await serverManager.TryAttachLauncherOwnedAsync(Settings, token)
                    ?? await serverManager.StartAsync(Settings, token);

                State.Set(LauncherState.WaitingForWorld, "Waiting for the world to open for logins...");
                await readyFileMonitor.WaitAsync(Settings.ReadyFilePath, server, IPAddress.Loopback, Settings.Port,
                    TimeSpan.FromSeconds(Settings.ServerStartupTimeoutSeconds), token);
            }

            State.Set(LauncherState.Ready, "The local world is ready. Starting Asheron's Call...");
            var password = secretProtector.Unprotect(Settings.ProtectedAccountPassword);
            await clientManager.StartAsync(Settings, password, token);
            State.Set(LauncherState.GameRunning, "Asheron's Call is running.");
            return true;
        }
        catch (OperationCanceledException)
        {
            State.Set(LauncherState.Ready, "Startup canceled.");
            return false;
        }
        catch (Exception ex)
        {
            log.Write("Launch failed: " + ex);
            State.Set(LauncherState.Error, ex.Message);
            await StopInfrastructureAfterFailureAsync();
            return false;
        }
        finally
        {
            playGate.Exit();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        await StopAsync(stopManagedDatabase: true, cancellationToken);
    }

    public async Task ShutdownAsync(CancellationToken cancellationToken = default)
    {
        await StopAsync(Settings.StopManagedDatabaseWhenLauncherExits, cancellationToken);
    }

    private async Task StopAsync(bool stopManagedDatabase, CancellationToken cancellationToken)
    {
        runCancellation?.Cancel();
        try
        {
            await clientManager.StopAsync(cancellationToken);
            await serverManager.StopAsync(cancellationToken);
            if (stopManagedDatabase && databaseRuntime?.IsManaged == true)
                await databaseRuntime.StopAsync(cancellationToken);
            State.Set(LauncherState.Ready, "Stopped. Click Play when you are ready.");
        }
        catch (Exception ex)
        {
            log.Write("Stop failed: " + ex);
            State.Set(LauncherState.Error, ex.Message);
        }
    }

    private async Task HandleClientExitAsync()
    {
        try
        {
            if (Settings.StopServerWhenGameExits)
            {
                await serverManager.StopAsync(CancellationToken.None);
                if (databaseRuntime?.IsManaged == true && Settings.StopManagedDatabaseWhenLauncherExits)
                    await databaseRuntime.StopAsync(CancellationToken.None);
            }
            State.Set(LauncherState.Ready, "Game closed. Click Play to return to your characters.");
        }
        catch (Exception ex)
        {
            log.Write("Cleanup after the game closed failed: " + ex);
            State.Set(LauncherState.Error, ex.Message);
        }
    }

    private async Task StopInfrastructureAfterFailureAsync()
    {
        try
        {
            await serverManager.StopAsync(CancellationToken.None);
            if (databaseRuntime?.IsManaged == true)
                await databaseRuntime.StopAsync(CancellationToken.None);
        }
        catch (Exception stopException)
        {
            log.Write("Cleanup after failed launch also failed: " + stopException.Message);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await ShutdownAsync(CancellationToken.None);
        if (databaseRuntime is not null)
            await databaseRuntime.DisposeAsync();
        runCancellation?.Dispose();
    }
}
