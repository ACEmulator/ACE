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
        //public static int IsInTownControlLandblock(Landblock currentLandblock)
        //{
        //    //Shoushi SE location 0xDE510015[49.071442 109.183655 16.004999] - 0.996967 0.000000 0.000000 0.077820

        //    //Holtburg West location 0xA5B4002D[140.216034 108.323578 54.058929] 0.138897 0.000000 0.000000 - 0.990307

        //    //Yaraq East Location 0x81640017[50.047153 147.723450 22.004999] - 0.989617 0.000000 0.000000 0.143732

        //    var cLandblock = currentLandblock.Id.Landblock;
        //    if (TownControlLandblocks.TownControlLandblocksMap[72].Contains(cLandblock))
        //        return 72;
        //    else if (TownControlLandblocks.TownControlLandblocksMap[91].Contains(cLandblock))
        //        return 91;
        //    else if (TownControlLandblocks.TownControlLandblocksMap[102].Contains(cLandblock))
        //        return 102;
        //    else
        //        return 0;

        //}

    }
}
