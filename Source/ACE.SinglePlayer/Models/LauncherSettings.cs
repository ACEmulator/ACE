using System.Text.Json.Serialization;

namespace ACE.SinglePlayer.Models;

public enum DatabaseMode
{
    External,
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
    public const int CurrentVersion = 1;

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
    public DatabaseMode DatabaseMode { get; set; } = DatabaseMode.External;
    public string DatabaseHost { get; set; } = "127.0.0.1";
    public ushort DatabasePort { get; set; } = 3306;
    public string DatabaseUsername { get; set; } = "root";
    public string ProtectedDatabasePassword { get; set; } = string.Empty;
    public string AuthenticationDatabaseName { get; set; } = "ace_auth";
    public string ShardDatabaseName { get; set; } = "ace_shard";
    public string WorldDatabaseName { get; set; } = "ace_world";
    public string ManagedDatabaseExePath { get; set; } = string.Empty;
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

    public static LauncherSettings CreateDefaults(string applicationDirectory)
    {
        var root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ACESinglePlayer");
        var packagedClient = Path.Combine(applicationDirectory, "Client", "acclient.exe");
        var packagedServer = Path.Combine(applicationDirectory, "Server", "ACE.Server.exe");

        return new LauncherSettings
        {
            ClientExePath = File.Exists(packagedClient) ? packagedClient : string.Empty,
            ServerExePath = File.Exists(packagedServer) ? packagedServer : string.Empty,
            DatFilesDirectory = File.Exists(packagedClient) ? Path.GetDirectoryName(packagedClient)! : string.Empty,
            ModsDirectory = Path.Combine(applicationDirectory, "Mods"),
            RuntimeDirectory = Path.Combine(applicationDirectory, "Runtime")
        };
    }
}
