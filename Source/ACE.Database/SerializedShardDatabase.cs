using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public class SerializedShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This is the base database that SerializedShardDatabase is a wrapper for.
        /// </summary>
        public readonly ShardDatabase BaseDatabase;

        private readonly BlockingCollection<Task> _queue = new BlockingCollection<Task>();

        private Thread _workerThread;

        internal SerializedShardDatabase(ShardDatabase shardDatabase)
        {
            BaseDatabase = shardDatabase;
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
                    catch (Exception ex)
                    {
                        log.Error($"[DATABASE] DoWork task failed with exception: {ex}");
                        // perhaps add failure callbacks?
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
                var result = BaseDatabase.GetMaxGuidFoundInRange(min, max);
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
                var result = BaseDatabase.GetSequenceGaps(min, limitAvailableIDsReturned);
                callback?.Invoke(result);
            }));
        }


        public void SaveBiota(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = BaseDatabase.SaveBiota(biota, rwLock);
                callback?.Invoke(result);
            }));
        }


        public void SaveBiotasInParallel(IEnumerable<(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)> biotas, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = BaseDatabase.SaveBiotasInParallel(biotas);
                callback?.Invoke(result);
            }));
        }

        public void RemoveBiota(uint id, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = BaseDatabase.RemoveBiota(id);
                callback?.Invoke(result);
            }));
        }

        public void RemoveBiota(uint id, Action<bool> callback, Action<TimeSpan, TimeSpan> performanceResults)
        {
            var initialCallTime = DateTime.UtcNow;

            _queue.Add(new Task(() =>
            {
                var taskStartTime = DateTime.UtcNow;
                var result = BaseDatabase.RemoveBiota(id);
                var taskCompletedTime = DateTime.UtcNow;
                callback?.Invoke(result);
                performanceResults?.Invoke(taskStartTime - initialCallTime, taskCompletedTime - taskStartTime);
            }));
        }

        public void RemoveBiotasInParallel(IEnumerable<uint> ids, Action<bool> callback, Action<TimeSpan, TimeSpan> performanceResults)
        {
            var initialCallTime = DateTime.UtcNow;

            _queue.Add(new Task(() =>
            {
                var taskStartTime = DateTime.UtcNow;
                var result = BaseDatabase.RemoveBiotasInParallel(ids);
                var taskCompletedTime = DateTime.UtcNow;
                callback?.Invoke(result);
                performanceResults?.Invoke(taskStartTime - initialCallTime, taskCompletedTime - taskStartTime);
            }));
        }


        public void GetPossessedBiotasInParallel(uint id, Action<PossessedBiotas> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = BaseDatabase.GetPossessedBiotasInParallel(id);
                callback?.Invoke(c);
            }));
        }

        public void GetInventoryInParallel(uint parentId, bool includedNestedItems, Action<List<Biota>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var c = BaseDatabase.GetInventoryInParallel(parentId, includedNestedItems);
                callback?.Invoke(c);
            }));

        }


        public void IsCharacterNameAvailable(string name, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = BaseDatabase.IsCharacterNameAvailable(name);
                callback?.Invoke(result);
            }));
        }

        public void GetCharacters(uint accountId, bool includeDeleted, Action<List<Character>> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = BaseDatabase.GetCharacters(accountId, includeDeleted);
                callback?.Invoke(result);
            }));
        }

        public void SaveCharacter(Character character, ReaderWriterLockSlim rwLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = BaseDatabase.SaveCharacter(character, rwLock);
                callback?.Invoke(result);
            }));
        }

        public void RenameCharacter(Character character, string newName, ReaderWriterLockSlim rwLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = BaseDatabase.RenameCharacter(character, newName, rwLock);
                callback?.Invoke(result);
            }));
        }

        public void SetCharacterAccessLevelByName(string name, AccessLevel accessLevel, Action<uint> callback)
        {
            // TODO
            throw new NotImplementedException();
        }


        public void AddCharacterInParallel(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim biotaLock, IEnumerable<(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)> possessions, Character character, ReaderWriterLockSlim characterLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                var result = BaseDatabase.AddCharacterInParallel(biota, biotaLock, possessions, character, characterLock);
                callback?.Invoke(result);
            }));
        }
    }
}
