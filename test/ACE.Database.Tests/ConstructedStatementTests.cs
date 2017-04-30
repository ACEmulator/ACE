using System;
using System.Collections.Generic;
using System.IO;
using ACE.Common;
using ACE.Entity;
using Xunit;

namespace ACE.Database.Tests
{
    public class ConstructedStatementTests
    {
        public enum TestEnum
        {
            FooInsert,
            FooUpdate,
            FooGet,
            GetLifestonesByLandblock
        }

        private static Database worldDb;

        public ConstructedStatementTests()
        {
            // copy config.json
            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\ACE\\Config.json"), ".\\Config.json", true);

            ConfigManager.Initialise();
            worldDb = new Database();
            worldDb.Initialise(ConfigManager.Config.MySql.World.Host,
                          ConfigManager.Config.MySql.World.Port,
                          ConfigManager.Config.MySql.World.Username,
                          ConfigManager.Config.MySql.World.Password,
                          ConfigManager.Config.MySql.World.Database);
        }

        [Fact]
        public void ExerciseInsertGetUpdate_SimpleCase_DoesNotThrow()
        {
            BaseAceObject o = new BaseAceObject();
            o.AceObjectId = 1;
            o.Name = "foo";

            worldDb.ConstructStatement(TestEnum.FooInsert, typeof(BaseAceObject), ConstructedStatementType.Insert);
            worldDb.ConstructStatement(TestEnum.FooUpdate, typeof(BaseAceObject), ConstructedStatementType.Update);
            worldDb.ConstructStatement(TestEnum.FooGet, typeof(BaseAceObject), ConstructedStatementType.Get);

            worldDb.ExecuteConstructedInsertStatement(TestEnum.FooInsert, typeof(BaseAceObject), o);

            BaseAceObject o2 = new BaseAceObject();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("baseAceObjectId", 1u);
            worldDb.ExecuteConstructedGetStatement(TestEnum.FooGet, typeof(BaseAceObject), criteria, o2);

            Assert.Equal(o.Name, o2.Name);
            Assert.Equal(o.AceObjectId, o2.AceObjectId);

            o2.Name = "bar";
            worldDb.ExecuteConstructedUpdateStatement(TestEnum.FooUpdate, typeof(BaseAceObject), o2);

            BaseAceObject o3 = new BaseAceObject();
            worldDb.ExecuteConstructedGetStatement(TestEnum.FooGet, typeof(BaseAceObject), criteria, o3);

            Assert.Equal(o2.Name, o3.Name);
        }

        [Fact]
        public void ExecuteGetListStatement_SimpleCase_DoesNotThrow()
        {
            worldDb.ConstructStatement(TestEnum.GetLifestonesByLandblock, typeof(AceObject), ConstructedStatementType.GetList);

            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("landblock", (ushort)458);
            var results = worldDb.ExecuteConstructedGetListStatement<TestEnum, AceObject>(TestEnum.GetLifestonesByLandblock, criteria);

            Assert.NotNull(results);
            Assert.True(results.Count > 0);
        }
    }
}
