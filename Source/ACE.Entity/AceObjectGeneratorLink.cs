using System;
using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_object_generator_link")]
    public class AceObjectGeneratorLink : ICloneable
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [DbField("index", (int)MySqlDbType.UByte)]
        public byte Index { get; set; }

        [DbField("generatorWeenieClassId", (int)MySqlDbType.UInt32)]
        public uint GeneratorWeenieClassId { get; set; }

        [DbField("generatorWeight", (int)MySqlDbType.UByte)]
        public byte GeneratorWeight { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
