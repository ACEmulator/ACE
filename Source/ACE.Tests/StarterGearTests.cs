using System;
using System.IO;
using ACE.Entity;
using ACE.Server.Entity;
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
            string contents = File.ReadAllText("../../../../../ACE.Server/starterGear.json");

            StarterGearConfiguration config = JsonConvert.DeserializeObject<StarterGearConfiguration>(contents);
        }
    }
}
