using System;
using ACE.Common;
using MySql.Data.MySqlClient;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    [DbTable("ace_object_properties_bigint")]
    public class AceObjectPropertiesInt64 : BaseAceProperty, ICloneable
    {
        private ulong? _value = 0;
        
        [DbField("bigIntPropertyId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public override uint PropertyId { get; set; }

        [DbField("propertyIndex", (int)MySqlDbType.Byte, IsCriteria = true, Update = false)]
        public byte Index { get; set; } = 0;

        [DbField("propertyValue", (int)MySqlDbType.UInt64)]
        public ulong? PropertyValue
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
        { get { return AceObjectPropertyType.PropertyInt64; } }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
