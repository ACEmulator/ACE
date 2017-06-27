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

        public static void Start()
        {
            Thread loadthread = new Thread(new ThreadStart(LoadAllLandblocks));
            loadthread.Start();
        }

        public static void LoadAllLandblocks()
        {
            ushort block = 0;
            processLandblockLoading = true;

            while (processLandblockLoading)
            {
                block++;
                LoadLandblock(block);
            }
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
