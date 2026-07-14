using System.Net;
using System.Net.Sockets;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;

using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class PrivateDatabaseTests
{
    [TestMethod]
    public void LocatorFindsNewestCompleteMariaDbInstallation()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            CreateFakeInstallation(root, "MariaDB 10.11");
            var expected = CreateFakeInstallation(root, "MariaDB 11.2");
            var incomplete = Path.Combine(root, "MariaDB 12.0", "bin");
            Directory.CreateDirectory(incomplete);
            File.WriteAllText(Path.Combine(incomplete, "mariadbd.exe"), string.Empty);

            Assert.AreEqual(expected, MariaDbInstallationLocator.FindUnderRoots(new[] { root }));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public void LocatorFindsImportClientBesideServer()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var server = CreateFakeInstallation(root, "MariaDB 11.2");
            var expected = Path.Combine(Path.GetDirectoryName(server)!, "mariadb.exe");
            Assert.AreEqual(expected, MariaDbInstallationLocator.FindClient(server));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public async Task WorldPackageInspectorRejectsSchemaOnlySqlAndAcceptsRequiredData()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var schemaOnly = Path.Combine(root, "WorldBase.sql");
            var populated = Path.Combine(root, "ACE-World-Database.sql");
            await File.WriteAllTextAsync(schemaOnly, "CREATE TABLE `weenie` (`class_Id` int);");
            await File.WriteAllTextAsync(populated,
                new string('-', 70_000) + "INSERT INTO `weenie` VALUES (1,'human',10,'2005-02-09 10:00:00');");

            Assert.IsFalse(await WorldSqlPackageInspector.ContainsRequiredWorldDataAsync(schemaOnly, CancellationToken.None));
            Assert.IsTrue(await WorldSqlPackageInspector.ContainsRequiredWorldDataAsync(populated, CancellationToken.None));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public async Task WorldImportStreamReplacesDatabaseNameAcrossBufferBoundaries()
    {
        var prefix = new string('x', 1024 * 1024 - 5);
        await using var source = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(prefix + "`ace_world` tail"));
        await using var destination = new MemoryStream();

        await DatabaseBootstrapper.CopyReplacingAsync(source, destination,
            System.Text.Encoding.UTF8.GetBytes("`ace_world`"),
            System.Text.Encoding.UTF8.GetBytes("`custom_world`"), CancellationToken.None);

        Assert.AreEqual(prefix + "`custom_world` tail", System.Text.Encoding.UTF8.GetString(destination.ToArray()));
    }

    [TestMethod]
    public void PortFinderSkipsAnOccupiedLoopbackPort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var occupied = (ushort)((IPEndPoint)listener.LocalEndpoint).Port;
        if (occupied > ushort.MaxValue - 10)
            return;

        var selected = PrivateDatabasePortFinder.FindAvailablePort(occupied, (ushort)(occupied + 10));
        Assert.AreNotEqual(occupied, selected);
    }

    [TestMethod]
    public void PrivateArgumentsStayLoopbackOnlyAndDoNotEnableRemoteRoot()
    {
        var initialization = ManagedMariaDbRuntime.CreateInitializationArguments(@"C:\ACE Runtime\Database", "secret value", 3307);
        var server = ManagedMariaDbRuntime.CreateServerArguments(@"C:\ACE Runtime\Database", 3307);

        CollectionAssert.Contains(initialization.ToArray(), "--password=secret value");
        CollectionAssert.Contains(server.ToArray(), "--bind-address=127.0.0.1");
        CollectionAssert.DoesNotContain(initialization.ToArray(), "--allow-remote-root-access");
        Assert.AreEqual("--no-defaults", server[0]);
    }

    [TestMethod]
    [TestCategory("MariaDbIntegration")]
    public async Task InstalledMariaDbCanInitializeBootstrapValidateAndStopPrivateInstance()
    {
        var mariaDb = Environment.GetEnvironmentVariable("ACE_TEST_MARIADBD");
        if (string.IsNullOrWhiteSpace(mariaDb) || !File.Exists(mariaDb))
            return;

        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var protector = new SecretProtector();
            var serverDirectory = Path.Combine(root, "Server");
            var baseDirectory = Path.Combine(serverDirectory, "DatabaseSetupScripts", "Base");
            Directory.CreateDirectory(baseDirectory);
            var repositoryRoot = FindRepositoryRoot();
            foreach (var name in new[] { "AuthenticationBase.sql", "ShardBase.sql", "WorldBase.sql" })
                File.Copy(Path.Combine(repositoryRoot, "Database", "Base", name), Path.Combine(baseDirectory, name));
            var populatedWorld = Environment.GetEnvironmentVariable("ACE_TEST_WORLD_SQL");
            if (string.IsNullOrWhiteSpace(populatedWorld) || !File.Exists(populatedWorld))
            {
                populatedWorld = Path.Combine(baseDirectory, "PopulatedWorld.sql");
                File.Copy(Path.Combine(baseDirectory, "WorldBase.sql"), populatedWorld);
                await File.AppendAllTextAsync(populatedWorld,
                    "\r\nUSE `ace_world`;\r\nINSERT INTO `weenie` VALUES (1,'human',10,'2005-02-09 10:00:00');\r\n");
            }

            var serverExe = Path.Combine(serverDirectory, "ACE.Server.exe");
            File.WriteAllText(serverExe, string.Empty);
            var settings = new LauncherSettings
            {
                DatabaseMode = DatabaseMode.Private,
                DatabaseHost = "127.0.0.1",
                DatabasePort = PrivateDatabasePortFinder.FindAvailablePort(3340, 3399),
                DatabaseUsername = "ace_singleplayer",
                ProtectedDatabasePassword = protector.Protect(SecretProtector.GeneratePassword()),
                ProtectedPrivateDatabaseAdminPassword = protector.Protect(SecretProtector.GeneratePassword()),
                ManagedDatabaseExePath = mariaDb,
                RuntimeDirectory = Path.Combine(root, "Runtime"),
                PrivateDatabaseDirectoryPath = Path.Combine(root, "Runtime", "Database"),
                ServerExePath = serverExe,
                WorldDatabaseSqlPath = populatedWorld
            };
            settings.ProtectedPrivateDatabasePassword = settings.ProtectedDatabasePassword;

            using var log = new LauncherLog(Path.Combine(root, "Logs"));
            var factory = new DatabaseConnectionFactory(protector);
            await using var runtime = new ManagedMariaDbRuntime(factory, new ExternalMariaDbRuntime(factory), log);
            await runtime.StartAsync(settings, CancellationToken.None);

            // Reproduce the old launcher's failure state: all world tables exist, but they are empty.
            await using (var connection = factory.Create(settings))
            {
                await connection.OpenAsync(CancellationToken.None);
                var schemaOnlySql = await File.ReadAllTextAsync(Path.Combine(baseDirectory, "WorldBase.sql"));
                await using var schemaOnly = new MySqlCommand(schemaOnlySql, connection) { CommandTimeout = 300 };
                await schemaOnly.ExecuteNonQueryAsync(CancellationToken.None);
            }

            await new DatabaseBootstrapper(factory).BootstrapAsync(settings, settings.WorldDatabaseSqlPath, CancellationToken.None);
            var validation = await runtime.ValidateAsync(settings, CancellationToken.None);

            Assert.IsTrue(validation.IsValid, validation.Message);
            Assert.IsTrue(Directory.Exists(Path.Combine(settings.PrivateDatabaseDirectory, "mysql")));
            await runtime.StopAsync(CancellationToken.None);
        }
        finally
        {
            for (var attempt = 0; attempt < 10; attempt++)
            {
                try
                {
                    Directory.Delete(root, true);
                    break;
                }
                catch (IOException) when (attempt < 9)
                {
                    await Task.Delay(200);
                }
            }
        }
    }

    private static string CreateFakeInstallation(string root, string name)
    {
        var bin = Path.Combine(root, name, "bin");
        Directory.CreateDirectory(bin);
        var server = Path.Combine(bin, "mariadbd.exe");
        File.WriteAllText(server, string.Empty);
        File.WriteAllText(Path.Combine(bin, "mariadb-install-db.exe"), string.Empty);
        File.WriteAllText(Path.Combine(bin, "mariadb.exe"), string.Empty);
        return server;
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !Directory.Exists(Path.Combine(directory.FullName, "Database", "Base")))
            directory = directory.Parent;
        return directory?.FullName ?? throw new DirectoryNotFoundException("Could not locate Database\\Base.");
    }
}
