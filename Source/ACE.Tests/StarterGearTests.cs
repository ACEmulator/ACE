using System;
using System.IO;
using ACE.Entity;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Tests
{
    [TestClass]
    public class StarterGearTests
    {
        [TestMethod]
        public void CanParseStarterGearJson()
        {
            string contents = File.ReadAllText("../../../../../ACE/starterGear.json");

            StarterGearConfiguration config = JsonConvert.DeserializeObject<StarterGearConfiguration>(contents);
        }
    }
}
