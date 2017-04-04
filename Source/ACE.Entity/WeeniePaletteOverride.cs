using ACE.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [DbTable("weenice_palette_changes")]
    [DbGetList("weenie_palette_changes", 4, "weenieClassId")]
    public class WeeniePaletteOverride
    {
        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public ushort WeenieClassId { get; set; }

        [DbField("subPaletteId", (int)MySqlDbType.UInt32)]
        public uint SubPaletteId { get; set; }

        [DbField("offset", (int)MySqlDbType.UInt16)]
        public ushort Offset { get; set; }

        [DbField("length", (int)MySqlDbType.UInt16)]
        public ushort Length { get; set; }
    }
}
