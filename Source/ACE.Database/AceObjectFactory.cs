using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    /// <summary>
    /// A smarter factory that takes a peek at the "bitField" property coming back
    /// from an AceObject to instantiate the proper AceObject sub-type.  Will 
    /// default to a generic AceObject if no other suitable type is found.
    /// </summary>
    public class AceObjectFactory : IObjectFactory
    {
        public T GetProperObjectType<T>(MySqlDataReader reader)
        {
            Type t = typeof(T);
            Type aceType = typeof(AceObject);
            object result;

            if (!aceType.IsAssignableFrom(t))
                return Activator.CreateInstance<T>();

            ObjectDescriptionFlag oDescFlag = (ObjectDescriptionFlag)reader.GetUInt32("bitField");

            // the whole point of this method is to create the proper instance of an object here
            // so that we don't have to round-trip to the database again later.

            if ((oDescFlag & ObjectDescriptionFlag.Portal) != 0)
                result = new AcePortalObject();
            else
                result = new AceObject();

            return (T)result;
        }
    }
}
