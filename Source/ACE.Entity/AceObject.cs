using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_object")]
    [DbGetList("vw_ace_object", 2, "landblock")]
    public class AceObject : BaseAceObject
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public override uint AceObjectId { get; set; }

        public Position Position
        {
            get { return new Position(Landblock, PosX, PosY, PosZ, QX, QY, QZ, QW); }
        }

        [DbField("landblock", (int)MySqlDbType.UInt16)]
        public uint Landblock { get; set; }

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
    }
}
