using Microsoft.Win32;

namespace ACE.SinglePlayer.ClientLaunch;

public sealed record DecalInstallation(string AgentDirectory, string InjectDllPath);

public static class DecalDetector
{
    private const string AgentKey = @"SOFTWARE\Decal\Agent";

    public static DecalInstallation? Detect()
    {
        if (!OperatingSystem.IsWindows())
            return null;

        foreach (var hive in new[] { RegistryHive.LocalMachine, RegistryHive.CurrentUser })
        foreach (var view in new[] { RegistryView.Registry32, RegistryView.Registry64 })
        {
            try
            {
                using var baseKey = RegistryKey.OpenBaseKey(hive, view);
                using var key = baseKey.OpenSubKey(AgentKey);
                var agentPath = key?.GetValue("AgentPath") as string;
                if (string.IsNullOrWhiteSpace(agentPath))
                    continue;

                var directory = agentPath.Trim().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var injectPath = Path.Combine(directory, "Inject.dll");
                if (File.Exists(injectPath))
                    return new DecalInstallation(directory, injectPath);
            }
            catch (UnauthorizedAccessException)
            {
                // Try the next registry view/hive.
            }
        }

        return null;
    }
}
