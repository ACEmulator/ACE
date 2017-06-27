using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity;
using System.Collections.Concurrent;

namespace ACE.Database
{
    public class CachingWorldDatabase : IWorldDatabase
    {
        /// <summary>
        /// wrapper to the the world database that actually does the lifting
        /// </summary>
        private IWorldDatabase _wrappedDatabase;

        /// <summary>
        /// so, caches are fun.  we have to be especially careful that we never hand out the same instance
        /// twice.  in order to do that, we always clone objects coming out of the cache and return the
        /// clone instead of the actual cached object.  Concurrent dictionary could have been used, but
        /// I went with a standard dictionary and a double locking system because the 99% use case is a
        /// cache hit, and Currents don't do that as fast.
        /// </summary>
        private volatile Dictionary<uint, AceObject> _weenieCache = new Dictionary<uint, AceObject>();

        private object _weenieCacheMutex = new object();

        public CachingWorldDatabase(IWorldDatabase wrappedDatabase)
        {
            _wrappedDatabase = wrappedDatabase;
        }

        public AceObject GetAceObjectByWeenie(uint weenieClassId)
        {
            if (!_weenieCache.ContainsKey(weenieClassId))
            {
                lock (_weenieCacheMutex)
                {
                    if (!_weenieCache.ContainsKey(weenieClassId))
                    {
                        _weenieCache.Add(weenieClassId, _wrappedDatabase.GetAceObjectByWeenie(weenieClassId));
                    }
                }
            }

            return (AceObject)_weenieCache[weenieClassId].Clone();
        }

        public AceObject GetObject(uint aceObjectId)
        {
            // if they're asking for a weenie, just give them the weenie.
            if (_weenieCache.ContainsKey(aceObjectId))
            {
                return (AceObject)_weenieCache[aceObjectId].Clone();
            }

            AceObject aceObject = _wrappedDatabase.GetObject(aceObjectId);

            if (aceObject.AceObjectId == aceObject.WeenieClassId)
            {
                // this is a weenie!  cache it.
                lock (_weenieCacheMutex)
                {
                    if (!_weenieCache.ContainsKey(aceObjectId))
                    {
                        _weenieCache.Add(aceObjectId, (AceObject)aceObject.Clone());
                    }
                }
            }

            return aceObject;
        }

        public List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            return _wrappedDatabase.GetObjectsByLandblock(landblock);
        }

        public List<TeleportLocation> GetPointsOfInterest()
        {
            return _wrappedDatabase.GetPointsOfInterest();
        }

        public List<CachedWeenieClass> GetRandomWeeniesOfType(uint typeId, uint numWeenies)
        {
            return _wrappedDatabase.GetRandomWeeniesOfType(typeId, numWeenies);
        }

        public Task<bool> SaveObject(AceObject aceObject)
        {
            return _wrappedDatabase.SaveObject(aceObject);
        }
    }
}
