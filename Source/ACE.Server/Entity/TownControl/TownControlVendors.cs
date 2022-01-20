using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Entity.TownControl
{
    public static class TownControlVendors
    {
        private static Dictionary<uint, TownControlVendor> _tcVendorMap;
        public static Dictionary<uint, TownControlVendor> TownControlVendorMap
        {
            get
            {
                if(_tcVendorMap == null)
                {
                    _tcVendorMap = new Dictionary<uint, TownControlVendor>();

                    //Shoushi
                    _tcVendorMap.Add(
                        42128707,
                        new TownControlVendor()
                        {
                            WeenieID = 42128707,
                            TownID = 91,
                            TownName = "Shoushi"
                        }
                    );

                    //Holtburg
                    _tcVendorMap.Add(
                        42128708,
                        new TownControlVendor()
                        {
                            WeenieID = 42128708,
                            TownID = 72,
                            TownName = "Holtburg"
                        }
                    );


                    //Yaraq
                    _tcVendorMap.Add(
                        42128709,
                        new TownControlVendor()
                        {
                            WeenieID = 42128709,
                            TownID = 102,
                            TownName = "Yaraq"
                        }
                    );
                }

                return _tcVendorMap;
            }
        }

        public static bool IsTownControlVendor(uint weenieId)
        {
            return TownControlVendors.TownControlVendorMap.ContainsKey(weenieId);
        }
    }

    public class TownControlVendor
    {
        public uint WeenieID { get; set; }

        public uint TownID { get; set; }

        public string TownName { get; set; }
    }    
}
