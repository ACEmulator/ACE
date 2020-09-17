using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class WeaponTypeChance
    {
        private static readonly ChanceTable<TreasureWeaponType> T1_T4_Chances = new ChanceTable<TreasureWeaponType>()
        {
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

        private static readonly ChanceTable<TreasureWeaponType> T5_T6_Chances = new ChanceTable<TreasureWeaponType>()
        {
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
            tier = Math.Clamp(tier, 1, 6);

            return weaponTiers[tier - 1].Roll();
        }
    }
}
