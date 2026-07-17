using System.Diagnostics;
using System.Text;

using MySqlConnector;

using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.CustomContent;

internal sealed class MariaDbWorldBackupService
{
    private readonly DatabaseConnectionFactory connectionFactory;
    private readonly LauncherLog log;

    public MariaDbWorldBackupService(DatabaseConnectionFactory connectionFactory, LauncherLog log)
    {
        this.connectionFactory = connectionFactory;
        this.log = log;
    }

    public async Task<string> CreateAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        var dumpExecutable = FindDumpExecutable(settings.ManagedDatabaseExePath)
            ?? throw new FileNotFoundException(
                "The bundled MariaDB backup program was not found. Extract the complete OpenDereth release again before importing custom content.");
        var privateRoot = Path.GetDirectoryName(settings.PrivateDatabaseDirectory)
            ?? throw new InvalidOperationException("The private database folder has no parent directory.");
        var backupDirectory = GetBackupDirectory(settings);
        Directory.CreateDirectory(backupDirectory);
        var stamp = DateTimeOffset.Now.ToString("yyyyMMdd-HHmmss");
        var backupPath = Path.Combine(backupDirectory,
            $"ace_world-before-custom-weenies-{stamp}-{Guid.NewGuid():N}.sql");
        var optionsPath = Path.Combine(privateRoot, "DatabaseBackup-" + Guid.NewGuid().ToString("N") + ".cnf");

        using var databaseConnection = connectionFactory.Create(settings, settings.WorldDatabaseName);
        var connection = new MySqlConnectionStringBuilder(databaseConnection.ConnectionString);
        var options = $"[client]{Environment.NewLine}" +
                      $"host={connection.Server}{Environment.NewLine}" +
                      $"port={connection.Port}{Environment.NewLine}" +
                      $"user={connection.UserID}{Environment.NewLine}" +
                      $"password={connection.Password}{Environment.NewLine}" +
                      $"protocol=tcp{Environment.NewLine}";

        try
        {
            await File.WriteAllTextAsync(optionsPath, options,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), cancellationToken);
            FilePermissionHardener.RestrictToCurrentUser(optionsPath);

            var startInfo = new ProcessStartInfo
            {
                FileName = dumpExecutable,
                WorkingDirectory = Path.GetDirectoryName(dumpExecutable)!,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            // MariaDB requires the defaults-file argument to be first.
            startInfo.ArgumentList.Add($"--defaults-extra-file={optionsPath}");
            startInfo.ArgumentList.Add("--single-transaction");
            startInfo.ArgumentList.Add("--quick");
            startInfo.ArgumentList.Add("--skip-lock-tables");
            startInfo.ArgumentList.Add("--no-tablespaces");
            startInfo.ArgumentList.Add("--hex-blob");
            startInfo.ArgumentList.Add("--default-character-set=utf8mb4");
            startInfo.ArgumentList.Add($"--result-file={backupPath}");
            startInfo.ArgumentList.Add(settings.WorldDatabaseName);

            using var process = new Process { StartInfo = startInfo };
            log.Write("Creating a complete ace_world backup before importing custom weenies.");
            if (!process.Start())
                throw new InvalidOperationException("The MariaDB backup program did not start.");

            var standardOutput = process.StandardOutput.ReadToEndAsync(cancellationToken);
            var standardError = process.StandardError.ReadToEndAsync(cancellationToken);
            try
            {
                await process.WaitForExitAsync(cancellationToken);
            }
            catch
            {
                if (!process.HasExited)
                    process.Kill(entireProcessTree: true);
                throw;
            }

            var output = await standardOutput;
            var error = await standardError;
            if (process.ExitCode != 0)
                throw new InvalidOperationException(
                    $"MariaDB could not create the safety backup (exit code {process.ExitCode}). {Condense(error)}");
            if (!File.Exists(backupPath) || new FileInfo(backupPath).Length == 0)
                throw new InvalidOperationException("MariaDB reported success, but the safety backup is empty.");
            if (!string.IsNullOrWhiteSpace(output))
                log.Write("[MariaDB backup] " + Condense(output));
            log.Write($"Custom-content safety backup created at '{backupPath}'.");
            return backupPath;
        }
        catch
        {
            TryDelete(backupPath);
            throw;
        }
        finally
        {
            TryDelete(optionsPath);
        }
    }

    internal static string? FindDumpExecutable(string serverExecutable)
    {
        if (string.IsNullOrWhiteSpace(serverExecutable))
            return null;
        var directory = Path.GetDirectoryName(Path.GetFullPath(serverExecutable));
        if (directory is null)
            return null;
        return new[] { "mariadb-dump.exe", "mysqldump.exe" }
            .Select(name => Path.Combine(directory, name))
            .FirstOrDefault(File.Exists);
    }

    internal static string GetBackupDirectory(LauncherSettings settings)
    {
        var root = Path.GetDirectoryName(settings.PrivateDatabaseDirectory)
            ?? ApplicationPaths.LocalRoot;
        return Path.Combine(root, "Backups", "CustomWeenies");
    }

    private static string Condense(string value)
    {
        var condensed = string.Join(" ", value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        return condensed.Length <= 800 ? condensed : condensed[..800] + "...";
    }

    private static void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        catch (IOException)
        {
            // A transient cleanup failure must not hide the original result.
        }
        catch (UnauthorizedAccessException)
        {
            // A transient cleanup failure must not hide the original result.
        }
    }
}
