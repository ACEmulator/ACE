using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_bool")]
    [DbGetList("ace_object_properties_bool", 19, "AceObjectId")]
    public class AceObjectPropertiesBool
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("boolPropertyId", (int)MySqlDbType.UInt32)]
        public uint BoolPropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.Bit)]
        public bool PropertyValue { get; set; }
    }
}
