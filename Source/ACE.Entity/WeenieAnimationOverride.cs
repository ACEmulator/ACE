using ACE.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [DbTable("ace_object_animation_change")]
    [DbGetList("ace_object_animation_change", 6, "aceObjectId")]
    public class WeenieAnimationOverride
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("index", (int)MySqlDbType.UByte)]
        public byte Index { get; set; }

        [DbField("animationId", (int)MySqlDbType.UInt32)]
        public uint AnimationId { get; set; }
    }
}
