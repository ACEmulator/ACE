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

    private static LauncherSettings ValidSettings(string root)
    {
        var client = Path.Combine(root, "acclient.exe");
        var server = Path.Combine(root, "ACE.Server.exe");
        File.WriteAllText(client, string.Empty);
        File.WriteAllText(server, string.Empty);
        foreach (var dat in SetupValidator.RequiredDatFiles)
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
