using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_string")]
    [DbList("ace_object_properties_string", "AceObjectId")]
    public class AceObjectPropertiesString
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("strPropertyId", (int)MySqlDbType.UInt16)]
        public ushort PropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.Text)]
        public string PropertyValue { get; set; }
    }
}
