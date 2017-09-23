using ACE.Common;
using ACE.Entity.Enum;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [DbTable("ace_object_inventory")]
    public class VendorItems
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32)]
        public uint AceObjectId { get; set; }

        [DbField("weenieClassId", (int)MySqlDbType.UInt32)]
        public uint WeenieClassId { get; set; }

        //[DbField("destinationType", (int)MySqlDbType.Byte)]
        //public uint DestinationType { get; set; }

        //[DbField("palette", (int)MySqlDbType.Byte)]
        //public uint Palette { get; set; }
    }
}