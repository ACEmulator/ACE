using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Factories.Enum;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class WeaponWcids
    {
        public static WeenieClassName Roll(TreasureDeath treasureDeath, ref TreasureWeaponType weaponType)
        {
            switch (weaponType)
            {
                /*case TreasureWeaponType.Sword:
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
                    return RollDaggerWcid(treasureDeath);*/

                case TreasureWeaponType.Axe:
                case TreasureWeaponType.Dagger:
                case TreasureWeaponType.Mace:
                case TreasureWeaponType.Spear:
                case TreasureWeaponType.Staff:
                case TreasureWeaponType.Sword:
                case TreasureWeaponType.Unarmed:
                    return RollMeleeWeapon(ref weaponType);

                case TreasureWeaponType.Bow:
                    return RollBowWcid(treasureDeath);

                case TreasureWeaponType.Crossbow:
                    return RollCrossbowWcid(treasureDeath);

                case TreasureWeaponType.Atlatl:
                    return RollAtlatlWcid(treasureDeath);

                case TreasureWeaponType.Caster:
                    return RollCaster(treasureDeath);

                case TreasureWeaponType.TwoHandedWeapon:
                    return RollTwoHandedWeaponWcid(ref weaponType);
            }
            return WeenieClassName.undef;
        }

        public static WeenieClassName RollMeleeWeapon(ref TreasureWeaponType weaponType)
        {
            // retail did something silly here --
            // instead of having an even chance to roll between heavy/light/finesse,
            // they kept everything divied up by weaponType

            // doing something slightly less silly here...

            // basically throwing out the weaponType here,
            // rolling for an even chance between heavy/light/finesse,
            // and then an even chance into each weapon for that skill

            // if you wish to maintain a profile closer to retail overall,
            // heavy 36% | light 30% | finesse 34%

            // however, it still wouldn't match up exactly,
            // as each weaponType still had slightly different chances,
            // which were most likely engrained deeply in the per-weaponType chance tables

            var weaponSkill = (MeleeWeaponSkill)ThreadSafeRandom.Next(1, 3);

            switch (weaponSkill)
            {
                case MeleeWeaponSkill.HeavyWeapons:
                    return HeavyWeaponWcids.Roll(out weaponType);

                case MeleeWeaponSkill.LightWeapons:
                    return LightWeaponWcids.Roll(out weaponType);

                case MeleeWeaponSkill.FinesseWeapons:
                    return FinesseWeaponWcids.Roll(out weaponType);
            }
            return WeenieClassName.undef;
        }

        public static TreasureHeritageGroup RollHeritage(TreasureDeath treasureDeath)
        {
            return HeritageChance.Roll(treasureDeath.UnknownChances);
        }

        public static WeenieClassName RollSwordWcid(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            switch (heritage)
            {
                case TreasureHeritageGroup.Aluvian:
                    return SwordWcids_Aluvian.Roll(treasureDeath.Tier);

                case TreasureHeritageGroup.Gharundim:
                    return SwordWcids_Gharundim.Roll(treasureDeath.Tier);

                case TreasureHeritageGroup.Sho:
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
                case TreasureHeritageGroup.Aluvian:
                case TreasureHeritageGroup.Sho:
                    return DaggerWcids_Aluvian_Sho.Roll(treasureDeath.Tier);

                case TreasureHeritageGroup.Gharundim:
                    return DaggerWcids_Gharundim.Roll(treasureDeath.Tier);
            }
            return WeenieClassName.undef;
        }

        public static WeenieClassName RollBowWcid(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            switch (heritage)
            {
                case TreasureHeritageGroup.Aluvian:
                    return BowWcids_Aluvian.Roll(treasureDeath.Tier);

                case TreasureHeritageGroup.Gharundim:
                    return BowWcids_Gharundim.Roll(treasureDeath.Tier);

                case TreasureHeritageGroup.Sho:
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

        public static WeenieClassName RollTwoHandedWeaponWcid(ref TreasureWeaponType weaponType)
        {
            return TwoHandedWeaponWcids.Roll(out weaponType);
        }
    }
}
