using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Factories.Enum;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class WeaponWcids
    {
        public static WeenieClassName Roll(TreasureDeath treasureDeath, TreasureWeaponType weaponType)
        {
            switch (weaponType)
            {
                case TreasureWeaponType.Sword:
                    return RollSwordWcid(treasureDeath);

                case TreasureWeaponType.Mace:
                    return RollMaceWcid(treasureDeath);

                case TreasureWeaponType.Axe:
                    return RollAxeWcid(treasureDeath);

                case TreasureWeaponType.Spear:
                    return RollSpearWcid(treasureDeath);

                case TreasureWeaponType.Unarmed:
                    return RollUnarmedWcid(treasureDeath);

                case TreasureWeaponType.Staff:
                    return RollStaffWcid(treasureDeath);

                case TreasureWeaponType.Dagger:
                    return RollDaggerWcid(treasureDeath);

                case TreasureWeaponType.Bow:
                    return RollBowWcid(treasureDeath);

                case TreasureWeaponType.Crossbow:
                    return RollCrossbowWcid(treasureDeath);

                case TreasureWeaponType.Atlatl:
                    return RollAtlatlWcid(treasureDeath);

                case TreasureWeaponType.Caster:
                    return RollCaster(treasureDeath);
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

        public static WeenieClassName RollCaster(TreasureDeath treasureDeath)
        {
            return CasterWcids.Roll(treasureDeath.Tier);
        }
    }
}
