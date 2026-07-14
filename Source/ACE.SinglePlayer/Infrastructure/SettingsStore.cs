using System.Text.Json;
using System.Text.Json.Serialization;

using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Infrastructure;

public sealed class SettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public SettingsStore(string? settingsPath = null)
    {
        SettingsPath = settingsPath ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ACESinglePlayer", "settings.json");
    }

    public string SettingsPath { get; }

    public bool Exists => File.Exists(SettingsPath);

    public async Task<LauncherSettings?> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(SettingsPath))
            return null;

        await using var stream = File.OpenRead(SettingsPath);
        var settings = await JsonSerializer.DeserializeAsync<LauncherSettings>(stream, JsonOptions, cancellationToken);
        if (settings is null)
            throw new InvalidDataException("The launcher settings file is empty or invalid.");
        if (settings.SettingsVersion > LauncherSettings.CurrentVersion)
            throw new InvalidDataException($"Settings version {settings.SettingsVersion} is newer than this launcher supports.");

        settings.SettingsVersion = LauncherSettings.CurrentVersion;
        return settings;
    }

    public async Task SaveAsync(LauncherSettings settings, CancellationToken cancellationToken = default)
    {
        settings.SettingsVersion = LauncherSettings.CurrentVersion;
        var directory = Path.GetDirectoryName(SettingsPath)!;
        Directory.CreateDirectory(directory);

        var temporaryPath = SettingsPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            await using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                await JsonSerializer.SerializeAsync(stream, settings, JsonOptions, cancellationToken);
            File.Move(temporaryPath, SettingsPath, true);
            FilePermissionHardener.RestrictToCurrentUser(SettingsPath);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }
}
