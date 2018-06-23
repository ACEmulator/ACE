using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using log4net;

namespace ACE.Database
{
    public class SerializedShardDatabase
    {
        private readonly ShardDatabase _wrappedDatabase;

        private readonly BlockingCollection<Task> _queue = new BlockingCollection<Task>();

        private Thread _workerThread;

        public ShardPropertyTables Config;

        internal SerializedShardDatabase(ShardDatabase shardDatabase)
        {
            _wrappedDatabase = shardDatabase;
            Config = new ShardPropertyTables();
        }

        public void Start()
        {
            _workerThread = new Thread(DoWork);
            _workerThread.Name = "Serialized Shard Database";
            _workerThread.Start();
        }

        public void Stop()
        {
            _queue.CompleteAdding();
            _workerThread.Join();
        }

        private void DoWork()
        {
            while (!_queue.IsAddingCompleted)
            {
                try
                {
                    Task t = _queue.Take();

                    try
                    {
                        t.Start();
                        t.Wait();
                    }
                    catch (Exception)
                    {
                        // log eventually, perhaps add failure callbacks?
                        // swallow for now.  can't block other db work because 1 fails.
                    }
                }
                catch (ObjectDisposedException)
                {
                    // the _queue has been disposed, we're good
                }
                catch (InvalidOperationException)
                {
                    // _queue is empty and CompleteForAdding has been called -- we're done here
                }
            }
        }


