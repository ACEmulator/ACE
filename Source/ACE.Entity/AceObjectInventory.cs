using System;
using ACE.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ACE.Entity
{
    [DbTable("ace_object_inventory")]
    public class AceObjectInventory : ICloneable
    {
        [JsonIgnore]
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [JsonProperty("weenieClassId")]
        [DbField("weenieClassId", (int)MySqlDbType.UInt32)]
        public uint WeenieClassId { get; set; }

        [JsonProperty("destinationType")]
        [DbField("destinationType", (int)MySqlDbType.Byte)]
        public sbyte DestinationType { get; set; }

        [JsonProperty("stackSize")]
        [DbField("stackSize", (int)MySqlDbType.Int32)]
        public int StackSize { get; set; }

        [JsonProperty("palette")]
        [DbField("palette", (int)MySqlDbType.Byte)]
        public sbyte Palette { get; set; }

        // [JsonProperty("shade")]
        [JsonIgnore]
        [DbField("shade", (int)MySqlDbType.Float)]
        public float Shade { get; set; }

        // [JsonProperty("tryToBond")]
        [JsonIgnore]
        [DbField("tryToBond", (int)MySqlDbType.Byte)]
        public sbyte TryToBond { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
