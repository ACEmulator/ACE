using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_wielded_object")]
    public class CachedWieldedObject
    {
        [DbField("wielderId", (int)MySqlDbType.UInt32, ListGet = true)]
        public uint ContainerId { get; set; }

        [DbField("aceObjectId", (int)MySqlDbType.UInt32)]
        public uint AceObjectId { get; set; }

        [DbField("wieldedLocation", (int)MySqlDbType.UInt32)]
        public uint Placement { get; set; }
    }
}
