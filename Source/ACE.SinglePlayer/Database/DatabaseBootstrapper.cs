using System.Diagnostics;
using System.Text;

using MySqlConnector;

using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Database;

public sealed class DatabaseBootstrapper
{
    private readonly DatabaseConnectionFactory connectionFactory;
    private readonly LauncherLog? log;

    public DatabaseBootstrapper(DatabaseConnectionFactory connectionFactory, LauncherLog? log = null)
    {
        this.connectionFactory = connectionFactory;
        this.log = log;
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
        await PrepareWorldAsync(connection, settings, worldSqlPath, cancellationToken);
    }

    private async Task PrepareWorldAsync(MySqlConnection connection, LauncherSettings settings, string worldSqlPath,
        CancellationToken cancellationToken)
    {
        var schemaExists = await ExternalMariaDbRuntime.SchemaExistsAsync(connection, settings.WorldDatabaseName, cancellationToken);
        var tableExists = schemaExists && await ExternalMariaDbRuntime.TableExistsAsync(
            connection, settings.WorldDatabaseName, "weenie", cancellationToken);
        var requiredDataExists = tableExists && await ExternalMariaDbRuntime.RequiredHumanExistsAsync(
            connection, settings.WorldDatabaseName, cancellationToken);
        if (requiredDataExists)
            return;

        if (schemaExists && settings.DatabaseMode == DatabaseMode.External)
            throw new InvalidOperationException(
                $"Database '{settings.WorldDatabaseName}' exists but is missing required ACE world data. It was not modified.");

        if (!File.Exists(worldSqlPath))
            throw new FileNotFoundException(
                "The bundled populated ACE World database is missing. Extract the complete release again, or select an advanced replacement in Settings.", worldSqlPath);
        if (!await WorldSqlPackageInspector.ContainsRequiredWorldDataAsync(worldSqlPath, cancellationToken))
            throw new InvalidDataException(
                "The configured World SQL file contains empty table definitions but not the required ACE world data. " +
                "Re-extract the complete release or select a populated ACE-World-Database package in advanced Settings.");

        if (settings.DatabaseMode == DatabaseMode.Private)
            await ImportPrivateWorldAsync(settings, worldSqlPath, cancellationToken);
        else
            await ApplySqlAsync(connection, settings.WorldDatabaseName, "weenie", worldSqlPath, "ace_world", cancellationToken);

        if (!await ExternalMariaDbRuntime.TableExistsAsync(connection, settings.WorldDatabaseName, "weenie", cancellationToken) ||
            !await ExternalMariaDbRuntime.RequiredHumanExistsAsync(connection, settings.WorldDatabaseName, cancellationToken))
        {
            throw new InvalidOperationException(
                $"The world SQL package completed but '{settings.WorldDatabaseName}' still lacks the required Human world record (weenie 1).");
        }
    }

