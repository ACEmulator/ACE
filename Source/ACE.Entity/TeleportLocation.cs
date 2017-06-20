using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_teleport_location")]
    public class TeleportLocation
    {
        [DbField("name", (int)MySqlDbType.Text)]
        public string Location { get; set; }

        [DbField("landblock", (int)MySqlDbType.UInt16)]
        public uint LandblockRaw { get; set; }

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

        public Position Position
        {
            get { return new Position(LandblockRaw, PosX, PosY, PosZ, QX, QY, QZ, QW); }
        }
    }
}