using System.Collections.Generic;
using ACE.Server.Entity;

namespace ACE.Server.Entity.TownControl
{
    public static class TownControlLandblocks
    {
        private static Dictionary<uint, uint[]> _landblocks;
        public static Dictionary<uint, uint[]> TownControlLandblocksMap
        {
            get
            {
                if (_landblocks == null)
                {
                    _landblocks = new Dictionary<uint, uint[]>();

                    uint[] shoushiLandblocks = { 0xDE51 };
                    uint[] yaraqLandblocks = { 0x8164, 0x8165 };
                    uint[] holtburgLandblocks = { 0xA5B4 };
                    _landblocks.Add(
                        72, holtburgLandblocks
                    );

                    _landblocks.Add(
                        91, shoushiLandblocks
                    );

                    _landblocks.Add(
                        102, yaraqLandblocks
                    );
                }

                return _landblocks;
            }
        }

        public static List<uint> _townControlLanblockList = null;

        public static List<uint> TownControlLanblockList
        {
            get
            {
                if(_townControlLanblockList == null)
                {
                    _townControlLanblockList = new List<uint>();

                    foreach(uint key in TownControlLandblocksMap.Keys)
                    {
                        foreach(uint landblockId in TownControlLandblocksMap[key])
                        {
                            _townControlLanblockList.Add(landblockId);
                        }
                    }
                }

                return _townControlLanblockList;
            }
        }

        public static bool IsTownControlLandblock(uint landblockId)
        {
            return TownControlLandblocks.TownControlLanblockList.Contains(landblockId);
        }

        public static uint? GetTownIdByLandblockId(uint landblockId)
        {
            foreach(uint key in TownControlLandblocksMap.Keys)
            {
                foreach(uint value in TownControlLandblocksMap[key])
                {
                    if(value == landblockId)
                    {
                        return key;
                    }
                }
            }

            return null;
        }
    }
}
