using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_did")]
    [DbGetList("ace_object_properties_did", 21, "AceObjectId")]
    public class AceObjectPropertiesDataId
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("didPropertyId", (int)MySqlDbType.UInt16)]
        public uint PropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.UInt32)]
        public uint PropertyValue { get; set; }
    }
}
