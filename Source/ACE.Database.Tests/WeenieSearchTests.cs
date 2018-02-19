using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum.Properties;
using System.Linq;

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
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\ACE.Server\\Config.json"), ".\\Config.json", true);

            ConfigManager.Initialize();
            worldDb = new WorldDatabase();
        }

        // [TestMethod]
        public void WeenieSearch_NullCriteria_ReturnsLotsOfWeenies()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses
            var results = worldDb.SearchWeenies(null);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);*/
        }

        //[TestMethod]
        public void WeenieSearch_ByName_ReturnsKnownWeenies()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses
            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.PartialName = "Pyreal Mote";
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);*/
        }

        //[TestMethod]
        public void WeenieSearch_ByWeenieClassId_DoesntExplode()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses

            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.WeenieClassId = 6353; // Pyreal Mote
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);*/
        }

        //[TestMethod]
        public void WeenieSearch_ByWeenieType_DoesntExplode()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses
            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.WeenieType = Entity.Enum.WeenieType.LifeStone;
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0, "no lifestones (weenietype) in the database is bad.");*/
        }

        //[TestMethod]
        public void WeenieSearch_ByItemType_DoesntExplode()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses

            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.ItemType = Entity.Enum.ItemType.LifeStone;
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0, "no lifestones (itemtype) in the database is bad.");*/
        }

        //[TestMethod]
        public void WeenieSearch_ByFloatProperty_ReturnsObject()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses
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
            Assert.IsTrue(results.Count > 0, "no Peerless Healing Kit in the database is bad.");*/
        }

        [TestMethod]
        public void GetAndSaveWeenie_ById_DoesNotThrow()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses
            var mote = worldDb.GetObject(6353);

            mote.IsDirty = true;

            worldDb.SaveObject(mote);*/
        }

        [TestMethod]
        public void GetWeenie_Pyreal_ById_ReturnsObject()
        {
            var result = worldDb.GetWeenie(273);

            var stringName = result.WeeniePropertiesString.FirstOrDefault(x => x.Type == (ushort)PropertyString.Name)?.Value; ;

            Assert.IsNotNull(result);
            Assert.IsTrue(stringName == "Pyreal");
            Assert.IsTrue(result.GetProperty(PropertyString.Name) == "Pyreal");
        }

        [TestMethod]
        public void GetWeenie_Pyreal_ByName_ReturnsObject()
        {
            var result = worldDb.GetWeenie("coinstack");

            var stringName = result.WeeniePropertiesString.FirstOrDefault(x => x.Type == (ushort)PropertyString.Name)?.Value; ;

            Assert.IsNotNull(result);
            Assert.IsTrue(stringName == "Pyreal");
            Assert.IsTrue(result.GetProperty(PropertyString.Name) == "Pyreal");
        }
    }
}
