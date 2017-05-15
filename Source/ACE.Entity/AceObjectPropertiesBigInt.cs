using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_bigint")]
    [DbGetList("ace_object_properties_bitint", 17, "AceObjectId")]
    public class AceObjectPropertiesBigInt
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("bigIntPropertyId", (int)MySqlDbType.UInt32)]
        public uint BigIntPropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.UInt64)]
        public ulong PropertyValue { get; set; }
    }
}
