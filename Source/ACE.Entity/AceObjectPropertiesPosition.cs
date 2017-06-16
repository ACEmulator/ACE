using System;
using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_position")]
    [DbList("ace_position", "aceObjectId")]
    public class AceObjectPropertiesPosition : ICloneable
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("landblockRaw", (int)MySqlDbType.UInt32)]
        public uint Cell { get; set; }

        [DbField("positionType", (int)MySqlDbType.UInt16, Update = false, IsCriteria = true)]
        public ushort DbPositionType { get; set; }

        [DbField("positionId", (int)MySqlDbType.UInt32)]
        public uint PositionId { get; set; }

        [DbField("posX", (int)MySqlDbType.Float)]
        public float PositionX { get; set; }

        [DbField("posY", (int)MySqlDbType.Float)]
        public float PositionY { get; set; }

        [DbField("posZ", (int)MySqlDbType.Float)]
        public float PositionZ { get; set; }

        [DbField("qW", (int)MySqlDbType.Float)]
        public float RotationW { get; set; }

        [DbField("qX", (int)MySqlDbType.Float)]
        public float RotationX { get; set; }

        [DbField("qY", (int)MySqlDbType.Float)]
        public float RotationY { get; set; }

        [DbField("qZ", (int)MySqlDbType.Float)]
        public float RotationZ { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
