namespace ACE.SinglePlayer.Database;

public static class MariaDbInstallationLocator
{
    public static string? FindServerExecutable(string? preferredPath = null)
    {
        if (!string.IsNullOrWhiteSpace(preferredPath) && File.Exists(preferredPath))
            return Path.GetFullPath(preferredPath);

        foreach (var pathEntry in (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var candidate = Path.Combine(pathEntry, "mariadbd.exe");
            if (File.Exists(candidate) && FindInitializer(candidate) is not null)
                return Path.GetFullPath(candidate);
        }

        return FindUnderRoots(new[]
        {
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
        });
    }

    internal static string? FindUnderRoots(IEnumerable<string> roots)
    {
        var candidates = new List<string>();
        foreach (var root in roots.Where(path => !string.IsNullOrWhiteSpace(path) && Directory.Exists(path)))
        {
            try
            {
                foreach (var directory in Directory.EnumerateDirectories(root, "MariaDB *", SearchOption.TopDirectoryOnly))
                {
                    var candidate = Path.Combine(directory, "bin", "mariadbd.exe");
                    if (File.Exists(candidate) && FindInitializer(candidate) is not null)
                        candidates.Add(candidate);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // A locked Program Files child should not prevent checking other roots.
            }
        }

        return candidates
            .OrderByDescending(GetVersionFromParentDirectory)
            .ThenByDescending(File.GetLastWriteTimeUtc)
            .FirstOrDefault();
    }

    public static string? FindInitializer(string serverExecutablePath)
    {
        var directory = Path.GetDirectoryName(serverExecutablePath);
        if (directory is null)
            return null;

        foreach (var name in new[] { "mariadb-install-db.exe", "mysql_install_db.exe" })
        {
            var candidate = Path.Combine(directory, name);
            if (File.Exists(candidate))
                return candidate;
        }

        return null;
    }

    public static string? FindClient(string serverExecutablePath)
    {
        var directory = Path.GetDirectoryName(serverExecutablePath);
        if (directory is null)
            return null;

        foreach (var name in new[] { "mariadb.exe", "mysql.exe" })
        {
            var candidate = Path.Combine(directory, name);
            if (File.Exists(candidate))
                return candidate;
        }

        return null;
    }

    private static Version GetVersionFromParentDirectory(string executablePath)
    {
        var directoryName = Directory.GetParent(Path.GetDirectoryName(executablePath)!)?.Name ?? string.Empty;
        var value = directoryName.StartsWith("MariaDB ", StringComparison.OrdinalIgnoreCase)
            ? directoryName["MariaDB ".Length..]
            : string.Empty;
        return Version.TryParse(value, out var version) ? version : new Version(0, 0);
    }
}
