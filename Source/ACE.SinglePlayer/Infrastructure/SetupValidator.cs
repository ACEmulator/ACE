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

    public static readonly string[] RequiredClientDatFiles =
    {
        "client_cell_1.dat",
        "client_portal.dat",
        "client_local_English.dat",
        "client_highres.dat"
    };

    public static ValidationResult Validate(LauncherSettings settings)
    {
        var errors = new List<string>(ValidateClient(settings).Errors);
        if (!File.Exists(settings.ServerExePath))
            errors.Add("The bundled ACE.Server executable is missing. Extract the complete OpenDereth ZIP again.");

        if (!Directory.Exists(settings.DatFilesDirectory))
            errors.Add("Select the folder containing acclient.exe. The DAT folder is filled automatically.");
        else
            foreach (var file in RequiredDatFiles)
                if (!File.Exists(Path.Combine(settings.DatFilesDirectory, file)))
                    errors.Add($"The DAT directory is missing {file}.");

        if (string.IsNullOrWhiteSpace(settings.ModsDirectory))
            errors.Add("The bundled Mods directory is missing.");
        if (string.IsNullOrWhiteSpace(settings.RuntimeDirectory))
            errors.Add("The private Runtime directory is missing.");
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
                errors.Add("The bundled private database runtime is missing. Extract the complete OpenDereth ZIP again.");
            else if (Database.MariaDbInstallationLocator.FindInitializer(settings.ManagedDatabaseExePath) is null)
                errors.Add("The bundled MariaDB initializer is missing. Extract the complete OpenDereth ZIP again.");
            if (string.IsNullOrWhiteSpace(settings.ProtectedDatabasePassword) ||
                string.IsNullOrWhiteSpace(settings.ProtectedPrivateDatabaseAdminPassword))
                errors.Add("The automatic private database credentials have not been generated.");
            if (!Directory.Exists(Path.Combine(settings.PrivateDatabaseDirectory, "mysql")) &&
                !File.Exists(settings.WorldDatabaseSqlPath))
                errors.Add("The bundled ACE World database is missing. Extract the complete OpenDereth ZIP again.");
        }

        return new ValidationResult(errors.Count == 0, errors);
    }

    public static ValidationResult ValidateClient(LauncherSettings settings)
    {
        var errors = new List<string>();
        if (!File.Exists(settings.ClientExePath))
            errors.Add("Select the folder containing acclient.exe.");
        else if (!string.Equals(Path.GetFileName(settings.ClientExePath), "acclient.exe", StringComparison.OrdinalIgnoreCase))
            errors.Add("The selected client executable must be acclient.exe.");
        else
        {
            var clientDirectory = Path.GetDirectoryName(Path.GetFullPath(settings.ClientExePath))!;
            var missingClientDats = RequiredClientDatFiles
                .Where(file => !File.Exists(Path.Combine(clientDirectory, file)))
                .ToArray();
            if (missingClientDats.Length > 0)
            {
                errors.Add("The folder containing acclient.exe is missing required client data files: " +
                    string.Join(", ", missingClientDats) +
                    ". The AC client must have its DAT files in the same folder as acclient.exe. Copy the complete client installation to a writable folder such as C:\\Games\\AsheronsCall, then select that acclient.exe.");
            }
            else if (!CanWriteDirectory(clientDirectory))
            {
                errors.Add("The folder containing acclient.exe is not writable. Copy the complete AC client installation to a normal folder such as C:\\Games\\AsheronsCall; do not run it from Program Files, OneDrive, or a read-only archive.");
            }
        }

        return new ValidationResult(errors.Count == 0, errors);
    }

    public static string? DetectDatDirectory(string clientExePath)
    {
        var directory = Path.GetDirectoryName(clientExePath);
        return directory is not null && RequiredClientDatFiles.All(file => File.Exists(Path.Combine(directory, file)))
            ? directory
            : null;
    }

    private static bool CanWriteDirectory(string directory)
    {
        var probe = Path.Combine(directory, ".ace-singleplayer-write-test-" + Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            using var stream = new FileStream(probe, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1, FileOptions.DeleteOnClose);
            stream.WriteByte(0);
            return true;
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or IOException)
        {
            return false;
        }
        finally
        {
            try
            {
                if (File.Exists(probe))
                    File.Delete(probe);
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException or IOException)
            {
                // A failed cleanup does not change whether the directory accepted the write probe.
            }
        }
    }
}
