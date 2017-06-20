using System;
using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_double")]
    public class AceObjectPropertiesDouble : ICloneable
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [DbField("dblPropertyId", (int)MySqlDbType.UInt16)]
        public ushort PropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.Double)]
        public double PropertyValue { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
