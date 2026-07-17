using System.Text.Json.Serialization;

using ACE.SinglePlayer.Infrastructure;

namespace ACE.SinglePlayer.Models;

public enum DatabaseMode
{
    Private,
    External,
    // Kept so version-1 settings deserialize and can be migrated safely.
    ManagedExperimental
}

public enum ClientLaunchMode
{
    Vanilla,
    Decal,
    ChoriziteFuture
}

public sealed class LauncherSettings
{
    public const int CurrentVersion = 3;

    public int SettingsVersion { get; set; } = CurrentVersion;
    public string ClientExePath { get; set; } = string.Empty;
    public string ServerExePath { get; set; } = string.Empty;
    public string DatFilesDirectory { get; set; } = string.Empty;
    public string ModsDirectory { get; set; } = string.Empty;
    public string RuntimeDirectory { get; set; } = string.Empty;
    public string Host { get; set; } = "127.0.0.1";
    public ushort Port { get; set; } = 9000;
    public string AccountName { get; set; } = "singleplayer";
    public string ProtectedAccountPassword { get; set; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DatabaseMode DatabaseMode { get; set; } = DatabaseMode.Private;
    public string DatabaseHost { get; set; } = "127.0.0.1";
    public ushort DatabasePort { get; set; } = 3307;
    public string DatabaseUsername { get; set; } = "ace_singleplayer";
    public string ProtectedDatabasePassword { get; set; } = string.Empty;
    public string ProtectedPrivateDatabasePassword { get; set; } = string.Empty;
    public string ProtectedExternalDatabasePassword { get; set; } = string.Empty;
    public string ProtectedPrivateDatabaseAdminPassword { get; set; } = string.Empty;
    public string PrivateDatabaseDirectoryPath { get; set; } = GetDefaultPrivateDatabaseDirectory();
    public string AuthenticationDatabaseName { get; set; } = "ace_auth";
    public string ShardDatabaseName { get; set; } = "ace_shard";
    public string WorldDatabaseName { get; set; } = "ace_world";
    public string ManagedDatabaseExePath { get; set; } = string.Empty;
    public string WorldDatabaseSqlPath { get; set; } = string.Empty;
    public bool StopServerWhenGameExits { get; set; } = true;
    public bool StopManagedDatabaseWhenLauncherExits { get; set; } = true;
    public bool MinimizeLauncherAfterClientStarts { get; set; } = true;
    public int ServerStartupTimeoutSeconds { get; set; } = 180;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClientLaunchMode ClientLaunchMode { get; set; } = ClientLaunchMode.Vanilla;

    [JsonIgnore]
    public string ConfigPath => Path.Combine(RuntimeDirectory, "Config.js");

    [JsonIgnore]
    public string ReadyFilePath => Path.Combine(RuntimeDirectory, "ace-server.ready.json");

    [JsonIgnore]
    public string OwnershipPath => Path.Combine(RuntimeDirectory, "ace-server.process.json");

    [JsonIgnore]
    public string PrivateDatabaseDirectory => string.IsNullOrWhiteSpace(PrivateDatabaseDirectoryPath)
        ? GetDefaultPrivateDatabaseDirectory()
        : PrivateDatabaseDirectoryPath;

    [JsonIgnore]
    public string LegacyPrivateDatabaseDirectory => Path.Combine(RuntimeDirectory, "Database");

    public static LauncherSettings CreateDefaults(string applicationDirectory)
    {
        var root = ApplicationPaths.LocalRoot;
        var packagedClient = Path.Combine(applicationDirectory, "Client", "acclient.exe");
        var packagedServer = Path.Combine(applicationDirectory, "Server", "ACE.Server.exe");

        return new LauncherSettings
        {
            ClientExePath = File.Exists(packagedClient) ? packagedClient : string.Empty,
            ServerExePath = File.Exists(packagedServer) ? packagedServer : string.Empty,
            DatFilesDirectory = File.Exists(packagedClient) ? Path.GetDirectoryName(packagedClient)! : string.Empty,
            ModsDirectory = Path.Combine(applicationDirectory, "Mods"),
            RuntimeDirectory = Path.Combine(root, "Runtime")
        };
    }

    private static string GetDefaultPrivateDatabaseDirectory() => Path.Combine(ApplicationPaths.LocalRoot, "Database");
}
