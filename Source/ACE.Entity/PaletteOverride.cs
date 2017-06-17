using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_object_palette_change")]
    public class PaletteOverride : ICloneable
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [DbField("subPaletteId", (int)MySqlDbType.UInt32)]
        public uint SubPaletteId { get; set; }

        [DbField("offset", (int)MySqlDbType.UInt16)]
        public ushort Offset { get; set; }

        [DbField("length", (int)MySqlDbType.UInt16)]
        public ushort Length { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
