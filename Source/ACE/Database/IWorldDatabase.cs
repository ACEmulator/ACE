using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database
{
    public interface IWorldDatabase
    {
        List<TeleportLocation> GetLocations();

        /// <summary>
        /// loads object properties into the provided db object
        /// </summary>
        void LoadProperties(DbObject dbObject);

        /// <summary>
        /// saves all object properties in the provided db object
        /// </summary>
        void SaveProperties(DbObject dbObject);
    }
}
