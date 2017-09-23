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
    [DbTable("ace_vendor_inventory")]
    public class VendorItems
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32)]
        public uint AceObjectId { get; set; }
    }
}