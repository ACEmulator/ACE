using System;
using ACE.Common;

using ACE.Entity.Enum;
using Newtonsoft.Json;

namespace ACE.Entity
{
    //[DbTable("ace_object_properties_iid")]
    public class AceObjectPropertiesInstanceId : BaseAceProperty, ICloneable
    {
        private uint? _value = 0;

        [JsonProperty("iidPropertyId")]
        //[DbField("iidPropertyId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public new uint PropertyId { get; set; }

        [JsonProperty("index")]
        //[DbField("propertyIndex", (int)MySqlDbType.Byte, IsCriteria = true, Update = false)]
        public byte Index { get; set; }

        [JsonProperty("value")]
        //[DbField("propertyValue", (int)MySqlDbType.UInt32)]
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
        { get { return AceObjectPropertyType.PropertyInstanceId; } }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
