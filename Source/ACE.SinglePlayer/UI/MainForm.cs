using System.Diagnostics;

using ACE.SinglePlayer.ClientLaunch;
using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;
using ACE.SinglePlayer.Orchestration;

namespace ACE.SinglePlayer.UI;

public sealed class MainForm : Form
{
    private static readonly Color Night = Color.FromArgb(14, 25, 32);
    private static readonly Color DeepSlate = Color.FromArgb(26, 43, 52);
    private static readonly Color WeatheredGold = Color.FromArgb(205, 160, 82);
    private static readonly Color PaleGold = Color.FromArgb(241, 220, 170);
    private static readonly Color Mist = Color.FromArgb(222, 231, 226);
    private readonly LauncherController controller;
    private readonly SettingsStore settingsStore;
    private readonly ISecretProtector secretProtector;
    private readonly DatabaseRuntimeFactory databaseRuntimeFactory;
    private readonly DatabaseBootstrapper databaseBootstrapper;
    private readonly LauncherLog log;
    private readonly Label title = new() { Text = "A C E   S I N G L E   P L A Y E R", AutoSize = false, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
    private readonly Label state = new() { AutoSize = false, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
    private readonly Button play = new() { Text = "PLAY", Width = 320, Height = 96 };
    private readonly CheckBox useDecal = new()
    {
        AutoSize = true,
        Margin = new Padding(3, 5, 3, 5),
        AccessibleName = "Use Decal"
    };
    private readonly Label useDecalLabel = new()
    {
        Text = "Use Decal",
        AutoSize = true,
        Margin = new Padding(2, 4, 4, 4),
        Cursor = Cursors.Hand
    };
    private readonly Button stop = new() { Text = "Stop", Width = 116, Height = 36, Margin = new Padding(4, 0, 4, 4) };
    private readonly TextBox diagnostics = new() { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Font = new Font(FontFamily.GenericMonospace, 9), BorderStyle = BorderStyle.None };
    private readonly Panel diagnosticPanel = new() { Dock = DockStyle.Fill, Visible = false, Padding = new Padding(12), BorderStyle = BorderStyle.FixedSingle };
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

        Text = "ACE Single Player - A Private World in Dereth";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(780, 620);
        Size = new Size(900, 710);
        BackColor = Night;
        ForeColor = Mist;
        DoubleBuffered = true;
        BackgroundImage = LoadLauncherBackground();
        BackgroundImageLayout = ImageLayout.Zoom;

        title.Font = new Font("Georgia", 21, FontStyle.Bold);
        title.ForeColor = PaleGold;
        title.BackColor = Color.FromArgb(188, Night);
        state.Font = new Font("Georgia", 12, FontStyle.Bold);
        state.ForeColor = PaleGold;
        state.BackColor = Color.FromArgb(188, Night);

        play.Font = new Font("Georgia", 26, FontStyle.Bold);
        play.ForeColor = PaleGold;
        play.BackColor = Color.FromArgb(186, 103, 45);
        play.FlatStyle = FlatStyle.Flat;
        play.FlatAppearance.BorderColor = WeatheredGold;
        play.FlatAppearance.BorderSize = 2;
        play.FlatAppearance.MouseOverBackColor = Color.FromArgb(205, 126, 55);
        play.FlatAppearance.MouseDownBackColor = Color.FromArgb(151, 80, 37);

        useDecal.BackColor = Color.Transparent;
        useDecalLabel.ForeColor = PaleGold;
        useDecalLabel.BackColor = Color.Transparent;
        useDecalLabel.Font = new Font(SystemFonts.MessageBoxFont!.FontFamily, 10, FontStyle.Bold);
        StyleSecondaryButton(stop);

        diagnostics.BackColor = Color.FromArgb(9, 17, 22);
        diagnostics.ForeColor = Color.FromArgb(190, 214, 204);
        diagnosticPanel.BackColor = Color.FromArgb(225, Night);

        var main = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 5, ColumnCount = 1, BackColor = Color.Transparent, Padding = new Padding(22, 18, 22, 18) };
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 67));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        main.Controls.Add(title, 0, 0);
        main.Controls.Add(state, 0, 1);

        var playHost = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, BackColor = Color.Transparent };
        playHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        playHost.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        playHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        var playPanel = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, BackColor = Color.Transparent, Padding = new Padding(0, 20, 0, 0) };
        playPanel.Controls.Add(play);
        var playOptions = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = DeepSlate,
            Padding = new Padding(10),
            Margin = new Padding(12, 4, 0, 0)
        };
        var decalOption = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            BackColor = Color.Transparent,
            Margin = new Padding(3, 3, 3, 8)
        };
        decalOption.Controls.Add(useDecal);
        decalOption.Controls.Add(useDecalLabel);
        playOptions.Controls.Add(decalOption);
        playOptions.Controls.Add(stop);
        playPanel.Controls.Add(playOptions);
        playHost.Controls.Add(playPanel, 1, 0);
        main.Controls.Add(playHost, 0, 2);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, BackColor = Color.FromArgb(216, Night), Padding = new Padding(12, 9, 0, 0) };
        var settings = new Button { Text = "Settings", AutoSize = true };
        var mods = new Button { Text = "Server Mods", AutoSize = true };
        var customWeenies = new Button { Text = "Custom Weenies", AutoSize = true };
        var logs = new Button { Text = "Open Logs", AutoSize = true };
        var showDiagnostics = new CheckBox { Text = "Show diagnostics", AutoSize = true, Padding = new Padding(10, 7, 0, 0) };
        StyleSecondaryButton(settings);
        StyleSecondaryButton(mods);
        StyleSecondaryButton(customWeenies);
        StyleSecondaryButton(logs);
        showDiagnostics.ForeColor = Mist;
        showDiagnostics.BackColor = Color.Transparent;
        actions.Controls.AddRange(new Control[] { settings, mods, customWeenies, logs, showDiagnostics });
        main.Controls.Add(actions, 0, 3);

        diagnosticPanel.Controls.Add(diagnostics);
        main.Controls.Add(diagnosticPanel, 0, 4);
        Controls.Add(main);

        var decalAvailable = IsDecalAvailable();
        useDecal.Checked = controller.Settings.ClientLaunchMode == ClientLaunchMode.Decal;
        useDecal.Enabled = decalAvailable;
        useDecalLabel.Text = decalAvailable ? "Use Decal" : "Use Decal (not detected)";
        var decalToolTip = decalAvailable
            ? "Launch the game with your installed Decal and ThwargLauncher. Uncheck for Vanilla."
            : "Install both Decal and ThwargLauncher to enable this option.";
        toolTip.SetToolTip(useDecal, decalToolTip);
        toolTip.SetToolTip(useDecalLabel, decalToolTip);

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
        useDecalLabel.Click += (_, _) =>
        {
            if (useDecal.Enabled)
                useDecal.Checked = !useDecal.Checked;
        };
        settings.Click += async (_, _) => await ShowSettingsAsync();
        mods.Click += (_, _) => new ModsForm(controller.Settings, () => controller.IsServerRunning).ShowDialog(this);
        customWeenies.Click += (_, _) => new CustomWeeniesForm(controller.Settings,
            () => controller.IsServerRunning, databaseRuntimeFactory,
            new DatabaseConnectionFactory(secretProtector), log).ShowDialog(this);
        logs.Click += (_, _) => Process.Start(new ProcessStartInfo { FileName = Path.GetDirectoryName(log.LogPath)!, UseShellExecute = true });
        showDiagnostics.CheckedChanged += (_, _) => diagnosticPanel.Visible = showDiagnostics.Checked;
        FormClosing += OnFormClosing;
        Disposed += (_, _) =>
        {
            BackgroundImage?.Dispose();
            toolTip.Dispose();
        };

        controller.State.Set(LauncherState.Ready, "Ready - click Play");
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
        state.ForeColor = change.State switch
        {
            LauncherState.Error => Color.FromArgb(255, 153, 125),
            LauncherState.GameRunning => Color.FromArgb(151, 222, 178),
            LauncherState.Ready => PaleGold,
            _ => Color.FromArgb(151, 201, 230)
        };
        stop.Enabled = controller.IsServerRunning || controller.IsClientRunning;
        play.Enabled = (change.State is LauncherState.Ready or LauncherState.Error or LauncherState.NotConfigured) && !controller.IsClientRunning;
        useDecal.Enabled = IsDecalAvailable() && !controller.IsServerRunning && !controller.IsClientRunning;
        if (change.State == LauncherState.Error)
            diagnosticPanel.Visible = true;
    }

    private static void StyleSecondaryButton(Button button)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderColor = Color.FromArgb(126, 146, 145);
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 77, 84);
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(33, 51, 59);
        button.BackColor = DeepSlate;
        button.ForeColor = Mist;
    }

    private static Image? LoadLauncherBackground()
    {
        using var stream = typeof(MainForm).Assembly.GetManifestResourceStream(
            "ACE.SinglePlayer.Assets.dereth-launcher-background.png");
        return stream is null ? null : new Bitmap(stream);
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
        LauncherState.PreparingData => "Preparing game data",
        LauncherState.CheckingDatabase => "Checking database",
        LauncherState.StartingDatabase => "Starting database",
        LauncherState.StartingServer => "Starting server",
        LauncherState.WaitingForWorld => "Waiting for world",
        LauncherState.GameRunning => "Game running",
        _ => value.ToString()
    };
}
