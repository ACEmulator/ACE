using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_object_animation_changes")]
    [DbGetList("ace_object_animation_changes", 4, "baseAceObjectId")]
    public class AnimationOverride
    {
        [DbField("baseAceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("index", (int)MySqlDbType.UByte)]
        public byte Index { get; set; }

        [DbField("animationId", (int)MySqlDbType.UInt32)]
        public uint AnimationId { get; set; }
    }
}
