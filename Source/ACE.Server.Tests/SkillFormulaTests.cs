using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Server.WorldObjects;

namespace ACE.Server.Tests
{
    [TestClass]
    public class SkillFormulaTests
    {
        [TestMethod]
        public void FiftyFiftyIsAccurate()
        {
            var result = SkillCheck.GetSkillChance(100, 100);
            Assert.AreEqual(0.5d, result);
        }
    }
}
