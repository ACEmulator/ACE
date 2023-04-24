using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Entity.Arenas
{
    public static class ArenaVendors
    {
        private static Dictionary<uint, ArenaVendor> _arenaVendorMap;
        public static Dictionary<uint, ArenaVendor> ArenaVendorMap
        {
            get
            {
                if (_arenaVendorMap == null)
                {
                    _arenaVendorMap = new Dictionary<uint, ArenaVendor>();

                    //Mid Rank
                    _arenaVendorMap.Add(
                        123456,
                        new ArenaVendor()
                        {
                            WeenieID = 123456,
                            RankRequirement = 80
                        }
                    );

                    //Elite Rank
                    _arenaVendorMap.Add(
                        456789,
                        new ArenaVendor()
                        {
                            WeenieID = 456789,
                            RankRequirement = 180,
                        }
                    );
                }

                return _arenaVendorMap;
            }
        }

        public static bool IsArenaVendor(uint weenieId)
        {
            return ArenaVendors.ArenaVendorMap.ContainsKey(weenieId);
        }
    }

    public class ArenaVendor
    {
        public uint WeenieID { get; set; }

        public uint RankRequirement { get; set; }
    }
}
