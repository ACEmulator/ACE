using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_int")]
    [DbGetList("ace_object_properties_int", 16, "AceObjectId")]
    public class AceObjectPropertiesInt
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("intPropertyId", (int)MySqlDbType.UInt32)]
        public uint IntPropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.UInt32)]
        public uint PropertyValue { get; set; }
    }
}

