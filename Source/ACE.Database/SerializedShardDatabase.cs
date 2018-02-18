using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using ACE.Database.Models.Shard;
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


        public void GetMaxGuidFoundInRange(uint min, uint max, Action<uint> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetMaxGuidFoundInRange(min, max);
                callback.Invoke(result);
            }));
        }


        public void GetCharacters(uint accountId, Action<List<Character>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetCharacters(accountId);
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

        public void AddCharacter(Character character, Biota biota, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.AddCharacter(character, biota);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void DeleteOrRestoreCharacter(ulong unixTime, uint guid, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.DeleteOrRestoreCharacter(unixTime, guid);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void MarkCharacterDeleted(uint guid, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.MarkCharacterDeleted(guid);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        public void AddBiota(Biota biota, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.AddBiota(biota);
                if (callback != null)
                    callback.Invoke(result);
            }));
        }

        /// <summary>
        /// Will return a biota from the db with tracking enabled.
        /// This will populate all sub collections except the followign: BiotaPropertiesEmoteAction
        /// </summary>
        public void GetBiota(uint id, Action<Biota> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetBiota(id);
                if (callback != null)
                    callback.Invoke(c);
            }));
        }























        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        public void AddFriend(uint characterId, uint friendCharacterId, Action callback)
        {
            throw new NotImplementedException();
        }

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

        public void SaveObject(AceObject aceObject, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void DeleteObject(AceObject aceObject, Action<bool> callback)
        {
            throw new NotImplementedException();
        }


        public void SetCharacterAccessLevelByName(string name, AccessLevel accessLevel, Action<uint> callback)
        {
            throw new NotImplementedException();
        }

        public bool DeleteContract(AceContractTracker contract, Action<bool> callback)
        {
            throw new NotImplementedException();
        }
    }
}
