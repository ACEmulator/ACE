using System;
using ACE.Database.Models.World;
using ACE.Entity.Enum;

namespace ACE.Database.Entity
{
    public class HouseListResults
    {
        public Weenie Weenie;
        public LandblockInstance LandblockInstance;

        public HouseType HouseType;

        public HouseListResults(Weenie weenie, LandblockInstance landblockInstance)
        {
            Weenie = weenie;
            LandblockInstance = landblockInstance;

            HouseType = GetHouseType(Weenie.ClassName);
        }

        public static HouseType GetHouseType(string classname)
        {
            if (classname.IndexOf("apartment", StringComparison.OrdinalIgnoreCase) != -1)
                return HouseType.Apartment;
            else if (classname.IndexOf("cottage", StringComparison.OrdinalIgnoreCase) != -1)
                return HouseType.Cottage;
            else if (classname.IndexOf("villa", StringComparison.OrdinalIgnoreCase) != -1)
                return HouseType.Villa;
            else if (classname.IndexOf("mansion", StringComparison.OrdinalIgnoreCase) != -1)
                return HouseType.Mansion;
            else
                return HouseType.Undef;
        }
    }
}
