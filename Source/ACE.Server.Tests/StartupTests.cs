using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Common;
using ACE.Database;
using ACE.Server.Managers;

namespace ACE.Server.Tests
{
    [TestClass]
    public class StartupTests
    {
        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            // copy config.js and initialize configuration
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\ACE.Server\\Config.js"), ".\\Config.js", true);
            ConfigManager.Initialize();
        }

        [TestMethod]
        public void DatabaseManager_Initialize()
        {
            // this triggers all the prepared statement validation
            DatabaseManager.Initialize();
        }

        [TestMethod]
        public void WorldManager_Initialize()
        {
            WorldManager.Initialize();
            WorldManager.StopWorld();
        }

        [TestMethod]
        public void CommandManager_Initialize()
        {
            // CommandManager.Initialize();
        }
    }
}
