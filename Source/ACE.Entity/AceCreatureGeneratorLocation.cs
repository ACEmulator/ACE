using ACE.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ACE.Entity
{
    [DbTable("ace_creature_generator_locations")]
    [DbGetList("ace_creature_generator_locations", 12, "landblock")]
    public class AceCreatureGeneratorLocation
    {    
        [DbField("id", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public uint Id { get; set; }

        [DbField("generatorId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public uint GeneratorId { get; set; }

        [DbField("quantity", (int)MySqlDbType.UByte)]
        public byte Quantity { get; set; }

        public Position Position
        {
            get { return new Position((((uint)Landblock) << 16) + Cell, PosX, PosY, PosZ, QX, QY, QZ, QW); }
        }

        [DbField("landblock", (int)MySqlDbType.UInt16)]
        public ushort Landblock { get; set; }

        [DbField("cell", (int)MySqlDbType.UInt16)]
        public ushort Cell { get; set; }

        [DbField("posX", (int)MySqlDbType.Float)]
        public float PosX { get; set; }

        [DbField("posY", (int)MySqlDbType.Float)]
        public float PosY { get; set; }

        [DbField("posZ", (int)MySqlDbType.Float)]
        public float PosZ { get; set; }

        [DbField("qW", (int)MySqlDbType.Float)]
        public float QW { get; set; }

        [DbField("qX", (int)MySqlDbType.Float)]
        public float QX { get; set; }

        [DbField("qY", (int)MySqlDbType.Float)]
        public float QY { get; set; }

        [DbField("qZ", (int)MySqlDbType.Float)]
        public float QZ { get; set; }

        public List<AceCreatureGeneratorData> CreatureGeneratorData = new List<AceCreatureGeneratorData>();
    }
}
