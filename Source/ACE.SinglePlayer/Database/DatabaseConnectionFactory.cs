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
        return Create(settings, settings.DatabaseUsername,
            secretProtector.Unprotect(settings.ProtectedDatabasePassword), database);
    }

    public MySqlConnection CreatePrivateAdministrator(LauncherSettings settings)
    {
        if (settings.DatabaseMode == DatabaseMode.External)
            throw new InvalidOperationException("Private administrator credentials are not available in external database mode.");

        return Create(settings, "root",
            secretProtector.Unprotect(settings.ProtectedPrivateDatabaseAdminPassword), database: null);
    }

    private static MySqlConnection Create(LauncherSettings settings, string username, string password, string? database)
    {
        var builder = new MySqlConnectionStringBuilder
        {
            Server = settings.DatabaseHost,
            Port = settings.DatabasePort,
            UserID = username,
            Password = password,
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
