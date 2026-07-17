using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.UI;

public sealed class SetupWizardForm : Form
{
    private readonly SettingsStore settingsStore;
    private readonly ISecretProtector secretProtector;
    private readonly DatabaseRuntimeFactory databaseRuntimeFactory;
    private readonly DatabaseBootstrapper databaseBootstrapper;
    private readonly TabControl pages = new() { Dock = DockStyle.Fill, Appearance = TabAppearance.FlatButtons, ItemSize = new Size(0, 1), SizeMode = TabSizeMode.Fixed };
    private readonly TextBox clientPath = new();
    private readonly TextBox datPath = new();
    private readonly TextBox serverPath = new();
    private readonly TextBox modsPath = new();
    private readonly TextBox runtimePath = new();
    private readonly ComboBox databaseMode = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox databaseHost = new();
    private readonly NumericUpDown databasePort = new() { Minimum = 1, Maximum = 65535 };
    private readonly TextBox databaseUser = new();
    private readonly TextBox databasePassword = new() { UseSystemPasswordChar = true };
    private readonly TextBox authDatabase = new();
    private readonly TextBox shardDatabase = new();
    private readonly TextBox worldDatabase = new();
    private readonly TextBox managedDatabaseExe = new();
    private readonly TextBox worldSql = new();
    private readonly TextBox accountName = new();
    private readonly NumericUpDown serverPort = new() { Minimum = 1, Maximum = 65534 };
    private readonly NumericUpDown startupTimeout = new() { Minimum = 30, Maximum = 900, Increment = 30 };
    private readonly CheckBox stopWithGame = new() { Text = "Stop the local server when the game exits", AutoSize = true };
    private readonly CheckBox stopManagedDatabase = new() { Text = "Stop launcher-managed MariaDB when this launcher exits", AutoSize = true };
    private readonly CheckBox minimize = new() { Text = "Minimize this launcher after the client starts", AutoSize = true };
    private readonly Label pageTitle = new() { AutoSize = true, Font = new Font(SystemFonts.MessageBoxFont!.FontFamily, 15, FontStyle.Bold) };
    private readonly Label databaseStatus = new() { AutoSize = true, MaximumSize = new Size(620, 0) };
    private readonly Label privateDatabaseNote = new()
    {
        Text = "Recommended: the launcher uses its bundled MariaDB and ACE World files to create an isolated database in local Windows app data. Credentials are generated automatically and the database binds only to this PC.",
        AutoSize = true,
        MaximumSize = new Size(650, 0)
    };
    private readonly Button testDatabase = new() { Text = "Check Database", AutoSize = true };
    private readonly Button initializeDatabase = new() { Text = "Prepare Private Database", AutoSize = true };
    private readonly Button back = new() { Text = "Back", AutoSize = true };
    private readonly Button next = new() { Text = "Next", AutoSize = true };
    private readonly Button finish = new() { Text = "Save Setup", AutoSize = true };
    private readonly List<Control> externalDatabaseControls = new();
    private readonly List<Control> privateDatabaseControls = new();
    private LauncherSettings settings;

    public SetupWizardForm(LauncherSettings settings, SettingsStore settingsStore, ISecretProtector secretProtector,
        DatabaseRuntimeFactory databaseRuntimeFactory, DatabaseBootstrapper databaseBootstrapper)
    {
        this.settings = settings;
        this.settingsStore = settingsStore;
        this.secretProtector = secretProtector;
        this.databaseRuntimeFactory = databaseRuntimeFactory;
        this.databaseBootstrapper = databaseBootstrapper;

        Text = settingsStore.Exists ? "OpenDereth Settings" : "OpenDereth Setup";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(760, 590);
        Size = new Size(820, 650);
        FormBorderStyle = FormBorderStyle.Sizable;

        databaseMode.Items.AddRange(new object[] { "Automatic private database (recommended)", "Existing MariaDB/MySQL (advanced)" });

        pages.TabPages.Add(CreateClientPage());
        pages.TabPages.Add(CreateServerPage());
        pages.TabPages.Add(CreateDatabasePage());
        pages.TabPages.Add(CreatePlayPage());

        var header = new Panel { Dock = DockStyle.Top, Height = 62, Padding = new Padding(18, 14, 18, 8) };
        header.Controls.Add(pageTitle);
        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 55,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(12),
            WrapContents = false
        };
        buttons.Controls.Add(finish);
        buttons.Controls.Add(next);
        buttons.Controls.Add(back);
        Controls.Add(pages);
        Controls.Add(buttons);
        Controls.Add(header);

