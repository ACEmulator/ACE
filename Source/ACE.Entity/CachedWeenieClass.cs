using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_weenie_class")]
    public class CachedWeenieClass
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("name", (int)MySqlDbType.String, ListGet = true)]
        public string Name { get; set; }

        [DbField("weenieClassId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint WeenieClassId { get; set; }

        [DbField("weenieClassDescription", (int)MySqlDbType.String, ListGet = true)]
        public string WeenieClassDescription { get; set; }

        [DbField("itemType", (int)MySqlDbType.UInt32, ListGet = true)]
        public uint ItemType { get; set; }
    }
}
