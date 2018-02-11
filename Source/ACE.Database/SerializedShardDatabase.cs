using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using ACE.Entity;
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

        public void AddFriend(uint characterId, uint friendCharacterId, Action callback)
        {
            _queue.Add(new Task(() =>
            {
                _wrappedDatabase.AddFriend(characterId, friendCharacterId);
                if (callback != null)
                    callback.Invoke();
            }));
        }

        public void DeleteFriend(uint characterId, uint friendCharacterId, Action callback)
        {
            _queue.Add(new Task(() =>
            {
                _wrappedDatabase.DeleteFriend(characterId, friendCharacterId);
                if (callback != null)
                    callback.Invoke();
            }));
        }

        public void DeleteOrRestore(ulong unixTime, uint id, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.DeleteOrRestore(unixTime, id);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void DeleteCharacter(uint id, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.DeleteCharacter(id);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void GetCharacter(uint id, Action<AceCharacter> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetCharacter(id);
                if (callback != null)
                    callback.Invoke(c);
            }));
        }

        public void GetCharacters(uint subscriptionId, Action<List<CachedCharacter>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetCharacters(subscriptionId);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void GetObject(uint aceObjectId, Action<AceObject> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetObject(aceObjectId);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void GetObjectInfoByName(string name, Action<ObjectInfo> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetObjectInfoByName(name);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void GetObjectsByLandblock(ushort landblock, Action<List<AceObject>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetObjectsByLandblock(landblock);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void IsCharacterNameAvailable(string name, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.IsCharacterNameAvailable(name);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void RemoveAllFriends(uint characterId, Action callback)
        {
            _queue.Add(new Task(() =>
            {
                _wrappedDatabase.RemoveAllFriends(characterId);
                if (callback != null)
                    callback.Invoke();
            }));
        }

        public void RenameCharacter(string currentName, string newName, Action<uint> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RenameCharacter(currentName, newName);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void SaveObject(AceObject aceObject, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.SaveObject(aceObject);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void DeleteObject(AceObject aceObject, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.DeleteObject(aceObject);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void GetCurrentId(uint min, uint max, Action<uint> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetCurrentId(min, max);
                callback.Invoke(result);
            }));
        }

        public void SetCharacterAccessLevelByName(string name, AccessLevel accessLevel, Action<uint> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.SetCharacterAccessLevelByName(name, accessLevel);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public bool DeleteContract(AceContractTracker contract, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
                {
                    bool result = _wrappedDatabase.DeleteContract(contract);
                    if (callback != null)
                        callback.Invoke(result);
            }));
            return true;
        }
    }
}
