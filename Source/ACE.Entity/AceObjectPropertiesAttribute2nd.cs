using System;
using ACE.Common;
using MySql.Data.MySqlClient;

// ReSharper disable InconsistentNaming
namespace ACE.Entity
{
    [DbTable("ace_object_properties_attribute2nd")]
    public class AceObjectPropertiesAttribute2nd : ICloneable
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [DbField("attribute2ndid", (int)MySqlDbType.UInt16)]
        public ushort Attribute2ndId { get; set; }

        [DbField("attribute2ndValue", (int)MySqlDbType.UInt24)]
        public uint Attribute2ndValue { get; set; }

        [DbField("attribute2ndRanks", (int)MySqlDbType.UInt16)]
        public ushort Attribute2ndRanks { get; set; }

        [DbField("attribute2ndXpSpent", (int)MySqlDbType.UInt32)]
        public uint Attribute2ndXpSpent { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
