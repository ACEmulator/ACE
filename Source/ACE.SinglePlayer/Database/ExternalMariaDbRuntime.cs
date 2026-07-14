using MySqlConnector;

using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Database;

public sealed class ExternalMariaDbRuntime : IDatabaseRuntime
{
    private static readonly (Func<LauncherSettings, string> Database, string Table)[] RequiredSchemas =
    {
        (settings => settings.AuthenticationDatabaseName, "account"),
        (settings => settings.ShardDatabaseName, "character"),
        (settings => settings.WorldDatabaseName, "weenie")
    };

    private readonly DatabaseConnectionFactory connectionFactory;

    public ExternalMariaDbRuntime(DatabaseConnectionFactory connectionFactory)
    {
        this.connectionFactory = connectionFactory;
    }

    public bool IsManaged => false;

    public async Task StartAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        var result = await ValidateAsync(settings, cancellationToken);
        if (!result.IsValid)
            throw new InvalidOperationException(result.Message);
    }

    public async Task<DatabaseValidationResult> ValidateAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = connectionFactory.Create(settings);
            await connection.OpenAsync(cancellationToken);

            foreach (var required in RequiredSchemas)
            {
                var database = required.Database(settings);
                if (!await SchemaExistsAsync(connection, database, cancellationToken))
                    return new(false, $"Database '{database}' is missing. Use Initialize Database during setup.");
                if (!await TableExistsAsync(connection, database, required.Table, cancellationToken))
                    return new(false, $"Database '{database}' is only partially initialized; required table '{required.Table}' is missing.");
            }

            if (!await RequiredHumanExistsAsync(connection, settings.WorldDatabaseName, cancellationToken))
            {
                return new(false,
                    $"World database '{settings.WorldDatabaseName}' has tables but not the required Human world record (weenie 1). " +
                    "Select the populated ACE-World-Database SQL package; Database\\Base\\WorldBase.sql contains only empty tables.");
            }

            return new(true, "MariaDB/MySQL is reachable and all three ACE databases contain the required ACE data.");
        }
        catch (MySqlException ex)
        {
            return new(false, $"MariaDB/MySQL connection failed: {ex.Message}");
        }
        catch (TimeoutException ex)
        {
            return new(false, ex.Message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    internal static async Task<bool> SchemaExistsAsync(MySqlConnection connection, string database, CancellationToken cancellationToken)
    {
        await using var command = new MySqlCommand(
            "SELECT COUNT(*) FROM information_schema.schemata WHERE schema_name = @database", connection);
        command.Parameters.AddWithValue("@database", database);
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken)) == 1;
    }

    internal static async Task<bool> TableExistsAsync(MySqlConnection connection, string database, string table, CancellationToken cancellationToken)
    {
        await using var command = new MySqlCommand(
            "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = @database AND table_name = @table", connection);
        command.Parameters.AddWithValue("@database", database);
        command.Parameters.AddWithValue("@table", table);
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken)) == 1;
    }

    internal static async Task<bool> RequiredHumanExistsAsync(MySqlConnection connection, string database,
        CancellationToken cancellationToken)
    {
        var escapedDatabase = database.Replace("`", "``", StringComparison.Ordinal);
        await using var command = new MySqlCommand(
            $"SELECT COUNT(*) FROM `{escapedDatabase}`.`weenie` WHERE `class_Id` = 1 AND `class_Name` = 'human'", connection);
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken)) == 1;
    }
}
