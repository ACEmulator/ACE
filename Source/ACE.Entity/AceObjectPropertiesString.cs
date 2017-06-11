using System;
using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_string")]
    [DbList("ace_object_properties_string", "aceObjectId")]
    public class AceObjectPropertiesString : ICloneable
    {
        [DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
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
