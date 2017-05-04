using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ACE.Database
{
    /// <summary>
    /// An Object Factory that simply uses the System.Activator class to create
    /// new instances of the desired type.  This implementation of IObjectFactory
    /// exists to preserve existing behavior.
    /// </summary>
    public class ActivatorFactory : IObjectFactory
    {
        public T GetProperObjectType<T>(MySqlDataReader reader)
        {
            return Activator.CreateInstance<T>();
        }
    }
}
