using System.Diagnostics;
using System.Text;

using MySqlConnector;

using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Database;

public sealed class ManagedMariaDbRuntime : IDatabaseRuntime
{
    private const string PrivateUser = "ace_singleplayer";
    private readonly DatabaseConnectionFactory connectionFactory;
    private readonly ExternalMariaDbRuntime validator;
    private readonly LauncherLog log;
    private Process? process;
    private LauncherSettings? activeSettings;

    public ManagedMariaDbRuntime(DatabaseConnectionFactory connectionFactory, ExternalMariaDbRuntime validator, LauncherLog log)
    {
        this.connectionFactory = connectionFactory;
        this.validator = validator;
        this.log = log;
    }

    public bool IsManaged => true;

    public async Task StartAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        if (process is { HasExited: false })
            return;

        ValidatePrivateSettings(settings);
        activeSettings = settings;

        var dataDirectory = settings.PrivateDatabaseDirectory;
        await MigrateLegacyDataDirectoryAsync(settings, cancellationToken);
        var isInitialized = IsInitialized(dataDirectory);

        if (!PrivateDatabasePortFinder.IsAvailable(settings.DatabasePort))
        {
            if (await CanConnectAsPrivateAdministratorAsync(settings, cancellationToken))
            {
                log.Write($"Using the already-running private MariaDB on 127.0.0.1:{settings.DatabasePort}.");
                await ProvisionApplicationUserAsync(settings, cancellationToken);
                return;
            }

            settings.DatabasePort = PrivateDatabasePortFinder.FindAvailablePort();
            log.Write($"Private MariaDB selected available loopback port {settings.DatabasePort}.");
        }

        if (!isInitialized)
            await InitializeDataDirectoryAsync(settings, cancellationToken);

        FilePermissionHardener.RestrictDirectoryToCurrentUser(dataDirectory);
        process = StartServerProcess(settings);

