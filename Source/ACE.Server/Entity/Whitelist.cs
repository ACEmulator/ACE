using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Entity
{
    public static class Whitelist
    {
        private static List<uint> _epicWhitelistedLandblocks;
        private static List<uint> EpicWhitelistedLandblocks
        {
            get
            {
                if (_epicWhitelistedLandblocks == null)
                {
                    _epicWhitelistedLandblocks = new List<uint>() { 0x0031 }; //Example adds Creepy Canyons landblock to the whitelist
                }

                return _epicWhitelistedLandblocks;
            }
        }

        public static bool IsEpicWhitelistedLandblock(uint landblockId)
        {
            return EpicWhitelistedLandblocks.Contains(landblockId);
        }


        private static List<uint> _legendaryWhitelistedLandblocks;
        private static List<uint> LegendaryWhitelistedLandblocks
        {
            get
            {
                if (_legendaryWhitelistedLandblocks == null)
                {
                    _legendaryWhitelistedLandblocks = new List<uint>() { };
                }

                return _legendaryWhitelistedLandblocks;
            }
        }

        public static bool IsLegendaryWhitelistedLandblock(uint landblockId)
        {
            return LegendaryWhitelistedLandblocks.Contains(landblockId);
        }

        private static List<uint> _equipmentSetWhitelistedLandblocks;
        private static List<uint> EquipmentSetWhitelistedLandblocks
        {
            get
            {
                if (_equipmentSetWhitelistedLandblocks == null)
                {
                    _equipmentSetWhitelistedLandblocks = new List<uint>()
                    {
                        0x002D, //DD
                        //0x00E1, //PotB East
                        0x002A, //PotB Mid
                        //0x004B, //PotB West
                        //0x0064, //EO East
                        0x002B, //EO Mid
                        //0x00C8, //EO West
                        0x6146, //Baishi Hive

                        //150 has these diagonal stripes of landblocks across it
                        0xCBEB, //150 Island
                        0xCBEC, //150 Island
                        0xCBED, //150 Island
                        0xCAEF, //150 Island
                        0xCAEE, //150 Island
                        0xCAED, //150 Island
                        0xCAEC, //150 Island
                        0xCAEB, //150 Island
                        0xCAEA, //150 Island
                        0xCAE9, //150 Island                       
                        0xC9F0, //150 Island
                        0xC9EF, //150 Island
                        0xC9EE, //150 Island
                        0xC9ED, //150 Island
                        0xC9EC, //150 Island
                        0xC9EB, //150 Island
                        0xC9EA, //150 Island
                        0xC9E9, //150 Island
                        0xC8F1, //150 Island
                        0xC8F0, //150 Island
                        0xC8EF, //150 Island
                        0xC8EE, //150 Island
                        0xC8ED, //150 Island
                        0xC8EC, //150 Island
                        0xC8EB, //150 Island
                        0xC8EA, //150 Island
                        0xC8E9, //150 Island
                        0xC8E8, //150 Island
                        0xC7F3, //150 Island
                        0xC7F2, //150 Island
                        0xC7F1, //150 Island
                        0xC7F0, //150 Island
                        0xC7EF, //150 Island
                        0xC7EE, //150 Island
                        0xC7ED, //150 Island
                        0xC7EC, //150 Island
                        0xC7EB, //150 Island
                        0xC7EA, //150 Island
                        0xC7E9, //150 Island
                        0xC6F5, //150 Island
                        0xC6F4, //150 Island
                        0xC6F3, //150 Island
                        0xC6F2, //150 Island
                        0xC6F1, //150 Island
                        0xC6F0, //150 Island
                        0xC6EF, //150 Island
                        0xC6EE, //150 Island
                        0xC6ED, //150 Island
                        0xC6EC, //150 Island
                        0xC6EB, //150 Island
                        0xC6EA, //150 Island
                        0xC5F6, //150 Island
                        0xC5F5, //150 Island
                        0xC5F4, //150 Island
                        0xC5F3, //150 Island
                        0xC5F2, //150 Island
                        0xC5F1, //150 Island
                        0xC5F0, //150 Island
                        0xC5EF, //150 Island
                        0xC5EE, //150 Island
                        0xC5ED, //150 Island
                        0xC5EC, //150 Island
                        0xC5EB, //150 Island
                        0xC4F7, //150 Island
                        0xC4F6, //150 Island
                        0xC4F5, //150 Island
                        0xC4F4, //150 Island
                        0xC4F3, //150 Island
                        0xC4F2, //150 Island
                        0xC4F1, //150 Island
                        0xC4F0, //150 Island
                        0xC4EF, //150 Island
                        0xC4EE, //150 Island
                        0xC4ED, //150 Island
                        0xC4EC, //150 Island
                        0xC4EB, //150 Island
                        0xC3F8, //150 Island
                        0xC3F7, //150 Island
                        0xC3F6, //150 Island
                        0xC3F5, //150 Island
                        0xC3F4, //150 Island
                        0xC3F3, //150 Island
                        0xC3F2, //150 Island
                        0xC3F1, //150 Island
                        0xC3F0, //150 Island
                        0xC3EE, //150 Island
                        0xC3ED, //150 Island
                        0xC3EC, //150 Island
                        0xC3EB, //150 Island
                        0xC2F7, //150 Island
                        0xC2F6, //150 Island
                        0xC2F5, //150 Island
                        0xC2F4, //150 Island
                        0xC2F3, //150 Island
                        0xC1F7, //150 Island
                        0xC1F6, //150 Island
                        0xC1F5, //150 Island                                                                        
                    }; 
                }

                return _equipmentSetWhitelistedLandblocks;
            }
        }

        public static bool IsEquipmentSetWhitelistedLandblock(uint landblockId)
        {
            return EquipmentSetWhitelistedLandblocks.Contains(landblockId);
        }

        private static List<uint> _aetheriaWhitelistedLandblocks;
        private static List<uint> AetheriaWhitelistedLandblocks
        {
            get
            {
                if (_aetheriaWhitelistedLandblocks == null)
                {
                    _aetheriaWhitelistedLandblocks = new List<uint>()
                    {                        
                        //150 has these diagonal stripes of landblocks across it
                        0xCBEB, //150 Island
                        0xCBEC, //150 Island
                        0xCBED, //150 Island
                        0xCAEF, //150 Island
                        0xCAEE, //150 Island
                        0xCAED, //150 Island
                        0xCAEC, //150 Island
                        0xCAEB, //150 Island
                        0xCAEA, //150 Island
                        0xCAE9, //150 Island                       
                        0xC9F0, //150 Island
                        0xC9EF, //150 Island
                        0xC9EE, //150 Island
                        0xC9ED, //150 Island
                        0xC9EC, //150 Island
                        0xC9EB, //150 Island
                        0xC9EA, //150 Island
                        0xC9E9, //150 Island
                        0xC8F1, //150 Island
                        0xC8F0, //150 Island
                        0xC8EF, //150 Island
                        0xC8EE, //150 Island
                        0xC8ED, //150 Island
                        0xC8EC, //150 Island
                        0xC8EB, //150 Island
                        0xC8EA, //150 Island
                        0xC8E9, //150 Island
                        0xC8E8, //150 Island
                        0xC7F3, //150 Island
                        0xC7F2, //150 Island
                        0xC7F1, //150 Island
                        0xC7F0, //150 Island
                        0xC7EF, //150 Island
                        0xC7EE, //150 Island
                        0xC7ED, //150 Island
                        0xC7EC, //150 Island
                        0xC7EB, //150 Island
                        0xC7EA, //150 Island
                        0xC7E9, //150 Island
                        0xC6F5, //150 Island
                        0xC6F4, //150 Island
                        0xC6F3, //150 Island
                        0xC6F2, //150 Island
                        0xC6F1, //150 Island
                        0xC6F0, //150 Island
                        0xC6EF, //150 Island
                        0xC6EE, //150 Island
                        0xC6ED, //150 Island
                        0xC6EC, //150 Island
                        0xC6EB, //150 Island
                        0xC6EA, //150 Island
                        0xC5F6, //150 Island
                        0xC5F5, //150 Island
                        0xC5F4, //150 Island
                        0xC5F3, //150 Island
                        0xC5F2, //150 Island
                        0xC5F1, //150 Island
                        0xC5F0, //150 Island
                        0xC5EF, //150 Island
                        0xC5EE, //150 Island
                        0xC5ED, //150 Island
                        0xC5EC, //150 Island
                        0xC5EB, //150 Island
                        0xC4F7, //150 Island
                        0xC4F6, //150 Island
                        0xC4F5, //150 Island
                        0xC4F4, //150 Island
                        0xC4F3, //150 Island
                        0xC4F2, //150 Island
                        0xC4F1, //150 Island
                        0xC4F0, //150 Island
                        0xC4EF, //150 Island
                        0xC4EE, //150 Island
                        0xC4ED, //150 Island
                        0xC4EC, //150 Island
                        0xC4EB, //150 Island
                        0xC3F8, //150 Island
                        0xC3F7, //150 Island
                        0xC3F6, //150 Island
                        0xC3F5, //150 Island
                        0xC3F4, //150 Island
                        0xC3F3, //150 Island
                        0xC3F2, //150 Island
                        0xC3F1, //150 Island
                        0xC3F0, //150 Island
                        0xC3EE, //150 Island
                        0xC3ED, //150 Island
                        0xC3EC, //150 Island
                        0xC3EB, //150 Island
                        0xC2F7, //150 Island
                        0xC2F6, //150 Island
                        0xC2F5, //150 Island
                        0xC2F4, //150 Island
                        0xC2F3, //150 Island
                        0xC1F7, //150 Island
                        0xC1F6, //150 Island
                        0xC1F5, //150 Island                                                                        
                    };
                }

                return _aetheriaWhitelistedLandblocks;
            }
        }

        public static bool IsAetheriaWhitelistedLandblock(uint landblockId)
        {
            return AetheriaWhitelistedLandblocks.Contains(landblockId);
        }
    }
}
