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
                    _equipmentSetWhitelistedLandblocks = new List<uint>() { 0x0031 }; //Example adds Creepy Canyons landblock to the whitelist
                }

                return _equipmentSetWhitelistedLandblocks;
            }
        }

        public static bool IsEquipmentSetWhitelistedLandblock(uint landblockId)
        {
            return EquipmentSetWhitelistedLandblocks.Contains(landblockId);
        }
    }
}
