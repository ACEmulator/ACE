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
        SettingsPath = settingsPath ?? Path.Combine(ApplicationPaths.LocalRoot, "settings.json");
    }

    public string SettingsPath { get; }

    public bool Exists => File.Exists(SettingsPath);

    public async Task<LauncherSettings?> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(SettingsPath))
            return null;

        LauncherSettings? settings;
        await using (var stream = File.OpenRead(SettingsPath))
            settings = await JsonSerializer.DeserializeAsync<LauncherSettings>(stream, JsonOptions, cancellationToken);
        if (settings is null)
            throw new InvalidDataException("The launcher settings file is empty or invalid.");
        if (settings.SettingsVersion > LauncherSettings.CurrentVersion)
            throw new InvalidDataException($"Settings version {settings.SettingsVersion} is newer than this launcher supports.");

        var requiresSave = settings.SettingsVersion < LauncherSettings.CurrentVersion;
        Migrate(settings);
        settings.SettingsVersion = LauncherSettings.CurrentVersion;
        if (requiresSave)
            await SaveAsync(settings, cancellationToken);
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

    private static void Migrate(LauncherSettings settings)
    {
        if (settings.SettingsVersion < 2)
        {
            var usedExperimentalManagedMode = settings.DatabaseMode == DatabaseMode.ManagedExperimental;
            var usedLocalRoot = settings.DatabaseMode == DatabaseMode.External &&
                string.Equals(settings.DatabaseHost, "127.0.0.1", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(settings.DatabaseUsername, "root", StringComparison.OrdinalIgnoreCase);

            if (usedExperimentalManagedMode || usedLocalRoot)
            {
                var previousPassword = settings.ProtectedDatabasePassword;
                var previousUserWasRoot = string.Equals(settings.DatabaseUsername, "root", StringComparison.OrdinalIgnoreCase);
                settings.DatabaseMode = DatabaseMode.Private;
                settings.DatabaseHost = "127.0.0.1";
                settings.DatabasePort = 3307;
                settings.DatabaseUsername = "ace_singleplayer";
                if (usedExperimentalManagedMode && previousUserWasRoot)
                    settings.ProtectedPrivateDatabaseAdminPassword = previousPassword;
                else
                    settings.ProtectedExternalDatabasePassword = previousPassword;
                settings.ProtectedDatabasePassword = string.Empty;
            }
        }

        if (settings.SettingsVersion < 3)
        {
            settings.RuntimeDirectory = ApplicationPaths.ReplaceLegacyRoot(settings.RuntimeDirectory);
            settings.PrivateDatabaseDirectoryPath = ApplicationPaths.ReplaceLegacyRoot(settings.PrivateDatabaseDirectoryPath);
        }
    }
}
