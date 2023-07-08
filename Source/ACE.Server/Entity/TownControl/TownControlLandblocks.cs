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

                    uint[] shoushiLandblocks = { 0xDE51, 0xE9F1, 0xE8F1, 0xE9F0 };
                    uint[] yaraqLandblocks = { 0x8164, 0x00AB };
                    uint[] holtburgLandblocks = { 0xA5B4, 0x4FF1 };
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

        private static Dictionary<uint, uint[]> _rewardlandblocks;
        public static Dictionary<uint, uint[]> TownControlRewardLandblocksMap
        {
            get
            {
                if (_rewardlandblocks == null)
                {
                    _rewardlandblocks = new Dictionary<uint, uint[]>();

                    uint[] shoushiLandblocks = { 0xE9F0, 0xE9F1, 0xE8F1 };
                    uint[] yaraqLandblocks = { 0x00AB };
                    uint[] holtburgLandblocks = { 0x4FF1 };
                    _rewardlandblocks.Add(
                        72, holtburgLandblocks
                    );

                    _rewardlandblocks.Add(
                        91, shoushiLandblocks
                    );

                    _rewardlandblocks.Add(
                        102, yaraqLandblocks
                    );
                }

                return _rewardlandblocks;
            }
        }

        /*
        private static Dictionary<uint, uint[]> _landcells;
        public static Dictionary<uint, uint[]> TownControlLandcellsMap
        {
            get
            {
                if (_landcells == null)
                {
                    _landcells = new Dictionary<uint, uint[]>();

                    uint[] shoushiLandcells = {
                        0xDE510003, 0xDE510004, 0xDE510005, 0xDE510006, 0xDE510007, 0xDE51000B,
                        0xDE51000C, 0xDE51000D, 0xDE51000E, 0xDE51000F, 0xDE510010, 0xDE510013,
                        0xDE510014, 0xDE510015, 0xDE510016, 0xDE510017, 0xDE510018, 0xDE51001B,
                        0xDE51001C, 0xDE51001D, 0xDE51001E, 0xDE51001F, 0xDE510020, 0xDE510028,
                        0xDE510027, 0xDE510026, 0xDE510025
                    };
                    uint[] yaraqLandcells = {
                        0x81640008, 0x81640007, 0x81640006, 0x81640005, 0x81640004, 0x81640003,
                        0x81640010, 0x8164000F, 0x8164000E, 0x8164000D, 0x8164000C, 0x8164000B,
                        0x81640018, 0x81640017, 0x81640016, 0x81640015, 0x81640014, 0x81640013,
                        0x81640020, 0x8164001F, 0x8164001E, 0x8164001D, 0x8164001C, 0x8164001B,
                        0x8064003B, 0x8064003C, 0x8064003D, 0x8064003E, 0x8064003F, 0x80640040,
                        0x80640038, 0x80640037, 0x80640036, 0x80640035, 0x80640034, 0x80640033,
                        0x80640030, 0x8064002F, 0x8064002E
                    };
                    uint[] holtburgLandcells = {
                        0xA9B40024
                    };
                    _landcells.Add(
                        72, holtburgLandcells
                    );

                    _landcells.Add(
                        91, shoushiLandcells
                    );

                    _landcells.Add(
                        102, yaraqLandcells
                    );
                }

                return _landcells;
            }
        }
        */

        public static List<uint> _townControlLanblockList = null;

        public static List<uint> TownControlLanblockList
        {
            get
            {
                if (_townControlLanblockList == null)
                {
                    _townControlLanblockList = new List<uint>();

                    foreach (uint key in TownControlLandblocksMap.Keys)
                    {
                        foreach (uint landblockId in TownControlLandblocksMap[key])
                        {
                            _townControlLanblockList.Add(landblockId);
                        }
                    }
                }

                return _townControlLanblockList;
            }
        }

        public static List<uint> _townControlRewardLanblockList = null;
        public static List<uint> TownControlRewardLanblockList
        {
            get
            {
                if (_townControlRewardLanblockList == null)
                {
                    _townControlRewardLanblockList = new List<uint>();

                    foreach (uint key in TownControlRewardLandblocksMap.Keys)
                    {
                        foreach (uint landblockId in TownControlRewardLandblocksMap[key])
                        {
                            _townControlRewardLanblockList.Add(landblockId);
                        }
                    }
                }

                return _townControlRewardLanblockList;
            }
        }

        public static bool IsTownControlLandblock(uint landblockId)
        {
            return TownControlLandblocks.TownControlLanblockList.Contains(landblockId);
        }

        public static bool IsTownControlRewardLandblock(uint landblockId)
        {
            return TownControlLandblocks.TownControlRewardLanblockList.Contains(landblockId);
        }

        public static uint? GetTownIdByLandblockId(uint landblockId)
        {
            foreach (uint key in TownControlLandblocksMap.Keys)
            {
                foreach (uint value in TownControlLandblocksMap[key])
                {
                    if (value == landblockId)
                    {
                        return key;
                    }
                }
            }

            return null;
        }

        /*
                public static List<uint> _townControlLandcellList = null;

                public static List<uint> TownControlLandcellList
                {
                    get
                    {
                        if (_townControlLandcellList == null)
                        {
                            _townControlLandcellList = new List<uint>();

                            foreach (uint key in TownControlLandcellsMap.Keys)
                            {
                                foreach (uint landcellId in TownControlLandcellsMap[key])
                                {
                                    _townControlLandcellList.Add(landcellId);
                                }
                            }
                        }

                        return _townControlLandcellList;
                    }
                }

                public static bool IsTownControlLandcell(uint landcellId)
                {
                    return TownControlLandblocks.TownControlLandcellList.Contains(landcellId);
                }

                public static uint? GetTownIdByLandcellId(uint landcellId)
                {
                    foreach (uint key in TownControlLandcellsMap.Keys)
                    {
                        foreach (uint value in TownControlLandcellsMap[key])
                        {
                            if (value == landcellId)
                            {
                                return key;
                            }
                        }
                    }

                    return null;
                }
        */

        private static Dictionary<uint, string> _landblockEvents;
        public static Dictionary<uint, string> LandblockEventsMap
        {
            get
            {
                if (_landblockEvents == null)
                {
                    _landblockEvents = new Dictionary<uint, string>();
                    _landblockEvents.Add(0xDE51, "towncontrol3"); //shoushi
                    _landblockEvents.Add(0xE9F1, "towncontrol3"); //shoushi
                    _landblockEvents.Add(0xE8F1, "towncontrol3"); //shoushi
                    _landblockEvents.Add(0xE9F0, "towncontrol3"); //shoushi

                    _landblockEvents.Add(0x8164, "towncontrol1"); //yaraq
                    _landblockEvents.Add(0x00AB, "towncontrol1"); //yaraq

                    _landblockEvents.Add(0xA5B4, "towncontrol2"); //holtburg
                    _landblockEvents.Add(0x4FF1, "towncontrol2"); //holtburg
                }

                return _landblockEvents;
            }
        }
    }
}
