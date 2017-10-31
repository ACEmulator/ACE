using System;
using ACE.Common;
using ACE.Entity.Enum;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ACE.Entity
{
    [DbTable("ace_object_properties_int")]
    public class AceObjectPropertiesInt : BaseAceProperty, ICloneable
    {
        private uint? _value = 0;

        [JsonProperty("intPropertyId")]
        [DbField("intPropertyId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public new uint PropertyId { get; set; }

        [JsonProperty("index")]
        [DbField("propertyIndex", (int)MySqlDbType.Byte, IsCriteria = true, Update = false)]
        public byte Index { get; set; } = 0;

        [JsonProperty("value")]
        [DbField("propertyValue", (int)MySqlDbType.UInt32)]
        public uint? PropertyValue
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
        
        [JsonIgnore]
        public override AceObjectPropertyType PropertyType
        { get { return AceObjectPropertyType.PropertyInt; } }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
