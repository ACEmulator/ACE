using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_attribute")]
    [DbGetList("ace_object_properties_attribute", 24, "AceObjectId")]
    public class AceObjectPropertiesAttribute
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("attributeId", (int)MySqlDbType.UInt16)]
        public ushort AttributeId { get; set; }

        [DbField("attributeBase", (int)MySqlDbType.UInt16)]
        public ushort AttributeBase { get; set; }

        [DbField("attributeRanks", (int)MySqlDbType.UInt16)]
        public ushort AttributeRanks { get; set; }

        [DbField("attributeXpSpent", (int)MySqlDbType.UInt32)]
        public uint AttributeXpSpent { get; set; }
    }
}
