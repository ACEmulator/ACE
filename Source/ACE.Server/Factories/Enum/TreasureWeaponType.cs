namespace ACE.Server.Factories.Enum
{
    public enum TreasureWeaponType
    {
        // even after MoA, retail appeared to retain the initial roll for melee weapons based on WeaponType
        MeleeWeapon,
        Sword,
        Mace,
        Axe,
        Spear,
        Unarmed,
        Staff,
        Dagger,

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
    }
}
