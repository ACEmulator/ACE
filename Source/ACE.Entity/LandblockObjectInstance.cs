using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_landblock")]
    public class LandblockObjectInstance
    {
        // not needed to be loaded into object
        // [DbField("instanceId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true)]
        // public uint InstanceId { get; set; }

        [DbField("landblock", (int)MySqlDbType.Int32, IsCriteria = true, ListGet = true)]
        public int Landblock { get; set; }

        [DbField("weenieClassId", (int)MySqlDbType.UInt32)]
        public uint WeenieClassId { get; set; }

        [DbField("preassignedGuid", (int)MySqlDbType.UInt32)]
        public uint PreassignedGuid { get; set; }

        [DbField("landblockRaw", (int)MySqlDbType.UInt32)]
        public uint LandblockRaw { get; set; }

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
    }
}
