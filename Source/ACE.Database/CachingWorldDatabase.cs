using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity;
using System.Collections.Concurrent;
using ACE.Entity.Enum;

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
        /// clone instead of the actual cached object.
        /// </summary>
        private ConcurrentDictionary<uint, AceObject> _weenieCache = new ConcurrentDictionary<uint, AceObject>();

        public CachingWorldDatabase(IWorldDatabase wrappedDatabase)
        {
            _wrappedDatabase = wrappedDatabase;
        }

        public AceObject GetAceObjectByWeenie(uint weenieClassId)
        {
            AceObject ret;
            try
            {
                ret = (AceObject)_weenieCache.GetOrAdd(weenieClassId, (wcId) => _wrappedDatabase.GetAceObjectByWeenie(wcId)).Clone();
            }
            catch (NullReferenceException)
            {
                ret = null;
            }
            return ret;
        }

        public AceObject GetAceObjectByWeenie(string weenieClassDescription)
        {
            uint weenieClassId = GetWeenieClassIdByWeenieClassDescription(weenieClassDescription);

            return GetAceObjectByWeenie(weenieClassId);
        }

        public uint GetWeenieClassIdByWeenieClassDescription(string weenieClassDescription)
        {
            return _wrappedDatabase.GetWeenieClassIdByWeenieClassDescription(weenieClassDescription);
        }

        public AceObject GetObject(uint aceObjectId)
        {
            // if they're asking for a weenie, just give them the weenie.
            if (aceObjectId <= AceObject.WEENIE_MAX)
            {
                return GetAceObjectByWeenie(aceObjectId);
            }

            return _wrappedDatabase.GetObject(aceObjectId);
        }

        public List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            return _wrappedDatabase.GetObjectsByLandblock(landblock);
        }

        public List<AceObject> GetWeenieInstancesByLandblock(ushort landblock)
        {
            return _wrappedDatabase.GetWeenieInstancesByLandblock(landblock);
        }

        public List<TeleportLocation> GetPointsOfInterest()
        {
            return _wrappedDatabase.GetPointsOfInterest();
        }

        public List<CachedWeenieClass> GetRandomWeeniesOfType(uint typeId, uint numWeenies)
        {
            return _wrappedDatabase.GetRandomWeeniesOfType(typeId, numWeenies);
        }

        public uint GetCurrentId(uint min, uint max)
        {
            return _wrappedDatabase.GetCurrentId(min, max);
        }

        public List<Recipe> GetAllRecipes()
        {
            return _wrappedDatabase.GetAllRecipes();
        }

        public void CreateRecipe(Recipe recipe)
        {
            _wrappedDatabase.CreateRecipe(recipe);
        }

        public void UpdateRecipe(Recipe recipe)
        {
            _wrappedDatabase.UpdateRecipe(recipe);
        }

        public void DeleteRecipe(Guid recipeGuid)
        {
            _wrappedDatabase.DeleteRecipe(recipeGuid);
        }

        public bool SaveObject(AceObject weenie)
        {
            if (_weenieCache.ContainsKey(weenie.WeenieClassId))
                _weenieCache[weenie.WeenieClassId] = weenie;

            return _wrappedDatabase.SaveObject(weenie);
        }
        
        public bool DeleteObject(AceObject aceObject)
        {
            AceObject weenie;
            if (_weenieCache.ContainsKey(aceObject.WeenieClassId))
                _weenieCache.TryRemove(aceObject.WeenieClassId, out weenie);

            return _wrappedDatabase.DeleteObject(aceObject);
        }

        public List<WeenieSearchResult> SearchWeenies(SearchWeeniesCriteria criteria)
        {
            return _wrappedDatabase.SearchWeenies(criteria);
        }

        public bool ReplaceObject(AceObject aceObject)
        {
            if (_weenieCache.ContainsKey(aceObject.WeenieClassId))
                _weenieCache[aceObject.WeenieClassId] = aceObject;

            return _wrappedDatabase.ReplaceObject(aceObject);
        }
    }
}
