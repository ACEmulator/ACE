using ACE.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [DbTable("weenie_class")]
    public class Weenie : BaseAceObject
    {
        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public ushort WeenieClassId { get; set; }

        [DbField("aceObjectId", (int)MySqlDbType.UInt32)]
        public override uint AceObjectId { get; set; }
    }
}
