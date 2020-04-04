using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;

using log4net;

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
                log.Debug("Start GetMaxGuidFoundInRange()");
                var result = BaseDatabase.GetMaxGuidFoundInRange(min, max);
                log.Debug("Callback Start GetMaxGuidFoundInRange()");
                callback?.Invoke(result);
                log.Debug("Return GetMaxGuidFoundInRange()");
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
                log.Debug("Start GetSequenceGaps()");
                var result = BaseDatabase.GetSequenceGaps(min, limitAvailableIDsReturned);
                log.Debug("Callback Start GetMaxGuidFoundInRange()");
                callback?.Invoke(result);
                log.Debug("Return GetSequenceGaps()");
            }));
        }


        public void SaveBiota(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                log.Debug("Start SaveBiota()");
                var result = BaseDatabase.SaveBiota(biota, rwLock);
                log.Debug("Callback Start SaveBiota()");
                callback?.Invoke(result);
                log.Debug("Return SaveBiota()");
            }));
        }


        public void SaveBiotasInParallel(IEnumerable<(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)> biotas, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                log.Debug("Start SaveBiotasInParallel()");
                var result = BaseDatabase.SaveBiotasInParallel(biotas);
                log.Debug("Callback Start SaveBiotasInParallel()");
                callback?.Invoke(result);
                log.Debug("Return SaveBiotasInParallel()");
            }));
        }

        public void RemoveBiota(uint id, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                log.Debug("Start RemoveBiota() a");
                var result = BaseDatabase.RemoveBiota(id);
                log.Debug("Callback Start RemoveBiota() a");
                callback?.Invoke(result);
                log.Debug("Return RemoveBiota() a");
            }));
        }

        public void RemoveBiota(uint id, Action<bool> callback, Action<TimeSpan, TimeSpan> performanceResults)
        {
            var initialCallTime = DateTime.UtcNow;

            _queue.Add(new Task(() =>
            {
                log.Debug("Start RemoveBiota() b");
                var taskStartTime = DateTime.UtcNow;
                var result = BaseDatabase.RemoveBiota(id);
                var taskCompletedTime = DateTime.UtcNow;
                log.Debug("Callback 1 Start RemoveBiota() b");
                callback?.Invoke(result);
                log.Debug("Callback 2 Start RemoveBiota() b");
                performanceResults?.Invoke(taskStartTime - initialCallTime, taskCompletedTime - taskStartTime);
                log.Debug("Return RemoveBiota() b");
            }));
        }

        public void RemoveBiotasInParallel(IEnumerable<uint> ids, Action<bool> callback, Action<TimeSpan, TimeSpan> performanceResults)
        {
            var initialCallTime = DateTime.UtcNow;

            _queue.Add(new Task(() =>
            {
                log.Debug("Start RemoveBiotasInParallel()");
                var taskStartTime = DateTime.UtcNow;
                var result = BaseDatabase.RemoveBiotasInParallel(ids);
                var taskCompletedTime = DateTime.UtcNow;
                log.Debug("Callback 1 Start RemoveBiotasInParallel()");
                callback?.Invoke(result);
                log.Debug("Callback 2 Start RemoveBiotasInParallel()");
                performanceResults?.Invoke(taskStartTime - initialCallTime, taskCompletedTime - taskStartTime);
                log.Debug("Return RemoveBiotasInParallel()");
            }));
        }


        public void GetPossessedBiotasInParallel(uint id, Action<PossessedBiotas> callback)
        {
            _queue.Add(new Task(() =>
            {
                log.Debug("Start GetPossessedBiotasInParallel()");
                var c = BaseDatabase.GetPossessedBiotasInParallel(id);
                log.Debug("Callback Start GetPossessedBiotasInParallel()");
                callback?.Invoke(c);
                log.Debug("Return GetPossessedBiotasInParallel()");
            }));
        }

        public void GetInventoryInParallel(uint parentId, bool includedNestedItems, Action<List<Biota>> callback)
        {
            _queue.Add(new Task(() =>
            {
                log.Debug("Start GetInventoryInParallel()");
                var c = BaseDatabase.GetInventoryInParallel(parentId, includedNestedItems);
                log.Debug("Callback Start GetInventoryInParallel()");
                callback?.Invoke(c);
                log.Debug("Return GetInventoryInParallel()");
            }));

        }


        public void IsCharacterNameAvailable(string name, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                log.Debug("Start IsCharacterNameAvailable()");
                var result = BaseDatabase.IsCharacterNameAvailable(name);
                log.Debug("Callback Start IsCharacterNameAvailable()");
                callback?.Invoke(result);
                log.Debug("Return IsCharacterNameAvailable()");
            }));
        }

        public void GetCharacters(uint accountId, bool includeDeleted, Action<List<Character>> callback)
        {
            _queue.Add(new Task(() =>
            {
                log.Debug("Start GetCharacters()");
                var result = BaseDatabase.GetCharacters(accountId, includeDeleted);
                log.Debug("Callback Start GetCharacters()");
                callback?.Invoke(result);
                log.Debug("Return GetCharacters()");
            }));
        }

        public void SaveCharacter(Character character, ReaderWriterLockSlim rwLock, Action<bool> callback)
        {
            _queue.Add(new Task(() =>
            {
                log.Debug("Start SaveCharacter()");
                var result = BaseDatabase.SaveCharacter(character, rwLock);
                log.Debug("Callback Start SaveCharacter()");
                callback?.Invoke(result);
                log.Debug("Return SaveCharacter()");
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
                log.Debug("Start AddCharacterInParallel()");
                var result = BaseDatabase.AddCharacterInParallel(biota, biotaLock, possessions, character, characterLock);
                log.Debug("Callback Start AddCharacterInParallel()");
                callback?.Invoke(result);
                log.Debug("Return AddCharacterInParallel()");
            }));
        }
    }
}
