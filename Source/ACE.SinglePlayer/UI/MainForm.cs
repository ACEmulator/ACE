using System.Diagnostics;

using ACE.SinglePlayer.ClientLaunch;
using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;
using ACE.SinglePlayer.Orchestration;

namespace ACE.SinglePlayer.UI;

public sealed class MainForm : Form
{
    private readonly LauncherController controller;
    private readonly SettingsStore settingsStore;
    private readonly ISecretProtector secretProtector;
    private readonly DatabaseRuntimeFactory databaseRuntimeFactory;
    private readonly DatabaseBootstrapper databaseBootstrapper;
    private readonly LauncherLog log;
    private readonly Label state = new() { AutoSize = false, Dock = DockStyle.Top, Height = 64, TextAlign = ContentAlignment.MiddleCenter, Font = new Font(SystemFonts.MessageBoxFont!.FontFamily, 14, FontStyle.Bold) };
    private readonly Button play = new() { Text = "PLAY", Width = 330, Height = 100, Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 26, FontStyle.Bold) };
    private readonly CheckBox useDecal = new() { Text = "Use Decal", AutoSize = true, Padding = new Padding(10, 38, 4, 0) };
    private readonly Button stop = new() { Text = "Stop", Width = 100, Height = 38 };
    private readonly TextBox diagnostics = new() { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Font = new Font(FontFamily.GenericMonospace, 9) };
    private readonly Panel diagnosticPanel = new() { Dock = DockStyle.Fill, Visible = false, Padding = new Padding(8) };
    private readonly ToolTip toolTip = new();
    private bool closingAllowed;
    private bool updatingDecalPreference;

    public MainForm(LauncherController controller, SettingsStore settingsStore, ISecretProtector secretProtector,
        DatabaseRuntimeFactory databaseRuntimeFactory, DatabaseBootstrapper databaseBootstrapper, LauncherLog log)
    {
        this.controller = controller;
        this.settingsStore = settingsStore;
        this.secretProtector = secretProtector;
        this.databaseRuntimeFactory = databaseRuntimeFactory;
        this.databaseBootstrapper = databaseBootstrapper;
        this.log = log;

        Text = "ACE Single Player";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(700, 520);
        Size = new Size(820, 650);

        var main = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1 };
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        main.Controls.Add(state, 0, 0);

        var playPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(150, 15, 0, 0) };
        playPanel.Controls.Add(play);
        playPanel.Controls.Add(useDecal);
        playPanel.Controls.Add(stop);
        main.Controls.Add(playPanel, 0, 1);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(15, 8, 0, 0) };
        var settings = new Button { Text = "Settings", AutoSize = true };
        var mods = new Button { Text = "Mods", AutoSize = true };
        var logs = new Button { Text = "Open Logs", AutoSize = true };
        var showDiagnostics = new CheckBox { Text = "Show diagnostics", AutoSize = true, Padding = new Padding(10, 7, 0, 0) };
        actions.Controls.AddRange(new Control[] { settings, mods, logs, showDiagnostics });
        main.Controls.Add(actions, 0, 2);

        diagnosticPanel.Controls.Add(diagnostics);
        main.Controls.Add(diagnosticPanel, 0, 3);
        Controls.Add(main);

        var decalAvailable = IsDecalAvailable();
        useDecal.Checked = controller.Settings.ClientLaunchMode == ClientLaunchMode.Decal;
        useDecal.Enabled = decalAvailable;
        useDecal.Text = decalAvailable ? "Use Decal" : "Use Decal (not detected)";
        toolTip.SetToolTip(useDecal, decalAvailable
            ? "Launch the game with your installed Decal and ThwargLauncher. Uncheck for Vanilla."
            : "Install both Decal and ThwargLauncher to enable this option.");

        controller.State.Changed += UpdateState;
        log.MessageWritten += AppendDiagnostic;
        play.Click += async (_, _) =>
        {
            play.Enabled = false;
            var started = await controller.PlayAsync();
            play.Enabled = !controller.IsClientRunning;
            if (started && controller.Settings.MinimizeLauncherAfterClientStarts)
                WindowState = FormWindowState.Minimized;
        };
        stop.Click += async (_, _) => await controller.StopAsync();
        useDecal.CheckedChanged += async (_, _) => await UpdateDecalPreferenceAsync();
        settings.Click += async (_, _) => await ShowSettingsAsync();
        mods.Click += (_, _) => new ModsForm(controller.Settings, () => controller.IsServerRunning).ShowDialog(this);
        logs.Click += (_, _) => Process.Start(new ProcessStartInfo { FileName = Path.GetDirectoryName(log.LogPath)!, UseShellExecute = true });
        showDiagnostics.CheckedChanged += (_, _) => diagnosticPanel.Visible = showDiagnostics.Checked;
        FormClosing += OnFormClosing;

        controller.State.Set(LauncherState.Ready, "Ready — click Play");
    }

    private async Task UpdateDecalPreferenceAsync()
    {
        if (updatingDecalPreference)
            return;

        var requestedMode = useDecal.Checked ? ClientLaunchMode.Decal : ClientLaunchMode.Vanilla;
        var previousMode = controller.Settings.ClientLaunchMode;
        if (requestedMode == ClientLaunchMode.Decal && !IsDecalAvailable())
        {
            SetDecalCheckbox(previousMode == ClientLaunchMode.Decal);
            MessageBox.Show(this, "Use Decal requires working Decal and ThwargLauncher installations.",
                "Decal unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        useDecal.Enabled = false;
        try
        {
            controller.Settings.ClientLaunchMode = requestedMode;
            await settingsStore.SaveAsync(controller.Settings);
            controller.UpdateSettings(controller.Settings);
        }
        catch (Exception ex)
        {
            controller.Settings.ClientLaunchMode = previousMode;
            SetDecalCheckbox(previousMode == ClientLaunchMode.Decal);
            MessageBox.Show(this, "The Decal preference could not be saved.\r\n\r\n" + ex.Message,
                "Settings error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            useDecal.Enabled = IsDecalAvailable() && !controller.IsServerRunning && !controller.IsClientRunning;
        }
    }

    private void SetDecalCheckbox(bool value)
    {
        updatingDecalPreference = true;
        try
        {
            useDecal.Checked = value;
        }
        finally
        {
            updatingDecalPreference = false;
        }
    }

    private static bool IsDecalAvailable() =>
        DecalDetector.Detect() is not null && ThwargDetector.Detect() is not null;

    private async Task ShowSettingsAsync()
    {
        if (controller.IsServerRunning)
        {
            MessageBox.Show(this, "Stop the game and local server before changing settings.", "Server is running", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using var wizard = new SetupWizardForm(controller.Settings, settingsStore, secretProtector, databaseRuntimeFactory, databaseBootstrapper);
        if (wizard.ShowDialog(this) == DialogResult.OK)
            controller.UpdateSettings(wizard.SavedSettings);
        await Task.CompletedTask;
    }

    private void UpdateState(LauncherStateChanged change)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => UpdateState(change));
            return;
        }
        state.Text = $"{ToDisplayName(change.State)}\r\n{change.Message}";
        stop.Enabled = controller.IsServerRunning || controller.IsClientRunning;
        play.Enabled = (change.State is LauncherState.Ready or LauncherState.Error or LauncherState.NotConfigured) && !controller.IsClientRunning;
        useDecal.Enabled = IsDecalAvailable() && !controller.IsServerRunning && !controller.IsClientRunning;
        if (change.State == LauncherState.Error)
            diagnosticPanel.Visible = true;
    }

    private void AppendDiagnostic(string line)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => AppendDiagnostic(line));
            return;
        }
        diagnostics.AppendText(line + Environment.NewLine);
    }

    private async void OnFormClosing(object? sender, FormClosingEventArgs args)
    {
        if (closingAllowed)
            return;
        args.Cancel = true;
        Enabled = false;
        await controller.ShutdownAsync();
        closingAllowed = true;
        Close();
    }

    private static string ToDisplayName(LauncherState value) => value switch
    {
        LauncherState.NotConfigured => "Not configured",
        LauncherState.CheckingDatabase => "Checking database",
        LauncherState.StartingDatabase => "Starting database",
        LauncherState.StartingServer => "Starting server",
        LauncherState.WaitingForWorld => "Waiting for world",
        LauncherState.GameRunning => "Game running",
        _ => value.ToString()
    };
}
