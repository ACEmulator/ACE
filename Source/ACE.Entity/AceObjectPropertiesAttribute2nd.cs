using ACE.Common;
using MySql.Data.MySqlClient;
// ReSharper disable InconsistentNaming
namespace ACE.Entity
{
    [DbTable("ace_object_properties_attribute2nd")]
    [DbGetList("ace_object_properties_attribute2nd", 25, "AceObjectId")]

    public class AceObjectPropertiesAttribute2nd
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("attribute2ndid", (int)MySqlDbType.UInt16)]
        public ushort Attribute2ndId { get; set; }

        [DbField("attribute2ndValue", (int)MySqlDbType.UInt16)]
        public ushort Attribute2ndValue { get; set; }

        [DbField("attribute2ndRanks", (int)MySqlDbType.UInt16)]
        public ushort Attribute2ndRanks { get; set; }

        [DbField("attribute2ndXpSpent", (int)MySqlDbType.UInt32)]
        public uint Attribute2ndXpSpend { get; set; }
    }
}
