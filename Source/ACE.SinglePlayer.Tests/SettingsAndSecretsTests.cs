using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class SettingsAndSecretsTests
{
    [TestMethod]
    public async Task SettingsRoundTripPreservesVersionAndPersistentAccount()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        try
        {
            var store = new SettingsStore(Path.Combine(directory, "settings.json"));
            var settings = new LauncherSettings
            {
                AccountName = "single player account",
                ProtectedAccountPassword = "protected-once",
                ClientLaunchMode = ClientLaunchMode.Decal,
                Port = 9123
            };
            await store.SaveAsync(settings);
            var loaded = await store.LoadAsync();

            Assert.IsNotNull(loaded);
            Assert.AreEqual(LauncherSettings.CurrentVersion, loaded.SettingsVersion);
            Assert.AreEqual(settings.AccountName, loaded.AccountName);
            Assert.AreEqual(settings.ProtectedAccountPassword, loaded.ProtectedAccountPassword);
            Assert.AreEqual(ClientLaunchMode.Decal, loaded.ClientLaunchMode);
            Assert.AreEqual((ushort)9123, loaded.Port);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [TestMethod]
    public async Task NewerSettingsVersionIsRejected()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        try
        {
            var path = Path.Combine(directory, "settings.json");
            await File.WriteAllTextAsync(path, JsonSerializer.Serialize(new { SettingsVersion = LauncherSettings.CurrentVersion + 1 }));
            var store = new SettingsStore(path);
            await AssertThrowsAsync<InvalidDataException>(() => store.LoadAsync());
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [TestMethod]
    public async Task VersionOneLocalRootSettingsMigrateToPrivateDatabase()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        try
        {
            var path = Path.Combine(directory, "settings.json");
            await File.WriteAllTextAsync(path,
                """
                {
                  "SettingsVersion": 1,
                  "DatabaseMode": "External",
                  "DatabaseHost": "127.0.0.1",
                  "DatabasePort": 3306,
                  "DatabaseUsername": "root",
                  "ProtectedDatabasePassword": "old-protected-password"
                }
                """);

            var loaded = await new SettingsStore(path).LoadAsync();

            Assert.IsNotNull(loaded);
            Assert.AreEqual(LauncherSettings.CurrentVersion, loaded.SettingsVersion);
            Assert.AreEqual(DatabaseMode.Private, loaded.DatabaseMode);
            Assert.AreEqual((ushort)3307, loaded.DatabasePort);
            Assert.AreEqual("ace_singleplayer", loaded.DatabaseUsername);
            Assert.AreEqual(string.Empty, loaded.ProtectedDatabasePassword);
            Assert.AreEqual("old-protected-password", loaded.ProtectedExternalDatabasePassword);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [TestMethod]
    public async Task VersionOneManagedRootPasswordBecomesPrivateAdministratorPassword()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        try
        {
            var path = Path.Combine(directory, "settings.json");
            await File.WriteAllTextAsync(path,
                """
                {
                  "SettingsVersion": 1,
                  "DatabaseMode": "ManagedExperimental",
                  "DatabaseHost": "127.0.0.1",
                  "DatabasePort": 3310,
                  "DatabaseUsername": "root",
                  "ProtectedDatabasePassword": "old-managed-root-password"
                }
                """);

            var loaded = await new SettingsStore(path).LoadAsync();

            Assert.IsNotNull(loaded);
            Assert.AreEqual(DatabaseMode.Private, loaded.DatabaseMode);
            Assert.AreEqual("old-managed-root-password", loaded.ProtectedPrivateDatabaseAdminPassword);
            Assert.AreEqual(string.Empty, loaded.ProtectedDatabasePassword);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [TestMethod]
    public void DpapiRoundTripUsesCurrentWindowsUser()
    {
        var protector = new SecretProtector();
        const string secret = "spaces, punctuation !@#$%^&*() and unicode Ω";
        var protectedValue = protector.Protect(secret);
        Assert.AreNotEqual(secret, protectedValue);
        Assert.AreEqual(secret, protector.Unprotect(protectedValue));
    }

    private static async Task AssertThrowsAsync<T>(Func<Task> action) where T : Exception
    {
        try
        {
            await action();
            Assert.Fail($"Expected {typeof(T).Name}.");
        }
        catch (T)
        {
        }
    }
}
