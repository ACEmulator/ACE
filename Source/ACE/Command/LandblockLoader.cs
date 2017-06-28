using ACE.Entity;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Command
{
    public static class LandblockLoader
    {
        private volatile static bool processLandblockLoading = false;

        public static void StartLoading()
        {
            Thread loadthread = new Thread(new ThreadStart(LoadAllLandblocks));
            loadthread.Start();
        }

        public static void LoadAllLandblocks()
        {
            ushort block = 0;
            processLandblockLoading = true;

            while (processLandblockLoading && block <= 0xFE01)
            {
                LoadLandblock(block++);
            }

            processLandblockLoading = false;
            LandblockManager.FinishedForceLoading();
        }

        public static void StopLoading()
        {
            processLandblockLoading = false;
        }

        public static void LoadLandblock(ushort block)
        {
            LandblockManager.ForceLoadLandBlock(new LandblockId(((uint)block) << 16));
        }
    }
}
