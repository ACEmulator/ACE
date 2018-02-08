using System.IO;

using Newtonsoft.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Server.Entity;

namespace ACE.Server.Tests
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
