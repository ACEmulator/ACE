using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Factories.Enum;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class WeaponWcids
    {
        public static WeenieClassName Roll(TreasureDeath treasureDeath, TreasureItemType_Orig weaponType)
        {
            switch (weaponType)
            {
                case TreasureItemType_Orig.SwordWeapon:
                    return RollSwordWcid(treasureDeath);

                case TreasureItemType_Orig.MaceWeapon:
                    return RollMaceWcid(treasureDeath);

                case TreasureItemType_Orig.AxeWeapon:
                    return RollAxeWcid(treasureDeath);

                case TreasureItemType_Orig.SpearWeapon:
                    return RollSpearWcid(treasureDeath);

                case TreasureItemType_Orig.UnarmedWeapon:
                    return RollUnarmedWcid(treasureDeath);

                case TreasureItemType_Orig.StaffWeapon:
                    return RollStaffWcid(treasureDeath);

                case TreasureItemType_Orig.DaggerWeapon:
                    return RollDaggerWcid(treasureDeath);

                case TreasureItemType_Orig.BowWeapon:
                    return RollBowWcid(treasureDeath);

                case TreasureItemType_Orig.CrossbowWeapon:
                    return RollCrossbowWcid(treasureDeath);

                case TreasureItemType_Orig.AtlatlWeapon:
                    return RollAtlatlWcid(treasureDeath);
            }
            return WeenieClassName.undef;
        }

        public static HeritageGroup RollHeritage(TreasureDeath treasureDeath)
        {
            return HeritageChance.Roll(treasureDeath.UnknownChances);
        }

        public static WeenieClassName RollSwordWcid(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            switch (heritage)
            {
                case HeritageGroup.Aluvian:
                    return SwordWcids_Aluvian.Roll(treasureDeath.Tier);

                case HeritageGroup.Gharundim:
                    return SwordWcids_Gharundim.Roll(treasureDeath.Tier);

                case HeritageGroup.Sho:
                    return SwordWcids_Sho.Roll(treasureDeath.Tier);
            }
            return WeenieClassName.undef;
        }

        public static WeenieClassName RollMaceWcid(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            return MaceWcids.Roll(heritage);
        }

        public static WeenieClassName RollAxeWcid(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            return AxeWcids.Roll(heritage);
        }

        public static WeenieClassName RollSpearWcid(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            return SpearWcids.Roll(heritage);
        }

        public static WeenieClassName RollUnarmedWcid(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            return UnarmedWcids.Roll(heritage);
        }

        public static WeenieClassName RollStaffWcid(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            return StaffWcids.Roll(heritage);
        }

        public static WeenieClassName RollDaggerWcid(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            switch (heritage)
            {
                case HeritageGroup.Aluvian:
                case HeritageGroup.Sho:
                    return DaggerWcids_Aluvian_Sho.Roll(treasureDeath.Tier);

                case HeritageGroup.Gharundim:
                    return DaggerWcids_Gharundim.Roll(treasureDeath.Tier);
            }
            return WeenieClassName.undef;
        }

        public static WeenieClassName RollBowWcid(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            switch (heritage)
            {
                case HeritageGroup.Aluvian:
                    return BowWcids_Aluvian.Roll(treasureDeath.Tier);

                case HeritageGroup.Gharundim:
                    return BowWcids_Gharundim.Roll(treasureDeath.Tier);

                case HeritageGroup.Sho:
                    return BowWcids_Sho.Roll(treasureDeath.Tier);
            }
            return WeenieClassName.undef;
        }

        public static WeenieClassName RollCrossbowWcid(TreasureDeath treasureDeath)
        {
            return CrossbowWcids.Roll(treasureDeath.Tier);
        }

        public static WeenieClassName RollAtlatlWcid(TreasureDeath treasureDeath)
        {
            return AtlatlWcids.Roll(treasureDeath.Tier);
        }
    }
}
