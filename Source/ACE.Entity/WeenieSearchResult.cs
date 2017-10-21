using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [DbField("weenieType", (int)MySqlDbType.UInt32)]
        public uint WeenieType_Binder
        {
            get { return (uint)WeenieType; }
            set { WeenieType = (WeenieType)value; }
        }

        public ItemType ItemType { get; set; }

        [JsonIgnore]
        [DbField("itemType", (int)MySqlDbType.UInt32)]
        public uint ItemType_Binder
        {
            get { return (uint)ItemType; }
            set { ItemType = (ItemType)value; }
        }

        [DbField("userModified", (int)MySqlDbType.Byte)]
        public bool UserModified { get; set; }
    }
}
