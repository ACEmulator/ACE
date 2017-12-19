using System;
using ACE.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ACE.Entity
{
    [DbTable("ace_object_generator_profile")]
    public class AceObjectGeneratorProfile : ICloneable
    {
        [JsonIgnore]
        [DbField("profileId", (int)MySqlDbType.UInt32)]
        public uint ProfileId { get; set; }

        [JsonIgnore]
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [JsonProperty("probability")]
        [DbField("probability", (int)MySqlDbType.Float)]
        public float Probability { get; set; }

        [JsonProperty("weenieClassId")]
        [DbField("weenieClassId", (int)MySqlDbType.UInt32)]
        public uint WeenieClassId { get; set; }

        [JsonProperty("delay")]
        [DbField("delay", (int)MySqlDbType.Float)]
        public float Delay { get; set; }

        [JsonProperty("initCreate")]
        [DbField("initCreate", (int)MySqlDbType.UInt32)]
        public uint InitCreate { get; set; }

        [JsonProperty("maxCreate")]
        [DbField("maxCreate", (int)MySqlDbType.UInt32)]
        public uint MaxCreate { get; set; }

        [JsonProperty("whenCreate")]
        [DbField("whenCreate", (int)MySqlDbType.UInt32)]
        public uint WhenCreate { get; set; }

        [JsonProperty("whereCreate")]
        [DbField("whereCreate", (int)MySqlDbType.UInt32)]
        public uint WhereCreate { get; set; }

        ////[JsonProperty("destinationType")]
        ////[DbField("destinationType", (int)MySqlDbType.Byte)]
        ////public sbyte DestinationType { get; set; }

        [JsonProperty("stackSize")]
        [DbField("stackSize", (int)MySqlDbType.Int32)]
        public int StackSize { get; set; }

        [JsonProperty("paletteId")]
        [DbField("paletteId", (int)MySqlDbType.UInt32)]
        public uint PaletteId { get; set; }

        [JsonProperty("shade")]
        [DbField("shade", (int)MySqlDbType.Float)]
        public float Shade { get; set; }

        // [JsonProperty("tryToBond")]
        // [DbField("tryToBond", (int)MySqlDbType.Byte)]
        // public bool TryToBond { get; set; }

        [DbField("landblockRaw", (int)MySqlDbType.UInt32)]
        public uint LandblockRaw { get; set; }

        [DbField("posX", (int)MySqlDbType.Float)]
        public float PositionX { get; set; }

        [DbField("posY", (int)MySqlDbType.Float)]
        public float PositionY { get; set; }

        [DbField("posZ", (int)MySqlDbType.Float)]
        public float PositionZ { get; set; }

        [DbField("qW", (int)MySqlDbType.Float)]
        public float RotationW { get; set; }

        [DbField("qX", (int)MySqlDbType.Float)]
        public float RotationX { get; set; }

        [DbField("qY", (int)MySqlDbType.Float)]
        public float RotationY { get; set; }

        [DbField("qZ", (int)MySqlDbType.Float)]
        public float RotationZ { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
