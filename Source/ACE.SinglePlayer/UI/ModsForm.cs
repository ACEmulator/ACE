using System.ComponentModel;
using System.Diagnostics;

using ACE.SinglePlayer.Models;
using ACE.SinglePlayer.Mods;

namespace ACE.SinglePlayer.UI;

public sealed class ModsForm : Form
{
    private readonly LauncherSettings settings;
    private readonly Func<bool> isServerRunning;
    private readonly DataGridView grid = new() { Dock = DockStyle.Fill, AutoGenerateColumns = false, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
    private BindingList<ModRecord> records = new();

    public ModsForm(LauncherSettings settings, Func<bool> isServerRunning)
    {
        this.settings = settings;
        this.isServerRunning = isServerRunning;
        Text = "Mods and client plugins";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1000, 600);

        AddColumns();
        grid.CellContentClick += async (_, args) => await ToggleAsync(args);
        grid.CellFormatting += FormatCell;

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(8), AutoSize = false };
        var refresh = new Button { Text = "Refresh", AutoSize = true };
        var openMods = new Button { Text = "Open Mods Folder", AutoSize = true };
        var openMod = new Button { Text = "Open Selected Folder", AutoSize = true };
        var openSettings = new Button { Text = "Open Settings.json", AutoSize = true };
        buttons.Controls.AddRange(new Control[] { refresh, openMods, openMod, openSettings });
        refresh.Click += (_, _) => RefreshCatalog();
        openMods.Click += (_, _) => Open(settings.ModsDirectory);
        openMod.Click += (_, _) => { if (Selected is { InstalledPath.Length: > 0 } record) Open(record.InstalledPath); };
        openSettings.Click += (_, _) =>
        {
            if (Selected is { SettingsPath.Length: > 0 } record && File.Exists(record.SettingsPath)) Open(record.SettingsPath);
            else MessageBox.Show(this, "This server mod has no Settings.json file.", "No settings file", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        var note = new Label
        {
            Dock = DockStyle.Top,
            Height = 48,
            Padding = new Padding(10),
            Text = "ACE server mods are loaded by ACE.Server's existing Harmony ModManager. Decal entries are read-only and load only in Decal mode. Server-mod changes require a restart."
        };
        Controls.Add(grid);
        Controls.Add(buttons);
        Controls.Add(note);
        RefreshCatalog();
    }

    private ModRecord? Selected => grid.CurrentRow?.DataBoundItem as ModRecord;

    private void RefreshCatalog()
    {
        var catalog = new ModCatalog(new IModProvider[]
        {
            new AceServerModProvider(settings.ModsDirectory),
            new DecalPluginProvider(),
            new ChorizitePluginProvider(),
            new AceContentPackProvider()
        });
        records = new BindingList<ModRecord>(catalog.Scan().ToList());
        grid.DataSource = records;
    }

    private async Task ToggleAsync(DataGridViewCellEventArgs args)
    {
        if (args.RowIndex < 0 || grid.Columns[args.ColumnIndex].Name != "Enabled" || grid.Rows[args.RowIndex].DataBoundItem is not ModRecord record)
            return;
        if (!record.CanToggle)
        {
            MessageBox.Show(this, record.Type == ModType.DecalPlugin
                ? "Decal registration is read-only until its current format is fully tested."
                : "Malformed metadata cannot be edited safely.", "Read-only", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (isServerRunning())
        {
            MessageBox.Show(this, "Stop ACE.Server before changing mod metadata. The change requires a server restart.", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            record.Enabled = !record.Enabled;
            await ModMetadataEditor.SetEnabledAsync(record.MetadataPath, record.Enabled);
            grid.InvalidateRow(args.RowIndex);
        }
        catch (Exception ex)
        {
            record.Enabled = !record.Enabled;
            MessageBox.Show(this, ex.Message, "Could not update Meta.json", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static void Open(string path)
    {
        Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
    }

    private void FormatCell(object? sender, DataGridViewCellFormattingEventArgs args)
    {
        if (args.RowIndex < 0 || grid.Rows[args.RowIndex].DataBoundItem is not ModRecord record)
            return;
        if (record.IsMalformed || record.CompatibilityStatus == CompatibilityStatus.LoadFailed)
            args.CellStyle.BackColor = Color.MistyRose;
        else if (record.Type == ModType.DecalPlugin)
            args.CellStyle.BackColor = Color.AliceBlue;
    }

    private void AddColumns()
    {
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModRecord.Type), HeaderText = "Type", Width = 110 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModRecord.Name), HeaderText = "Name", Width = 160 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModRecord.Author), HeaderText = "Author", Width = 110 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModRecord.Version), HeaderText = "Version", Width = 75 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModRecord.Priority), HeaderText = "Priority", Width = 65 });
        grid.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Enabled", DataPropertyName = nameof(ModRecord.Enabled), HeaderText = "Enabled", Width = 65 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModRecord.CompatibilityStatus), HeaderText = "Compatibility", Width = 120 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModRecord.Description), HeaderText = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ModRecord.LastLoadError), HeaderText = "Last error", Width = 180 });
    }
}
