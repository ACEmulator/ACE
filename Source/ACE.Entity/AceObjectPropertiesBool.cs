using System;
using ACE.Common;

using ACE.Entity.Enum;
using Newtonsoft.Json;

namespace ACE.Entity
{
    //[DbTable("ace_object_properties_bool")]
    public class AceObjectPropertiesBool : BaseAceProperty, ICloneable
    {
        private bool? _value = false;

        [JsonProperty("boolPropertyId")]
        //[DbField("boolPropertyId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public new uint PropertyId { get; set; }

        [JsonProperty("index")]
        //[DbField("propertyIndex", (int)MySqlDbType.Byte, IsCriteria = true, Update = false)]
        public byte Index { get; set; }

        [JsonProperty("value")]
        //[DbField("propertyValue", (int)MySqlDbType.Bit)]

        public bool? PropertyValue
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
        { get { return AceObjectPropertyType.PropertyBool; } }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
