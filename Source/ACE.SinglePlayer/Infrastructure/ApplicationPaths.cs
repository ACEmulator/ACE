namespace ACE.SinglePlayer.Infrastructure;

public static class ApplicationPaths
{
    public const string ProductName = "OpenDereth";
    public const string LegacyProductDirectoryName = "ACESinglePlayer";

    public static string LocalRoot => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ProductName);

    public static string LegacyLocalRoot => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), LegacyProductDirectoryName);

    public static bool MigrateLegacyLocalData() => MigrateLegacyLocalData(LegacyLocalRoot, LocalRoot);

    internal static bool MigrateLegacyLocalData(string legacyRoot, string localRoot)
    {
        if (Directory.Exists(localRoot) || !Directory.Exists(legacyRoot))
            return false;

        Directory.Move(legacyRoot, localRoot);
        return true;
    }

    public static string ReplaceLegacyRoot(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return path;

        try
        {
            var fullPath = Path.GetFullPath(path);
            var legacyRoot = Path.GetFullPath(LegacyLocalRoot)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (string.Equals(fullPath, legacyRoot, StringComparison.OrdinalIgnoreCase))
                return LocalRoot;

            var legacyPrefix = legacyRoot + Path.DirectorySeparatorChar;
            if (!fullPath.StartsWith(legacyPrefix, StringComparison.OrdinalIgnoreCase))
                return path;

            return Path.Combine(LocalRoot, fullPath[legacyPrefix.Length..]);
        }
        catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
        {
            return path;
        }
    }
}
