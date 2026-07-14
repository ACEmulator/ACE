using System.Diagnostics;
using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.SinglePlayer.ClientLaunch;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class ClientLaunchTests
{
    [TestMethod]
    public async Task DirectLaunchKeepsEveryCredentialArgumentSeparateAndExact()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        try
        {
            var recordPath = Path.Combine(directory, "arguments.json");
            const string account = "single player account";
            const string password = "p a\"ss&^!%()[]{}";
            var settings = new LauncherSettings
            {
                ClientExePath = TestPaths.TestHost,
                AccountName = account,
                Host = "127.0.0.1",
                Port = 9000
            };
            var info = DirectClientLaunchProvider.CreateStartInfo(new ClientLaunchRequest(settings, password));
            info.Environment["ACE_SINGLEPLAYER_TEST_RECORD_ARGUMENTS"] = recordPath;

            using var process = Process.Start(info);
            Assert.IsNotNull(process);
            await process.WaitForExitAsync();
            var arguments = JsonSerializer.Deserialize<string[]>(await File.ReadAllTextAsync(recordPath));

            CollectionAssert.AreEqual(new[]
            {
                "-a", account, "-v", password, "-h", "127.0.0.1:9000", "-rodat", "on"
            }, arguments);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }
}
