using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ACE.Entity;
using ACE.Entity.Enum;
using System.Collections.Concurrent;
using System.Threading;

using log4net;

namespace ACE.Database
{
    public class SerializedShardDatabase : ISerializedShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IShardDatabase _wrappedDatabase;

        private BlockingCollection<Task> _queue = new BlockingCollection<Task>();

        private Thread _workerThread;

        internal SerializedShardDatabase(IShardDatabase shardDatabase)
        {
            _wrappedDatabase = shardDatabase;
        }

        public void Start()
        {
            _workerThread = new Thread(DoWork);
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
                    catch (Exception ex)
                    {
                        // log eventually, perhaps add failure callbacks?
                        // swallow for now.  can't block other db work because 1 fails.
                        log.Error("DB Task failure!" + ex.Message);
                        log.Error("  AT:" + ex.StackTrace);
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

        public async Task AddFriend(uint characterId, uint friendCharacterId)
        {
            Task t = new Task(() =>
            {
                _wrappedDatabase.AddFriend(characterId, friendCharacterId);
            });
            _queue.Add(t);
            await t;
        }

        public async Task DeleteFriend(uint characterId, uint friendCharacterId)
        {
            Task t = new Task(() =>
            {
                _wrappedDatabase.DeleteFriend(characterId, friendCharacterId);
            });

            _queue.Add(t);
            await t;
        }

        public async Task<bool> DeleteOrRestore(ulong unixTime, uint id)
        {
            Task<bool> t = new Task<bool>(() =>
            {
                var result = _wrappedDatabase.DeleteOrRestore(unixTime, id);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<bool> DeleteCharacter(uint id)
        {
            Task<bool> t = new Task<bool>(() =>
            {
                var result = _wrappedDatabase.DeleteCharacter(id);
                return result;
            });
            _queue.Add(t);
            return await t;
        }

        public async Task<AceCharacter> GetCharacter(uint id)
        {
            Task<AceCharacter> t = new Task<AceCharacter>(() =>
            {
                var c = _wrappedDatabase.GetCharacter(id);
                return c;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<List<CachedCharacter>> GetCharacters(uint subscriptionId)
        {
            Task<List<CachedCharacter>> t = new Task<List<CachedCharacter>>(() =>
            {
                var result = _wrappedDatabase.GetCharacters(subscriptionId);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<AceObject> GetObject(uint aceObjectId)
        {
            Task<AceObject> t = new Task<AceObject>(() =>
            {
                var result = _wrappedDatabase.GetObject(aceObjectId);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<ObjectInfo> GetObjectInfoByName(string name)
        {
            Task<ObjectInfo> t = new Task<ObjectInfo>(() =>
            {
                var result = _wrappedDatabase.GetObjectInfoByName(name);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<List<AceObject>> GetObjectsByLandblock(ushort landblock)
        {
            Task<List<AceObject>> t = new Task<List<AceObject>>(() =>
            {
                var result = _wrappedDatabase.GetObjectsByLandblock(landblock);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<bool> IsCharacterNameAvailable(string name)
        {
            Task<bool> t = new Task<bool>(() =>
            {
                var result = _wrappedDatabase.IsCharacterNameAvailable(name);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task RemoveAllFriends(uint characterId)
        {
            Task t = new Task(() =>
            {
                _wrappedDatabase.RemoveAllFriends(characterId);
            });

            _queue.Add(t);

            await t;
        }

        public async Task<uint> RenameCharacter(string currentName, string newName)
        {
            Task<uint> t = new Task<uint>(() =>
            {
                var result = _wrappedDatabase.RenameCharacter(currentName, newName);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<bool> SaveObject(AceObject aceObject)
        {
            Task<bool> t = new Task<bool>(() =>
            {
                var result = _wrappedDatabase.SaveObject(aceObject);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<bool> DeleteObject(AceObject aceObject)
        {
            Task<bool> t = new Task<bool>(() =>
            {
                var result = _wrappedDatabase.DeleteObject(aceObject);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<uint> GetCurrentId(uint min, uint max)
        {
            Task<uint> t = new Task<uint>(() =>
            {
                var result = _wrappedDatabase.GetCurrentId(min, max);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<uint> SetCharacterAccessLevelByName(string name, AccessLevel accessLevel)
        {
            Task<uint> t = new Task<uint>(() =>
            {
                var result = _wrappedDatabase.SetCharacterAccessLevelByName(name, accessLevel);
                return result;
            });

            _queue.Add(t);
            return await t;
        }

        public async Task<bool> DeleteContract(AceContractTracker contract)
        {
            Task<bool> t = new Task<bool>(() =>
                {
                    bool result = _wrappedDatabase.DeleteContract(contract);
                    return result;
            });

            _queue.Add(t);
            return await t;
        }
    }
}
