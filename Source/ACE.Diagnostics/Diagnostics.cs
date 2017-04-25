using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Diagnostics
{
    public static class Diagnostics
    {
        private static readonly object landBlockMutex = new object();
        private static LandBlockStatus[,] landBlockInfos = new LandBlockStatus[256, 256];
        public static bool LandBlockDiag = false;

        public static void SetLandBlockKeys(LandBlockStatus[,] info)
        {
            lock (landBlockMutex)
            {
                landBlockInfos = info;
            }
        }

        public static LandBlockStatus[,] GetLandBlockKeys(LandBlockStatus[,] infos)
        {
            return landBlockInfos;
        }

        public static void SetLandBlockKey(int row, int col, LandBlockStatus info)
        {
            lock (landBlockMutex)
            {
                landBlockInfos[row, col] = info;
            }
        }

        public static LandBlockStatusFlag GetLandBlockKeyFlag(int row, int col)
        {
            if (landBlockInfos[row, col] == null)
                return LandBlockStatusFlag.IdleUnloaded;
            else
                return landBlockInfos[row, col].LandBlockStatusFlag;
        }

        public static LandBlockStatus GetLandBlockKey(int row, int col)
        {
            if (landBlockInfos[row, col] == null)
                return null;
            else
                return landBlockInfos[row, col];
        }
    }
}
