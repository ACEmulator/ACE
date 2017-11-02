using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_object")]
    public class CachedWorldObject
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("landblock", (int)MySqlDbType.UInt16, ListGet = true)]
        public uint Landblock { get; set; }

        [DbField("itemType", (int)MySqlDbType.Int32, ListGet = true)]
        public int ItemType { get; set; }
    }
}