        back.Click += (_, _) => { if (pages.SelectedIndex > 0) pages.SelectedIndex--; UpdateNavigation(); };
        next.Click += (_, _) => { if (pages.SelectedIndex < pages.TabCount - 1) pages.SelectedIndex++; UpdateNavigation(); };
        finish.Click += async (_, _) => await SaveAsync();
        pages.SelectedIndexChanged += (_, _) => UpdateNavigation();
        databaseMode.SelectedIndexChanged += (_, _) => UpdateDatabaseModeControls();

        LoadSettings();
        pages.SelectedIndex = 0;
        UpdateNavigation();
    }

    public LauncherSettings SavedSettings => settings;

    private TabPage CreateClientPage()
    {
        var page = NewPage();
        var layout = NewFields();
        AddPathRow(layout, "acclient.exe", clientPath, "Select acclient.exe", () => BrowseFile(clientPath, "acclient.exe|acclient.exe"), 0);
        datPath.ReadOnly = true;
        AddPathRow(layout, "DAT files (automatic)", datPath, "Automatically uses the DAT files beside acclient.exe", () => BrowseFolder(datPath), 1);
        var note = new Label
        {
            Text = "Select acclient.exe from a complete, writable AC client folder. The client requires client_cell_1.dat, client_portal.dat, client_local_English.dat, and client_highres.dat beside acclient.exe. The launcher does not download or redistribute these proprietary files.",
            AutoSize = true,
            MaximumSize = new Size(650, 0)
        };
        layout.Controls.Add(note, 0, 2);
        layout.SetColumnSpan(note, 3);
        page.Controls.Add(layout);
        return page;
    }

    private TabPage CreateServerPage()
    {
        var page = NewPage();
        var layout = NewFields();
        AddPathRow(layout, "ACE.Server.exe", serverPath, "Select the published server executable", () => BrowseFile(serverPath, "ACE.Server|ACE.Server.exe"), 0);
        AddPathRow(layout, "Mods directory", modsPath, "Select the ACE server-mod directory", () => BrowseFolder(modsPath), 1);
        AddPathRow(layout, "Runtime directory", runtimePath, "Select the private runtime-data directory", () => BrowseFolder(runtimePath), 2);
        var note = new Label
        {
            Text = "The dedicated single-player Config.js, ready file, process record, and optional managed database data live under Runtime. The server always binds to 127.0.0.1.",
            AutoSize = true,
            MaximumSize = new Size(650, 0)
        };
        layout.Controls.Add(note, 0, 3);
        layout.SetColumnSpan(note, 3);
        page.Controls.Add(layout);
        return page;
    }

    private TabPage CreateDatabasePage()
    {
        var page = NewPage();
        var layout = NewFields();
        AddRow(layout, "Database mode", databaseMode, 0);
        layout.Controls.Add(privateDatabaseNote, 0, 1);
        layout.SetColumnSpan(privateDatabaseNote, 3);
        privateDatabaseControls.Add(privateDatabaseNote);

        TrackExternal(AddRow(layout, "Host", databaseHost, 2), databaseHost);
        TrackExternal(AddRow(layout, "Port", databasePort, 3), databasePort);
        TrackExternal(AddRow(layout, "Username", databaseUser, 4), databaseUser);
        TrackExternal(AddRow(layout, "Password", databasePassword, 5), databasePassword);
        TrackExternal(AddRow(layout, "Authentication DB", authDatabase, 6), authDatabase);
        TrackExternal(AddRow(layout, "Shard DB", shardDatabase, 7), shardDatabase);
        TrackExternal(AddRow(layout, "World DB", worldDatabase, 8), worldDatabase);
        var mariaDbPathControls = AddPathRow(layout, "MariaDB program", managedDatabaseExe,
            "Automatically detected; browse to mariadbd.exe only when needed",
            () => BrowseFile(managedDatabaseExe, "MariaDB server|mariadbd.exe"), 9);
        privateDatabaseControls.AddRange(new Control[] { mariaDbPathControls.Label, managedDatabaseExe, mariaDbPathControls.Button });
        AddPathRow(layout, "World SQL package", worldSql,
            "Bundled automatically; browse only to use an advanced replacement world",
            () => BrowseFile(worldSql, "SQL files|*.sql"), 10);

        var actions = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        actions.Controls.Add(testDatabase);
        actions.Controls.Add(initializeDatabase);
        layout.Controls.Add(actions, 1, 11);
        layout.SetColumnSpan(actions, 2);
        layout.Controls.Add(databaseStatus, 1, 12);
        layout.SetColumnSpan(databaseStatus, 2);
        testDatabase.Click += async (_, _) => await TestDatabaseAsync(testDatabase);
        initializeDatabase.Click += async (_, _) => await InitializeDatabaseAsync(initializeDatabase);
        page.Controls.Add(layout);
        return page;
    }

    private TabPage CreatePlayPage()
    {
        var page = NewPage();
        var layout = NewFields();
        AddRow(layout, "Persistent account", accountName, 0);
        AddRow(layout, "Local server port", serverPort, 1);
        AddRow(layout, "Startup timeout (seconds)", startupTimeout, 2);
        layout.Controls.Add(stopWithGame, 1, 3);
        layout.SetColumnSpan(stopWithGame, 2);
        layout.Controls.Add(stopManagedDatabase, 1, 4);
        layout.SetColumnSpan(stopManagedDatabase, 2);
        layout.Controls.Add(minimize, 1, 5);
        layout.SetColumnSpan(minimize, 2);
        var note = new Label
        {
            Text = "A strong account password is generated once and protected for your Windows user. The same account is reused on every launch, so its characters remain in the shard database.",
            AutoSize = true,
            MaximumSize = new Size(650, 0)
        };
        layout.Controls.Add(note, 0, 6);
        layout.SetColumnSpan(note, 3);
        page.Controls.Add(layout);
        return page;
    }

    private void LoadSettings()
    {
        SettingsPathRepairer.Repair(settings, AppContext.BaseDirectory);
        clientPath.Text = settings.ClientExePath;
        datPath.Text = settings.DatFilesDirectory;
        serverPath.Text = settings.ServerExePath;
        modsPath.Text = settings.ModsDirectory;
        runtimePath.Text = settings.RuntimeDirectory;
        if (settings.DatabaseMode != DatabaseMode.External)
            settings.ManagedDatabaseExePath = MariaDbInstallationLocator.FindServerExecutable(settings.ManagedDatabaseExePath)
                ?? settings.ManagedDatabaseExePath;
        databaseMode.SelectedIndex = settings.DatabaseMode == DatabaseMode.External ? 1 : 0;
        databaseHost.Text = settings.DatabaseHost;
        databasePort.Value = settings.DatabasePort;
        databaseUser.Text = settings.DatabaseUsername;
        try
        {
            databasePassword.Text = settings.DatabaseMode != DatabaseMode.External || string.IsNullOrEmpty(settings.ProtectedDatabasePassword)
                ? string.Empty
                : secretProtector.Unprotect(settings.ProtectedDatabasePassword);
        }
        catch (Exception ex) when (ex is FormatException or System.ComponentModel.Win32Exception)
        {
            databasePassword.Text = string.Empty;
            databaseStatus.Text = "The saved database password could not be decrypted. Enter it again.";
        }
        authDatabase.Text = settings.AuthenticationDatabaseName;
        shardDatabase.Text = settings.ShardDatabaseName;
        worldDatabase.Text = settings.WorldDatabaseName;
        managedDatabaseExe.Text = settings.ManagedDatabaseExePath;
        worldSql.Text = settings.WorldDatabaseSqlPath;
        accountName.Text = settings.AccountName;
        serverPort.Value = settings.Port;
        startupTimeout.Value = Math.Clamp(settings.ServerStartupTimeoutSeconds, (int)startupTimeout.Minimum, (int)startupTimeout.Maximum);
        stopWithGame.Checked = settings.StopServerWhenGameExits;
        stopManagedDatabase.Checked = settings.StopManagedDatabaseWhenLauncherExits;
        minimize.Checked = settings.MinimizeLauncherAfterClientStarts;
        UpdateDatabaseModeControls();
    }

    private LauncherSettings BuildSettings()
    {
        var result = settings;
        result.ClientExePath = clientPath.Text.Trim();
        result.DatFilesDirectory = SetupValidator.DetectDatDirectory(result.ClientExePath) ?? datPath.Text.Trim();
        result.ServerExePath = serverPath.Text.Trim();
        result.ModsDirectory = modsPath.Text.Trim();
        result.RuntimeDirectory = runtimePath.Text.Trim();
        result.Host = "127.0.0.1";
        result.Port = (ushort)serverPort.Value;
        result.ServerStartupTimeoutSeconds = (int)startupTimeout.Value;
        result.AccountName = accountName.Text.Trim();
        if (string.IsNullOrWhiteSpace(result.ProtectedAccountPassword))
            result.ProtectedAccountPassword = secretProtector.Protect(SecretProtector.GeneratePassword());
        var previousDatabaseMode = result.DatabaseMode;
        var usePrivateDatabase = databaseMode.SelectedIndex != 1;
        if (usePrivateDatabase)
        {
            if (previousDatabaseMode == DatabaseMode.External)
            {
                result.ProtectedExternalDatabasePassword = result.ProtectedDatabasePassword;
                result.ProtectedDatabasePassword = result.ProtectedPrivateDatabasePassword;
            }

            result.DatabaseMode = DatabaseMode.Private;
            result.DatabaseHost = "127.0.0.1";
            if (previousDatabaseMode == DatabaseMode.External || result.DatabasePort == 3306)
                result.DatabasePort = PrivateDatabasePortFinder.FindAvailablePort();
            result.DatabaseUsername = "ace_singleplayer";
            var privateDatabaseExists = Directory.Exists(Path.Combine(result.PrivateDatabaseDirectory, "mysql"));
            if (string.IsNullOrWhiteSpace(result.ProtectedDatabasePassword))
                result.ProtectedDatabasePassword = secretProtector.Protect(SecretProtector.GeneratePassword());
            if (string.IsNullOrWhiteSpace(result.ProtectedPrivateDatabaseAdminPassword) && !privateDatabaseExists)
                result.ProtectedPrivateDatabaseAdminPassword = secretProtector.Protect(SecretProtector.GeneratePassword());
            result.ProtectedPrivateDatabasePassword = result.ProtectedDatabasePassword;
            result.ManagedDatabaseExePath = MariaDbInstallationLocator.FindServerExecutable(managedDatabaseExe.Text.Trim())
                ?? managedDatabaseExe.Text.Trim();
        }
        else
        {
            if (previousDatabaseMode != DatabaseMode.External)
                result.ProtectedPrivateDatabasePassword = result.ProtectedDatabasePassword;
            result.DatabaseMode = DatabaseMode.External;
            result.DatabaseHost = databaseHost.Text.Trim();
            result.DatabasePort = (ushort)databasePort.Value;
            result.DatabaseUsername = databaseUser.Text.Trim();
            result.ProtectedDatabasePassword = secretProtector.Protect(databasePassword.Text);
            result.ProtectedExternalDatabasePassword = result.ProtectedDatabasePassword;
            result.ManagedDatabaseExePath = managedDatabaseExe.Text.Trim();
        }
        result.AuthenticationDatabaseName = authDatabase.Text.Trim();
        result.ShardDatabaseName = shardDatabase.Text.Trim();
        result.WorldDatabaseName = worldDatabase.Text.Trim();
        result.WorldDatabaseSqlPath = worldSql.Text.Trim();
        result.StopServerWhenGameExits = stopWithGame.Checked;
        result.StopManagedDatabaseWhenLauncherExits = stopManagedDatabase.Checked;
        result.MinimizeLauncherAfterClientStarts = minimize.Checked;
        return result;
    }

    private async Task SaveAsync()
    {
        settings = BuildSettings();
        var validation = SetupValidator.Validate(settings);
        if (!validation.IsValid)
        {
            MessageBox.Show(this, validation.Message, "Setup needs attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        finish.Enabled = false;
        try
        {
            Directory.CreateDirectory(settings.RuntimeDirectory);
            Directory.CreateDirectory(settings.ModsDirectory);
            if (settings.DatabaseMode != DatabaseMode.External)
            {
                pages.SelectedIndex = 2;
                databaseStatus.Text = "Preparing the private database. The first world import can take several minutes...";
                await PrepareDatabaseAsync(settings, CancellationToken.None);
            }

            await settingsStore.SaveAsync(settings);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            databaseStatus.Text = ex.Message;
            MessageBox.Show(this, ex.Message, "Database setup needs attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            finish.Enabled = true;
        }
    }

    private async Task TestDatabaseAsync(Button button)
    {
        button.Enabled = false;
        databaseStatus.Text = "Checking...";
        try
        {
            var current = BuildSettings();
            await using var runtime = databaseRuntimeFactory.Create(current);
            await runtime.StartAsync(current, CancellationToken.None);
            var result = await runtime.ValidateAsync(current, CancellationToken.None);
            databaseStatus.Text = result.Message;
            await runtime.StopAsync(CancellationToken.None);
        }
        catch (Exception ex)
        {
            databaseStatus.Text = ex.Message;
        }
        finally
        {
            button.Enabled = true;
        }
    }

    private async Task InitializeDatabaseAsync(Button button)
    {
        button.Enabled = false;
        databaseStatus.Text = databaseMode.SelectedIndex == 0
            ? "Preparing the private database. The first world import can take several minutes..."
            : "Initializing only databases that are completely missing...";
        var current = BuildSettings();
        try
        {
            databaseStatus.Text = await PrepareDatabaseAsync(current, CancellationToken.None);
        }
        catch (Exception ex)
        {
            databaseStatus.Text = ex.Message;
        }
        finally
        {
            button.Enabled = true;
        }
    }

    private async Task<string> PrepareDatabaseAsync(LauncherSettings current, CancellationToken cancellationToken)
    {
        await using var runtime = databaseRuntimeFactory.Create(current);
        try
        {
            if (runtime.IsManaged)
                await runtime.StartAsync(current, cancellationToken);
            await databaseBootstrapper.BootstrapAsync(current, current.WorldDatabaseSqlPath, cancellationToken);
            var result = await runtime.ValidateAsync(current, cancellationToken);
            if (!result.IsValid)
                throw new InvalidOperationException(result.Message);
            return result.Message;
        }
        finally
        {
            await runtime.StopAsync(CancellationToken.None);
        }
    }

    private void UpdateDatabaseModeControls()
    {
        var usePrivateDatabase = databaseMode.SelectedIndex != 1;
        foreach (var control in externalDatabaseControls)
            control.Visible = !usePrivateDatabase;
        foreach (var control in privateDatabaseControls)
            control.Visible = usePrivateDatabase;

        testDatabase.Text = usePrivateDatabase ? "Check Private Database" : "Test Connection";
        initializeDatabase.Text = usePrivateDatabase ? "Prepare Private Database" : "Initialize Missing Databases";
        stopManagedDatabase.Visible = usePrivateDatabase;
    }

    private void TrackExternal(Label label, Control field)
    {
        externalDatabaseControls.Add(label);
        externalDatabaseControls.Add(field);
    }

    private void BrowseFile(TextBox target, string filter)
    {
        using var dialog = new OpenFileDialog { Filter = filter, CheckFileExists = true, FileName = target.Text };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;
        target.Text = dialog.FileName;
        if (target == clientPath && SetupValidator.DetectDatDirectory(dialog.FileName) is { } detected)
            datPath.Text = detected;
    }

    private void BrowseFolder(TextBox target)
    {
        using var dialog = new FolderBrowserDialog { SelectedPath = target.Text, ShowNewFolderButton = true };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            target.Text = dialog.SelectedPath;
    }

    private void UpdateNavigation()
    {
        var titles = new[] { "Choose your Asheron's Call client", "Choose the local ACE server", "Connect persistent storage", "One-click Play settings" };
        var selectedIndex = pages.SelectedIndex;
        if (selectedIndex < 0 || selectedIndex >= Math.Min(titles.Length, pages.TabCount))
            selectedIndex = 0;

        pageTitle.Text = titles[selectedIndex];
        back.Enabled = selectedIndex > 0;
        next.Visible = selectedIndex < pages.TabCount - 1;
        finish.Visible = selectedIndex == pages.TabCount - 1;
    }

    private static TabPage NewPage() => new() { Padding = new Padding(20) };

    private static TableLayoutPanel NewFields() => new()
    {
        Dock = DockStyle.Fill,
        AutoScroll = true,
        ColumnCount = 3,
        RowCount = 14,
        Padding = new Padding(5),
        ColumnStyles =
        {
            new ColumnStyle(SizeType.Absolute, 160),
            new ColumnStyle(SizeType.Percent, 100),
            new ColumnStyle(SizeType.Absolute, 105)
        }
    };

    private static Label AddRow(TableLayoutPanel layout, string label, Control control, int row)
    {
        control.Dock = DockStyle.Top;
        var labelControl = new Label { Text = label, AutoSize = true, Padding = new Padding(0, 5, 0, 0) };
        layout.Controls.Add(labelControl, 0, row);
        layout.Controls.Add(control, 1, row);
        layout.SetColumnSpan(control, 2);
        return labelControl;
    }

    private static (Label Label, Button Button) AddPathRow(TableLayoutPanel layout, string label, TextBox textBox, string accessibleDescription, Action browse, int row)
    {
        textBox.Dock = DockStyle.Top;
        textBox.AccessibleDescription = accessibleDescription;
        var button = new Button { Text = "Browse...", AutoSize = true, Dock = DockStyle.Top };
        button.Click += (_, _) => browse();
        var labelControl = new Label { Text = label, AutoSize = true, Padding = new Padding(0, 5, 0, 0) };
        layout.Controls.Add(labelControl, 0, row);
        layout.Controls.Add(textBox, 1, row);
        layout.Controls.Add(button, 2, row);
        return (labelControl, button);
    }
}
