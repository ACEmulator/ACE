using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Common;

namespace ACE.Database.Tests
{
    [TestClass]
    public class WeenieSearchTests
    {
        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            // copy config.json
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\ACE.Server\\Config.json"), ".\\Config.json", true);

            ConfigManager.Initialize();
        }

        // [TestMethod]
        public void WeenieSearch_NullCriteria_ReturnsLotsOfWeenies()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses
            var results = worldDb.SearchWeenies(null);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);*/
        }

        [TestMethod]
        public void WeenieSearch_ByName_ReturnsKnownWeenies()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses
            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.PartialName = "Pyreal Mote";
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);*/
        }

        [TestMethod]
        public void WeenieSearch_ByWeenieClassId_DoesntExplode()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses

            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.WeenieClassId = 6353; // Pyreal Mote
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);*/
        }

        [TestMethod]
        public void WeenieSearch_ByWeenieType_DoesntExplode()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses
            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.WeenieType = Entity.Enum.WeenieType.LifeStone;
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0, "no lifestones (weenietype) in the database is bad.");*/
        }

        [TestMethod]
        public void WeenieSearch_ByItemType_DoesntExplode()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses

            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.ItemType = Entity.Enum.ItemType.LifeStone;
            var results = worldDb.SearchWeenies(criteria);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0, "no lifestones (itemtype) in the database is bad.");*/
        }

        [TestMethod]
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
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses
            var results = worldDb.GetAceObjectByWeenie(273);

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Name == "Pyreal");*/
        }

        [TestMethod]
        public void GetWeenie_Pyreal_ByName_ReturnsObject()
        {
            throw new NotImplementedException(); /* I don't like these tests. They should use functions the rest of the code uses
            var results = worldDb.GetAceObjectByWeenie("coinstack");

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Name == "Pyreal");*/
        }
    }
}
