using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Entity
{
    public static class ZergControlLandblocks
    {

        private static Dictionary<uint, ZergControlArea> _zergControlLandblocksMap;

        public static Dictionary<uint, ZergControlArea> ZergControlLandblocksMap
        {
            get
            {
                if (_zergControlLandblocksMap == null)
                {
                    _zergControlLandblocksMap = new Dictionary<uint, ZergControlArea>();

                    //Zerg control Town Control landblocks to 6
                    var holtTC = new ZergControlArea();
                    holtTC.MaxPlayersPerAllegiance = 6;
                    holtTC.AreaLandblockIds = new uint[] { 0x4FF1 };
                    _zergControlLandblocksMap.Add(0x4FF1, holtTC);

                    var yaraqTC = new ZergControlArea();
                    yaraqTC.MaxPlayersPerAllegiance = 6;
                    yaraqTC.AreaLandblockIds = new uint[] { 0x00AB };
                    _zergControlLandblocksMap.Add(0x00AB, yaraqTC);

                    var shoushiTC1 = new ZergControlArea();
                    shoushiTC1.MaxPlayersPerAllegiance = 6;
                    shoushiTC1.AreaLandblockIds = new uint[] { 0xE9F0, 0xE9F1, 0xE8F1 };
                    _zergControlLandblocksMap.Add(0xE9F1, shoushiTC1);

                    var shoushiTC2 = new ZergControlArea();
                    shoushiTC2.MaxPlayersPerAllegiance = 6;
                    shoushiTC2.AreaLandblockIds = new uint[] { 0xE9F0, 0xE9F1, 0xE8F1 };
                    _zergControlLandblocksMap.Add(0xE9F0, shoushiTC2);

                    var shoushiTC3 = new ZergControlArea();
                    shoushiTC3.MaxPlayersPerAllegiance = 6;
                    shoushiTC3.AreaLandblockIds = new uint[] { 0xE9F0, 0xE9F1, 0xE8F1 };
                    _zergControlLandblocksMap.Add(0xE8F1, shoushiTC3);
                }

                return _zergControlLandblocksMap;
            }
        }

        public static bool IsZergControlLandblock(uint landblockId)
        {
            return ZergControlLandblocksMap.ContainsKey(landblockId);
        }

        public static ZergControlArea GetLandblockZergControlArea(uint landblockId)
        {
            if (IsZergControlLandblock(landblockId))
            {
                return ZergControlLandblocksMap[landblockId];
            }

            return null;
        }
    }

    public class ZergControlArea
    {
        public uint[] AreaLandblockIds;
        public uint MaxPlayersPerAllegiance;
    }
}
