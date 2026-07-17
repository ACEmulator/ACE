using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Common;
using ACE.SinglePlayer.Configuration;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class ConfigurationAndValidationTests
{
    [TestMethod]
    public void GeneratedConfigurationIsLoopbackTypedAndSinglePlayerSafe()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var protector = new TestProtector();
            var settings = ValidSettings(root);
            settings.ProtectedDatabasePassword = protector.Protect("database secret");
            var configuration = new AceConfigurationWriter(protector).Create(settings);

            Assert.AreEqual("OpenDereth", configuration.Server.WorldName);
            Assert.AreEqual("127.0.0.1", configuration.Server.Network.Host);
            Assert.AreEqual((uint)9000, configuration.Server.Network.Port);
            Assert.AreEqual((uint)4, configuration.Server.Network.MaximumAllowedSessions);
            Assert.IsTrue(configuration.Server.Accounts.AllowAutoAccountCreation);
            Assert.IsFalse(configuration.Server.WorldDatabasePrecaching);
            Assert.IsTrue(configuration.Offline.AutoApplyDatabaseUpdates);
            Assert.IsFalse(configuration.Offline.AutoUpdateWorldDatabase);
            Assert.AreEqual("database secret", configuration.MySql.Shard.Password);

            var json = JsonSerializer.Serialize(configuration, ConfigManager.SerializerOptions);
            Assert.IsNotNull(JsonSerializer.Deserialize<MasterConfiguration>(json, ConfigManager.SerializerOptions));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public void ValidatorRequiresExactCurrentAceDatFiles()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var settings = ValidSettings(root);
            File.Delete(Path.Combine(settings.DatFilesDirectory, "client_local_English.dat"));
            var result = SetupValidator.Validate(settings);
            Assert.IsFalse(result.IsValid);
            StringAssert.Contains(result.Message, "client_local_English.dat");
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public void ValidatorRejectsClientWhoseDatFilesAreOnlyInASeparateServerDirectory()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var clientDirectory = Path.Combine(root, "ClientWithoutDats");
            var datDirectory = Path.Combine(root, "ServerDats");
            Directory.CreateDirectory(clientDirectory);
            Directory.CreateDirectory(datDirectory);
            var settings = ValidSettings(root);
            settings.ClientExePath = Path.Combine(clientDirectory, "acclient.exe");
            settings.DatFilesDirectory = datDirectory;
            File.WriteAllText(settings.ClientExePath, string.Empty);
            foreach (var dat in SetupValidator.RequiredClientDatFiles)
                File.WriteAllText(Path.Combine(datDirectory, dat), string.Empty);

            var result = SetupValidator.Validate(settings);

            Assert.IsFalse(result.IsValid);
            StringAssert.Contains(result.Message, "same folder as acclient.exe");
            StringAssert.Contains(result.Message, "client_highres.dat");
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public void PathRepairerUsesClientDatsOnlyUntilPrivateServerCopyExists()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var applicationDirectory = Path.Combine(root, "InstalledApp");
            var clientDirectory = Path.Combine(root, "CompleteClient");
            var oldDirectory = Path.Combine(root, "OldBuild");
            Directory.CreateDirectory(Path.Combine(applicationDirectory, "Server"));
            Directory.CreateDirectory(Path.Combine(applicationDirectory, "Mods"));
            Directory.CreateDirectory(clientDirectory);
            Directory.CreateDirectory(Path.Combine(oldDirectory, "Mods"));
            File.WriteAllText(Path.Combine(applicationDirectory, "Server", "ACE.Server.exe"), string.Empty);
            File.WriteAllText(Path.Combine(oldDirectory, "ACE.Server.exe"), string.Empty);
            var client = Path.Combine(clientDirectory, "acclient.exe");
            File.WriteAllText(client, string.Empty);
            foreach (var dat in SetupValidator.RequiredClientDatFiles)
                File.WriteAllText(Path.Combine(clientDirectory, dat), string.Empty);

            var settings = new LauncherSettings
            {
                ClientExePath = client,
                DatFilesDirectory = Path.Combine(root, "MissingServerData"),
                ServerExePath = Path.Combine(oldDirectory, "ACE.Server.exe"),
                ModsDirectory = Path.Combine(oldDirectory, "Mods")
            };

            Assert.IsTrue(SettingsPathRepairer.Repair(settings, applicationDirectory));
            Assert.AreEqual(clientDirectory, settings.DatFilesDirectory);
            Assert.AreEqual(Path.Combine(applicationDirectory, "Server", "ACE.Server.exe"), settings.ServerExePath);
            Assert.AreEqual(Path.Combine(applicationDirectory, "Mods"), settings.ModsDirectory);
            Assert.IsFalse(SettingsPathRepairer.Repair(settings, applicationDirectory));

            var privateServerData = Path.Combine(root, "PrivateServerData");
            Directory.CreateDirectory(privateServerData);
            foreach (var dat in SetupValidator.RequiredClientDatFiles)
                File.WriteAllText(Path.Combine(privateServerData, dat), string.Empty);
            settings.DatFilesDirectory = privateServerData;

            Assert.IsFalse(SettingsPathRepairer.Repair(settings, applicationDirectory));
            Assert.AreEqual(privateServerData, settings.DatFilesDirectory);
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public async Task ServerDatProvisionerCreatesAndRefreshesPrivateCopyWithoutChangingClientFiles()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var clientDirectory = Path.Combine(root, "Client");
            var targetDirectory = Path.Combine(root, "PrivateServerData");
            var logDirectory = Path.Combine(root, "Logs");
            Directory.CreateDirectory(clientDirectory);
            var client = Path.Combine(clientDirectory, "acclient.exe");
            await File.WriteAllTextAsync(client, string.Empty);
            foreach (var dat in SetupValidator.RequiredClientDatFiles)
                await File.WriteAllTextAsync(Path.Combine(clientDirectory, dat), "original-" + dat);

            var settings = new LauncherSettings { ClientExePath = client };
            using var log = new LauncherLog(logDirectory);
            Assert.AreEqual("OpenDereth.log", Path.GetFileName(log.LogPath));
            var provisioner = new ServerDatProvisioner(log, targetDirectory);

            var result = await provisioner.EnsureAsync(settings, CancellationToken.None);

            Assert.AreEqual(targetDirectory, result);
            Assert.IsFalse(provisioner.RequiresRefresh(settings));
            foreach (var dat in SetupValidator.RequiredClientDatFiles)
                Assert.AreEqual("original-" + dat, await File.ReadAllTextAsync(Path.Combine(targetDirectory, dat)));

            var changedDat = Path.Combine(clientDirectory, SetupValidator.RequiredClientDatFiles[0]);
            await File.AppendAllTextAsync(changedDat, "-changed");
            Assert.IsTrue(provisioner.RequiresRefresh(settings));
            await provisioner.EnsureAsync(settings, CancellationToken.None);
            Assert.AreEqual(await File.ReadAllTextAsync(changedDat),
                await File.ReadAllTextAsync(Path.Combine(targetDirectory, SetupValidator.RequiredClientDatFiles[0])));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public void PortableBundleAutomaticallyConfiguresDatabaseWorldAndProtectedCredentials()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var applicationDirectory = Path.Combine(root, "InstalledApp");
            var clientDirectory = Path.Combine(root, "CompleteClient");
            CreatePortableBundle(applicationDirectory);
            Directory.CreateDirectory(clientDirectory);
            var client = Path.Combine(clientDirectory, "acclient.exe");
            File.WriteAllText(client, string.Empty);
            foreach (var dat in SetupValidator.RequiredClientDatFiles)
                File.WriteAllText(Path.Combine(clientDirectory, dat), string.Empty);

            var settings = LauncherSettings.CreateDefaults(applicationDirectory);
            settings.ClientExePath = client;
            settings.DatFilesDirectory = clientDirectory;
            settings.RuntimeDirectory = Path.Combine(root, "Runtime");
            settings.PrivateDatabaseDirectoryPath = Path.Combine(root, "PrivateDatabase");

            Assert.IsTrue(BundledDistribution.IsComplete(applicationDirectory));
            Assert.IsTrue(AutomaticSetupConfigurator.Configure(settings, applicationDirectory, new TestProtector()));
            Assert.AreEqual(DatabaseMode.Private, settings.DatabaseMode);
            Assert.AreEqual(BundledDistribution.MariaDbServerPath(applicationDirectory), settings.ManagedDatabaseExePath);
            Assert.AreEqual(Path.Combine(applicationDirectory, "Dependencies", "World", "ACE-World-Database-v0.9.294.sql"),
                settings.WorldDatabaseSqlPath);
            Assert.IsFalse(string.IsNullOrWhiteSpace(settings.ProtectedAccountPassword));
            Assert.IsFalse(string.IsNullOrWhiteSpace(settings.ProtectedDatabasePassword));
            Assert.IsFalse(string.IsNullOrWhiteSpace(settings.ProtectedPrivateDatabaseAdminPassword));
            Assert.AreEqual(settings.ProtectedDatabasePassword, settings.ProtectedPrivateDatabasePassword);
            Assert.IsTrue(SetupValidator.Validate(settings).IsValid, SetupValidator.Validate(settings).Message);
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    private static void CreatePortableBundle(string applicationDirectory)
    {
        var serverDirectory = Path.Combine(applicationDirectory, "Server");
        var mariaDbDirectory = Path.Combine(applicationDirectory, "Dependencies", "MariaDB", "bin");
        var worldDirectory = Path.Combine(applicationDirectory, "Dependencies", "World");
        Directory.CreateDirectory(serverDirectory);
        Directory.CreateDirectory(Path.Combine(applicationDirectory, "Mods"));
        Directory.CreateDirectory(mariaDbDirectory);
        Directory.CreateDirectory(worldDirectory);
        File.WriteAllText(Path.Combine(serverDirectory, "ACE.Server.exe"), string.Empty);
        File.WriteAllText(Path.Combine(mariaDbDirectory, "mariadbd.exe"), string.Empty);
        File.WriteAllText(Path.Combine(mariaDbDirectory, "mariadb-install-db.exe"), string.Empty);
        File.WriteAllText(Path.Combine(mariaDbDirectory, "mariadb.exe"), string.Empty);
        File.WriteAllText(Path.Combine(worldDirectory, "ACE-World-Database-v0.9.294.sql"),
            "INSERT INTO `weenie` VALUES (1, 'human');");
    }

    private static LauncherSettings ValidSettings(string root)
    {
        var client = Path.Combine(root, "acclient.exe");
        var server = Path.Combine(root, "ACE.Server.exe");
        File.WriteAllText(client, string.Empty);
        File.WriteAllText(server, string.Empty);
        foreach (var dat in SetupValidator.RequiredClientDatFiles)
            File.WriteAllText(Path.Combine(root, dat), string.Empty);
        return new LauncherSettings
        {
            ClientExePath = client,
            ServerExePath = server,
            DatFilesDirectory = root,
            ModsDirectory = Path.Combine(root, "Mods"),
            RuntimeDirectory = Path.Combine(root, "Runtime"),
            DatabaseMode = DatabaseMode.External,
            DatabaseHost = "127.0.0.1",
            DatabasePort = 3306,
            DatabaseUsername = "root",
            ProtectedAccountPassword = "protected",
            ProtectedDatabasePassword = "protected"
        };
    }

    private sealed class TestProtector : ISecretProtector
    {
        public string Protect(string value) => Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
        public string Unprotect(string protectedValue) => System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(protectedValue));
    }
}
