using ACE.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database
{
    /// <summary>
    /// this object factory is designed to allow database classes to create the proper
    /// sub-classed instance types when doing the dynamic prepared statement object
    /// generation.
    /// </summary>
    public interface IObjectFactory
    {
        T GetProperObjectType<T>(MySqlDataReader reader);
    }
}
