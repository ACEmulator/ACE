using System.ComponentModel;
using System.Diagnostics;

using ACE.SinglePlayer.Models;
using ACE.SinglePlayer.Mods;

namespace ACE.SinglePlayer.UI;

public sealed class ModsForm : Form
{
    private static readonly Color Night = Color.FromArgb(14, 25, 32);
    private static readonly Color DeepSlate = Color.FromArgb(26, 43, 52);
    private static readonly Color WeatheredGold = Color.FromArgb(205, 160, 82);
    private static readonly Color PaleGold = Color.FromArgb(241, 220, 170);
    private static readonly Color Mist = Color.FromArgb(222, 231, 226);

    private readonly LauncherSettings settings;
    private readonly Func<bool> isServerRunning;
    private readonly ModPackageInstaller installer = new();
    private readonly DataGridView grid = new()
    {
        Dock = DockStyle.Fill,
        AutoGenerateColumns = false,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        AllowUserToResizeRows = false,
        MultiSelect = false,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        BackgroundColor = Night,
        BorderStyle = BorderStyle.None
    };
    private readonly Label detailTitle = new() { AutoSize = false, Dock = DockStyle.Top, Height = 42 };
    private readonly Label detailStatus = new() { AutoSize = false, Dock = DockStyle.Top, Height = 44 };
    private readonly TextBox detailText = new()
    {
        Dock = DockStyle.Fill,
        Multiline = true,
        ReadOnly = true,
        ScrollBars = ScrollBars.Vertical,
        BorderStyle = BorderStyle.None
    };
    private readonly LinkLabel sourceLink = new() { Text = "View original source code", AutoSize = true };
    private readonly LinkLabel portedSourceLink = new() { Text = "View OpenDereth ported code", AutoSize = true };
    private readonly Button install = new() { Text = "Install", AutoSize = true };
    private readonly Button toggle = new() { Text = "Turn off", AutoSize = true };
    private readonly Button remove = new() { Text = "Remove", AutoSize = true };
    private readonly Button openSettings = new() { Text = "Settings", AutoSize = true };
    private readonly Button importPackage = new() { Text = "Import a Mod ZIP...", AutoSize = true };
    private readonly Button authorGuide = new() { Text = "How to Make a Mod", AutoSize = true };
    private BindingList<ModListItem> items = new();