        try
        {
            await WaitForAdministratorConnectionAsync(settings, cancellationToken);
            await ProvisionApplicationUserAsync(settings, cancellationToken);
            log.Write("Private MariaDB is ready and its ACE-only database user is configured.");
        }
        catch
        {
            await StopAsync(CancellationToken.None);
            throw;
        }
    }

    public Task<DatabaseValidationResult> ValidateAsync(LauncherSettings settings, CancellationToken cancellationToken) =>
        validator.ValidateAsync(settings, cancellationToken);

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (process is null || process.HasExited)
            return;

        if (activeSettings is not null)
        {
            try
            {
                await using var connection = connectionFactory.CreatePrivateAdministrator(activeSettings);
                await connection.OpenAsync(cancellationToken);
                await using var command = new MySqlCommand("SHUTDOWN", connection) { CommandTimeout = 10 };
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (Exception ex) when (ex is MySqlException or TimeoutException or OperationCanceledException)
            {
                log.Write("Private MariaDB did not accept a graceful shutdown request: " + ex.Message);
            }
        }

        try
        {
            await process.WaitForExitAsync(cancellationToken).WaitAsync(TimeSpan.FromSeconds(15), cancellationToken);
        }
        catch (TimeoutException)
        {
            log.Write("Private MariaDB did not stop within 15 seconds; stopping only the launcher-owned process.");
            process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync(cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync(CancellationToken.None);
        process?.Dispose();
    }

    internal static IReadOnlyList<string> CreateInitializationArguments(string dataDirectory, string adminPassword, ushort port) =>
        new[]
        {
            $"--datadir={dataDirectory}",
            $"--password={adminPassword}",
            $"--port={port}",
            "--silent"
        };

    internal static IReadOnlyList<string> CreateServerArguments(string dataDirectory, ushort port) =>
        new[]
        {
            "--no-defaults",
            $"--datadir={dataDirectory}",
            $"--port={port}",
            "--bind-address=127.0.0.1",
            "--skip-networking=0",
            "--max-connections=20",
            "--local-infile=0",
            "--console"
        };

    private static bool IsInitialized(string dataDirectory) => Directory.Exists(Path.Combine(dataDirectory, "mysql"));

    private async Task MigrateLegacyDataDirectoryAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        var source = Path.GetFullPath(settings.LegacyPrivateDatabaseDirectory);
        var target = Path.GetFullPath(settings.PrivateDatabaseDirectory);
        if (string.Equals(source, target, StringComparison.OrdinalIgnoreCase) || !IsInitialized(source) ||
            (Directory.Exists(target) && Directory.EnumerateFileSystemEntries(target).Any()))
            return;

        if (Directory.Exists(target))
            Directory.Delete(target);

        var parent = Path.GetDirectoryName(target)
            ?? throw new InvalidOperationException("The private database directory has no parent directory.");
        Directory.CreateDirectory(parent);
        var staging = target + ".migrating-" + Guid.NewGuid().ToString("N");
        log.Write("Moving the private MariaDB data out of the launcher folder so cloud-sync software cannot lock its database files.");

        try
        {
            Directory.CreateDirectory(staging);
            foreach (var sourceDirectory in Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories))
            {
                cancellationToken.ThrowIfCancellationRequested();
                Directory.CreateDirectory(Path.Combine(staging, Path.GetRelativePath(source, sourceDirectory)));
            }

            foreach (var sourceFile in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var destinationFile = Path.Combine(staging, Path.GetRelativePath(source, sourceFile));
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile)!);
                await using var input = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 1024 * 1024, FileOptions.Asynchronous | FileOptions.SequentialScan);
                await using var output = new FileStream(destinationFile, FileMode.CreateNew, FileAccess.Write, FileShare.None,
                    bufferSize: 1024 * 1024, FileOptions.Asynchronous | FileOptions.SequentialScan);
                await input.CopyToAsync(output, cancellationToken);
            }

            if (!IsInitialized(staging))
                throw new InvalidOperationException("The copied private database does not contain MariaDB system tables.");
            FilePermissionHardener.RestrictDirectoryToCurrentUser(staging);
            Directory.Move(staging, target);
            log.Write($"Private MariaDB data moved to '{target}'. The previous copy was retained at '{source}' as a backup.");
        }
        catch
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
            throw;
        }
    }

    private static void ValidatePrivateSettings(LauncherSettings settings)
    {
        if (settings.DatabaseMode == DatabaseMode.External)
            throw new InvalidOperationException("The private MariaDB runtime cannot start in external database mode.");
        if (!File.Exists(settings.ManagedDatabaseExePath))
            throw new FileNotFoundException("The bundled MariaDB runtime was not found. Extract the complete release again, or select mariadbd.exe in advanced Settings.", settings.ManagedDatabaseExePath);
        if (MariaDbInstallationLocator.FindInitializer(settings.ManagedDatabaseExePath) is null)
            throw new FileNotFoundException("The MariaDB initializer was not found beside mariadbd.exe. Repair the MariaDB installation and retry.");
        if (!string.Equals(settings.DatabaseHost, "127.0.0.1", StringComparison.Ordinal))
            throw new InvalidOperationException("The private database must use 127.0.0.1.");
        if (!string.Equals(settings.DatabaseUsername, PrivateUser, StringComparison.Ordinal))
            throw new InvalidOperationException($"The private database must use its isolated '{PrivateUser}' account.");
        if (string.IsNullOrWhiteSpace(settings.ProtectedDatabasePassword) ||
            string.IsNullOrWhiteSpace(settings.ProtectedPrivateDatabaseAdminPassword))
            throw new InvalidOperationException("Private database credentials have not been generated. Open Settings and save the setup again.");
    }

    private async Task InitializeDataDirectoryAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        var target = settings.PrivateDatabaseDirectory;
        if (Directory.Exists(target) && Directory.EnumerateFileSystemEntries(target).Any())
            throw new InvalidOperationException(
                $"The private database folder exists but is not a valid initialized MariaDB data directory: {target}. Its contents were left untouched.");

        var parent = Path.GetDirectoryName(target)
            ?? throw new InvalidOperationException("The private database directory has no parent directory.");
        Directory.CreateDirectory(parent);
        var staging = Path.Combine(parent, "Database.initializing-" + Guid.NewGuid().ToString("N"));
        var initializer = MariaDbInstallationLocator.FindInitializer(settings.ManagedDatabaseExePath)!;
        var adminPassword = GetAdministratorPassword(settings);
        var output = new StringBuilder();

        try
        {
            using var initializerProcess = new Process
            {
                StartInfo = CreateProcessStartInfo(initializer, Path.GetDirectoryName(initializer)!,
                    CreateInitializationArguments(staging, adminPassword, settings.DatabasePort)),
                EnableRaisingEvents = true
            };
            initializerProcess.OutputDataReceived += (_, args) => AppendRedacted(output, args.Data, adminPassword);
            initializerProcess.ErrorDataReceived += (_, args) => AppendRedacted(output, args.Data, adminPassword);

            log.Write("Initializing a private MariaDB data directory for this launcher. The system MariaDB service is not being changed.");
            if (!initializerProcess.Start())
                throw new InvalidOperationException("The MariaDB initializer did not start.");
            initializerProcess.BeginOutputReadLine();
            initializerProcess.BeginErrorReadLine();

            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TimeSpan.FromMinutes(2));
            try
            {
                await initializerProcess.WaitForExitAsync(timeout.Token);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                if (!initializerProcess.HasExited)
                    initializerProcess.Kill(entireProcessTree: true);
                throw new TimeoutException("MariaDB initialization did not finish within two minutes.");
            }

            if (initializerProcess.ExitCode != 0 || !IsInitialized(staging))
                throw new InvalidOperationException(
                    $"MariaDB initialization failed with exit code {initializerProcess.ExitCode}. {Condense(Snapshot(output))}");

            if (Directory.Exists(target))
                Directory.Delete(target);
            Directory.Move(staging, target);
            FilePermissionHardener.RestrictDirectoryToCurrentUser(target);
            log.Write("Private MariaDB data directory initialized successfully.");
        }
        finally
        {
            if (Directory.Exists(staging))
            {
                try
                {
                    Directory.Delete(staging, recursive: true);
                }
                catch (IOException ex)
                {
                    log.Write("A failed MariaDB initialization staging folder could not be removed: " + ex.Message);
                }
            }
        }
    }

    private Process StartServerProcess(LauncherSettings settings)
    {
        var startInfo = CreateProcessStartInfo(settings.ManagedDatabaseExePath,
            Path.GetDirectoryName(settings.ManagedDatabaseExePath)!,
            CreateServerArguments(settings.PrivateDatabaseDirectory, settings.DatabasePort));

        var serverProcess = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
        var adminPassword = GetAdministratorPassword(settings);
        var applicationPassword = GetApplicationPassword(settings);
        serverProcess.OutputDataReceived += (_, args) => LogMariaDbLine(args.Data, adminPassword, applicationPassword);
        serverProcess.ErrorDataReceived += (_, args) => LogMariaDbLine(args.Data, adminPassword, applicationPassword);
        if (!serverProcess.Start())
            throw new InvalidOperationException("Private MariaDB did not start.");
        serverProcess.BeginOutputReadLine();
        serverProcess.BeginErrorReadLine();
        return serverProcess;
    }

    private async Task WaitForAdministratorConnectionAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        var deadline = DateTime.UtcNow.AddSeconds(45);
        Exception? lastError = null;
        while (DateTime.UtcNow < deadline)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (process is { HasExited: true })
                throw new InvalidOperationException($"Private MariaDB exited unexpectedly (exit code {process.ExitCode}). Open Logs for details.");

            try
            {
                await using var connection = connectionFactory.CreatePrivateAdministrator(settings);
                await connection.OpenAsync(cancellationToken);
                return;
            }
            catch (MySqlException ex)
            {
                lastError = ex;
            }

            await Task.Delay(300, cancellationToken);
        }

        throw new TimeoutException(
            "Private MariaDB started but its protected administrator credential was not accepted. " +
            "The existing private data was not changed. Restore the matching settings before retrying. " + lastError?.Message);
    }

    private async Task<bool> CanConnectAsPrivateAdministratorAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = connectionFactory.CreatePrivateAdministrator(settings);
            await connection.OpenAsync(cancellationToken);
            return true;
        }
        catch (MySqlException)
        {
            return false;
        }
    }

    private async Task ProvisionApplicationUserAsync(LauncherSettings settings, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.CreatePrivateAdministrator(settings);
        await connection.OpenAsync(cancellationToken);
        var password = GetApplicationPassword(settings);

        foreach (var host in new[] { "127.0.0.1", "localhost" })
        {
            await ExecuteWithPasswordAsync(connection,
                $"CREATE USER IF NOT EXISTS '{PrivateUser}'@'{host}' IDENTIFIED BY @password", password, cancellationToken);
            await ExecuteWithPasswordAsync(connection,
                $"ALTER USER '{PrivateUser}'@'{host}' IDENTIFIED BY @password", password, cancellationToken);

            foreach (var database in new[]
                {
                    settings.AuthenticationDatabaseName,
                    settings.ShardDatabaseName,
                    settings.WorldDatabaseName
                })
            {
                var escapedDatabase = database.Replace("`", "``", StringComparison.Ordinal);
                await using var grant = new MySqlCommand(
                    $"GRANT ALL PRIVILEGES ON `{escapedDatabase}`.* TO '{PrivateUser}'@'{host}'", connection);
                await grant.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }

    private static async Task ExecuteWithPasswordAsync(MySqlConnection connection, string sql, string password,
        CancellationToken cancellationToken)
    {
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@password", password);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static ProcessStartInfo CreateProcessStartInfo(string executable, string workingDirectory,
        IEnumerable<string> arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = executable,
            WorkingDirectory = workingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        foreach (var argument in arguments)
            startInfo.ArgumentList.Add(argument);
        return startInfo;
    }

    private string GetAdministratorPassword(LauncherSettings settings)
    {
        using var connection = connectionFactory.CreatePrivateAdministrator(settings);
        return new MySqlConnectionStringBuilder(connection.ConnectionString).Password;
    }

    private string GetApplicationPassword(LauncherSettings settings)
    {
        using var connection = connectionFactory.Create(settings);
        return new MySqlConnectionStringBuilder(connection.ConnectionString).Password;
    }

    private void LogMariaDbLine(string? line, params string[] secrets)
    {
        if (line is null)
            return;
        foreach (var secret in secrets.Where(value => value.Length > 0))
            line = line.Replace(secret, "[REDACTED]", StringComparison.Ordinal);
        log.Write("[Private MariaDB] " + line);
    }

    private static void AppendRedacted(StringBuilder output, string? line, string secret)
    {
        if (line is not null)
            lock (output)
                output.AppendLine(line.Replace(secret, "[REDACTED]", StringComparison.Ordinal));
    }

    private static string Snapshot(StringBuilder output)
    {
        lock (output)
            return output.ToString();
    }

    private static string Condense(string value)
    {
        var condensed = string.Join(" ", value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        return condensed.Length <= 800 ? condensed : condensed[..800] + "...";
    }
}
