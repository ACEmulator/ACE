using Microsoft.Win32;

namespace ACE.SinglePlayer.ClientLaunch;

public sealed record ThwargInstallation(string Directory, string InjectorDllPath);

public static class ThwargDetector
{
    private const string LauncherKey = @"SOFTWARE\Thwargle Games\ThwargLauncher";

    public static ThwargInstallation? Detect()
    {
        if (!OperatingSystem.IsWindows())
            return null;

        foreach (var hive in new[] { RegistryHive.LocalMachine, RegistryHive.CurrentUser })
        foreach (var view in new[] { RegistryView.Registry32, RegistryView.Registry64 })
        {
            try
            {
                using var baseKey = RegistryKey.OpenBaseKey(hive, view);
                using var key = baseKey.OpenSubKey(LauncherKey);
                var launcherPath = key?.GetValue("Path") as string;
                if (string.IsNullOrWhiteSpace(launcherPath))
                    continue;

                var directory = launcherPath.Trim().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var injectorPath = Path.Combine(directory, "injector.dll");
                if (File.Exists(injectorPath))
                    return new ThwargInstallation(directory, injectorPath);
            }
            catch (UnauthorizedAccessException)
            {
                // Try the next registry view/hive.
            }
        }

        return null;
    }
}
