namespace ACE.Server.Factories.Enum
{
    public enum TreasureWeaponType
    {
        Undef,

        // even after MoA, retail appeared to retain the initial roll for melee weapons based on WeaponType
        MeleeWeapon,
        Axe,
        Dagger,
        DaggerMS,
        Mace,
        MaceJitte,
        Spear,
        Staff,
        Sword,
        SwordMS,
        Unarmed,

        MissileWeapon,
        Bow,
        Crossbow,
        Atlatl,

        Caster,

        // this could also be placed in WeaponWcids, however the roll for TwoHandedWeapons appears to have worked differently from Heavy/Light/Finesse
        // for Heavy/Light/Finesse, there did not appear to be any kind of pre-roll that divied up weapons evenly between these types
        // overall, the chances appeared to be about 36/30/34, however for a particular weapon type (which are definitely even in retail logs), these ratios vary,
        // with little discernable pattern (ie. not by total # of weapons for that type), other than simply entering different values in the inner chance tables
        // two-handed however, is much more consistent: each of the different two-handed weapons appeared to have an even chance
        // because of the differences between heavy/light/finesse and two-handed, placing this here makes more sense currently,
        // as the weapon type would essentially still get discarded, even if it was in WeaponWcids

        TwoHandedWeapon,

        TwoHandedAxe,
        TwoHandedMace,
        TwoHandedSpear,
        TwoHandedSword,
    }

    public static class TreasureWeaponTypeExtensions
    {
        public static string GetScriptName(this TreasureWeaponType weaponType)
        {
            switch (weaponType)
            {
                case TreasureWeaponType.Axe:
                case TreasureWeaponType.TwoHandedAxe:
                    return "axe";
                case TreasureWeaponType.Dagger:
                    return "dagger";
                case TreasureWeaponType.DaggerMS:
                    return "dagger_ms";
                case TreasureWeaponType.Mace:
                case TreasureWeaponType.TwoHandedMace:
                    return "mace";
                case TreasureWeaponType.MaceJitte:
                    return "mace_jitte";
                case TreasureWeaponType.Spear:
                case TreasureWeaponType.TwoHandedSpear:
                    return "spear";
                case TreasureWeaponType.Staff:
                    return "staff";
                case TreasureWeaponType.Sword:
                case TreasureWeaponType.TwoHandedSword:
                    return "sword";
                case TreasureWeaponType.SwordMS:
                    return "sword_ms";
                case TreasureWeaponType.Unarmed:
                    return "unarmed";
            }
            return null;
        }

        public static string GetScriptShortName(this TreasureWeaponType weaponType)
        {
            switch (weaponType)
            {
                case TreasureWeaponType.Axe:
                    return "axe";
                case TreasureWeaponType.Dagger:
                case TreasureWeaponType.DaggerMS:
                    return "dagger";
                case TreasureWeaponType.Mace:
                    return "mace";
                case TreasureWeaponType.MaceJitte:
                    return "mace_jitte";
                case TreasureWeaponType.Spear:
                    return "spear";
                case TreasureWeaponType.Staff:
                    return "staff";
                case TreasureWeaponType.Sword:
                case TreasureWeaponType.SwordMS:
                    return "sword";
                case TreasureWeaponType.Unarmed:
                    return "unarmed";

                case TreasureWeaponType.TwoHandedAxe:
                    return "two_handed_axe";
                case TreasureWeaponType.TwoHandedMace:
                    return "two_handed_mace";
                case TreasureWeaponType.TwoHandedSpear:
                    return "two_handed_spear";
                case TreasureWeaponType.TwoHandedSword:
                    return "two_handed_sword";
            }
            return null;
        }
    }
}
