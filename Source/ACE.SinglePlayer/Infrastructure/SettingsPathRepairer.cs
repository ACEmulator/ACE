using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Infrastructure;

public static class SettingsPathRepairer
{
    public static bool Repair(LauncherSettings settings, string applicationDirectory)
    {
        var changed = false;
        var completeClient = FindCompleteClient(settings.ClientExePath, applicationDirectory);
        if (completeClient is not null)
        {
            var clientDirectory = SetupValidator.DetectDatDirectory(completeClient)!;
            changed |= SetIfDifferent(settings.ClientExePath, completeClient, value => settings.ClientExePath = value);
            if (!HasServerDatFiles(settings.DatFilesDirectory))
                changed |= SetIfDifferent(settings.DatFilesDirectory, clientDirectory, value => settings.DatFilesDirectory = value);
        }

        var packagedServer = Path.GetFullPath(Path.Combine(applicationDirectory, "Server", "ACE.Server.exe"));
        if (File.Exists(packagedServer))
            changed |= SetIfDifferent(settings.ServerExePath, packagedServer, value => settings.ServerExePath = value);

        var packagedMods = Path.GetFullPath(Path.Combine(applicationDirectory, "Mods"));
        if (Directory.Exists(packagedMods))
            changed |= SetIfDifferent(settings.ModsDirectory, packagedMods, value => settings.ModsDirectory = value);

        var bundledMariaDb = BundledDistribution.MariaDbServerPath(applicationDirectory);
        if (File.Exists(bundledMariaDb))
            changed |= SetIfDifferent(settings.ManagedDatabaseExePath, bundledMariaDb,
                value => settings.ManagedDatabaseExePath = value);

        var bundledWorld = BundledDistribution.FindWorldSqlPath(applicationDirectory);
        if (bundledWorld is not null)
            changed |= SetIfDifferent(settings.WorldDatabaseSqlPath, bundledWorld,
                value => settings.WorldDatabaseSqlPath = value);

        return changed;
    }

    private static string? FindCompleteClient(string configuredPath, string applicationDirectory)
    {
        var candidates = new[]
        {
            configuredPath,
            Path.Combine(applicationDirectory, "Client", "acclient.exe"),
            @"C:\Turbine\Asheron's Call\acclient.exe",
            @"C:\Games\Asheron's Call\acclient.exe",
            @"C:\Games\AsheronsCall\acclient.exe",
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Turbine", "Asheron's Call", "acclient.exe")
        };

        foreach (var candidate in candidates.Where(path => !string.IsNullOrWhiteSpace(path)).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            try
            {
                var fullPath = Path.GetFullPath(candidate);
                if (File.Exists(fullPath) && SetupValidator.DetectDatDirectory(fullPath) is not null)
                    return fullPath;
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
            {
                // Ignore a malformed stale path and continue through the known safe locations.
            }
        }

        return null;
    }

    private static bool SetIfDifferent(string current, string replacement, Action<string> setter)
    {
        if (string.Equals(current, replacement, StringComparison.OrdinalIgnoreCase))
            return false;

        setter(replacement);
        return true;
    }

    private static bool HasServerDatFiles(string directory) =>
        !string.IsNullOrWhiteSpace(directory) &&
        Directory.Exists(directory) &&
        SetupValidator.RequiredDatFiles.All(file => File.Exists(Path.Combine(directory, file)));
}
