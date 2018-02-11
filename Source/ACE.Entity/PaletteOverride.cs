using System;
using ACE.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ACE.Entity
{
    [DbTable("ace_object_palette_change")]
    public class PaletteOverride : ICloneable
    {
        [JsonIgnore]
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [JsonProperty("subPaletteId")]
        [DbField("subPaletteId", (int)MySqlDbType.UInt32)]
        public uint SubPaletteId { get; set; }

        [JsonProperty("offset")]
        [DbField("offset", (int)MySqlDbType.UInt16)]
        public ushort Offset { get; set; }

        [JsonProperty("length")]
        [DbField("length", (int)MySqlDbType.UInt16)]
        public ushort Length { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
