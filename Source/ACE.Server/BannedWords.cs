using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ACE.Database
{
    public static class BadWords // TODO: Replace loading of words to use DAT and merge with json for additional
    {
        public static List<string> BadWordsList;
        public static void ReadFile()
        {
            using (var fs = new FileStream("./TabooTable.json", FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fs))
            {
                dynamic jsonData = JsonConvert.DeserializeObject(reader.ReadToEnd());
                BadWordsList = jsonData.words.ToObject<List<string>>();
            }
        }
    }
}
