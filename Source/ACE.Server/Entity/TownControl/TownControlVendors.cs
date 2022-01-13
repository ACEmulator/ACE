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
                        4200001,
                        new TownControlVendor()
                        {
                            WeenieID = 4200001,
                            TownID = 72,
                            TownName = "Holtburg"
                        }
                    );


                    //Yaraq
                    _tcVendorMap.Add(
                        4200003, //TODO
                        new TownControlVendor()
                        {
                            WeenieID = 4200003, //TODO
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
