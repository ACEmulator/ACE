using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.Tests
{
    [TestClass]
    public class WeenieSearchTests
    {
        private static WorldDatabase worldDb;

        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            // copy config.js
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\ACE.Server\\Config.js"), ".\\Config.js", true);

            ConfigManager.Initialize();
            worldDb = new WorldDatabase();
        }

        [TestMethod]
        public void GetWeenie_Pyreal_ById_ReturnsObject()
        {
            var result = worldDb.GetWeenie(273);

            var stringName = result.WeeniePropertiesString.FirstOrDefault(x => x.Type == (ushort)PropertyString.Name)?.Value;

            Assert.IsNotNull(result);
            Assert.IsTrue(stringName == "Pyreal");
            Assert.IsTrue(result.GetProperty(PropertyString.Name) == "Pyreal");
        }

        [TestMethod]
        public void GetWeenie_Pyreal_ByName_ReturnsObject()
        {
            var result = worldDb.GetWeenie("coinstack");

            var stringName = result.WeeniePropertiesString.FirstOrDefault(x => x.Type == (ushort)PropertyString.Name)?.Value;

            Assert.IsNotNull(result);
            Assert.IsTrue(stringName == "Pyreal");
            Assert.IsTrue(result.GetProperty(PropertyString.Name) == "Pyreal");
        }
    }
}
