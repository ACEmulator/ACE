using System;
using System.IO;

using System.Text.Json;
using System.Text.Json.Serialization;

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
            var testDir = AppContext.BaseDirectory;
            var starterGearPath = Path.GetFullPath(Path.Combine(testDir, "..", "..", "..", "..", "..", "ACE.Server", "starterGear.json"));
            string contents = File.ReadAllText(starterGearPath);

            StarterGearConfiguration config = JsonSerializer.Deserialize<StarterGearConfiguration>(contents, SerializerOptions);
        }

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            AllowTrailingCommas = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = true
        };
    }
}
