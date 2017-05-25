using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACE.Common;
using ACE.Entity;

namespace ACE.Database.Tests
{
    [TestClass]
    public class ConstructedStatementTests
    {
        public enum TestEnum
        {
            FooInsert,
            FooUpdate,
            GetLifestonesByLandblock,
            FooGet
        }

        private static Database worldDb;

        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            // copy config.json
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\ACE\\Config.json"), ".\\Config.json", true);

            ConfigManager.Initialize();
            worldDb = new Database();
            worldDb.Initialize(ConfigManager.Config.MySql.World.Host,
                          ConfigManager.Config.MySql.World.Port,
                          ConfigManager.Config.MySql.World.Username,
                          ConfigManager.Config.MySql.World.Password,
                          ConfigManager.Config.MySql.World.Database);
        }

        [TestMethod]
        public void ExerciseInsertGetUpdate_SimpleCase_DoesNotThrow()
        {
            var o = new Weenie
            {
                WeenieClassId = 1000000,
                WeenieClassDescription = "foo"
            };

            // NOTE: we don't have a delete so you have to make sure you have cleared the test insert before you run the test.
            worldDb.ConstructStatement(TestEnum.FooInsert, typeof(Weenie), ConstructedStatementType.Insert);
            worldDb.ConstructStatement(TestEnum.FooUpdate, typeof(Weenie), ConstructedStatementType.Update);
            worldDb.ConstructStatement(TestEnum.FooGet, typeof(Weenie), ConstructedStatementType.Get);
            worldDb.ExecuteConstructedInsertStatement(TestEnum.FooInsert, typeof(Weenie), o);

            var o2 = new Weenie();
            var criteria = new Dictionary<string, object>();
            criteria.Add("WeenieClassId", 1000000u);
            worldDb.ExecuteConstructedGetStatement(TestEnum.FooGet, typeof(Weenie), criteria, o2);

            Assert.AreEqual(o.WeenieClassDescription, o2.WeenieClassDescription);
            Assert.AreEqual(o.WeenieClassId, o2.WeenieClassId);

            o2.WeenieClassDescription = "bar";
            worldDb.ExecuteConstructedUpdateStatement(TestEnum.FooUpdate, typeof(Weenie), o2);

            var o3 = new Weenie();
            worldDb.ExecuteConstructedGetStatement(TestEnum.FooGet, typeof(Weenie), criteria, o3);

            Assert.AreEqual(o2.WeenieClassDescription, o3.WeenieClassDescription);
        }

        [TestMethod]
        public void ExecuteGetListStatement_SimpleCase_DoesNotThrow()
        {
            worldDb.ConstructStatement(TestEnum.GetLifestonesByLandblock, typeof(AceObject), ConstructedStatementType.GetList);

            var criteria = new Dictionary<string, object>();
            criteria.Add("landblock", (ushort)458);
            var results = worldDb.ExecuteConstructedGetListStatement<TestEnum, AceObject>(TestEnum.GetLifestonesByLandblock, criteria);

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
        }
    }
}
