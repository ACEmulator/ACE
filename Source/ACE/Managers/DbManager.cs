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

namespace ACE.Managers
{
    public static class DbManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static bool running = false;
        private static Queue<AceObject> saveObjectsQue = new Queue<AceObject>();
        private static readonly object saveObjectsCacheLocker = new object();

        public static void SaveObject(AceObject ao)
        {
            lock (saveObjectsCacheLocker)
            {
                saveObjectsQue.Enqueue(ao);
            }
        }

        public static void Initialize()
        {
            // starts game loop.
            running = true;
            new Thread(Tick).Start();
        }

        private static void Tick()
        {
            while (running)
            {
                Thread.Sleep(1);

                AceObject aceobj;

                lock (saveObjectsCacheLocker)
                {
                    if (saveObjectsQue.Count > 0)
                    {
                        aceobj = saveObjectsQue.Dequeue();
                        TickAsync(aceobj);
                    }
                }
            }
        }

        private static async Task<bool> TickAsync(AceObject aceobj)
        {
            bool saveSuccess = await DatabaseManager.Shard.SaveObject(aceobj);
            /// todo: report error.
            return saveSuccess;
        }
    }
}
