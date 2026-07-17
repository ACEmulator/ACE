namespace ACE.SinglePlayer.CustomContent;

internal static class BundledWeenieCatalog
{
    public static string GetRootDirectory(string baseDirectory) =>
        Path.Combine(Path.GetFullPath(baseDirectory), "Weenies");

    public static IReadOnlyList<string> FindSqlFiles(string baseDirectory)
    {
        var root = GetRootDirectory(baseDirectory);
        return Directory.Exists(root)
            ? Directory.EnumerateFiles(root, "*.sql", SearchOption.AllDirectories)
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToArray()
            : Array.Empty<string>();
    }
}
