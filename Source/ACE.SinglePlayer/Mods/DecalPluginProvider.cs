using Microsoft.Win32;

namespace ACE.SinglePlayer.Mods;

public sealed class DecalPluginProvider : IModProvider
{
    private static readonly string[] Categories = { "Plugins", "Services", "NetworkFilters" };

    public ModType Type => ModType.DecalPlugin;

    public IReadOnlyList<ModRecord> Scan()
    {
        if (!OperatingSystem.IsWindows())
            return Array.Empty<ModRecord>();

        var records = new Dictionary<string, ModRecord>(StringComparer.OrdinalIgnoreCase);
        foreach (var hive in new[] { RegistryHive.LocalMachine, RegistryHive.CurrentUser })
        foreach (var view in new[] { RegistryView.Registry32, RegistryView.Registry64 })
        foreach (var category in Categories)
        {
            try
            {
                using var baseKey = RegistryKey.OpenBaseKey(hive, view);
                using var categoryKey = baseKey.OpenSubKey(@"SOFTWARE\Decal\" + category);
                if (categoryKey is null)
                    continue;
                foreach (var subkeyName in categoryKey.GetSubKeyNames())
                {
                    using var key = categoryKey.OpenSubKey(subkeyName);
                    var path = (key?.GetValue("Path") as string ?? string.Empty).Trim();
                    var name = key?.GetValue("Name") as string;
                    var enabled = Convert.ToInt32(key?.GetValue("Enabled", 0)) == 1;
                    var identity = $"{category}:{subkeyName}:{path}";
                    records.TryAdd(identity, new ModRecord
                    {
                        Type = ModType.DecalPlugin,
                        Name = string.IsNullOrWhiteSpace(name) ? subkeyName : name,
                        Description = $"Registered Decal {category.TrimEnd('s')} (read-only inventory).",
                        RequiredClientFramework = "Decal",
                        RequiredDependencies = "An installed Decal runtime and the registered plugin files",
                        Enabled = enabled,
                        InstalledPath = path,
                        CompatibilityStatus = File.Exists(path) || Directory.Exists(path)
                            ? CompatibilityStatus.Unknown
                            : CompatibilityStatus.MissingDependency,
                        LastLoadError = File.Exists(path) || Directory.Exists(path) ? string.Empty : "The registered path is missing."
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Inventory remains best-effort and read-only.
            }
        }

        return records.Values.OrderBy(record => record.Name, StringComparer.OrdinalIgnoreCase).ToArray();
    }
}
