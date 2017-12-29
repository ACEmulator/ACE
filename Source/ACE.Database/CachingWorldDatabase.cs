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
    public class CachingWorldDatabase : IWorldCachedDatabase
    {
        /// <summary>
        /// wrapper to the the world database that actually does the lifting
        /// </summary>
        private IWorldDatabase _wrappedDatabase;

        private class CacheResult
        {
            public CacheResult(Task t)
            {
                FetchingTask = t;
            }

            public Task FetchingTask;

            public async Task Run()
            {
                await FetchingTask;
            } 
        }

        private class CacheResult<T> : CacheResult
        {
            public CacheResult(Task<T> t) : base(t)
            {
            }

            public Task<T> ResultTask
            {
                get
                {
                    return FetchingTask as Task<T>;
                }
            }

            public async Task<T> GetResult()
            {
                await Run();
                return ResultTask.Result;
            }
        }

        /// <summary>
        /// so, caches are fun.  we have to be especially careful that we never hand out the same instance
        /// twice.  in order to do that, we always clone objects coming out of the cache and return the
        /// clone instead of the actual cached object.
        /// </summary>
        private ConcurrentDictionary<uint, CacheResult<AceObject>> _weenieCache = new ConcurrentDictionary<uint, CacheResult<AceObject>>();

        private async Task<V> GetCacheResult<K,V>(ConcurrentDictionary<K, CacheResult<V>> cache, K key, Func<K, V> filler)
        {
            CacheResult<V> res = cache.GetOrAdd(key, (keyId) =>
            {
                Task<V> t = Task<V>.Run(() =>
                {
                    return filler(keyId);
                });
                return new CacheResult<V>(t);
            });

            return await res.GetResult();
        }

        public CachingWorldDatabase(IWorldDatabase wrappedDatabase)
        {
            _wrappedDatabase = wrappedDatabase;
        }

        public async Task<AceObject> GetAceObjectByWeenie(uint weenieClassId)
        {
            AceObject res = await GetCacheResult<uint, AceObject>(_weenieCache, weenieClassId,
                (wcId) =>
                {
                    return _wrappedDatabase.GetAceObjectByWeenie(wcId);
                });

            res = (AceObject)res.Clone();

            return res;
        }

        public async Task<AceObject> GetObject(uint aceObjectId)
        {
            // if they're asking for a weenie, just give them the weenie.
            if (aceObjectId <= AceObject.WEENIE_MAX)
            {
                return await GetAceObjectByWeenie(aceObjectId);
            }

            return await Task.Run(() => { return _wrappedDatabase.GetObject(aceObjectId); });
        }

        public async Task<List<AceObject>> GetObjectsByLandblock(ushort landblock)
        {
            return await Task.Run(() =>
            {
                return _wrappedDatabase.GetObjectsByLandblock(landblock);
            });
        }

        public async Task<List<AceObject>> GetWeenieInstancesByLandblock(ushort landblock)
        {
            return await Task.Run(() =>
            {
                return _wrappedDatabase.GetWeenieInstancesByLandblock(landblock);
            });
        }

        public async Task<List<TeleportLocation>> GetPointsOfInterest()
        {
            return await Task.Run(() =>
            {
                return _wrappedDatabase.GetPointsOfInterest();
            });
        }

        public async Task<List<CachedWeenieClass>> GetRandomWeeniesOfType(uint typeId, uint numWeenies)
        {
            return await Task.Run(() =>
            {
                return _wrappedDatabase.GetRandomWeeniesOfType(typeId, numWeenies);
            });
        }

        public async Task<uint> GetCurrentId(uint min, uint max)
        {
            return await Task.Run(() =>
            {
                return _wrappedDatabase.GetCurrentId(min, max);
            });
        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            return await Task.Run(() =>
            {
                return _wrappedDatabase.GetAllRecipes();
            });
        }

        public async Task CreateRecipe(Recipe recipe)
        {
            await Task.Run(() =>
            {
                _wrappedDatabase.CreateRecipe(recipe);
            });
        }

        public async Task UpdateRecipe(Recipe recipe)
        {
            await Task.Run(() =>
            {
                _wrappedDatabase.UpdateRecipe(recipe);
            });
        }

        public async Task DeleteRecipe(Guid recipeGuid)
        {
            await Task.Run(() =>
            {
                _wrappedDatabase.DeleteRecipe(recipeGuid);
            });
        }

        public async Task<List<Content>> GetAllContent()
        {
            return await Task.Run(() =>
            {
                return _wrappedDatabase.GetAllContent();
            });
        }

        public async Task CreateContent(Content content)
        {
            await Task.Run(() =>
            {
                _wrappedDatabase.CreateContent(content);
            });
        }

        public async Task UpdateContent(Content content)
        {
            await Task.Run(() =>
            {
                _wrappedDatabase.UpdateContent(content);
            });
        }

        public async Task DeleteContent(Guid contentGuid)
        {
            await Task.Run(() =>
            {
                _wrappedDatabase.DeleteContent(contentGuid);
            });
        }

        public async Task<bool> SaveObject(AceObject weenie)
        {
            if (_weenieCache.ContainsKey(weenie.WeenieClassId))
            {
                _weenieCache[weenie.WeenieClassId] = new CacheResult<AceObject>(
                    Task.Run(() =>
                    {
                        return weenie;
                    }));
            }

            return await Task.Run(() =>
            {
                return _wrappedDatabase.SaveObject(weenie);
            });
        }
        
        public async Task<bool> DeleteObject(AceObject aceObject)
        {
            CacheResult<AceObject> weenie;
            if (_weenieCache.ContainsKey(aceObject.WeenieClassId))
                _weenieCache.TryRemove(aceObject.WeenieClassId, out weenie);

            return await Task.Run(() =>
            {
                return _wrappedDatabase.DeleteObject(aceObject);
            });
        }

        public async Task<List<WeenieSearchResult>> SearchWeenies(SearchWeeniesCriteria criteria)
        {
            return await Task.Run(() =>
            {
                return _wrappedDatabase.SearchWeenies(criteria);
            });
        }

        public bool UserModifiedFlagPresent()
        {
            return _wrappedDatabase.UserModifiedFlagPresent();
        }

        public async Task<bool> ReplaceObject(AceObject aceObject)
        {
            if (_weenieCache.ContainsKey(aceObject.WeenieClassId))
                _weenieCache[aceObject.WeenieClassId] = new CacheResult<AceObject>(Task.Run(() =>
                {
                    return aceObject;
                }));

            return await Task.Run(() =>
            {
                return _wrappedDatabase.ReplaceObject(aceObject);
            });
        }
    }
}
