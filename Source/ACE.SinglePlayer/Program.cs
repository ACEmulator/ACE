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
                "ACE Single Player could not start. Details were written to the launcher log.\r\n\r\n" + ex.Message,
                "ACE Single Player startup error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static void RunApplication()
    {
        using var instance = new SingleInstance(@"Local\ACE.SinglePlayer");
        if (!instance.IsPrimary)
        {
            MessageBox.Show("ACE Single Player is already open.", "ACE Single Player", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var localRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ACESinglePlayer");
        using var log = new LauncherLog(Path.Combine(localRoot, "Logs"));
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

        if (settings.DatabaseMode != DatabaseMode.External)
            settings.ManagedDatabaseExePath = MariaDbInstallationLocator.FindServerExecutable(settings.ManagedDatabaseExePath)
                ?? settings.ManagedDatabaseExePath;

        if (SettingsPathRepairer.Repair(settings, AppContext.BaseDirectory))
        {
            log.Write("Automatically repaired the client/DAT or packaged server paths for this installation.");
            if (store.Exists)
                store.SaveAsync(settings).GetAwaiter().GetResult();
        }

        if (!store.Exists || !SetupValidator.Validate(settings).IsValid)
        {
            using var wizard = new SetupWizardForm(settings, store, protector, runtimeFactory, bootstrapper);
            if (wizard.ShowDialog() != DialogResult.OK)
                return;
            settings = wizard.SavedSettings;
        }

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
            var logDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ACESinglePlayer", "Logs");
            Directory.CreateDirectory(logDirectory);
            File.AppendAllText(Path.Combine(logDirectory, "ACE.SinglePlayer.log"),
                $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss zzz} Startup failed: {exception}{Environment.NewLine}");
        }
        catch
        {
            // Preserve the original startup error even if logging is unavailable.
        }
    }
}
