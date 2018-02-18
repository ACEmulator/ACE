using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Entity;

namespace ACE.Server.Tests
{
    [TestClass]
    public class SkillFormulaTests
    {
        [TestMethod]
        public void FiftyFiftyIsAccurate()
        {
            var result = CreatureSkillOld.GetPercentSuccess(100, 100);
            Assert.AreEqual(0.5d, result);
        }
    }
}
