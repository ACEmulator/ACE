using System;
using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_bigint")]
    [DbList("ace_object_properties_bigint", "aceObjectId")]
    public class AceObjectPropertiesInt64 : ICloneable
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("bigIntPropertyId", (int)MySqlDbType.UInt16)]
        public uint PropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.UInt64)]
        public ulong PropertyValue { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
