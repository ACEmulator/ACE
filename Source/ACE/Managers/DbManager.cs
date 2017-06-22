using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using ACE.Common;
using ACE.Entity;
using ACE.Network;

using log4net;
using ACE.Database;
using System.Collections.Concurrent;

namespace ACE.Managers
{
    public static class DbManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static BlockingCollection<Task<bool>> saveObjects = new BlockingCollection<Task<bool>>();
        private static Thread taskCleanThread;

        public static void Initialize()
        {
            // starts game loop.
            taskCleanThread = new Thread(Tick);
            taskCleanThread.Start();
        }

        /// <summary>
        /// Saves AceObject to database, does not pause execution of main game thread.
        /// </summary>
        /// <param name="ao"></param>
        public static Task<bool> SaveObject(AceObject ao)
        {
            Task<bool> ret = DatabaseManager.Shard.SaveObject(ao);
            saveObjects.Add(ret);
            return ret;
        }

        /// <summary>
        /// Shutdowns DB Saver Manager Safely
        /// </summary>
        public static void ShutDown()
        {
            saveObjects.CompleteAdding();

            // Wait for all the tasks to finish
            taskCleanThread.Join();
        }

        private static void Tick()
        {
            // Stops us from shutting down before all the saves are done
            while (!saveObjects.IsCompleted)
            {
                Task<bool> saveTask = saveObjects.Take();

                saveTask.Wait();
            }
        }
    }
}