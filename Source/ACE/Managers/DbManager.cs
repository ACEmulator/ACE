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
        private static bool running = false;
        private static BlockingCollection<AceObject> saveObjects = new BlockingCollection<AceObject>();

        public static void Initialize()
        {
            // starts game loop.
            running = true;
            new Thread(Tick).Start();
        }

        /// <summary>
        /// Saves AceObject to database, does not pause execution of main game thread.
        /// </summary>
        /// <param name="ao"></param>
        public static void SaveObject(AceObject ao)
        {
                saveObjects.Add(ao);
        }

        /// <summary>
        /// Shutdowns DB Saver Manager Safely
        /// </summary>
        public static void ShutDown()
        {
            running = false;
            ProcessQue();
        }

        private static void Tick()
        {
            while (running)
            {
                ProcessQue();
            }
        }
        private static void ProcessQue()
        {
            foreach (AceObject ao in saveObjects.GetConsumingEnumerable())
            {
                DatabaseManager.Shard.SaveObjectAsync(ao);
            }
        }
    }
}