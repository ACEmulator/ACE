using System.ComponentModel;
using System.Diagnostics;

using ACE.SinglePlayer.CustomContent;
using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.UI;

public sealed class CustomWeeniesForm : Form
{
    private const string AceForgeReleaseUrl = "https://github.com/shemtar-90/AceForge/releases/tag/v0.3.36";
    private static readonly Color Night = Color.FromArgb(14, 25, 32);
    private static readonly Color DeepSlate = Color.FromArgb(26, 43, 52);
    private static readonly Color PaleGold = Color.FromArgb(241, 220, 170);
    private static readonly Color Mist = Color.FromArgb(222, 231, 226);

    private readonly LauncherSettings settings;
    private readonly Func<bool> isServerRunning;
    private readonly CustomWeenieSqlInspector inspector = new();
    private readonly CustomWeenieImportService importer;
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
    private readonly TextBox issues = new()
    {
        Dock = DockStyle.Fill,
        Multiline = true,
        ReadOnly = true,
        ScrollBars = ScrollBars.Vertical,
        BorderStyle = BorderStyle.None
    };
    private readonly Label status = new()
    {
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleLeft,
        AutoEllipsis = true
    };
    private readonly Button chooseFolder = new() { Text = "Choose AceForge Folder...", AutoSize = true };
    private readonly Button chooseFiles = new() { Text = "Choose SQL Files...", AutoSize = true };
    private readonly Button openAceForge = new() { Text = "Open AceForge v0.3.36", AutoSize = true };
    private readonly Button openBackups = new() { Text = "Open Backups", AutoSize = true };
    private readonly Button import = new() { Text = "IMPORT VALID WEENIES", AutoSize = true, Enabled = false };
    private IReadOnlyList<CustomWeenieDefinition> selected = Array.Empty<CustomWeenieDefinition>();

