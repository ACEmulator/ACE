using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_double")]
    [DbGetList("ace_object_properties_double", 18, "AceObjectId")]
    public class AceObjectPropertiesDouble
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("dblPropertyId", (int)MySqlDbType.UInt32)]
        public uint DblPropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.Double)]
        public double PropertyValue { get; set; }
    }
}
