using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity;
using ACE.Entity.Enum;
using System.Collections.Concurrent;
using System.Threading;

namespace ACE.Database
{
    public class SerializedShardDatabase : ISerializedShardDatabase
    {
        private IShardDatabase _wrappedDatabase;

        private ConcurrentQueue<Task> _queue = new ConcurrentQueue<Task>();

        private volatile object _queueLock = new object();

        private volatile bool _keepWorking = true;

        private Thread _workerThread;

        internal SerializedShardDatabase(IShardDatabase shardDatabase)
        {
            _wrappedDatabase = shardDatabase;
        }

        public void Start()
        {
            _keepWorking = true;
            _workerThread = new Thread(DoWork);
            _workerThread.Start();
        }

        public void Stop()
        {
            _keepWorking = false;
            _workerThread.Join();
        }

        private void DoWork()
        {
            Task t;
            bool workToDo = false;

            while (_keepWorking)
            {
                lock (_queueLock)
                {
                    workToDo = _queue.TryDequeue(out t);
                }

                if (workToDo)
                {
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
            }

            // we were told to stop, but finish what we've been given
            lock (_queueLock)
            {
                workToDo = _queue.TryDequeue(out t);
            }

            while (workToDo)
            {
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

                lock (_queueLock)
                {
                    workToDo = _queue.TryDequeue(out t);
                }
            }
        }

        public void AddFriend(uint characterId, uint friendCharacterId, Action callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    _wrappedDatabase.AddFriend(characterId, friendCharacterId);
                    if (callback != null)
                        callback.Invoke();
                }));
            }
        }

        public void DeleteFriend(uint characterId, uint friendCharacterId, Action callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    _wrappedDatabase.DeleteFriend(characterId, friendCharacterId);
                    if (callback != null)
                        callback.Invoke();
                }));
            }
        }

        public void DeleteOrRestore(ulong unixTime, uint id, Action<bool> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.DeleteOrRestore(unixTime, id);
                    if (callback != null)
                        callback.Invoke(result);
                }));
            }
        }

        public void GetCharacter(uint id, Action<AceCharacter> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var c = _wrappedDatabase.GetCharacter(id);
                    if (callback != null)
                        callback.Invoke(c);
                }));
            }
        }

        public void GetCharacters(uint accountId, Action<List<CachedCharacter>> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.GetCharacters(accountId);
                    if (callback != null)
                        callback.Invoke(result);
                }));
            }
        }

        public void GetNextCharacterId(Action<uint> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.GetNextCharacterId();
                    if (callback != null)
                        callback.Invoke(result);
                }));
            }
        }

        public void GetObject(uint aceObjectId, Action<AceObject> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.GetObject(aceObjectId);
                    if (callback != null)
                        callback.Invoke(result);
                }));
            }
        }

        public void GetObjectInfoByName(string name, Action<ObjectInfo> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.GetObjectInfoByName(name);
                    if (callback != null)
                        callback.Invoke(result);
                }));
            }
        }

        public void GetObjectsByLandblock(ushort landblock, Action<List<AceObject>> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.GetObjectsByLandblock(landblock);
                    if (callback != null)
                        callback.Invoke(result);
                }));
            }
        }

        public void IsCharacterNameAvailable(string name, Action<bool> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.IsCharacterNameAvailable(name);
                    if (callback != null)
                        callback.Invoke(result);
                }));
            }
        }

        public void RemoveAllFriends(uint characterId, Action callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    _wrappedDatabase.RemoveAllFriends(characterId);
                    if (callback != null)
                        callback.Invoke();
                }));
            }
        }

        public void RenameCharacter(string currentName, string newName, Action<uint> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.RenameCharacter(currentName, newName);
                    if (callback != null)
                        callback.Invoke(result);
                }));
            }
        }

        public void SaveObject(AceObject aceObject, Action<bool> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.SaveObject(aceObject);
                    if (callback != null)
                        callback.Invoke(result);
                }));
            }
        }

        public void GetMaxPlayerId(Action<uint> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.GetMaxPlayerId();
                    callback.Invoke(result);
                }));
            }
        }

        public void SetCharacterAccessLevelByName(string name, AccessLevel accessLevel, Action<uint> callback)
        {
            lock (_queueLock)
            {
                _queue.Enqueue(new Task(() =>
                {
                    var result = _wrappedDatabase.SetCharacterAccessLevelByName(name, accessLevel);
                    if (callback != null)
                        callback.Invoke(result);
                }));
            }
        }
    }
}