    public ModsForm(LauncherSettings settings, Func<bool> isServerRunning)
    {
        this.settings = settings;
        this.isServerRunning = isServerRunning;

        Text = "OpenDereth - Mod Library";
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(1040, 680);
        Size = new Size(1240, 780);
        BackColor = Night;
        ForeColor = Mist;
        Font = new Font(SystemFonts.MessageBoxFont!.FontFamily, 9.5f);

        AddColumns();
        StyleGrid();

        var heading = new Label
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(18, 10, 18, 4),
            Font = new Font("Georgia", 19, FontStyle.Bold),
            ForeColor = PaleGold,
            Text = "MOD LIBRARY\r\nPick a mod to see what it changes before installing it."
        };
        var headerActions = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            Width = 330,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(0, 20, 12, 0),
            BackColor = Color.Transparent
        };
        foreach (var button in new[] { importPackage, authorGuide })
            StyleButton(button);
        headerActions.Controls.AddRange(new Control[] { importPackage, authorGuide });
        var header = new Panel { Dock = DockStyle.Top, Height = 82, BackColor = Night };
        header.Controls.Add(heading);
        header.Controls.Add(headerActions);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            SplitterWidth = 6,
            BackColor = Night
        };
        split.Panel1.Padding = new Padding(14, 8, 6, 14);
        split.Panel2.Padding = new Padding(8, 8, 14, 14);
        split.Panel1.Controls.Add(grid);
        split.Panel2.Controls.Add(BuildDetailsPanel());

        Controls.Add(split);
        Controls.Add(header);

        // SplitContainer starts with a small design-time width. Setting a large
        // distance or panel minimum in the initializer throws before the form has
        // completed layout, especially with display scaling enabled.
        Shown += (_, _) => ConfigureSplitter(split);

        grid.SelectionChanged += (_, _) => UpdateDetails();
        install.Click += async (_, _) => await InstallSelectedAsync();
        toggle.Click += async (_, _) => await ToggleSelectedAsync();
        remove.Click += (_, _) => RemoveSelected();
        openSettings.Click += (_, _) => OpenSelectedSettings();
        importPackage.Click += async (_, _) => await ImportPackageAsync();
        authorGuide.Click += (_, _) => OpenModAuthorGuide();
        sourceLink.LinkClicked += (_, _) => OpenSelectedSource();
        portedSourceLink.LinkClicked += (_, _) => OpenSelectedPortSource();

        RefreshCatalog();
    }

    private static void ConfigureSplitter(SplitContainer split)
    {
        var layout = CalculateSplitterLayout(split.ClientSize.Width, split.SplitterWidth);

        // Clear the defaults first so every assignment is valid even on a narrow,
        // scaled display. The calculated minimums are restored after the divider.
        split.Panel1MinSize = 0;
        split.Panel2MinSize = 0;
        split.SplitterDistance = layout.Distance;
        split.Panel1MinSize = layout.Panel1MinSize;
        split.Panel2MinSize = layout.Panel2MinSize;
    }

    internal static SplitterLayout CalculateSplitterLayout(int controlWidth, int splitterWidth)
    {
        var available = Math.Max(0, controlWidth - splitterWidth);
        var panel1Minimum = Math.Min(540, available);
        var panel2Minimum = Math.Min(360, Math.Max(0, available - panel1Minimum));
        var maximumDistance = Math.Max(panel1Minimum, available - panel2Minimum);
        var distance = Math.Clamp(730, panel1Minimum, maximumDistance);
        return new SplitterLayout(distance, panel1Minimum, panel2Minimum);
    }

    internal sealed record SplitterLayout(int Distance, int Panel1MinSize, int Panel2MinSize);

    private ModListItem? Selected => grid.CurrentRow?.DataBoundItem as ModListItem;

    private Control BuildDetailsPanel()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = DeepSlate, Padding = new Padding(16) };
        detailTitle.Font = new Font("Georgia", 17, FontStyle.Bold);
        detailTitle.ForeColor = PaleGold;
        detailStatus.Font = new Font(Font, FontStyle.Bold);
        detailStatus.ForeColor = WeatheredGold;
        detailText.BackColor = DeepSlate;
        detailText.ForeColor = Mist;
        detailText.Font = new Font(Font.FontFamily, 10);
        sourceLink.LinkColor = Color.FromArgb(142, 197, 222);
        sourceLink.ActiveLinkColor = PaleGold;
        portedSourceLink.LinkColor = Color.FromArgb(142, 197, 222);
        portedSourceLink.ActiveLinkColor = PaleGold;

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 84,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(0, 9, 0, 0),
            BackColor = Color.Transparent
        };
        foreach (var button in new[] { install, toggle, remove, openSettings })
            StyleButton(button);
        actions.Controls.AddRange(new Control[] { install, toggle, remove, openSettings });

        var sourcePanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 48,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(0, 8, 0, 0)
        };
        sourcePanel.Controls.AddRange(new Control[] { sourceLink, portedSourceLink });

        panel.Controls.Add(detailText);
        panel.Controls.Add(sourcePanel);
        panel.Controls.Add(actions);
        panel.Controls.Add(detailStatus);
        panel.Controls.Add(detailTitle);
        return panel;
    }

    private void RefreshCatalog()
    {
        var installedCatalog = new ModCatalog(CreateDisplayedModProviders(settings.ModsDirectory));
        var service = new ModCatalogService(CuratedModCatalog.Entries, AppContext.BaseDirectory);
        items = new BindingList<ModListItem>(service.Merge(installedCatalog.Scan()).ToList());
        grid.DataSource = items;
        if (grid.Rows.Count > 0)
            grid.Rows[0].Selected = true;
        UpdateDetails();
    }

    internal static IReadOnlyList<IModProvider> CreateDisplayedModProviders(string modsDirectory) =>
        new IModProvider[]
        {
            new AceServerModProvider(modsDirectory),
            new ChorizitePluginProvider(),
            new AceContentPackProvider()
        };

    private void UpdateDetails()
    {
        var item = Selected;
        if (item is null)
        {
            detailTitle.Text = "Select a mod";
            detailStatus.Text = string.Empty;
            detailText.Text = string.Empty;
            SetActions(false, false, false, false, false);
            return;
        }

        detailTitle.Text = item.Name;
        detailStatus.Text = $"{item.Status}  |  {item.Safety}";
        detailStatus.ForeColor = item.CompatibilityStatus switch
        {
            CompatibilityStatus.Compatible => Color.FromArgb(151, 222, 178),
            CompatibilityStatus.LoadFailed or CompatibilityStatus.Conflict => Color.FromArgb(255, 153, 125),
            _ => WeatheredGold
        };

        var dependencies = item.Catalog.Dependencies.Count == 0
            ? "None declared"
            : string.Join(", ", item.Catalog.Dependencies.Select(FindCatalogName));
        var testingStatus = item.Catalog.Availability == ModCatalogAvailability.Preview
            ? "PREVIEW - " + (string.IsNullOrWhiteSpace(item.Catalog.PreviewNotice)
                ? "automated compatibility checks passed, but thorough in-game testing has not been completed."
                : item.Catalog.PreviewNotice)
            : item.Catalog.Availability == ModCatalogAvailability.Ready
                ? "CURATED - packaged for this ACE release."
                : "NOT PORTED - source is listed for reference only.";
        detailText.Text =
            $"WHAT IT DOES\r\n{item.Catalog.Description}\r\n\r\n" +
            $"DETAILS\r\n{item.Catalog.Details}\r\n\r\n" +
            $"SAVED-GAME SAFETY\r\n{item.Catalog.SafetyNotice}\r\n\r\n" +
            $"COMPATIBILITY\r\n{item.CompatibilityMessage}\r\n\r\n" +
            $"TESTING STATUS\r\n{testingStatus}\r\n\r\n" +
            $"REQUIRES\r\n{dependencies}\r\n\r\n" +
            $"AUTHOR / VERSION TARGET\r\n{item.Author}  |  {item.Catalog.TargetFramework}  |  {item.Catalog.TargetAceVersion}";

        var installedRecord = item.Installed;
        install.Text = item.CompatibilityStatus == CompatibilityStatus.Compatible
            ? "Install"
            : "Why unavailable?";
        toggle.Text = installedRecord?.Enabled == true ? "Turn off" : "Turn on";
        SetActions(
            installEnabled: installedRecord is null,
            toggleEnabled: installedRecord?.CanToggle == true,
            removeEnabled: installedRecord?.Type == ModType.AceServer,
            settingsEnabled: installedRecord is not null && File.Exists(installedRecord.SettingsPath),
            sourceEnabled: !string.IsNullOrWhiteSpace(item.Catalog.SourceUrl));
    }

    private async Task InstallSelectedAsync()
    {
        var item = Selected;
        if (item is null || item.Installed is not null)
            return;
        if (item.CompatibilityStatus != CompatibilityStatus.Compatible)
        {
            MessageBox.Show(this,
                item.CompatibilityMessage +
                "\r\n\r\nImport a Mod ZIP can install a separately rebuilt package, but importing the original source does not port it to this ACE version.",
                "Mod is not packaged for this ACE version", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!EnsureServerStopped())
            return;

        var answer = MessageBox.Show(this,
            $"Install {item.Name}?\r\n\r\n{item.Catalog.Description}\r\n\r\n{item.Catalog.SafetyNotice}" +
            (item.Catalog.Availability == ModCatalogAvailability.Preview
                ? "\r\n\r\nPREVIEW WARNING: automated checks passed, but this mod has not been thoroughly tested in game."
                : string.Empty) +
            "\r\n\r\nThe local server must restart before the mod becomes active.",
            "Install mod", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (answer != DialogResult.Yes)
            return;

        SetActions(false, false, false, false, false);
        try
        {
            await installer.InstallAsync(
                item.PackagePath,
                item.Catalog.Id,
                settings.ModsDirectory,
                Path.Combine(settings.RuntimeDirectory, "ModStaging"));
            RefreshCatalog();
            MessageBox.Show(this, $"{item.Name} is installed. It will load the next time you click Play.",
                "Mod installed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Mod was not installed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateDetails();
        }
    }

    private async Task ImportPackageAsync()
    {
        if (!EnsureServerStopped())
            return;

        using var dialog = new OpenFileDialog
        {
            Title = "Import an OpenDereth mod package",
            Filter = "ACE mod packages (*.zip)|*.zip",
            CheckFileExists = true,
            Multiselect = false
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        importPackage.Enabled = false;
        try
        {
            var manifest = await installer.InspectAsync(dialog.FileName);
            var catalog = CuratedModCatalog.Entries.FirstOrDefault(entry =>
                string.Equals(entry.Id, manifest.Id, StringComparison.OrdinalIgnoreCase));
            var compatibilityWarning = catalog?.Availability switch
            {
                null => "This package is not in the curated catalog. The launcher can validate its checksum and file layout, but cannot prove that its code is compatible or safe for saved characters.",
                ModCatalogAvailability.Ready => catalog.SafetyNotice,
                ModCatalogAvailability.Preview => catalog.SafetyNotice + "\r\n\r\nPREVIEW WARNING: automated checks passed, but this mod has not been thoroughly tested in game.",
                _ => "The bundled catalog still marks this mod as needing a source port. Only continue if this ZIP was rebuilt and tested specifically for the current OpenDereth release."
            };

            var answer = MessageBox.Show(this,
                $"Import {manifest.Name} {manifest.Version}?\r\n\r\nPackage ID: {manifest.Id}\r\n\r\n{compatibilityWarning}\r\n\r\n" +
                "A matching .sha256 checksum file was verified. Back up %LOCALAPPDATA%\\OpenDereth before importing unverified code. The server will restart when you next click Play.",
                "Import mod package", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (answer != DialogResult.Yes)
                return;

            var destination = await installer.InstallAsync(
                dialog.FileName,
                manifest.Id,
                settings.ModsDirectory,
                Path.Combine(settings.RuntimeDirectory, "ModStaging"));
            RefreshCatalog();
            MessageBox.Show(this,
                $"{manifest.Name} was imported to:\r\n{destination}\r\n\r\nIt will load the next time you click Play.",
                "Mod imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this,
                ex.Message +
                "\r\n\r\nA supported package contains ace-mod.json at the ZIP root, its files under mod/, and a matching .zip.sha256 file beside the ZIP.",
                "Mod was not imported", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            importPackage.Enabled = true;
            UpdateDetails();
        }
    }

    private async Task ToggleSelectedAsync()
    {
        var item = Selected;
        var record = item?.Installed;
        if (item is null || record is null || !record.CanToggle)
            return;
        if (!EnsureServerStopped())
            return;

        var enabling = !record.Enabled;
        if (!enabling && item.Catalog.RemovalPolicy is not ModRemovalPolicy.Safe)
        {
            var warning = item.Catalog.RemovalPolicy == ModRemovalPolicy.DoNotRemove
                ? "This mod may be required by saved characters, items, or world data. Turning it off is strongly discouraged."
                : "Some changes made by this mod will remain in the saved game after it is turned off.";
            var answer = MessageBox.Show(this,
                $"{warning}\r\n\r\n{item.Catalog.SafetyNotice}\r\n\r\nBack up %LOCALAPPDATA%\\OpenDereth first. Turn it off anyway?",
                "Saved-game warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (answer != DialogResult.Yes)
                return;
        }

        try
        {
            await ModMetadataEditor.SetEnabledAsync(record.MetadataPath, enabling);
            RefreshCatalog();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Could not change the mod", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RemoveSelected()
    {
        var item = Selected;
        var record = item?.Installed;
        if (item is null || record?.Type != ModType.AceServer)
            return;
        if (!EnsureServerStopped())
            return;
        if (item.Catalog.RemovalPolicy == ModRemovalPolicy.DoNotRemove)
        {
            MessageBox.Show(this,
                "Removal is blocked because saved characters, items, or world data may require this mod. You can turn it off for troubleshooting, but keep the files until a tested cleanup migration exists.\r\n\r\n" + item.Catalog.SafetyNotice,
                "Removal blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var answer = MessageBox.Show(this,
            $"Remove {item.Name}?\r\n\r\n{item.Catalog.SafetyNotice}\r\n\r\nThe files will be moved to a recovery folder, not deleted.",
            "Remove mod", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (answer != DialogResult.Yes)
            return;

        try
        {
            var destination = installer.MoveToQuarantine(
                record.InstalledPath,
                settings.ModsDirectory,
                Path.Combine(settings.RuntimeDirectory, "RemovedMods"));
            RefreshCatalog();
            MessageBox.Show(this, $"The mod was removed from ACE and retained for recovery at:\r\n{destination}",
                "Mod removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Could not remove the mod", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OpenSelectedSettings()
    {
        var path = Selected?.Installed?.SettingsPath;
        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            Open(path);
    }

    private void OpenSelectedSource()
    {
        var url = Selected?.Catalog.SourceUrl;
        if (!string.IsNullOrWhiteSpace(url))
            Open(url);
    }

    private void OpenSelectedPortSource()
    {
        var url = Selected?.Catalog.PortSourceUrl;
        if (!string.IsNullOrWhiteSpace(url))
            Open(url);
    }

    private static void OpenModAuthorGuide()
    {
        var localGuide = Path.Combine(AppContext.BaseDirectory, "Docs", "MOD_AUTHOR_GUIDE.md");
        Open(File.Exists(localGuide)
            ? localGuide
            : "https://github.com/titaniumweiner/OpenDereth/blob/main/docs/MOD_AUTHOR_GUIDE.md");
    }

    private bool EnsureServerStopped()
    {
        if (!isServerRunning())
            return true;
        MessageBox.Show(this, "Stop the game and local server before changing mods.",
            "Server is running", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return false;
    }

    private static void Open(string path) =>
        Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });

    private static string FindCatalogName(string id) =>
        CuratedModCatalog.Entries.FirstOrDefault(entry => string.Equals(entry.Id, id, StringComparison.OrdinalIgnoreCase))?.Name ?? id;

    private void SetActions(bool installEnabled, bool toggleEnabled, bool removeEnabled, bool settingsEnabled, bool sourceEnabled)
    {
        install.Enabled = installEnabled;
        install.Visible = installEnabled;
        toggle.Enabled = toggleEnabled;
        toggle.Visible = toggleEnabled;
        remove.Enabled = removeEnabled;
        remove.Visible = removeEnabled;
        openSettings.Enabled = settingsEnabled;
        openSettings.Visible = settingsEnabled;
        sourceLink.Enabled = sourceEnabled;
        sourceLink.Visible = sourceEnabled;
        var portedSourceEnabled = sourceEnabled && !string.IsNullOrWhiteSpace(Selected?.Catalog.PortSourceUrl);
        portedSourceLink.Enabled = portedSourceEnabled;
        portedSourceLink.Visible = portedSourceEnabled;
    }

    private void AddColumns()
    {
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModListItem.Status), HeaderText = "Status", Width = 122 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModListItem.Name), HeaderText = "Mod", Width = 145 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModListItem.Safety), HeaderText = "Saved-game impact", Width = 155 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModListItem.Description), HeaderText = "What it does", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
    }

    private void StyleGrid()
    {
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = DeepSlate;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = PaleGold;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font(Font, FontStyle.Bold);
        grid.ColumnHeadersHeight = 38;
        grid.DefaultCellStyle.BackColor = Color.FromArgb(20, 34, 41);
        grid.DefaultCellStyle.ForeColor = Mist;
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 80, 67);
        grid.DefaultCellStyle.SelectionForeColor = Color.White;
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 40, 48);
        grid.RowTemplate.Height = 66;
        grid.CellFormatting += (_, args) =>
        {
            if (args.RowIndex < 0 || grid.Rows[args.RowIndex].DataBoundItem is not ModListItem item)
                return;
            if (item.CompatibilityStatus is CompatibilityStatus.LoadFailed or CompatibilityStatus.Conflict)
                args.CellStyle.ForeColor = Color.FromArgb(255, 174, 151);
            else if (item.Installed is not null)
                args.CellStyle.ForeColor = Color.FromArgb(165, 226, 188);
        };
    }

    private static void StyleButton(Button button)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderColor = Color.FromArgb(126, 146, 145);
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 77, 84);
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(33, 51, 59);
        button.BackColor = Night;
        button.ForeColor = PaleGold;
        button.Font = new Font(button.Font, FontStyle.Bold);
        button.UseVisualStyleBackColor = false;
    }
}
