using ACE.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [DbTable("weenie_texture_map_changes")]
    [DbGetList("weenie_texture_map_changes", 5, "weenieClassId")]
    public class WeenieTextureMapOverride
    {
        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public ushort WeenieClassId { get; set; }

        [DbField("index", (int)MySqlDbType.UByte)]
        public byte Index { get; set; }

        [DbField("oldId", (int)MySqlDbType.UInt32)]
        public uint OldId { get; set; }

        [DbField("newId", (int)MySqlDbType.UInt32)]
        public uint NewId { get; set; }
    }
}
