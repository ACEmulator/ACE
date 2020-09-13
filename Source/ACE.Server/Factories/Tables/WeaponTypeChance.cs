using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class WeaponTypeChance
    {
        private static readonly ChanceTable<TreasureItemType_Orig> T1_T4_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SwordWeapon,    0.12f ),
            ( TreasureItemType_Orig.MaceWeapon,     0.12f ),
            ( TreasureItemType_Orig.AxeWeapon,      0.12f ),
            ( TreasureItemType_Orig.SpearWeapon,    0.12f ),
            ( TreasureItemType_Orig.UnarmedWeapon,  0.12f ),
            ( TreasureItemType_Orig.StaffWeapon,    0.12f ),
            ( TreasureItemType_Orig.DaggerWeapon,   0.12f ),
            ( TreasureItemType_Orig.BowWeapon,      0.04f ),
            ( TreasureItemType_Orig.CrossbowWeapon, 0.04f ),
            ( TreasureItemType_Orig.AtlatlWeapon,   0.04f ),
            ( TreasureItemType_Orig.Caster,         0.04f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T5_T6_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SwordWeapon,    0.09f ),
            ( TreasureItemType_Orig.MaceWeapon,     0.09f ),
            ( TreasureItemType_Orig.AxeWeapon,      0.09f ),
            ( TreasureItemType_Orig.SpearWeapon,    0.09f ),
            ( TreasureItemType_Orig.UnarmedWeapon,  0.09f ),
            ( TreasureItemType_Orig.StaffWeapon,    0.09f ),
            ( TreasureItemType_Orig.DaggerWeapon,   0.09f ),
            ( TreasureItemType_Orig.BowWeapon,      0.09f ),
            ( TreasureItemType_Orig.CrossbowWeapon, 0.09f ),
            ( TreasureItemType_Orig.AtlatlWeapon,   0.09f ),
            ( TreasureItemType_Orig.Caster,         0.10f ),
        };

        private static readonly List<ChanceTable<TreasureItemType_Orig>> weaponTiers = new List<ChanceTable<TreasureItemType_Orig>>()
        {
            T1_T4_Chances,
            T1_T4_Chances,
            T1_T4_Chances,
            T1_T4_Chances,
            T5_T6_Chances,
            T5_T6_Chances,
        };

        public static TreasureItemType_Orig Roll(int tier)
        {
            // todo: add unique profiles for t7 / t8?
            tier = Math.Clamp(tier, 1, 6);

            return weaponTiers[tier - 1].Roll();
        }
    }
}
