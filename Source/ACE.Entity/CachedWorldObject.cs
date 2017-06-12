using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_object")]
    [DbList("vw_ace_object", "landblock")]
    public class CachedWordObject
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        /*
        [DbField("name", (int)MySqlDbType.Text)]
        public string Name { get; set; }

        [DbField("weenieClassId", (int)MySqlDbType.UInt32)]
        public uint WeenieClassId { get; set; }

        [DbField("currentMotionState", (int)MySqlDbType.Text)]
        public string CurrentMotionState { get; set; }

        [DbField("weenieClassDescription", (int)MySqlDbType.Text)]
        public string WeenieClassDescription { get; set; }

        [DbField("aceObjectDescriptionFlags", (int)MySqlDbType.UInt32)]
        public uint AceObjectDescriptionFlags { get; set; }

        [DbField("physicsDescriptionFlag", (int)MySqlDbType.UInt32)]
        public uint PhysicsDescriptionFlag { get; set; }

        [DbField("weenieHeaderFlags", (int)MySqlDbType.UInt32)]
        public uint WeenieHeaderFlags { get; set; }

        [DbField("LandblockRaw", (int)MySqlDbType.UInt32)]
        public uint LandblockRaw { get; set; }
        */

        [DbField("landblock", (int)MySqlDbType.UInt16)]
        public ushort Landblock { get; set; }

        /*
        [DbField("cell", (int)MySqlDbType.UInt16)]
        public ushort Cell { get; set; }
        */

        public CachedWordObject() { }
    }
}
