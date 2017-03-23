using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            FooGet
        }

        [TestMethod]
        public void ExersiceInsertGetUpdate_SimpleCase_DoesNotThrow()
        {
            BaseAceObject o = new BaseAceObject();
            o.AceObjectId = 1;
            o.Name = "foo";

            Database db = new Database();
            db.Initialise("localhost", 3306, "root", "!Turbine1", "ace_world");
            db.ContructStatement(TestEnum.FooInsert, typeof(BaseAceObject), ConstructedStatementType.Insert);
            db.ContructStatement(TestEnum.FooUpdate, typeof(BaseAceObject), ConstructedStatementType.Update);
            db.ContructStatement(TestEnum.FooGet, typeof(BaseAceObject), ConstructedStatementType.Get);

            db.ExecuteConstructedInsertStatement(TestEnum.FooInsert, typeof(BaseAceObject), o);
            
            BaseAceObject o2 = new BaseAceObject();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("baseAceObjectId", 1u);
            db.ExecuteConstructedGetStatement(TestEnum.FooGet, typeof(BaseAceObject), criteria, o2);

            Assert.AreEqual(o.Name, o2.Name);
            Assert.AreEqual(o.AceObjectId, o2.AceObjectId);

            o2.Name = "bar";
            db.ExecuteConstructedUpdateStatement(TestEnum.FooUpdate, typeof(BaseAceObject), o2);

            BaseAceObject o3 = new BaseAceObject();
            db.ExecuteConstructedGetStatement(TestEnum.FooGet, typeof(BaseAceObject), criteria, o3);

            Assert.AreEqual(o2.Name, o3.Name);
        }
    }
}
