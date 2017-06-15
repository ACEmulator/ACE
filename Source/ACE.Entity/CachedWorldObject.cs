﻿using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_object")]
    [DbList("vw_ace_object", "landblock")]
    public class CachedWordObject
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("landblock", (int)MySqlDbType.UInt16)]
        public ushort Landblock { get; set; }
    }
}
