using System;
using ACE.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ACE.Entity
{
    [DbTable("ace_object_generator_link")]
    public class AceObjectGeneratorLink : ICloneable
    {
        [JsonIgnore]
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [JsonProperty("index")]
        [DbField("index", (int)MySqlDbType.UByte)]
        public byte Index { get; set; }

        [JsonProperty("generatorWeenieClassId")]
        [DbField("generatorWeenieClassId", (int)MySqlDbType.UInt32)]
        public uint GeneratorWeenieClassId { get; set; }

        [JsonProperty("generatorWeight")]
        [DbField("generatorWeight", (int)MySqlDbType.UByte)]
        public byte GeneratorWeight { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
