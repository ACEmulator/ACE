using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_iid")]
    [DbGetList("ace_object_properties_iid", 22, "AceObjectId")]
    public class AceObjectPropertiesIid
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("iidPropertyId", (int)MySqlDbType.UInt16)]
        public uint IidPropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.UInt32)]
        public uint PropertyValue { get; set; }
    }
}
