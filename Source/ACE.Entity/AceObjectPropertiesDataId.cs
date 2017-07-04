using System;
using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_did")]
    public class AceObjectPropertiesDataId : BaseAceProperty, ICloneable
    {
        private uint? _value = 0;
        
        [DbField("didPropertyId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public uint PropertyId { get; set; }

        [DbField("propertyIndex", (int)MySqlDbType.Byte, IsCriteria = true, Update = false)]
        public byte Index { get; set; } = 0;

        /// <summary>
        /// set to null to delete.  do not directly remove.
        /// </summary>
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
        
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
