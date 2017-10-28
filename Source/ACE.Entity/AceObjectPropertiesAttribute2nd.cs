using System;
using ACE.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace ACE.Entity
{
    [DbTable("ace_object_properties_attribute2nd")]
    public class AceObjectPropertiesAttribute2nd : BaseAceProperty, ICloneable
    {
        private uint _xpSpent = 0;
        private ushort _ranks = 0;
        private uint _value = 0;

        [JsonProperty("vitalId")]
        [DbField("attribute2ndId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public ushort Attribute2ndId { get; set; }

        [JsonProperty("value")]
        [DbField("attribute2ndValue", (int)MySqlDbType.UInt24)]
        public uint Attribute2ndValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                IsDirty = true;
            }
        }

        [JsonProperty("ranks")]
        [DbField("attribute2ndRanks", (int)MySqlDbType.UInt16)]
        public ushort Attribute2ndRanks
        {
            get
            {
                return _ranks;
            }
            set
            {
                _ranks = value;
                IsDirty = true;
            }
        }

        [JsonProperty("experienceSpent")]
        [DbField("attribute2ndXpSpent", (int)MySqlDbType.UInt32)]
        public uint Attribute2ndXpSpent
        {
            get
            {
                return _xpSpent;
            }
            set
            {
                _xpSpent = value;
                IsDirty = true;
            }
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
