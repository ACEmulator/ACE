﻿using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_object_texture_map_change")]
    [DbList("ace_object_texture_map_change", "aceObjectId")]
    public class TextureMapOverride
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
