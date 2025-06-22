using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class WeaponTypeChance
    {
        private static ChanceTable<TreasureWeaponType> T1_T4_Chances = new ChanceTable<TreasureWeaponType>()
        {
            // melee: 84%
            // missile: 12%
            // caster: 4%
            ( TreasureWeaponType.Sword,    0.12f ),
            ( TreasureWeaponType.Mace,     0.12f ),
            ( TreasureWeaponType.Axe,      0.12f ),
            ( TreasureWeaponType.Spear,    0.12f ),
            ( TreasureWeaponType.Unarmed,  0.12f ),
            ( TreasureWeaponType.Staff,    0.12f ),
            ( TreasureWeaponType.Dagger,   0.12f ),
            ( TreasureWeaponType.Bow,      0.04f ),
            ( TreasureWeaponType.Crossbow, 0.04f ),
            ( TreasureWeaponType.Atlatl,   0.04f ),
            ( TreasureWeaponType.Caster,   0.04f ),
        };

        private static ChanceTable<TreasureWeaponType> T5_T6_Chances = new ChanceTable<TreasureWeaponType>()
        {
            // melee: 63%
            // missile: 27%
            // caster: 10%
            ( TreasureWeaponType.Sword,    0.09f ),
            ( TreasureWeaponType.Mace,     0.09f ),
            ( TreasureWeaponType.Axe,      0.09f ),
            ( TreasureWeaponType.Spear,    0.09f ),
            ( TreasureWeaponType.Unarmed,  0.09f ),
            ( TreasureWeaponType.Staff,    0.09f ),
            ( TreasureWeaponType.Dagger,   0.09f ),
            ( TreasureWeaponType.Bow,      0.09f ),
            ( TreasureWeaponType.Crossbow, 0.09f ),
            ( TreasureWeaponType.Atlatl,   0.09f ),
            ( TreasureWeaponType.Caster,   0.10f ),
        };

        // from magloot corpse logs
        // it appears they might have gotten rid of the tier chances
        private static ChanceTable<TreasureWeaponType> RetailChances = new ChanceTable<TreasureWeaponType>()
        {
            // melee: 63%
            // missile: 20%
            // two-handed: 10%
            // caster: 7%
            ( TreasureWeaponType.Sword,           0.09f ),
            ( TreasureWeaponType.Mace,            0.09f ),
            ( TreasureWeaponType.Axe,             0.09f ),
            ( TreasureWeaponType.Spear,           0.09f ),
            ( TreasureWeaponType.Unarmed,         0.09f ),
            ( TreasureWeaponType.Staff,           0.09f ),
            ( TreasureWeaponType.Dagger,          0.09f ),
            ( TreasureWeaponType.Bow,             0.07f ),
            ( TreasureWeaponType.Crossbow,        0.07f ),
            ( TreasureWeaponType.Atlatl,          0.06f ),
            ( TreasureWeaponType.Caster,          0.07f ),
            ( TreasureWeaponType.TwoHandedWeapon, 0.10f ),      // see TreasureWeaponType for an explanation of why this is here,
                                                                // and not deeper in WeaponWcids.cs
        };

        /// <summary>
        /// Even chance for all melee weapon types
        /// Only used for test methods
        /// </summary>
        public static ChanceTable<TreasureWeaponType> MeleeChances = new ChanceTable<TreasureWeaponType>()
        {
            ( TreasureWeaponType.Sword,           0.125f ),
            ( TreasureWeaponType.Mace,            0.125f ),
            ( TreasureWeaponType.Axe,             0.125f ),
            ( TreasureWeaponType.Spear,           0.125f ),
            ( TreasureWeaponType.Unarmed,         0.125f ),
            ( TreasureWeaponType.Staff,           0.125f ),
            ( TreasureWeaponType.Dagger,          0.125f ),
            ( TreasureWeaponType.TwoHandedWeapon, 0.125f ),
        };

        /// <summary>
        /// Roughly even chance for all missile weapon types
        /// Only used for test methods
        /// </summary>
        public static ChanceTable<TreasureWeaponType> MissileChances = new ChanceTable<TreasureWeaponType>()
        {
            ( TreasureWeaponType.Bow,             0.334f ),
            ( TreasureWeaponType.Crossbow,        0.333f ),
            ( TreasureWeaponType.Atlatl,          0.333f ),
        };

        private static readonly List<ChanceTable<TreasureWeaponType>> weaponTiers = new List<ChanceTable<TreasureWeaponType>>()
        {
            T1_T4_Chances,
            T1_T4_Chances,
            T1_T4_Chances,
            T1_T4_Chances,
            T5_T6_Chances,
            T5_T6_Chances,
        };

        public static TreasureWeaponType Roll(int tier)
        {
            // todo: add unique profiles for t7 / t8?
            //tier = Math.Clamp(tier, 1, 6);

            //return weaponTiers[tier - 1].Roll();

            return RetailChances.Roll();
        }
    }
}
