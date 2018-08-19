using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public class SerializedShardDatabase
    {
        private readonly ShardDatabase _wrappedDatabase;

        private readonly BlockingCollection<Task> _queue = new BlockingCollection<Task>();

        private Thread _workerThread;

        internal SerializedShardDatabase(ShardDatabase shardDatabase)
        {
            _wrappedDatabase = shardDatabase;
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


        public void GetCharacters(uint accountId, bool includeDeleted, Action<List<Character>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetCharacters(accountId, includeDeleted);
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

        public void AddCharacter(Biota biota, IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> possessions, Character character, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.AddCharacterInParallel(biota, possessions, character);
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


        public void GetBiota(uint id, Action<Biota> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetBiota(id);
                callback?.Invoke(c);
            }));
        }

        public void SaveBiota(Biota biota, ReaderWriterLockSlim rwLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.SaveBiota(biota, rwLock);
                callback?.Invoke(result);
            }));
        }

        public void SaveBiotas(IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> biotas, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.SaveBiotasInParallel(biotas);
                callback?.Invoke(result);
            }));
        }

        public void RemoveBiota(Biota biota, ReaderWriterLockSlim rwLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveBiota(biota, rwLock);
                callback?.Invoke(result);
            }));
        }

        public void RemoveBiotas(IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> biotas, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveBiotasInParallel(biotas);
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


        public void GetPlayerBiotas(uint id, Action<PlayerBiotas> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetPlayerBiotasInParallel(id);
                callback?.Invoke(c);
            }));
        }

        public void GetInventory(uint parentId, bool includedNestedItems, Action<List<Biota>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetInventoryInParallel(parentId, includedNestedItems);
                callback?.Invoke(c);
            }));

        }

        public void GetWieldedItems(uint parentId, Action<List<Biota>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetWieldedItemsInParallel(parentId);
                callback?.Invoke(c);
            }));

        }

        public void GetObjectsByLandblock(ushort landblockId, Action<List<Biota>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetObjectsByLandblockInParallel(landblockId);
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
}
