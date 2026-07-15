using System.Text.Json;
using System.Text.Json.Nodes;

namespace ACE.SinglePlayer.Mods;

public static class ModMetadataEditor
{
    public static JsonObject Parse(string json)
    {
        var node = JsonNode.Parse(json, documentOptions: new JsonDocumentOptions
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip
        });
        return node as JsonObject ?? throw new JsonException("Meta.json must contain one JSON object.");
    }

    public static void SetEnabled(JsonObject metadata, bool enabled)
    {
        var existingName = metadata.Select(property => property.Key)
            .FirstOrDefault(name => string.Equals(name, "Enabled", StringComparison.OrdinalIgnoreCase));
        metadata[existingName ?? "Enabled"] = enabled;
    }

    public static async Task SetEnabledAsync(string metadataPath, bool enabled, CancellationToken cancellationToken = default)
    {
        var metadata = Parse(await File.ReadAllTextAsync(metadataPath, cancellationToken));
        SetEnabled(metadata, enabled);
        var temporaryPath = metadataPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            await File.WriteAllTextAsync(temporaryPath, metadata.ToJsonString(new JsonSerializerOptions { WriteIndented = true }), cancellationToken);
            File.Move(temporaryPath, metadataPath, true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }
}
