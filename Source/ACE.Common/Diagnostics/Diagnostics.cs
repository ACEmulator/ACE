namespace ACE.Common
{
    public static class Diagnostics
    {
        private static readonly object landBlockMutex = new object();
        private static LandBlockStatusFlag[,] landBlockKeys;
        public static bool LandBlockDiag = false;

        public static void Init()
        {
            landBlockKeys = new LandBlockStatusFlag[256, 256];
        }

        public static void SetLandBlockKeys(LandBlockStatusFlag[,] keys)
        {
            lock (landBlockMutex)
            {
                landBlockKeys = keys;
            }
        }

        public static LandBlockStatusFlag[,] GetLandBlockKeys(LandBlockStatusFlag[,] keys)
        {
            return landBlockKeys;
        }

        public static void SetLandBlockKey(int row, int col, LandBlockStatusFlag key)
        {
           landBlockKeys[row, col] = key;
        }

        public static LandBlockStatusFlag GetLandBlockKey(int row, int col)
        {
            return landBlockKeys[row, col];
        }
    }
}
