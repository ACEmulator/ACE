using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.UI;

public sealed class QuickSetupForm : Form
{
    private readonly LauncherSettings settings;
    private readonly TextBox clientDirectory = new() { Dock = DockStyle.Fill };

    public QuickSetupForm(LauncherSettings settings)
    {
        this.settings = settings;
        Text = "OpenDereth - Choose Asheron's Call";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(690, 260);

        var existingDirectory = Path.GetDirectoryName(settings.ClientExePath);
        clientDirectory.Text = Directory.Exists(existingDirectory) ? existingDirectory : string.Empty;

        var title = new Label
        {
            Text = "Where is Asheron's Call installed?",
            AutoSize = true,
            Font = new Font(SystemFonts.MessageBoxFont!.FontFamily, 15, FontStyle.Bold),
            Margin = new Padding(0, 0, 0, 12)
        };
        var explanation = new Label
        {
            Text = "Select the folder containing acclient.exe and the four client DAT files. Everything else—the ACE server, private database, and world—is already included.",
            AutoSize = true,
            MaximumSize = new Size(640, 0),
            Margin = new Padding(0, 0, 0, 14)
        };
        var browse = new Button { Text = "Browse...", AutoSize = true, Dock = DockStyle.Fill };
        browse.Click += (_, _) => BrowseForClient();
        var pathRow = new TableLayoutPanel { Width = 640, Height = 34, ColumnCount = 2 };
        pathRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        pathRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pathRow.Controls.Add(clientDirectory, 0, 0);
        pathRow.Controls.Add(browse, 1, 0);

        var continueButton = new Button { Text = "Save and Continue", AutoSize = true };
        continueButton.Click += (_, _) => SaveClientFolder();
        var cancel = new Button { Text = "Cancel", AutoSize = true, DialogResult = DialogResult.Cancel };
        var buttons = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 12, 0, 0)
        };
        buttons.Controls.Add(continueButton);
        buttons.Controls.Add(cancel);

        var content = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(24),
            AutoScroll = true
        };
        content.Controls.Add(title);
        content.Controls.Add(explanation);
        content.Controls.Add(pathRow);
        content.Controls.Add(buttons);
        Controls.Add(content);
        AcceptButton = continueButton;
        CancelButton = cancel;
    }

    private void BrowseForClient()
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select the folder containing acclient.exe and the Asheron's Call DAT files",
            SelectedPath = clientDirectory.Text,
            ShowNewFolderButton = false
        };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            clientDirectory.Text = dialog.SelectedPath;
    }

    private void SaveClientFolder()
    {
        var directory = clientDirectory.Text.Trim();
        settings.ClientExePath = Path.Combine(directory, "acclient.exe");
        settings.DatFilesDirectory = directory;
        var validation = SetupValidator.ValidateClient(settings);
        if (!validation.IsValid)
        {
            MessageBox.Show(this, validation.Message, "Choose the complete AC folder", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }
}
