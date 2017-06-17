using System;
using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_string")]
    public class AceObjectPropertiesString : ICloneable
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [DbField("strPropertyId", (int)MySqlDbType.UInt16)]
        public ushort PropertyId { get; set; }

        [DbField("propertyValue", (int)MySqlDbType.Text)]
        public string PropertyValue { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
