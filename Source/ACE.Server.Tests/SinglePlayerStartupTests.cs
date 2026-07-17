using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Common;

namespace ACE.Server.Tests
{
    [TestClass]
    public class SinglePlayerStartupTests
    {
        [TestMethod]
        public void LaunchOptionsAreOptional()
        {
            var options = ServerLaunchOptions.Parse(Array.Empty<string>());
            Assert.IsNull(options.ConfigPath);
            Assert.IsNull(options.ReadyFilePath);
        }

        [TestMethod]
        public void LaunchOptionsPreserveAbsolutePathsWithSpaces()
        {
            var root = Path.Combine(Path.GetTempPath(), "OpenDereth Test");
            var options = ServerLaunchOptions.Parse(new[]
            {
                "--config", Path.Combine(root, "Config.js"),
                "--ready-file", Path.Combine(root, "server ready.json")
            });
            Assert.AreEqual(Path.GetFullPath(Path.Combine(root, "Config.js")), options.ConfigPath);
            Assert.AreEqual(Path.GetFullPath(Path.Combine(root, "server ready.json")), options.ReadyFilePath);
        }

        [TestMethod]
        public void RelativeLauncherPathsAreRejected()
        {
            Assert.ThrowsExactly<ArgumentException>(() => ServerLaunchOptions.Parse(new[] { "--config", "Config.js" }));
        }

        [TestMethod]
        public void ReadyFileIsAtomicValidAndRemovedOnShutdown()
        {
            var directory = Path.Combine(Path.GetTempPath(), "ACE.SinglePlayer.Server.Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, "ready.json");
            try
            {
                ConfigManager.Initialize(new MasterConfiguration());
                ConfigManager.Config.Server.WorldName = "Ready Test";
                ConfigManager.Config.Server.Network.Host = "127.0.0.1";
                ConfigManager.Config.Server.Network.Port = 9123;
                var signal = new ReadyFileSignal(path);
                signal.Write();

                var json = File.ReadAllText(path);
                StringAssert.Contains(json, "Ready Test");
                StringAssert.Contains(json, "127.0.0.1");
                Assert.AreEqual(0, Directory.GetFiles(directory, "*.tmp").Length);

                signal.Delete();
                Assert.IsFalse(File.Exists(path));
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
