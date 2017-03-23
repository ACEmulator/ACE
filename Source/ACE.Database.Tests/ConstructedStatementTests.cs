﻿using System;
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
        enum TestEnum
        {
            FooInsert,
            FooUpdate,
            FooGet,
            GetLifestonesByLandblock
        }

        Database worldDb;

        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            // copy config.json
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\ACE\\Config.json"), ".\\Config.json", true);

            ConfigManager.Initialise();
            Database db = new Database();
            db.Initialise(ConfigManager.Config.MySql.World.Host,
                          ConfigManager.Config.MySql.World.Port,
                          ConfigManager.Config.MySql.World.Username,
                          ConfigManager.Config.MySql.World.Password,
                          ConfigManager.Config.MySql.World.Database);
        }

        [TestMethod]
        public void ExersiceInsertGetUpdate_SimpleCase_DoesNotThrow()
        {
            BaseAceObject o = new BaseAceObject();
            o.AceObjectId = 1;
            o.Name = "foo";

            worldDb.ContructStatement(TestEnum.FooInsert, typeof(BaseAceObject), ConstructedStatementType.Insert);
            worldDb.ContructStatement(TestEnum.FooUpdate, typeof(BaseAceObject), ConstructedStatementType.Update);
            worldDb.ContructStatement(TestEnum.FooGet, typeof(BaseAceObject), ConstructedStatementType.Get);

            worldDb.ExecuteConstructedInsertStatement(TestEnum.FooInsert, typeof(BaseAceObject), o);
            
            BaseAceObject o2 = new BaseAceObject();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("baseAceObjectId", 1u);
            worldDb.ExecuteConstructedGetStatement(TestEnum.FooGet, typeof(BaseAceObject), criteria, o2);

            Assert.AreEqual(o.Name, o2.Name);
            Assert.AreEqual(o.AceObjectId, o2.AceObjectId);

            o2.Name = "bar";
            worldDb.ExecuteConstructedUpdateStatement(TestEnum.FooUpdate, typeof(BaseAceObject), o2);

            BaseAceObject o3 = new BaseAceObject();
            worldDb.ExecuteConstructedGetStatement(TestEnum.FooGet, typeof(BaseAceObject), criteria, o3);

            Assert.AreEqual(o2.Name, o3.Name);
        }

        [TestMethod]
        public void ExecuteGetListStatement_SimpleCase_DoesNotThrow()
        {
            worldDb.ContructStatement(TestEnum.GetLifestonesByLandblock, typeof(AceObject), ConstructedStatementType.GetList);

            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("landblock", (ushort)458);
            var results = worldDb.ExecuteConstructedGetListStatement<TestEnum, AceObject>(TestEnum.GetLifestonesByLandblock, criteria);

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
        }
    }
}
