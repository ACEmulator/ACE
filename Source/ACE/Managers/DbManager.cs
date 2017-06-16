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

        private static Queue<AceObject> saveObjectsQue = new Queue<AceObject>();
        private static readonly object saveObjectsCacheLocker = new object();

        public static void SaveObject(AceObject ao)
        {
            lock (saveObjectsCacheLocker)
            {
                saveObjectsQue.Enqueue(ao);
            }
        }

        private static void Tick()
        {
            AceObject aceobj;
            lock (saveObjectsCacheLocker)
            {
                if (saveObjectsQue.Count > 0)
                    aceobj = saveObjectsQue.Dequeue();
                else
                    return;
            }
            TickAsync(aceobj);
        }

        private static async Task<bool> TickAsync(AceObject aceobj)
        {
            bool saveSuccess = await DatabaseManager.Shard.SaveObject(aceobj);
            /// todo: report error.
            return saveSuccess;
        }
    }
}
