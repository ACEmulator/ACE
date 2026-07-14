namespace ACE.SinglePlayer.Tests;

internal static class TestPaths
{
    public static string TestHost
    {
        get
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null && !string.Equals(directory.Name, "Source", StringComparison.OrdinalIgnoreCase))
                directory = directory.Parent;
            if (directory is null)
                throw new DirectoryNotFoundException("Could not locate the Source directory from the test output.");

            var configuration = new DirectoryInfo(AppContext.BaseDirectory).Parent?.Parent?.Name ?? "Debug";
            return Path.Combine(directory.FullName, "ACE.SinglePlayer.TestHost", "bin", configuration,
                "net10.0", "win-x64", "ACE.SinglePlayer.TestHost.exe");
        }
    }

    public static string CreateTemporaryDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), "ACE.SinglePlayer.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }
}
