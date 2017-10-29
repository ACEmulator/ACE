using System;
using ACE.Common;
using MySql.Data.MySqlClient;
using ACE.Entity.Enum;
using Newtonsoft.Json;

namespace ACE.Entity
{
    [DbTable("ace_object_properties_string")]
    public class AceObjectPropertiesString : BaseAceProperty, ICloneable
    {
        private string _value = null;

        [JsonProperty("stringPropertyId")]
        [DbField("strPropertyId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public new ushort PropertyId { get; set; }

        [JsonProperty("index")]
        [DbField("propertyIndex", (int)MySqlDbType.Byte, IsCriteria = true, Update = false)]
        public byte Index { get; set; } = 0;

        [JsonProperty("value")]
        [DbField("propertyValue", (int)MySqlDbType.Text)]
        public string PropertyValue
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

        public override AceObjectPropertyType PropertyType
        { get { return AceObjectPropertyType.PropertyString; } }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
