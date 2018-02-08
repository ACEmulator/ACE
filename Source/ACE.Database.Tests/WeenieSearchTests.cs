using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database.Tests
{
    [TestClass]
    public class WeenieSearchTests
    {
        private static WorldDatabase worldDb;

        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            // copy config.json
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\ACE\\Config.json"), ".\\Config.json", true);

            ConfigManager.Initialize();
            worldDb = new WorldDatabase();
            worldDb.Initialize(ConfigManager.Config.MySql.World.Host,
                          ConfigManager.Config.MySql.World.Port,
                          ConfigManager.Config.MySql.World.Username,
                          ConfigManager.Config.MySql.World.Password,
                          ConfigManager.Config.MySql.World.Database);
        }

        // [TestMethod]
        public void WeenieSearch_NullCriteria_ReturnsLotsOfWeenies()
        {
            var results = worldDb.SearchWeenies(null);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
        }

        [TestMethod]
        public void WeenieSearch_ByName_ReturnsKnownWeenies()
        {
            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.PartialName = "Pyreal Mote";
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
        }

        [TestMethod]
        public void WeenieSearch_ByWeenieClassId_DoesntExplode()
        {
            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.WeenieClassId = 6353; // Pyreal Mote
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void WeenieSearch_ByWeenieType_DoesntExplode()
        {
            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.WeenieType = Entity.Enum.WeenieType.LifeStone;
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0, "no lifestones (weenietype) in the database is bad.");
        }

        [TestMethod]
        public void WeenieSearch_ByItemType_DoesntExplode()
        {
            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.ItemType = Entity.Enum.ItemType.LifeStone;
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0, "no lifestones (itemtype) in the database is bad.");
        }

        [TestMethod]
        public void WeenieSearch_ByFloatProperty_ReturnsObject()
        {
            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.PartialName = "Peerless Healing Kit";
            criteria.PropertyCriteria.Add(new SearchWeenieProperty()
            {
                PropertyType = AceObjectPropertyType.PropertyDouble,
                PropertyId = (uint)PropertyDouble.HealkitMod,
                PropertyValue = "1.75"
            });
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0, "no Peerless Healing Kit in the database is bad.");
        }

        [TestMethod]
        public void GetAndSaveWeenie_ById_DoesNotThrow()
        {
            var mote = worldDb.GetObject(6353);

            mote.IsDirty = true;

            worldDb.SaveObject(mote);
        }

        [TestMethod]
        public void GetWeenie_Pyreal_ById_ReturnsObject()
        {
            var results = worldDb.GetAceObjectByWeenie(273);

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Name == "Pyreal");
        }

        [TestMethod]
        public void GetWeenie_Pyreal_ByName_ReturnsObject()
        {
            var results = worldDb.GetAceObjectByWeenie("coinstack");

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Name == "Pyreal");
        }
    }
}
