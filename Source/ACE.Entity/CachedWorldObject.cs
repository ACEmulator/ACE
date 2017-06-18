using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_object")]
    public class CachedWordObject
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("landblock", (int)MySqlDbType.UInt16, ListGet = true)]
        public ushort Landblock { get; set; }

        [DbField("itemType", (int)MySqlDbType.UInt32, ListGet = true)]
        public uint ItemType { get; set; }
    }
}
