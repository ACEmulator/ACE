using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    public class AnimationOverride
    {
        [DbField("index", (int)MySqlDbType.UByte)]
        public byte Index { get; set; }

        [DbField("animationId", (int)MySqlDbType.UInt32)]
        public uint AnimationId { get; set; }
    }
}
