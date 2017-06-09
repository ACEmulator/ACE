using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_double")]
    [DbList("ace_object_properties_double", "AceObjectId")]
    public class AceObjectPropertiesDouble
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("dblPropertyId", (int)MySqlDbType.UInt16)]
        public ushort PropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.Double)]
        public double PropertyValue { get; set; }
    }
}
