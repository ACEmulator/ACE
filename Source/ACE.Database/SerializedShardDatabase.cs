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
                    Task t = _queue.Take(); // when exiting - System.InvalidOperationException: 'The collection argument is empty and has been marked as complete with regards to additions.'

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
                    break;
                }
                catch (InvalidOperationException)
                {
                    // _queue is empty and CompleteForAdding has been called -- we're done here
                    break;
                }
            }
        }


        public int QueueCount => _queue.Count;

        public void GetCurrentQueueWaitTime(Action<TimeSpan> callback)
        {
            var initialCallTime = DateTime.UtcNow;

            _queue.Add(new Task(() =>
            {
                callback?.Invoke(DateTime.UtcNow - initialCallTime);
            }));
        }


        /// <summary>
        /// Will return uint.MaxValue if no records were found within the range provided.
        /// </summary>
        public void GetMaxGuidFoundInRange(uint min, uint max, Action<uint> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetMaxGuidFoundInRange(min, max);
                callback?.Invoke(result);
            }));
        }

        /// <summary>
        /// This will return available id's, in the form of sequence gaps starting from min.<para />
        /// If a gap is just 1 value wide, then both start and end will be the same number.
        /// </summary>
        public void GetSequenceGaps(uint min, uint limitAvailableIDsReturned, Action<List<(uint start, uint end)>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetSequenceGaps(min, limitAvailableIDsReturned);
                callback?.Invoke(result);
            }));
        }


        public int GetBiotaCount()
        {
            return _wrappedDatabase.GetBiotaCount();
        }

        public Biota GetBiota(uint id)
        {
            return _wrappedDatabase.GetBiota(id);
        }

        public List<Biota> GetBiotasByWcid(uint id)
        {
            return _wrappedDatabase.GetBiotasByWcid(id);
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

        public void SaveBiota(Biota biota, ReaderWriterLockSlim rwLock, Action<bool> callback, Action<TimeSpan, TimeSpan> performanceResults)
        {
            var initialCallTime = DateTime.UtcNow;

            _queue.Add(new Task(() =>
            {
                var taskStartTime = DateTime.UtcNow;
                var result = _wrappedDatabase.SaveBiota(biota, rwLock);
                var taskCompletedTime = DateTime.UtcNow;
                callback?.Invoke(result);
                performanceResults?.Invoke(taskStartTime - initialCallTime, taskCompletedTime - taskStartTime);
            }));
        }

        public void SaveBiotasInParallel(IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> biotas, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.SaveBiotasInParallel(biotas);
                callback?.Invoke(result);
            }));
        }

        public void SaveBiotasInParallel(IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> biotas, Action<bool> callback, Action<TimeSpan, TimeSpan> performanceResults)
        {
            var initialCallTime = DateTime.UtcNow;

            _queue.Add(new Task(() =>
            {
                var taskStartTime = DateTime.UtcNow;
                var result = _wrappedDatabase.SaveBiotasInParallel(biotas);
                var taskCompletedTime = DateTime.UtcNow;
                callback?.Invoke(result);
                performanceResults?.Invoke(taskStartTime - initialCallTime, taskCompletedTime - taskStartTime);
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

        public void RemoveBiota(Biota biota, ReaderWriterLockSlim rwLock, Action<bool> callback, Action<TimeSpan, TimeSpan> performanceResults)
        {
            var initialCallTime = DateTime.UtcNow;

            _queue.Add(new Task(() =>
            {
                var taskStartTime = DateTime.UtcNow;
                var result = _wrappedDatabase.RemoveBiota(biota, rwLock);
                var taskCompletedTime = DateTime.UtcNow;
                callback?.Invoke(result);
                performanceResults?.Invoke(taskStartTime - initialCallTime, taskCompletedTime - taskStartTime);
            }));
        }

        public void RemoveBiotasInParallel(IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> biotas, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.RemoveBiotasInParallel(biotas);
                callback?.Invoke(result);
            }));
        }

        public void RemoveBiotasInParallel(IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> biotas, Action<bool> callback, Action<TimeSpan, TimeSpan> performanceResults)
        {
            var initialCallTime = DateTime.UtcNow;

            _queue.Add(new Task(() =>
            {
                var taskStartTime = DateTime.UtcNow;
                var result = _wrappedDatabase.RemoveBiotasInParallel(biotas);
                var taskCompletedTime = DateTime.UtcNow;
                callback?.Invoke(result);
                performanceResults?.Invoke(taskStartTime - initialCallTime, taskCompletedTime - taskStartTime);
            }));
        }

        public void FreeBiotaAndDisposeContext(Biota biota)
        {
            _queue.Add(new Task(() =>
            {
                _wrappedDatabase.FreeBiotaAndDisposeContext(biota);
            }));
        }

        public void FreeBiotaAndDisposeContexts(IEnumerable<Biota> biotas)
        {
            _queue.Add(new Task(() =>
            {
                _wrappedDatabase.FreeBiotaAndDisposeContexts(biotas);
            }));
        }


        public void GetPossessedBiotasInParallel(uint id, Action<PossessedBiotas> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetPossessedBiotasInParallel(id);
                callback?.Invoke(c);
            }));
        }

        public void GetInventoryInParallel(uint parentId, bool includedNestedItems, Action<List<Biota>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetInventoryInParallel(parentId, includedNestedItems);
                callback?.Invoke(c);
            }));

        }

        public void GetWieldedItemsInParallel(uint parentId, Action<List<Biota>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = _wrappedDatabase.GetWieldedItemsInParallel(parentId);
                callback?.Invoke(c);
            }));

        }

        public List<Biota> GetStaticObjectsByLandblock(ushort landblockId)
        {
            return _wrappedDatabase.GetStaticObjectsByLandblock(landblockId);
        }

        public List<Biota> GetStaticObjectsByLandblockInParallel(ushort landblockId)
        {
            return _wrappedDatabase.GetStaticObjectsByLandblockInParallel(landblockId);
        }

        public List<Biota> GetDynamicObjectsByLandblock(ushort landblockId)
        {
            return _wrappedDatabase.GetDynamicObjectsByLandblock(landblockId);
        }

        public List<Biota> GetDynamicObjectsByLandblockInParallel(ushort landblockId)
        {
            return _wrappedDatabase.GetDynamicObjectsByLandblockInParallel(landblockId);
        }


        public void IsCharacterNameAvailable(string name, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.IsCharacterNameAvailable(name);
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

        public Character GetCharacterByName(string name)
        {
            return _wrappedDatabase.GetCharacterByName(name);
        }

        public void SaveCharacter(Character character, ReaderWriterLockSlim rwLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.SaveCharacter(character, rwLock);
                callback?.Invoke(result);
            }));
        }

        public void GetCharacterTransfers(Action<List<CharacterTransfer>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.GetCharacterTransfers();
                callback?.Invoke(result);
            }));
        }

        public void SaveCharacterTransfer(CharacterTransfer characterTransfer, ReaderWriterLockSlim rwLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.SaveCharacterTransfer(characterTransfer, rwLock);
                callback?.Invoke(result);
            }));
        }


        public void AddCharacterInParallel(Biota biota, ReaderWriterLockSlim biotaLock, IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> possessions, Character character, ReaderWriterLockSlim characterLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = _wrappedDatabase.AddCharacterInParallel(biota, biotaLock, possessions, character, characterLock);
                callback?.Invoke(result);
            }));
        }


        /// <summary>
        /// This will get all player biotas that are backed by characters that are not deleted.
        /// </summary>
        public List<Biota> GetAllPlayerBiotasInParallel()
        {
            return _wrappedDatabase.GetAllPlayerBiotasInParallel();
        }

        public List<Biota> GetHousesOwned()
        {
            return _wrappedDatabase.GetHousesOwned();
        }

        public uint? GetAllegianceID(uint monarchID)
        {
            return _wrappedDatabase.GetAllegianceID(monarchID);
        }



        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        public void SetCharacterAccessLevelByName(string name, AccessLevel accessLevel, Action<uint> callback)
        {
            throw new NotImplementedException();
        }
    }
}
