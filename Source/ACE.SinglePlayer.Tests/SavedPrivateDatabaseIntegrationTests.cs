using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.SinglePlayer.Database;
using ACE.SinglePlayer.Infrastructure;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class SavedPrivateDatabaseIntegrationTests
{
    [TestMethod]
    [TestCategory("SavedPrivateDatabaseIntegration")]
    public async Task SavedPrivateDatabaseCanBootstrapAndValidate()
    {
        if (!string.Equals(Environment.GetEnvironmentVariable("ACE_TEST_SAVED_PRIVATE_DATABASE"), "1",
                StringComparison.Ordinal))
            return;

        var settings = await new SettingsStore().LoadAsync()
            ?? throw new InvalidOperationException("OpenDereth settings have not been saved.");
        Assert.AreEqual(DatabaseMode.Private, settings.DatabaseMode);

        var logRoot = TestPaths.CreateTemporaryDirectory();
        try
        {
            using var log = new LauncherLog(logRoot);
            var factory = new DatabaseConnectionFactory(new SecretProtector());
            await using var runtime = new ManagedMariaDbRuntime(factory, new ExternalMariaDbRuntime(factory), log);
            try
            {
                await runtime.StartAsync(settings, CancellationToken.None);
                await new DatabaseBootstrapper(factory, log).BootstrapAsync(
                    settings, settings.WorldDatabaseSqlPath, CancellationToken.None);
                var validation = await runtime.ValidateAsync(settings, CancellationToken.None);
                Assert.IsTrue(validation.IsValid, validation.Message);
            }
            finally
            {
                await runtime.StopAsync(CancellationToken.None);
            }
        }
        finally
        {
            Directory.Delete(logRoot, recursive: true);
        }
    }
}
