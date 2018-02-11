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

        public static void SetLandBlockKey(int col, int row, LandBlockStatus info)
        {
            lock (landBlockMutex)
            {
                landBlockInfos[col, row] = info;
            }
        }

        public static LandBlockStatusFlag GetLandBlockKeyFlag(int col, int row)
        {
            if (landBlockInfos[col, row] == null)
                return LandBlockStatusFlag.IdleUnloaded;
            return landBlockInfos[col, row].LandBlockStatusFlag;
        }

        public static LandBlockStatus GetLandBlockKey(int col, int row)
        {
            if (landBlockInfos[col, row] == null)
                return null;
            return landBlockInfos[col, row];
        }
    }
}
