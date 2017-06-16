using ACE.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ACE.Entity
{
    [DbTable("ace_creature_generator_data")]
    [DbList("ace_creature_generator_data", "generatorId")]
    public class AceCreatureGeneratorData
    {
        [DbField("generatorId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public uint GeneratorId { get; set; }

        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public ushort WeenieClassId { get; set; }

        [DbField("probability", (int)MySqlDbType.UByte)]
        public byte Probability { get; set; }
    }
}
