using ACE.SinglePlayer.ClientLaunch;
using ACE.SinglePlayer.Configuration;
using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;
using ACE.SinglePlayer.Networking;
using ACE.SinglePlayer.Orchestration;
using ACE.SinglePlayer.Processes;
using ACE.SinglePlayer.UI;

namespace ACE.SinglePlayer;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        try
        {
            RunApplication();
        }
        catch (Exception ex)
        {
            WriteStartupFailure(ex);
            MessageBox.Show(
                "OpenDereth could not start. Details were written to the launcher log.\r\n\r\n" + ex.Message,
                "OpenDereth startup error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static void RunApplication()
    {
        using var instance = new SingleInstance(@"Local\ACE.SinglePlayer");
        if (!instance.IsPrimary)
        {
            MessageBox.Show("OpenDereth is already open.", "OpenDereth", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var migratedLegacyData = ApplicationPaths.MigrateLegacyLocalData();
        var localRoot = ApplicationPaths.LocalRoot;
        using var log = new LauncherLog(Path.Combine(localRoot, "Logs"));
        if (migratedLegacyData)
            log.Write($"Migrated local data from '{ApplicationPaths.LegacyLocalRoot}' to '{ApplicationPaths.LocalRoot}'.");
        var store = new SettingsStore();
        var protector = new SecretProtector();
        var connectionFactory = new DatabaseConnectionFactory(protector);
        var runtimeFactory = new DatabaseRuntimeFactory(connectionFactory, log);
        var bootstrapper = new DatabaseBootstrapper(connectionFactory, log);

        LauncherSettings settings;
        try
        {
            settings = store.LoadAsync().GetAwaiter().GetResult() ?? LauncherSettings.CreateDefaults(AppContext.BaseDirectory);
        }
        catch (Exception ex)
        {
            log.Write("Settings could not be loaded: " + ex);
            MessageBox.Show("The saved settings could not be read. Setup will open so they can be replaced.\r\n\r\n" + ex.Message,
                "Settings error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            settings = LauncherSettings.CreateDefaults(AppContext.BaseDirectory);
        }

        var portableBundle = BundledDistribution.IsComplete(AppContext.BaseDirectory);
        var settingsChanged = SettingsPathRepairer.Repair(settings, AppContext.BaseDirectory);
        if (portableBundle && settings.DatabaseMode != DatabaseMode.External)
            settingsChanged |= AutomaticSetupConfigurator.Configure(settings, AppContext.BaseDirectory, protector);
        else if (settings.DatabaseMode != DatabaseMode.External)
            settings.ManagedDatabaseExePath = MariaDbInstallationLocator.FindServerExecutable(settings.ManagedDatabaseExePath)
                ?? settings.ManagedDatabaseExePath;

        if (settingsChanged)
        {
            log.Write("Automatically configured the bundled server, world, database, or client paths for this installation.");
            if (store.Exists)
                store.SaveAsync(settings).GetAwaiter().GetResult();
        }

        if (portableBundle && settings.DatabaseMode != DatabaseMode.External && !SetupValidator.ValidateClient(settings).IsValid)
        {
            using var quickSetup = new QuickSetupForm(settings);
            if (quickSetup.ShowDialog() != DialogResult.OK)
                return;
            AutomaticSetupConfigurator.Configure(settings, AppContext.BaseDirectory, protector);
            store.SaveAsync(settings).GetAwaiter().GetResult();
        }
        else if ((!portableBundle || settings.DatabaseMode == DatabaseMode.External) &&
                 (!store.Exists || !SetupValidator.Validate(settings).IsValid))
        {
            using var wizard = new SetupWizardForm(settings, store, protector, runtimeFactory, bootstrapper);
            if (wizard.ShowDialog() != DialogResult.OK)
                return;
            settings = wizard.SavedSettings;
        }

        var validation = SetupValidator.Validate(settings);
        if (!validation.IsValid)
        {
            MessageBox.Show(validation.Message, "OpenDereth needs attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!store.Exists)
            store.SaveAsync(settings).GetAwaiter().GetResult();

        using var serverManager = new AceServerProcessManager(log);
        using var clientManager = new ClientProcessManager(new IClientLaunchProvider[]
        {
            new DirectClientLaunchProvider(),
            new DecalClientLaunchProvider(log)
        }, log);
        var controller = new LauncherController(settings, new AceConfigurationWriter(protector), runtimeFactory,
            bootstrapper, store, serverManager, clientManager, new ReadyFileMonitor(new UdpPortProbe()), protector, log);
        using var main = new MainForm(controller, store, protector, runtimeFactory, bootstrapper, log);
        Application.Run(main);
        controller.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    private static void WriteStartupFailure(Exception exception)
    {
        try
        {
            var failureRoot = Directory.Exists(ApplicationPaths.LegacyLocalRoot) && !Directory.Exists(ApplicationPaths.LocalRoot)
                ? ApplicationPaths.LegacyLocalRoot
                : ApplicationPaths.LocalRoot;
            var logDirectory = Path.Combine(failureRoot, "Logs");
            Directory.CreateDirectory(logDirectory);
            File.AppendAllText(Path.Combine(logDirectory, "OpenDereth.log"),
                $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss zzz} Startup failed: {exception}{Environment.NewLine}");
        }
        catch
        {
            // Preserve the original startup error even if logging is unavailable.
        }
    }
}
