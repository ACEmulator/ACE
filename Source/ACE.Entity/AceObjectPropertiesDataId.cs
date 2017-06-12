using System;
using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_did")]
    [DbList("ace_object_properties_did", "aceObjectId")]
    public class AceObjectPropertiesDataId : ICloneable
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("didPropertyId", (int)MySqlDbType.UInt16)]
        public uint PropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.UInt32)]
        public uint PropertyValue { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
