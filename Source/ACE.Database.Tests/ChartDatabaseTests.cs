using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Database.Tests
{
    [TestClass]
    public class ChartDatabaseTests
    {
        [TestMethod]
        public void GetLevelingXpChart_HappyPath_Loads275Levels()
        {
            // this path will be the bin/config directory of the unit test project
            var path = AppDomain.CurrentDomain.BaseDirectory;

            // path up and around to levels.json
            var absPath = Path.GetFullPath(Path.Combine(path, "../../../ACE"));

            ChartDatabase db = new ChartDatabase(absPath);

            var levelChart = db.GetLevelingXpChart();

            Assert.IsNotNull(levelChart);
            Assert.AreEqual(275, levelChart.Levels.Count);
        }
    }
}
