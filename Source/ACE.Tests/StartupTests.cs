using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACE.Common;
using System.IO;
using ACE.Database;
using ACE.Managers;
using ACE.Command;

namespace ACE.Tests
{
    [TestClass]
    public class StartupTests
    {
        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            // copy config.json and initialize configuration
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\ACE\\Config.json"), ".\\Config.json", true);
            ConfigManager.Initialize();
        }

        [TestMethod]
        public void DatabaseManager_Initialize()
        {
            // this triggers all the prepared statement validation
            DatabaseManager.Initialize();
        }

        [TestMethod]
        public void AssetManager_Initialize()
        {
            AssetManager.Initialize();
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
