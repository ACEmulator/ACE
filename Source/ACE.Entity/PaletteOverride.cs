using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    public class PaletteOverride
    {
        [DbField("subPaletteId", (int)MySqlDbType.UByte)]
        public byte SubPaletteId { get; set; }

        [DbField("offset", (int)MySqlDbType.UByte)]
        public byte Offset { get; set; }

        [DbField("length", (int)MySqlDbType.UByte)]
        public byte Length { get; set; }
    }
}
