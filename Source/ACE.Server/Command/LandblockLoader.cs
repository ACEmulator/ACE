using System.Threading;
using ACE.Entity;
using ACE.Server.Managers;

namespace ACE.Server.Command
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
