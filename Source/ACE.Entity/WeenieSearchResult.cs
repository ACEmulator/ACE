using ACE.Entity.Enum;
using ACE.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ACE.Entity
{
    [DbTable("vw_weenie_search")]
    public class WeenieSearchResult
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32)]
        public uint WeenieClassId { get; set; }

        [DbField("name", (int)MySqlDbType.Text)]
        public string Name { get; set; }

        [DbField("weenieClassDescription", (int)MySqlDbType.Text)]
        public string Description { get; set; }
        
        public WeenieType WeenieType { get; set; }

        [JsonIgnore]
        [DbField("weenieType", (int)MySqlDbType.Int32)]
        public int WeenieType_Binder
        {
            get { return (int)WeenieType; }
            set { WeenieType = (WeenieType)value; }
        }

        public ItemType ItemType { get; set; }

        [JsonIgnore]
        [DbField("itemType", (int)MySqlDbType.Int32)]
        public int ItemType_Binder
        {
            get { return (int)ItemType; }
            set { ItemType = (ItemType)value; }
        }
    }
}