    public CustomWeeniesForm(LauncherSettings settings, Func<bool> isServerRunning,
        DatabaseRuntimeFactory runtimeFactory, DatabaseConnectionFactory connectionFactory, LauncherLog log)
    {
        this.settings = settings;
        this.isServerRunning = isServerRunning;
        importer = new CustomWeenieImportService(runtimeFactory, connectionFactory, log);

        Text = "OpenDereth - Custom Weenies";
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(940, 680);
        Size = new Size(1120, 780);
        BackColor = Night;
        ForeColor = Mist;
        Font = new Font(SystemFonts.MessageBoxFont!.FontFamily, 9.5f);

        ConfigureGrid();
        issues.BackColor = Color.FromArgb(20, 34, 41);
        issues.ForeColor = Mist;
        status.ForeColor = PaleGold;
        status.Font = new Font(Font, FontStyle.Bold);
        foreach (var button in new[] { chooseFolder, chooseFiles, openAceForge, openBackups, import })
            StyleButton(button);
        import.BackColor = Color.FromArgb(133, 76, 38);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 5,
            ColumnCount = 1,
            Padding = new Padding(16),
            BackColor = Night
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 92));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));

        var heading = new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Georgia", 19, FontStyle.Bold),
            ForeColor = PaleGold,
            Text = "CUSTOM WEENIES\r\nImport custom ACE World objects without hand-editing the database."
        };
        var explanation = new Label
        {
            Dock = DockStyle.Fill,
            ForeColor = Mist,
            Padding = new Padding(0, 8, 0, 4),
            Text = "Select the folder where AceForge saved its .sql files, or choose individual files. " +
                   "The launcher validates every statement, previews each WCID, checks for collisions, and creates a complete ace_world backup before importing. " +
                   "Quest, recipe, event, and treasure scripts are reported but are not executed by this section."
        };
        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(0, 7, 0, 0),
            BackColor = Color.Transparent
        };
        actions.Controls.AddRange(new Control[] { chooseFolder, chooseFiles, openAceForge, openBackups });

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 1,
            BackColor = DeepSlate,
            Padding = new Padding(10)
        };
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
        content.Controls.Add(grid, 0, 0);
        var issueGroup = new GroupBox
        {
            Dock = DockStyle.Fill,
            Text = "Validation notes and skipped files",
            ForeColor = PaleGold,
            Padding = new Padding(8)
        };
        issueGroup.Controls.Add(issues);
        content.Controls.Add(issueGroup, 0, 1);

        var footer = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Padding = new Padding(0, 8, 0, 0)
        };
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        footer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        footer.Controls.Add(status, 0, 0);
        footer.Controls.Add(import, 1, 0);

        root.Controls.Add(heading, 0, 0);
        root.Controls.Add(explanation, 0, 1);
        root.Controls.Add(actions, 0, 2);
        root.Controls.Add(content, 0, 3);
        root.Controls.Add(footer, 0, 4);
        Controls.Add(root);

        chooseFolder.Click += (_, _) => ChooseFolder();
        chooseFiles.Click += (_, _) => ChooseFiles();
        openAceForge.Click += (_, _) => Open(AceForgeReleaseUrl);
        openBackups.Click += (_, _) => OpenBackupFolder();
        import.Click += async (_, _) => await ImportAsync();
        status.Text = "Choose AceForge weenie SQL files to begin.";
        issues.Text = "No files selected.";
    }

    private void ChooseFolder()
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Choose the AceForge output folder containing custom weenie SQL files",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = false
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            LoadFiles(Directory.EnumerateFiles(dialog.SelectedPath, "*.sql", SearchOption.AllDirectories));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MessageBox.Show(this, ex.Message, "Could not read the folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ChooseFiles()
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Choose AceForge custom weenie SQL files",
            Filter = "SQL files (*.sql)|*.sql",
            Multiselect = true,
            CheckFileExists = true
        };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            LoadFiles(dialog.FileNames);
    }

    private void LoadFiles(IEnumerable<string> filePaths)
    {
        var result = inspector.InspectFiles(filePaths);
        selected = result.Definitions;
        grid.DataSource = new BindingList<CustomWeenieDefinition>(selected.ToList());
        issues.Text = result.Issues.Count == 0
            ? "All selected files passed the Custom Weenie safety checks."
            : string.Join(Environment.NewLine + Environment.NewLine, result.Issues.Select(issue =>
                $"{(string.IsNullOrWhiteSpace(issue.FilePath) ? "Selection" : Path.GetFileName(issue.FilePath))}: {issue.Message}"));
        import.Enabled = selected.Count > 0;
        status.Text = selected.Count == 0
            ? $"No importable weenies found. {result.Issues.Count} file(s) or selection issue(s) reported."
            : $"Ready: {selected.Count} valid weenie{(selected.Count == 1 ? string.Empty : "s")}" +
              (result.Issues.Count == 0 ? "." : $"; {result.Issues.Count} file(s) skipped.");
    }

    private async Task ImportAsync()
    {
        if (settings.DatabaseMode != DatabaseMode.Private)
        {
            MessageBox.Show(this,
                "Safe automatic imports currently require Private Database mode. The launcher will not modify an external MariaDB installation automatically.",
                "Private Database required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (isServerRunning())
        {
            MessageBox.Show(this, "Stop the game and local server before importing custom weenies.",
                "Server is running", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (selected.Count == 0)
            return;

        var answer = MessageBox.Show(this,
            $"Import {selected.Count} custom weenie{(selected.Count == 1 ? string.Empty : "s")} into your saved world?\r\n\r\n" +
            "A complete ace_world backup will be created first. Imported world data does not have a one-click uninstaller.",
            "Confirm Custom Weenie import", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (answer != DialogResult.Yes)
            return;

        SetBusy(true);
        try
        {
            var progress = new Progress<string>(message => status.Text = message);
            var result = await importer.ImportAsync(settings, selected, ConfirmReplacement, progress, CancellationToken.None);
            if (!result.Imported)
            {
                status.Text = "Import canceled. No database changes were made.";
                return;
            }

            status.Text = $"Imported {selected.Count} custom weenie{(selected.Count == 1 ? string.Empty : "s")}. Start the game to use them.";
            MessageBox.Show(this,
                $"The custom weenies were imported successfully.\r\n\r\nSafety backup:\r\n{result.BackupPath}",
                "Import complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            status.Text = "Import failed. The launcher did not complete the database transaction.";
            MessageBox.Show(this, ex.Message, "Custom Weenie import failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private bool ConfirmReplacement(IReadOnlyList<ExistingWeenie> existing)
    {
        var list = string.Join("\r\n", existing.Take(12).Select(item => $"WCID {item.ClassId}: {item.ClassName}"));
        if (existing.Count > 12)
            list += $"\r\n...and {existing.Count - 12} more";
        return MessageBox.Show(this,
                   $"{existing.Count} selected WCID{(existing.Count == 1 ? string.Empty : "s")} already exist in this world:\r\n\r\n" +
                   list + "\r\n\r\nContinuing will replace those records and may affect saved characters or objects. Replace them?",
                   "Existing WCIDs found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
    }

    private void SetBusy(bool busy)
    {
        chooseFolder.Enabled = !busy;
        chooseFiles.Enabled = !busy;
        import.Enabled = !busy && selected.Count > 0;
        UseWaitCursor = busy;
    }

    private void OpenBackupFolder()
    {
        var path = MariaDbWorldBackupService.GetBackupDirectory(settings);
        Directory.CreateDirectory(path);
        Open(path);
    }

    private void ConfigureGrid()
    {
        grid.Columns.Add(new DataGridViewTextBoxColumn
            { DataPropertyName = nameof(CustomWeenieDefinition.ClassId), HeaderText = "WCID", Width = 95 });
        grid.Columns.Add(new DataGridViewTextBoxColumn
            { DataPropertyName = nameof(CustomWeenieDefinition.ClassName), HeaderText = "Name", Width = 230 });
        grid.Columns.Add(new DataGridViewTextBoxColumn
            { DataPropertyName = nameof(CustomWeenieDefinition.TypeName), HeaderText = "Type", Width = 135 });
        grid.Columns.Add(new DataGridViewTextBoxColumn
            { DataPropertyName = nameof(CustomWeenieDefinition.FileName), HeaderText = "AceForge SQL file", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = DeepSlate;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = PaleGold;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font(Font, FontStyle.Bold);
        grid.ColumnHeadersHeight = 38;
        grid.DefaultCellStyle.BackColor = Color.FromArgb(20, 34, 41);
        grid.DefaultCellStyle.ForeColor = Mist;
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 80, 67);
        grid.DefaultCellStyle.SelectionForeColor = Color.White;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 40, 48);
        grid.RowTemplate.Height = 34;
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

    private static void Open(string path) =>
        Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
}
