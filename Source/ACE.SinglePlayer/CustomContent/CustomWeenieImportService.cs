using System.Text.Json;

using MySqlConnector;

using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.CustomContent;

internal sealed record ExistingWeenie(uint ClassId, string ClassName);

internal sealed record CustomWeenieImportResult(bool Imported, string BackupPath)
{
    public static CustomWeenieImportResult Canceled { get; } = new(false, string.Empty);
}

internal sealed class CustomWeenieImportService
{
    private readonly DatabaseRuntimeFactory runtimeFactory;
    private readonly DatabaseConnectionFactory connectionFactory;
    private readonly MariaDbWorldBackupService backupService;
    private readonly LauncherLog log;

    public CustomWeenieImportService(DatabaseRuntimeFactory runtimeFactory,
        DatabaseConnectionFactory connectionFactory, LauncherLog log)
    {
        this.runtimeFactory = runtimeFactory;
        this.connectionFactory = connectionFactory;
        this.log = log;
        backupService = new MariaDbWorldBackupService(connectionFactory, log);
    }

    public async Task<CustomWeenieImportResult> ImportAsync(
        LauncherSettings settings,
        IReadOnlyList<CustomWeenieDefinition> definitions,
        Func<IReadOnlyList<ExistingWeenie>, bool> confirmReplacement,
        IProgress<string>? progress,
        CancellationToken cancellationToken)
    {
        if (settings.DatabaseMode != DatabaseMode.Private)
            throw new InvalidOperationException(
                "Automatic Custom Weenie imports currently require the launcher's Private Database mode so a verified backup can be created first.");
        if (definitions.Count == 0)
            throw new InvalidOperationException("No valid custom weenies were selected.");
        var duplicate = definitions.GroupBy(item => item.ClassId).FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
            throw new InvalidOperationException($"WCID {duplicate.Key} appears more than once in this import.");

        progress?.Report("Starting the private database...");
        await using var runtime = runtimeFactory.Create(settings);
        await runtime.StartAsync(settings, cancellationToken);

        await using var connection = connectionFactory.Create(settings, settings.WorldDatabaseName);
        await connection.OpenAsync(cancellationToken);
        var existing = await FindExistingAsync(connection, definitions, cancellationToken);
        if (existing.Count > 0 && !confirmReplacement(existing))
            return CustomWeenieImportResult.Canceled;

        progress?.Report("Creating a complete ace_world safety backup...");
        var backupPath = await backupService.CreateAsync(settings, cancellationToken);

        progress?.Report($"Importing {definitions.Count} custom weenie{(definitions.Count == 1 ? string.Empty : "s")}...");
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            foreach (var definition in definitions)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await using var command = new MySqlCommand(definition.Sql, connection, transaction)
                {
                    CommandTimeout = 120
                };
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            try
            {
                await transaction.RollbackAsync(CancellationToken.None);
            }
            catch (MySqlException rollbackError)
            {
                log.Write("Custom weenie import rollback reported an error: " + rollbackError.Message);
            }
            throw;
        }

        log.Write($"Imported {definitions.Count} custom weenie(s): {string.Join(", ", definitions.Select(item => item.ClassId))}.");
        await WriteManifestBestEffortAsync(backupPath, definitions);
        progress?.Report("Import complete. The new content will be available the next time the server starts.");
        return new CustomWeenieImportResult(true, backupPath);
    }

    private static async Task<IReadOnlyList<ExistingWeenie>> FindExistingAsync(MySqlConnection connection,
        IReadOnlyList<CustomWeenieDefinition> definitions, CancellationToken cancellationToken)
    {
        var parameterNames = definitions.Select((_, index) => "@id" + index).ToArray();
        await using var command = new MySqlCommand(
            $"SELECT `class_Id`, `class_Name` FROM `weenie` WHERE `class_Id` IN ({string.Join(",", parameterNames)}) ORDER BY `class_Id`",
            connection);
        for (var index = 0; index < definitions.Count; index++)
            command.Parameters.AddWithValue(parameterNames[index], definitions[index].ClassId);

        var result = new List<ExistingWeenie>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
            result.Add(new ExistingWeenie(reader.GetUInt32(0), reader.GetString(1)));
        return result;
    }

    private async Task WriteManifestBestEffortAsync(string backupPath,
        IReadOnlyList<CustomWeenieDefinition> definitions)
    {
        try
        {
            var manifestPath = Path.ChangeExtension(backupPath, ".import.json");
            var manifest = new
            {
                importedAt = DateTimeOffset.Now,
                safetyBackup = backupPath,
                weenies = definitions.Select(item => new
                {
                    wcid = item.ClassId,
                    name = item.ClassName,
                    type = item.WeenieType,
                    sourceFile = item.FilePath,
                    sha256 = item.Sha256
                }).ToArray()
            };
            await File.WriteAllTextAsync(manifestPath,
                JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            log.Write("The custom weenie import succeeded, but its history manifest could not be written: " + ex.Message);
        }
    }
}
