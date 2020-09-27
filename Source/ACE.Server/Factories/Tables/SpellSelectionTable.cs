using System.Collections.Generic;

using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    /// <summary>
    /// Defines which spells can be found on item types
    /// </summary>
    public static class SpellSelectionTable
    {
        // gems
        private static readonly ChanceTable<SpellId> spellSelectionGroup1 = new ChanceTable<SpellId>()
        {
            ( SpellId.StrengthSelf1,            0.06f ),
            ( SpellId.EnduranceSelf1,           0.06f ),
            ( SpellId.CoordinationSelf1,        0.06f ),
            ( SpellId.QuicknessSelf1,           0.06f ),
            ( SpellId.FocusSelf1,               0.06f ),
            ( SpellId.WillpowerSelf1,           0.06f ),
            ( SpellId.RegenerationSelf1,        0.11f ),
            ( SpellId.RejuvenationSelf1,        0.11f ),
            ( SpellId.ManaRenewalSelf1,         0.11f ),
            ( SpellId.AcidProtectionSelf1,      0.03f ),
            ( SpellId.BludgeonProtectionSelf1,  0.03f ),
            ( SpellId.ColdProtectionSelf1,      0.03f ),
            ( SpellId.LightningProtectionSelf1, 0.03f ),
            ( SpellId.FireProtectionSelf1,      0.03f ),
            ( SpellId.BladeProtectionSelf1,     0.03f ),
            ( SpellId.PiercingProtectionSelf1,  0.03f ),
            ( SpellId.ArmorSelf1,               0.10f ),
        };

        // jewelry
        private static readonly ChanceTable<SpellId> spellSelectionGroup2 = new ChanceTable<SpellId>()
        {
            ( SpellId.StrengthSelf1,            0.04f ),
            ( SpellId.EnduranceSelf1,           0.04f ),
            ( SpellId.CoordinationSelf1,        0.04f ),
            ( SpellId.QuicknessSelf1,           0.04f ),
            ( SpellId.FocusSelf1,               0.04f ),
            ( SpellId.WillpowerSelf1,           0.04f ),
            ( SpellId.MagicResistanceSelf1,     0.09f ),
            ( SpellId.ManaMasterySelf1,         0.03f ),
            ( SpellId.ArcaneEnlightenmentSelf1, 0.02f ),
            ( SpellId.ArmorExpertiseSelf1,      0.01f ),
            ( SpellId.ItemExpertiseSelf1,       0.01f ),
            ( SpellId.MagicItemExpertiseSelf1,  0.01f ),
            ( SpellId.WeaponExpertiseSelf1,     0.01f ),
            ( SpellId.MonsterAttunementSelf1,   0.01f ),
            ( SpellId.PersonAttunementSelf1,    0.01f ),
            ( SpellId.DeceptionMasterySelf1,    0.01f ),
            ( SpellId.FealtySelf1,              0.01f ),
            ( SpellId.RegenerationSelf1,        0.03f ),
            ( SpellId.RejuvenationSelf1,        0.03f ),
            ( SpellId.ManaRenewalSelf1,         0.03f ),
            ( SpellId.AcidProtectionSelf1,      0.05f ),
            ( SpellId.BludgeonProtectionSelf1,  0.05f ),
            ( SpellId.ColdProtectionSelf1,      0.05f ),
            ( SpellId.LightningProtectionSelf1, 0.05f ),
            ( SpellId.FireProtectionSelf1,      0.05f ),
            ( SpellId.BladeProtectionSelf1,     0.05f ),
            ( SpellId.PiercingProtectionSelf1,  0.05f ),
            ( SpellId.ArmorSelf1,               0.10f ),
        };

        // crowns
        private static readonly ChanceTable<SpellId> spellSelectionGroup3 = new ChanceTable<SpellId>()
        {
            ( SpellId.FocusSelf1,               0.12f ),
            ( SpellId.MagicResistanceSelf1,     0.12f ),
            ( SpellId.ManaMasterySelf1,         0.03f ),
            ( SpellId.ArcaneEnlightenmentSelf1, 0.03f ),
            ( SpellId.PersonAttunementSelf1,    0.05f ),
            ( SpellId.LeadershipMasterySelf1,   0.17f ),
            ( SpellId.RegenerationSelf1,        0.12f ),
            ( SpellId.RejuvenationSelf1,        0.12f ),
            ( SpellId.ManaRenewalSelf1,         0.12f ),
            ( SpellId.ArmorSelf1,               0.12f ),
        };

        // orbs, casters
        private static readonly ChanceTable<SpellId> spellSelectionGroup4 = new ChanceTable<SpellId>()
        {
            ( SpellId.CreatureEnchantmentMasterySelf1, 0.20f ),
            ( SpellId.ItemEnchantmentMasterySelf1,     0.20f ),
            ( SpellId.LifeMagicMasterySelf1,           0.25f ),
            ( SpellId.WarMagicMasterySelf1,            0.15f ),
            ( SpellId.ArcaneEnlightenmentSelf1,        0.10f ),
            ( SpellId.ManaMasterySelf1,                0.10f ),
        };

        // wands, staffs, sceptres, batons
        private static readonly ChanceTable<SpellId> spellSelectionGroup5 = new ChanceTable<SpellId>()
        {
            ( SpellId.CreatureEnchantmentMasterySelf1, 0.15f ),
            ( SpellId.ItemEnchantmentMasterySelf1,     0.15f ),
            ( SpellId.LifeMagicMasterySelf1,           0.20f ),
            ( SpellId.WarMagicMasterySelf1,            0.30f ),
            ( SpellId.ArcaneEnlightenmentSelf1,        0.10f ),
            ( SpellId.ManaMasterySelf1,                0.10f ),
        };

        // melee and missile weapons
        private static readonly ChanceTable<SpellId> spellSelectionGroup6 = new ChanceTable<SpellId>()
        {
            ( SpellId.StrengthSelf1,     0.25f ),
            ( SpellId.EnduranceSelf1,    0.25f ),
            ( SpellId.CoordinationSelf1, 0.25f ),
            ( SpellId.QuicknessSelf1,    0.25f ),
        };

        // bracers, breastplates, coats, cuirasses, girths, hauberks, pauldrons, chest armor, sleeves
        private static readonly ChanceTable<SpellId> spellSelectionGroup7 = new ChanceTable<SpellId>()
        {
            ( SpellId.StrengthSelf1,        0.30f ),
            ( SpellId.EnduranceSelf1,       0.30f ),
            ( SpellId.MagicResistanceSelf1, 0.15f ),
            ( SpellId.FealtySelf1,          0.05f ),
            ( SpellId.RejuvenationSelf1,    0.10f ),
            ( SpellId.RegenerationSelf1,    0.10f ),
        };

        // shields
        private static readonly ChanceTable<SpellId> spellSelectionGroup8 = new ChanceTable<SpellId>()
        {
            ( SpellId.StrengthSelf1,        0.10f ),
            ( SpellId.EnduranceSelf1,       0.10f ),
            ( SpellId.ImpregnabilitySelf1,  0.20f ),
            ( SpellId.InvulnerabilitySelf1, 0.20f ),
            ( SpellId.FealtySelf1,          0.15f ),
            ( SpellId.RejuvenationSelf1,    0.15f ),
            ( SpellId.MagicResistanceSelf1, 0.10f ),
        };

        // gauntlets, sollerets, sandals, boots
        private static readonly ChanceTable<SpellId> spellSelectionGroup9 = new ChanceTable<SpellId>()
        {
            ( SpellId.CoordinationSelf1,          0.16f ),
            ( SpellId.LightWeaponsMasterySelf1,   0.10f ),
            ( SpellId.FinesseWeaponsMasterySelf1, 0.08f ),
            ( SpellId.LightWeaponsMasterySelf1,   0.10f ),
            ( SpellId.LightWeaponsMasterySelf1,   0.10f ),
            ( SpellId.LightWeaponsMasterySelf1,   0.08f ),
            ( SpellId.HeavyWeaponsMasterySelf1,   0.10f ),
            ( SpellId.LightWeaponsMasterySelf1,   0.08f ),
            ( SpellId.MissileWeaponsMasterySelf1, 0.05f ),
            ( SpellId.MissileWeaponsMasterySelf1, 0.05f ),
            ( SpellId.ThrownWeaponMasterySelf1,   0.05f ),
            ( SpellId.HealingMasterySelf1,        0.05f ),
        };

        // helms, basinets, helmets, coifs, cowls, heaumes, kabutons
        private static readonly ChanceTable<SpellId> spellSelectionGroup10 = new ChanceTable<SpellId>()
        {
            ( SpellId.MagicResistanceSelf1,   0.30f ),
            ( SpellId.ArmorExpertiseSelf1,    0.10f ),
            ( SpellId.WeaponExpertiseSelf1,   0.10f ),
            ( SpellId.MonsterAttunementSelf1, 0.10f ),
            ( SpellId.FealtySelf1,            0.10f ),
            ( SpellId.RejuvenationSelf1,      0.10f ),
            ( SpellId.ManaRenewalSelf1,       0.10f ),
            ( SpellId.HealingMasterySelf1,    0.10f ),
        };

        // boots, sandals, shoes, slippers, sollerets
        private static readonly ChanceTable<SpellId> spellSelectionGroup11 = new ChanceTable<SpellId>()
        {
            ( SpellId.QuicknessSelf1,             0.15f ),
            ( SpellId.JumpingMasterySelf1,        0.06f ),
            ( SpellId.SprintSelf1,                0.06f ),
            ( SpellId.CoordinationSelf1,          0.15f ),
            ( SpellId.LightWeaponsMasterySelf1,   0.06f ),
            ( SpellId.FinesseWeaponsMasterySelf1, 0.04f ),
            ( SpellId.LightWeaponsMasterySelf1,   0.06f ),
            ( SpellId.LightWeaponsMasterySelf1,   0.06f ),
            ( SpellId.LightWeaponsMasterySelf1,   0.04f ),
            ( SpellId.HeavyWeaponsMasterySelf1,   0.08f ),
            ( SpellId.LightWeaponsMasterySelf1,   0.04f ),
            ( SpellId.MissileWeaponsMasterySelf1, 0.08f ),
            ( SpellId.MissileWeaponsMasterySelf1, 0.06f ),
            ( SpellId.ThrownWeaponMasterySelf1,   0.06f ),
        };

        // breeches, jerkins, shirts, pants, tunics, doublets, trousers, pantaloons
        private static readonly ChanceTable<SpellId> spellSelectionGroup12 = new ChanceTable<SpellId>()
        {
            ( SpellId.AcidProtectionSelf1,      0.10f ),
            ( SpellId.BludgeonProtectionSelf1,  0.10f ),
            ( SpellId.ColdProtectionSelf1,      0.10f ),
            ( SpellId.LightningProtectionSelf1, 0.10f ),
            ( SpellId.FireProtectionSelf1,      0.10f ),
            ( SpellId.BladeProtectionSelf1,     0.10f ),
            ( SpellId.PiercingProtectionSelf1,  0.10f ),
            ( SpellId.ArmorSelf1,               0.30f ),
        };

        // caps, qafiyas, turbans, fezs, berets
        private static readonly ChanceTable<SpellId> spellSelectionGroup13 = new ChanceTable<SpellId>()
        {
            ( SpellId.FocusSelf1,               0.20f ),
            ( SpellId.MagicResistanceSelf1,     0.10f ),
            ( SpellId.ArcaneEnlightenmentSelf1, 0.05f ),
            ( SpellId.ArmorExpertiseSelf1,      0.05f ),
            ( SpellId.ItemExpertiseSelf1,       0.05f ),
            ( SpellId.MagicItemExpertiseSelf1,  0.05f ),
            ( SpellId.WeaponExpertiseSelf1,     0.05f ),
            ( SpellId.MonsterAttunementSelf1,   0.05f ),
            ( SpellId.PersonAttunementSelf1,    0.05f ),
            ( SpellId.DeceptionMasterySelf1,    0.10f ),
            ( SpellId.ManaRenewalSelf1,         0.10f ),
            ( SpellId.AlchemyMasterySelf1,      0.05f ),
            ( SpellId.FletchingMasterySelf1,    0.05f ),
            ( SpellId.CookingMasterySelf1,      0.05f ),
        };

        // cloth gloves (1 entry?)
        private static readonly ChanceTable<SpellId> spellSelectionGroup14 = new ChanceTable<SpellId>()
        {
            ( SpellId.CoordinationSelf1,       0.10f ),
            ( SpellId.HealingMasterySelf1,     0.10f ),
            ( SpellId.LockpickMasterySelf1,    0.10f ),
            ( SpellId.ArmorExpertiseSelf1,     0.10f ),
            ( SpellId.ItemExpertiseSelf1,      0.10f ),
            ( SpellId.MagicItemExpertiseSelf1, 0.10f ),
            ( SpellId.WeaponExpertiseSelf1,    0.10f ),
            ( SpellId.AlchemyMasterySelf1,     0.10f ),
            ( SpellId.FletchingMasterySelf1,   0.10f ),
            ( SpellId.CookingMasterySelf1,     0.10f ),
        };

        // greaves, leggings, tassets, leather pants
        private static readonly ChanceTable<SpellId> spellSelectionGroup15 = new ChanceTable<SpellId>()
        {
            ( SpellId.QuicknessSelf1,      0.30f ),
            ( SpellId.JumpingMasterySelf1, 0.15f ),
            ( SpellId.SprintSelf1,         0.15f ),
            ( SpellId.StrengthSelf1,       0.30f ),
            ( SpellId.EnduranceSelf1,      0.10f ),
        };

        // dinnerware
        private static readonly ChanceTable<SpellId> spellSelectionGroup16 = new ChanceTable<SpellId>()
        {
            ( SpellId.StrengthSelf1,           0.03f ),
            ( SpellId.EnduranceSelf1,          0.03f ),
            ( SpellId.CoordinationSelf1,       0.04f ),
            ( SpellId.QuicknessSelf1,          0.03f ),
            ( SpellId.FocusSelf1,              0.04f ),
            ( SpellId.WillpowerSelf1,          0.03f ),
            ( SpellId.ArmorExpertiseSelf1,     0.10f ),
            ( SpellId.ItemExpertiseSelf1,      0.10f ),
            ( SpellId.MagicItemExpertiseSelf1, 0.10f ),
            ( SpellId.WeaponExpertiseSelf1,    0.10f ),
            ( SpellId.AlchemyMasterySelf1,     0.10f ),
            ( SpellId.FletchingMasterySelf1,   0.10f ),
            ( SpellId.CookingMasterySelf1,     0.10f ),
            ( SpellId.LockpickMasterySelf1,    0.10f ),
        };

        /// <summary>
        /// Key is (PropertyInt.TsysMutationData >> 24) - 1
        /// </summary>
        private static readonly List<ChanceTable<SpellId>> spellSelectionGroup = new List<ChanceTable<SpellId>>()
        {
            spellSelectionGroup1,
            spellSelectionGroup2,
            spellSelectionGroup3,
            spellSelectionGroup4,
            spellSelectionGroup5,
            spellSelectionGroup6,
            spellSelectionGroup7,
            spellSelectionGroup8,
            spellSelectionGroup9,
            spellSelectionGroup10,
            spellSelectionGroup11,
            spellSelectionGroup12,
            spellSelectionGroup13,
            spellSelectionGroup14,
            spellSelectionGroup15,
            spellSelectionGroup16,
        };

        /// <summary>
        /// Rolls for a creature / life spell for an item
        /// </summary>
        /// <param name="spellCode">the SpellCode from WorldObject</param>
        public static SpellId Roll(int spellCode)
        {
            return spellSelectionGroup[spellCode - 1].Roll();
        }
    }
}
