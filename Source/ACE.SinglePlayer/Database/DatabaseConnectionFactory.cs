using MySqlConnector;

using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Database;

public sealed class DatabaseConnectionFactory
{
    private readonly ISecretProtector secretProtector;

    public DatabaseConnectionFactory(ISecretProtector secretProtector)
    {
        this.secretProtector = secretProtector;
    }

    public MySqlConnection Create(LauncherSettings settings, string? database = null)
    {
        var builder = new MySqlConnectionStringBuilder
        {
            Server = settings.DatabaseHost,
            Port = settings.DatabasePort,
            UserID = settings.DatabaseUsername,
            Password = secretProtector.Unprotect(settings.ProtectedDatabasePassword),
            Database = database ?? string.Empty,
            SslMode = MySqlSslMode.None,
            AllowPublicKeyRetrieval = true,
            AllowUserVariables = true,
            ConnectionTimeout = 5,
            DefaultCommandTimeout = 120,
            ApplicationName = "ACE.SinglePlayer"
        };
        return new MySqlConnection(builder.ConnectionString);
    }
}
