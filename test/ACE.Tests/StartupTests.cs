using System;
using ACE.Common;
using System.IO;
using ACE.Database;
using ACE.Managers;
using ACE.Command;
using Xunit;

namespace ACE.Tests
{
    public class StartupTests
    {
        public StartupTests()
        {
            // copy config.json and Initialise configuration
            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\ACE\\Config.json"), ".\\Config.json", true);
            ConfigManager.Initialise();

        }

        [Fact]
        public void DatabaseManager_Initialise()
        {
            // this triggers all the prepared statement validation
            DatabaseManager.Initialise();
        }

        [Fact]
        public void AssetManager_Initialise()
        {
            AssetManager.Initialise();
        }

        [Fact]
        public void WorldManager_Initialise()
        {
            WorldManager.Initialise();
            WorldManager.StopWorld();
        }

        [Fact]
        public void CommandManager_Initialise()
        {
            CommandManager.Initialise();
        }
    }
}
