using System;
using ACE.Common;

using Newtonsoft.Json;

namespace ACE.Entity
{
    //[DbTable("ace_object_properties_attribute")]
    public class AceObjectPropertiesAttribute : BaseAceProperty, ICloneable
    {
        private uint _xpSpent;
        private ushort _ranks;
        private ushort _base;

        [JsonProperty("attributeId")]
        //[DbField("attributeId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public ushort AttributeId { get; set; }

        [JsonProperty("baseValue")]
        //[DbField("attributeBase", (int)MySqlDbType.UInt16)]
        public ushort AttributeBase
        {
            get
            {
                return _base;
            }
            set
            {
                _base = value;
                IsDirty = true;
            }
        }

        [JsonProperty("ranks")]
        //[DbField("attributeRanks", (int)MySqlDbType.UInt16)]
        public ushort AttributeRanks
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
        //[DbField("attributeXpSpent", (int)MySqlDbType.UInt32)]
        public uint AttributeXpSpent
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

        [JsonIgnore]
        public uint ActiveValue
        {
            get
            {
                return (uint)AttributeBase + AttributeRanks;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