        public void GetMaxGuidFoundInRange(uint min, uint max, Action<uint> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetMaxGuidFoundInRange(min, max);
                callback?.Invoke(result);
            }));
        }


        public void GetCharacters(uint accountId, Action<List<Character>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetCharacters(accountId);
                callback?.Invoke(result);
            }));
        }

        public void GetAllCharacters(Action<List<Character>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetAllCharacters();
                callback?.Invoke(result);
            }));
        }

        public void IsCharacterNameAvailable(string name, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.IsCharacterNameAvailable(name);
                callback?.Invoke(result);
            }));
        }

        public bool IsCharacterPlussed(uint biotaId)
        {
            return _wrappedDatabase.IsCharacterPlussed(biotaId);
        }

        public void AddCharacter(Character character, Biota biota, IEnumerable<Biota> possessions, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.AddCharacter(character, biota, possessions);
                callback?.Invoke(result);
            }));
        }

        public void DeleteOrRestoreCharacter(ulong unixTime, uint guid, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.DeleteOrRestoreCharacter(unixTime, guid);
                callback?.Invoke(result);
            }));
        }

        public void MarkCharacterDeleted(uint guid, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.MarkCharacterDeleted(guid);
                callback?.Invoke(result);
            }));
        }

        public void SaveCharacter(Character character, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.SaveCharacter(character);
                callback?.Invoke(result);
            }));
        }

        public void AddBiota(Biota biota, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.AddBiota(biota);
                callback?.Invoke(result);
            }));
        }

        public void AddBiotas(IEnumerable<Biota> biotas, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.AddBiotas(biotas);
                callback?.Invoke(result);
            }));
        }

        public void GetBiota(uint id, Action<Biota> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetBiota(id);
                callback?.Invoke(c);
            }));
        }

        public void SaveBiota(Biota biota, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.SaveBiota(biota);
                callback?.Invoke(result);
            }));
        }

        public void SaveBiotas(IEnumerable<Biota> biotas, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.SaveBiotas(biotas);
                callback?.Invoke(result);
            }));
        }

        public void RemoveBiota(Biota biota, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveBiota(biota);
                callback?.Invoke(result);
            }));
        }

        public void AddeEntity(object entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.AddEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(object entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(BiotaPropertiesBool entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(BiotaPropertiesDID entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(BiotaPropertiesEnchantmentRegistry entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(BiotaPropertiesFloat entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(BiotaPropertiesIID entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(BiotaPropertiesInt entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(BiotaPropertiesInt64 entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(BiotaPropertiesPosition entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(CharacterPropertiesShortcutBar entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(CharacterPropertiesSpellBar entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(BiotaPropertiesSpellBook entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }

        public void RemoveEntity(BiotaPropertiesString entity, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveEntity(entity);
                callback?.Invoke(result);
            }));
        }


        public void GetPlayerBiotas(uint id, Action<PlayerBiotas> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetPlayerBiotas(id);
                callback?.Invoke(c);
            }));
        }

        public void GetInventory(uint parentId, bool includedNestedItems, Action<List<Biota>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetInventory(parentId, includedNestedItems);
                callback?.Invoke(c);
            }));

        }

        public void GetWieldedItems(uint parentId, Action<List<Biota>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetWieldedItems(parentId);
                callback?.Invoke(c);
            }));

        }

        public void GetObjectsByLandblock(ushort landblockId, Action<List<Biota>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetObjectsByLandblock(landblockId);
                callback?.Invoke(c);
            }));
        }























        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        public void DeleteFriend(uint characterId, uint friendCharacterId, Action callback)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllFriends(uint characterId, Action callback)
        {
            throw new NotImplementedException();
        }

        public void RenameCharacter(string currentName, string newName, Action<uint> callback)
        {
            throw new NotImplementedException();
        }

        public void SetCharacterAccessLevelByName(string name, AccessLevel accessLevel, Action<uint> callback)
        {
            throw new NotImplementedException();
        }
    }

    public class ShardPropertyTables
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void AddBool(string key, bool value, string description = "")
        {
            var stat = new ConfigPropertiesBoolean
            {
                Key = key,
                Value = value,
                Description = description
            };

            using (var context = new ShardDbContext())
            {
                context.ConfigPropertiesBoolean.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyBool(ConfigPropertiesBoolean stat)
        {
            using (var context = new ShardDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public ConfigPropertiesBoolean GetBool(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesBoolean.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool BoolExists(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesBoolean.Any(r => r.Key == key);
        }

        public void AddLong(string key, long value, string description = "")
        {
            var stat = new ConfigPropertiesLong
            {
                Key = key,
                Value = value,
                Description = description
            };

            using (var context = new ShardDbContext())
            {
                context.ConfigPropertiesLong.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyLong(ConfigPropertiesLong stat)
        {
            using (var context = new ShardDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public ConfigPropertiesLong GetLong(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesLong.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool LongExists(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesLong.Any(r => r.Key == key);
        }

        public void AddDouble(string key, double value, string description = "")
        {
            var stat = new ConfigPropertiesDouble
            {
                Key = key,
                Value = value,
                Description = description
            };

            using (var context = new ShardDbContext())
            {
                context.ConfigPropertiesDouble.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyDouble(ConfigPropertiesDouble stat)
        {
            using (var context = new ShardDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public ConfigPropertiesDouble GetDouble(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesDouble.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool DoubleExists(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesDouble.Any(r => r.Key == key);
        }

        public void AddString(string key, string value, string description = "")
        {
            var stat = new ConfigPropertiesString
            {
                Key = key,
                Value = value,
                Description = description
            };

            using (var context = new ShardDbContext())
            {
                context.ConfigPropertiesString.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyString(ConfigPropertiesString stat)
        {
            using (var context = new ShardDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public ConfigPropertiesString GetString(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesString.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool StringExists(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesString.Any(r => r.Key == key);
        }

        public List<ConfigPropertiesBoolean> GetAllBools()
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesBoolean.AsNoTracking().ToList();
        }

        public List<ConfigPropertiesLong> GetAllLongs()
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesLong.AsNoTracking().ToList();
        }

        public List<ConfigPropertiesDouble> GetAllDoubles()
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesDouble.AsNoTracking().ToList();
        }

        public List<ConfigPropertiesString> GetAllStrings()
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesString.AsNoTracking().ToList();
        }
    }
}
