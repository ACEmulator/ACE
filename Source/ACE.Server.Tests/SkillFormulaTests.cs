using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.Tests
{
    [TestClass]
    public class SkillFormulaTests
    {
        [TestMethod]
        public void FiftyFiftyIsAccurate()
        {
            var result = CreatureSkill.GetPercentSuccess(100, 100);
            Assert.AreEqual(0.5d, result);
        }
    }
}
