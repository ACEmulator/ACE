using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_inventory_object")]
    public class CachedInventoryObject
    {
        [DbField("containerId", (int)MySqlDbType.UInt32, ListGet = true)]
        public uint ContainerId { get; set; }

        [DbField("aceObjectId", (int)MySqlDbType.UInt32)]
        public uint AceObjectId { get; set; }

        [DbField("placement", (int)MySqlDbType.UInt32)]
        public uint Placement { get; set; }
    }
}
