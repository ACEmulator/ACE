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

                    uint[] shoushiLandblocks = {
                        0xDE510003, 0xDE510004, 0xDE510005, 0xDE510006, 0xDE510007, 0xDE51000B,
                        0xDE51000C, 0xDE51000D, 0xDE51000E, 0xDE51000F, 0xDE510010, 0xDE510013,
                        0xDE510014, 0xDE510015, 0xDE510016, 0xDE510017, 0xDE510018, 0xDE51001B,
                        0xDE51001C, 0xDE51001D, 0xDE51001E, 0xDE51001F, 0xDE510020, 0xDE510028,
                        0xDE510027, 0xDE510026, 0xDE510025
                    };
                    uint[] yaraqLandblocks = {
                        0x81640008, 0x81640007, 0x81640006, 0x81640005, 0x81640004, 0x81640003,
                        0x81640010, 0x8164000F, 0x8164000E, 0x8164000D, 0x8164000C, 0x8164000B,
                        0x81640018, 0x81640017, 0x81640016, 0x81640015, 0x81640014, 0x81640013,
                        0x81640020, 0x8164001F, 0x8164001E, 0x8164001D, 0x8164001C, 0x8164001B,
                        0x8064003B, 0x8064003C, 0x8064003D, 0x8064003E, 0x8064003F, 0x80640040,
                        0x80640038, 0x80640037, 0x80640036, 0x80640035, 0x80640034, 0x80640033,
                        0x80640030, 0x8064002F, 0x8064002E
                    };
                    uint[] holtburgLandblocks = {
                        0xA5B40039, 0xA5B4003A, 0xA5B4003B, 0xA5B4003C, 0xA5B4003D, 0xA5B4003E,
                        0xA5B40031, 0xA5B40032, 0xA5B40033, 0xA5B40033, 0xA5B40034, 0xA5B40035,
                        0xA5B40036, 0xA5B40029, 0xA5B4002A, 0xA5B4002B, 0xA5B4002C, 0xA5B4002D,
                        0xA5B4002E, 0xA5B40021, 0xA5B40022, 0xA5B40023, 0xA5B40024, 0xA5B40025,
                        0xA5B40026, 0xA6B40001, 0xA6B40002, 0xA6B40003, 0xA6B40004, 0xA6B40005,
                        0xA6B40006
                    };
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
