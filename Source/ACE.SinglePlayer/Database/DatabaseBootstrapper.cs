using MySqlConnector;

using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Database;

public sealed class DatabaseBootstrapper
{
    private readonly DatabaseConnectionFactory connectionFactory;

    public DatabaseBootstrapper(DatabaseConnectionFactory connectionFactory)
    {
        this.connectionFactory = connectionFactory;
    }

    public async Task BootstrapAsync(LauncherSettings settings, string worldSqlPath, CancellationToken cancellationToken)
    {
        var baseDirectory = Path.Combine(Path.GetDirectoryName(settings.ServerExePath)!, "DatabaseSetupScripts", "Base");
        var authenticationSql = Path.Combine(baseDirectory, "AuthenticationBase.sql");
        var shardSql = Path.Combine(baseDirectory, "ShardBase.sql");
        if (!File.Exists(authenticationSql) || !File.Exists(shardSql))
            throw new FileNotFoundException("ACE authentication and shard base SQL files were not found under Server\\DatabaseSetupScripts\\Base.");

        await using var connection = connectionFactory.Create(settings);
        await connection.OpenAsync(cancellationToken);

        await ApplyIfMissingAsync(connection, settings.AuthenticationDatabaseName, "account", authenticationSql, "ace_auth", cancellationToken);
        await ApplyIfMissingAsync(connection, settings.ShardDatabaseName, "character", shardSql, "ace_shard", cancellationToken);

        if (!await ExternalMariaDbRuntime.SchemaExistsAsync(connection, settings.WorldDatabaseName, cancellationToken))
        {
            if (!File.Exists(worldSqlPath))
                throw new FileNotFoundException("Select an ACE world-database SQL package before initializing a missing world database.", worldSqlPath);
            await ApplyIfMissingAsync(connection, settings.WorldDatabaseName, "weenie", worldSqlPath, "ace_world", cancellationToken);
        }
        else if (!await ExternalMariaDbRuntime.TableExistsAsync(connection, settings.WorldDatabaseName, "weenie", cancellationToken))
        {
            throw new InvalidOperationException($"Database '{settings.WorldDatabaseName}' exists but is partial. It was not modified.");
        }
    }

    private static async Task ApplyIfMissingAsync(MySqlConnection connection, string database, string coreTable,
        string scriptPath, string defaultDatabase, CancellationToken cancellationToken)
    {
        if (await ExternalMariaDbRuntime.SchemaExistsAsync(connection, database, cancellationToken))
        {
            if (!await ExternalMariaDbRuntime.TableExistsAsync(connection, database, coreTable, cancellationToken))
                throw new InvalidOperationException($"Database '{database}' exists but is partial. It was not modified.");
            return;
        }

        var sql = await File.ReadAllTextAsync(scriptPath, cancellationToken);
        sql = sql.Replace($"`{defaultDatabase}`", $"`{database.Replace("`", "``")}`", StringComparison.Ordinal);
        await using var command = new MySqlCommand(sql, connection) { CommandTimeout = 300 };
        try
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Initializing '{database}' failed. The database may now be partial; correct or remove it before retrying. {ex.Message}", ex);
        }

        if (!await ExternalMariaDbRuntime.TableExistsAsync(connection, database, coreTable, cancellationToken))
            throw new InvalidOperationException($"The SQL package completed but '{database}.{coreTable}' is still missing.");
    }
}