    private async Task ImportPrivateWorldAsync(LauncherSettings settings, string scriptPath,
        CancellationToken cancellationToken)
    {
        var client = MariaDbInstallationLocator.FindClient(settings.ManagedDatabaseExePath)
            ?? throw new FileNotFoundException(
                "The MariaDB import program (mariadb.exe or mysql.exe) was not found beside mariadbd.exe. Repair the MariaDB installation and retry.");
        var privateRoot = Path.GetDirectoryName(settings.PrivateDatabaseDirectory)
            ?? throw new InvalidOperationException("The private database directory has no parent directory.");
        Directory.CreateDirectory(privateRoot);
        var optionsPath = Path.Combine(privateRoot, "DatabaseImport-" + Guid.NewGuid().ToString("N") + ".cnf");
        var targetDatabase = settings.WorldDatabaseName.Replace("`", "``", StringComparison.Ordinal);
        using var administrator = connectionFactory.CreatePrivateAdministrator(settings);
        var connection = new MySqlConnectionStringBuilder(administrator.ConnectionString);
        var options = $"[client]{Environment.NewLine}" +
            $"host={connection.Server}{Environment.NewLine}" +
            $"port={connection.Port}{Environment.NewLine}" +
            $"user={connection.UserID}{Environment.NewLine}" +
            $"password={connection.Password}{Environment.NewLine}" +
            $"protocol=tcp{Environment.NewLine}";

        try
        {
            await File.WriteAllTextAsync(optionsPath, options, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), cancellationToken);
            FilePermissionHardener.RestrictToCurrentUser(optionsPath);

            var startInfo = new ProcessStartInfo
            {
                FileName = client,
                WorkingDirectory = Path.GetDirectoryName(client)!,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            // MariaDB requires the defaults-file argument to be first.
            startInfo.ArgumentList.Add($"--defaults-extra-file={optionsPath}");
            startInfo.ArgumentList.Add("--binary-mode=1");
            startInfo.ArgumentList.Add("--max-allowed-packet=1G");

            using var process = new Process { StartInfo = startInfo };
            log?.Write($"Importing the populated ACE world database from '{Path.GetFileName(scriptPath)}'. This can take several minutes.");
            if (!process.Start())
                throw new InvalidOperationException("The MariaDB world import process did not start.");

            var standardOutput = process.StandardOutput.ReadToEndAsync();
            var standardError = process.StandardError.ReadToEndAsync();
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TimeSpan.FromMinutes(20));
            var importToken = timeout.Token;
            Exception? copyError = null;
            try
            {
                try
                {
                    await using var sql = new FileStream(scriptPath, FileMode.Open, FileAccess.Read, FileShare.Read,
                        bufferSize: 1024 * 1024, FileOptions.Asynchronous | FileOptions.SequentialScan);
                    await CopyReplacingAsync(sql, process.StandardInput.BaseStream,
                        Encoding.UTF8.GetBytes("`ace_world`"), Encoding.UTF8.GetBytes($"`{targetDatabase}`"), importToken);
                    await process.StandardInput.BaseStream.FlushAsync(importToken);
                }
                catch (Exception ex) when (ex is IOException or ObjectDisposedException)
                {
                    copyError = ex;
                }
                finally
                {
                    process.StandardInput.Close();
                }

                await process.WaitForExitAsync(importToken);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException("The ACE world database import did not finish within 20 minutes.");
            }
            finally
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                    await process.WaitForExitAsync(CancellationToken.None);
                }
            }

            var output = Condense((await standardError) + " " + (await standardOutput));
            if (process.ExitCode != 0 || copyError is not null)
            {
                throw new InvalidOperationException(
                    $"The ACE world database import failed with exit code {process.ExitCode}. {output}", copyError);
            }

            log?.Write("The populated ACE world database imported successfully.");
        }
        finally
        {
            if (File.Exists(optionsPath))
            {
                try
                {
                    File.Delete(optionsPath);
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    log?.Write("The temporary private-database import settings could not be removed: " + ex.Message);
                }
            }
        }
    }

    internal static async Task CopyReplacingAsync(Stream source, Stream destination, byte[] search, byte[] replacement,
        CancellationToken cancellationToken)
    {
        if (search.Length == 0)
            throw new ArgumentException("The search sequence cannot be empty.", nameof(search));

        var buffer = new byte[1024 * 1024 + search.Length - 1];
        var retained = 0;
        while (true)
        {
            var read = await source.ReadAsync(buffer.AsMemory(retained, buffer.Length - retained), cancellationToken);
            var total = retained + read;
            var finalBlock = read == 0;
            var processThrough = finalBlock ? total : Math.Max(0, total - search.Length + 1);
            var segmentStart = 0;
            var index = 0;

            while (index < processThrough && index <= total - search.Length)
            {
                if (buffer.AsSpan(index, search.Length).SequenceEqual(search))
                {
                    await destination.WriteAsync(buffer.AsMemory(segmentStart, index - segmentStart), cancellationToken);
                    await destination.WriteAsync(replacement, cancellationToken);
                    index += search.Length;
                    segmentStart = index;
                }
                else
                {
                    index++;
                }
            }

            var keepFrom = finalBlock ? total : Math.Max(segmentStart, processThrough);
            await destination.WriteAsync(buffer.AsMemory(segmentStart, keepFrom - segmentStart), cancellationToken);
            if (finalBlock)
                return;

            retained = total - keepFrom;
            buffer.AsSpan(keepFrom, retained).CopyTo(buffer);
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

        await ApplySqlAsync(connection, database, coreTable, scriptPath, defaultDatabase, cancellationToken);
    }

    private static async Task ApplySqlAsync(MySqlConnection connection, string database, string coreTable,
        string scriptPath, string defaultDatabase, CancellationToken cancellationToken)
    {
        var sql = await File.ReadAllTextAsync(scriptPath, cancellationToken);
        sql = sql.Replace($"`{defaultDatabase}`", $"`{database.Replace("`", "``", StringComparison.Ordinal)}`", StringComparison.Ordinal);
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

    private static string Condense(string value)
    {
        var condensed = string.Join(" ", value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        return condensed.Length <= 1000 ? condensed : condensed[..1000] + "...";
    }
}
