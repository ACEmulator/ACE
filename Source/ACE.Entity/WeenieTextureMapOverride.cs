using ACE.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [DbTable("ace_object_texture_map_change")]
    [DbGetList("ace_object_texture_map_change", 5, "aceObjectId")]
    public class WeenieTextureMapOverride
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("index", (int)MySqlDbType.UByte)]
        public byte Index { get; set; }

        [DbField("oldId", (int)MySqlDbType.UInt32)]
        public uint OldId { get; set; }

        [DbField("newId", (int)MySqlDbType.UInt32)]
        public uint NewId { get; set; }
    }
}
