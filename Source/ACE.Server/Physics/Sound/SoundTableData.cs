using System.Collections.Generic;

namespace ACE.Server.Physics.Sound
{
    public class SoundTableData
    {
        public Dictionary<long, SoundTableData> SoundHash;
        public int NumSTDatas;
        public SoundData Data;
    }
}
