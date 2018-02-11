using System;
using ACE.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ACE.Entity
{
    [DbTable("ace_object_animation_change")]
    public class AnimationOverride : ICloneable
    {
        [JsonIgnore]
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [JsonProperty("index")]
        [DbField("index", (int)MySqlDbType.UByte)]
        public byte Index { get; set; }

        [JsonProperty("animationId")]
        [DbField("animationId", (int)MySqlDbType.UInt32)]
        public uint AnimationId { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
