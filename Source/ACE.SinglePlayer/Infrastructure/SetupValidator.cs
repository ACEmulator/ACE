using System.Net;

using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Infrastructure;

public sealed record ValidationResult(bool IsValid, IReadOnlyList<string> Errors)
{
    public string Message => string.Join(Environment.NewLine, Errors);
}

public static class SetupValidator
{
    public static readonly string[] RequiredDatFiles =
    {
        "client_cell_1.dat",
        "client_portal.dat",
        "client_local_English.dat"
    };

    public static ValidationResult Validate(LauncherSettings settings)
    {
        var errors = new List<string>();
        if (!File.Exists(settings.ClientExePath))
            errors.Add("Select a valid acclient.exe file.");
        else if (!string.Equals(Path.GetFileName(settings.ClientExePath), "acclient.exe", StringComparison.OrdinalIgnoreCase))
            errors.Add("The selected client executable must be acclient.exe.");

        if (!File.Exists(settings.ServerExePath))
            errors.Add("Select a published ACE.Server executable.");

        if (!Directory.Exists(settings.DatFilesDirectory))
            errors.Add("Select the directory containing the Asheron's Call DAT files.");
        else
            foreach (var file in RequiredDatFiles)
                if (!File.Exists(Path.Combine(settings.DatFilesDirectory, file)))
                    errors.Add($"The DAT directory is missing {file}.");

        if (string.IsNullOrWhiteSpace(settings.ModsDirectory))
            errors.Add("Select a Mods directory.");
        if (string.IsNullOrWhiteSpace(settings.RuntimeDirectory))
            errors.Add("Select a Runtime directory.");
        if (!IPAddress.TryParse(settings.Host, out var host) || !IPAddress.IsLoopback(host))
            errors.Add("The standard single-player host must be a loopback address (127.0.0.1).");
        if (settings.Port is 0 or ushort.MaxValue)
            errors.Add("Choose a server port between 1 and 65534 (ACE also uses the next port).");
        if (string.IsNullOrWhiteSpace(settings.AccountName))
            errors.Add("Enter a persistent local account name.");
        if (string.IsNullOrWhiteSpace(settings.ProtectedAccountPassword))
            errors.Add("The persistent local account password has not been generated.");
        if (settings.DatabaseMode == DatabaseMode.External)
        {
            if (string.IsNullOrWhiteSpace(settings.DatabaseHost) || settings.DatabasePort == 0)
                errors.Add("Enter valid MariaDB/MySQL connection information.");
            if (string.IsNullOrWhiteSpace(settings.DatabaseUsername))
                errors.Add("Enter the MariaDB/MySQL username.");
        }
        else
        {
            if (!string.Equals(settings.DatabaseHost, "127.0.0.1", StringComparison.Ordinal))
                errors.Add("The automatic private database must use 127.0.0.1.");
            if (!string.Equals(settings.DatabaseUsername, "ace_singleplayer", StringComparison.Ordinal))
                errors.Add("The automatic private database must use its isolated ACE account.");
            if (!File.Exists(settings.ManagedDatabaseExePath))
                errors.Add("MariaDB was not detected. Install MariaDB or select its mariadbd.exe file.");
            else if (Database.MariaDbInstallationLocator.FindInitializer(settings.ManagedDatabaseExePath) is null)
                errors.Add("The MariaDB initializer is missing beside mariadbd.exe. Repair the MariaDB installation.");
            if (string.IsNullOrWhiteSpace(settings.ProtectedDatabasePassword) ||
                string.IsNullOrWhiteSpace(settings.ProtectedPrivateDatabaseAdminPassword))
                errors.Add("The automatic private database credentials have not been generated.");
            if (!Directory.Exists(Path.Combine(settings.PrivateDatabaseDirectory, "mysql")) &&
                !File.Exists(settings.WorldDatabaseSqlPath))
                errors.Add("Select an ACE world-database SQL package for the private database's first setup.");
        }

        return new ValidationResult(errors.Count == 0, errors);
    }

    public static string? DetectDatDirectory(string clientExePath)
    {
        var directory = Path.GetDirectoryName(clientExePath);
        return directory is not null && RequiredDatFiles.All(file => File.Exists(Path.Combine(directory, file)))
            ? directory
            : null;
    }
}
