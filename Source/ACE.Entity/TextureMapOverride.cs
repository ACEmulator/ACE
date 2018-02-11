using System;
using ACE.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ACE.Entity
{
    [DbTable("ace_object_texture_map_change")]
    public class TextureMapOverride : ICloneable
    {
        [JsonIgnore]
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [JsonProperty("index")]
        [DbField("index", (int)MySqlDbType.UByte)]
        public byte Index { get; set; }

        [JsonProperty("oldTextureId")]
        [DbField("oldId", (int)MySqlDbType.UInt32)]
        public uint OldId { get; set; }

        [JsonProperty("newTextureId")]
        [DbField("newId", (int)MySqlDbType.UInt32)]
        public uint NewId { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
