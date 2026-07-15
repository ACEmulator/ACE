namespace ACE.SinglePlayer.Infrastructure;

public static class BundledDistribution
{
    public const string MariaDbVersion = "12.3.2";
    public const string WorldVersion = "0.9.294";

    public static string MariaDbServerPath(string applicationDirectory) => Path.GetFullPath(Path.Combine(
        applicationDirectory, "Dependencies", "MariaDB", "bin", "mariadbd.exe"));

    public static string? FindWorldSqlPath(string applicationDirectory)
    {
        var directory = Path.GetFullPath(Path.Combine(applicationDirectory, "Dependencies", "World"));
        if (!Directory.Exists(directory))
            return null;

        return Directory.EnumerateFiles(directory, "ACE-World-Database-*.sql", SearchOption.TopDirectoryOnly)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault();
    }

    public static bool IsComplete(string applicationDirectory)
    {
        var mariaDb = MariaDbServerPath(applicationDirectory);
        return File.Exists(mariaDb) && Database.MariaDbInstallationLocator.FindInitializer(mariaDb) is not null &&
            Database.MariaDbInstallationLocator.FindClient(mariaDb) is not null &&
            FindWorldSqlPath(applicationDirectory) is not null;
    }
}
