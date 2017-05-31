using ACE.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database.LightweightEntities
{
    internal class SlimCharacter
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint Id { get; set; }

        [DbField("name", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public string Name { get; set; }
    }
}
