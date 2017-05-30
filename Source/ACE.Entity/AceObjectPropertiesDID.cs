using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_did")]
    [DbGetList("ace_object_properties_did", 21, "AceObjectId")]
    public class AceObjectPropertiesDid
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("didPropertyId", (int)MySqlDbType.UInt16)]
        public uint DidPropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.UInt32)]
        public uint PropertyValue { get; set; }
    }
}
