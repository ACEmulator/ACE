using ACE.Entity.Enum;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    public class SearchWeenieProperty
    {
        public AceObjectPropertyType PropertyType { get; set; }

        public uint PropertyId { get; set; }

        public string PropertyValue { get; set; }

        public MySqlDbType GetMySqlDbType()
        {
            switch (PropertyType)
            {
                case AceObjectPropertyType.PropertyBool:
                    return MySqlDbType.Bit;
                case AceObjectPropertyType.PropertyDataId:
                case AceObjectPropertyType.PropertyInstanceId:
                case AceObjectPropertyType.PropertyInt:
                    return MySqlDbType.UInt32;
                case AceObjectPropertyType.PropertyInt64:
                    return MySqlDbType.UInt64;
                case AceObjectPropertyType.PropertyString:
                    return MySqlDbType.Text;
                case AceObjectPropertyType.PropertyDouble:
                    return MySqlDbType.Float;
                default:
                    return MySqlDbType.Text;
            }
        }
    }
}
