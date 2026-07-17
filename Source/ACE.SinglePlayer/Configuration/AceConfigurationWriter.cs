using System.Text.Json;

using ACE.Common;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Configuration;

public sealed class AceConfigurationWriter
{
    private readonly ISecretProtector secretProtector;

    public AceConfigurationWriter(ISecretProtector secretProtector)
    {
        this.secretProtector = secretProtector;
    }

    public MasterConfiguration Create(LauncherSettings settings)
    {
        var databasePassword = secretProtector.Unprotect(settings.ProtectedDatabasePassword);
        var configuration = new MasterConfiguration();
        configuration.Server.WorldName = "OpenDereth";
        configuration.Server.Network.Host = "127.0.0.1";
        configuration.Server.Network.Port = settings.Port;
        configuration.Server.Network.MaximumAllowedSessions = 4;
        configuration.Server.Network.MaximumAllowedSessionsPerIPAddress = 4;
        configuration.Server.Network.AllowUnlimitedSessionsFromIPAddresses = Array.Empty<string>();
        configuration.Server.Accounts.AllowAutoAccountCreation = true;
        configuration.Server.DatFilesDirectory = Path.GetFullPath(settings.DatFilesDirectory);
        configuration.Server.ModsDirectory = Path.GetFullPath(settings.ModsDirectory);
        configuration.Server.WorldDatabasePrecaching = false;
        configuration.Server.ServerPerformanceMonitorAutoStart = false;
        configuration.Server.ShutdownInterval = 0;

        SetDatabase(configuration.MySql.Authentication, settings, settings.AuthenticationDatabaseName, databasePassword);
        SetDatabase(configuration.MySql.Shard, settings, settings.ShardDatabaseName, databasePassword);
        SetDatabase(configuration.MySql.World, settings, settings.WorldDatabaseName, databasePassword);

        configuration.Offline.AutoApplyDatabaseUpdates = true;
        configuration.Offline.AutoUpdateWorldDatabase = false;
        configuration.Offline.AutoApplyWorldCustomizations = false;
        configuration.Offline.AutoServerUpdateCheck = false;
        configuration.DDD.EnableDATPatching = false;
        configuration.DDD.PrecacheCompressedDATFiles = false;
        return configuration;
    }

    public async Task WriteAsync(LauncherSettings settings, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(settings.RuntimeDirectory);
        var temporaryPath = settings.ConfigPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            await using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                await JsonSerializer.SerializeAsync(stream, Create(settings), ConfigManager.SerializerOptions, cancellationToken);
            File.Move(temporaryPath, settings.ConfigPath, true);
            FilePermissionHardener.RestrictToCurrentUser(settings.ConfigPath);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }

    private static void SetDatabase(MySqlConfiguration target, LauncherSettings settings, string database, string password)
    {
        target.Host = settings.DatabaseHost;
        target.Port = settings.DatabasePort;
        target.Database = database;
        target.Username = settings.DatabaseUsername;
        target.Password = password;
        target.EnableDetailedErrors = false;
        target.EnableSensitiveDataLogging = false;
    }
}
