using System.Text.Json.Nodes;

namespace ACE.SinglePlayer.Mods;

public sealed class AceServerModProvider : IModProvider
{
    private readonly string modsDirectory;

    public AceServerModProvider(string modsDirectory)
    {
        this.modsDirectory = modsDirectory;
    }

    public ModType Type => ModType.AceServer;

    public IReadOnlyList<ModRecord> Scan()
    {
        if (!Directory.Exists(modsDirectory))
            return Array.Empty<ModRecord>();

        return Directory.GetDirectories(modsDirectory)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .Select(Read)
            .ToArray();
    }

    public static ModRecord Read(string directory)
    {
        var metadataPath = Path.Combine(directory, "Meta.json");
        if (!File.Exists(metadataPath))
            return Malformed(directory, metadataPath, "Meta.json is missing.");

        try
        {
            var metadata = ModMetadataEditor.Parse(File.ReadAllText(metadataPath));
            var name = ReadString(metadata, "Name");
            if (string.IsNullOrWhiteSpace(name))
                return Malformed(directory, metadataPath, "Meta.json has no Name.");

            return new ModRecord
            {
                CatalogId = ReadString(metadata, "CatalogId"),
                Type = ModType.AceServer,
                Name = name,
                Author = ReadString(metadata, "Author"),
                Description = ReadString(metadata, "Description"),
                Version = ReadString(metadata, "Version"),
                TargetAceVersion = FirstNonEmpty(ReadString(metadata, "TargetAceVersion"), ReadString(metadata, "ACEVersion")),
                TargetFramework = ReadString(metadata, "TargetFramework"),
                RequiredClientFramework = "None (server-side)",
                RequiredDependencies = ReadNode(metadata, "Dependencies"),
                Priority = ReadUInt(metadata, "Priority"),
                Enabled = ReadBool(metadata, "Enabled", defaultValue: true),
                InstalledPath = directory,
                MetadataPath = metadataPath,
                SettingsPath = Path.Combine(directory, "Settings.json"),
                CompatibilityStatus = CompatibilityStatus.Unknown
            };
        }
        catch (Exception ex) when (ex is IOException or System.Text.Json.JsonException or InvalidOperationException)
        {
            return Malformed(directory, metadataPath, ex.Message);
        }
    }

    private static ModRecord Malformed(string directory, string metadataPath, string error) => new()
    {
        Type = ModType.AceServer,
        Name = Path.GetFileName(directory),
        InstalledPath = directory,
        MetadataPath = metadataPath,
        IsMalformed = true,
        LastLoadError = error,
        CompatibilityStatus = CompatibilityStatus.LoadFailed
    };

    private static JsonNode? Find(JsonObject value, string name) =>
        value.FirstOrDefault(property => string.Equals(property.Key, name, StringComparison.OrdinalIgnoreCase)).Value;

    private static string ReadString(JsonObject value, string name) => Find(value, name)?.ToString() ?? string.Empty;

    private static bool ReadBool(JsonObject value, string name, bool defaultValue) =>
        Find(value, name) is JsonValue node && node.TryGetValue<bool>(out var result) ? result : defaultValue;

    private static uint ReadUInt(JsonObject value, string name) =>
        Find(value, name) is JsonValue node && node.TryGetValue<uint>(out var result) ? result : 0;

    private static string ReadNode(JsonObject value, string name) => Find(value, name)?.ToJsonString() ?? string.Empty;

    private static string FirstNonEmpty(string first, string second) => string.IsNullOrWhiteSpace(first) ? second : first;
}
