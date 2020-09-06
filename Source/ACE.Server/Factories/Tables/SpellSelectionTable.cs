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
        private static readonly List<SpellChance> spellSelectionGroup1 = new List<SpellChance>()
        {
            new SpellChance(SpellId.StrengthSelf1, 0.06f),
            new SpellChance(SpellId.EnduranceSelf1, 0.06f),
            new SpellChance(SpellId.CoordinationSelf1, 0.06f),
            new SpellChance(SpellId.QuicknessSelf1, 0.06f),
            new SpellChance(SpellId.FocusSelf1, 0.06f),
            new SpellChance(SpellId.WillpowerSelf1, 0.06f),
            new SpellChance(SpellId.RegenerationSelf1, 0.11f),
            new SpellChance(SpellId.RejuvenationSelf1, 0.11f),
            new SpellChance(SpellId.ManaRenewalSelf1, 0.11f),
            new SpellChance(SpellId.AcidProtectionSelf1, 0.03f),
            new SpellChance(SpellId.BludgeonProtectionSelf1, 0.03f),
            new SpellChance(SpellId.ColdProtectionSelf1, 0.03f),
            new SpellChance(SpellId.LightningProtectionSelf1, 0.03f),
            new SpellChance(SpellId.FireProtectionSelf1, 0.03f),
            new SpellChance(SpellId.BladeProtectionSelf1, 0.03f),
            new SpellChance(SpellId.PiercingProtectionSelf1, 0.03f),
            new SpellChance(SpellId.ArmorSelf1, 0.1f),
        };

        // jewelry
        private static readonly List<SpellChance> spellSelectionGroup2 = new List<SpellChance>()
        {
            new SpellChance(SpellId.StrengthSelf1, 0.04f),
            new SpellChance(SpellId.EnduranceSelf1, 0.04f),
            new SpellChance(SpellId.CoordinationSelf1, 0.04f),
            new SpellChance(SpellId.QuicknessSelf1, 0.04f),
            new SpellChance(SpellId.FocusSelf1, 0.04f),
            new SpellChance(SpellId.WillpowerSelf1, 0.04f),
            new SpellChance(SpellId.MagicResistanceSelf1, 0.09f),
            new SpellChance(SpellId.ManaMasterySelf1, 0.03f),
            new SpellChance(SpellId.ArcaneEnlightenmentSelf1, 0.02f),
            new SpellChance(SpellId.ArmorExpertiseSelf1, 0.01f),
            new SpellChance(SpellId.ItemExpertiseSelf1, 0.01f),
            new SpellChance(SpellId.MagicItemExpertiseSelf1, 0.01f),
            new SpellChance(SpellId.WeaponExpertiseSelf1, 0.01f),
            new SpellChance(SpellId.MonsterAttunementSelf1, 0.01f),
            new SpellChance(SpellId.PersonAttunementSelf1, 0.01f),
            new SpellChance(SpellId.DeceptionMasterySelf1, 0.01f),
            new SpellChance(SpellId.FealtySelf1, 0.01f),
            new SpellChance(SpellId.RegenerationSelf1, 0.03f),
            new SpellChance(SpellId.RejuvenationSelf1, 0.03f),
            new SpellChance(SpellId.ManaRenewalSelf1, 0.03f),
            new SpellChance(SpellId.AcidProtectionSelf1, 0.05f),
            new SpellChance(SpellId.BludgeonProtectionSelf1, 0.05f),
            new SpellChance(SpellId.ColdProtectionSelf1, 0.05f),
            new SpellChance(SpellId.LightningProtectionSelf1, 0.05f),
            new SpellChance(SpellId.FireProtectionSelf1, 0.05f),
            new SpellChance(SpellId.BladeProtectionSelf1, 0.05f),
            new SpellChance(SpellId.PiercingProtectionSelf1, 0.05f),
            new SpellChance(SpellId.ArmorSelf1, 0.1f),
        };

        // crowns
        private static readonly List<SpellChance> spellSelectionGroup3 = new List<SpellChance>()
        {
            new SpellChance(SpellId.FocusSelf1, 0.12f),
            new SpellChance(SpellId.MagicResistanceSelf1, 0.12f),
            new SpellChance(SpellId.ManaMasterySelf1, 0.03f),
            new SpellChance(SpellId.ArcaneEnlightenmentSelf1, 0.03f),
            new SpellChance(SpellId.PersonAttunementSelf1, 0.05f),
            new SpellChance(SpellId.LeadershipMasterySelf1, 0.17f),
            new SpellChance(SpellId.RegenerationSelf1, 0.12f),
            new SpellChance(SpellId.RejuvenationSelf1, 0.12f),
            new SpellChance(SpellId.ManaRenewalSelf1, 0.12f),
            new SpellChance(SpellId.ArmorSelf1, 0.12f),
        };

        // orbs, casters
        private static readonly List<SpellChance> spellSelectionGroup4 = new List<SpellChance>()
        {
            new SpellChance(SpellId.CreatureEnchantmentMasterySelf1, 0.2f),
            new SpellChance(SpellId.ItemEnchantmentMasterySelf1, 0.2f),
            new SpellChance(SpellId.LifeMagicMasterySelf1, 0.25f),
            new SpellChance(SpellId.WarMagicMasterySelf1, 0.15f),
            new SpellChance(SpellId.ArcaneEnlightenmentSelf1, 0.1f),
            new SpellChance(SpellId.ManaMasterySelf1, 0.1f),
        };

        // wands, staffs, sceptres, batons
        private static readonly List<SpellChance> spellSelectionGroup5 = new List<SpellChance>()
        {
            new SpellChance(SpellId.CreatureEnchantmentMasterySelf1, 0.15f),
            new SpellChance(SpellId.ItemEnchantmentMasterySelf1, 0.15f),
            new SpellChance(SpellId.LifeMagicMasterySelf1, 0.2f),
            new SpellChance(SpellId.WarMagicMasterySelf1, 0.3f),
            new SpellChance(SpellId.ArcaneEnlightenmentSelf1, 0.1f),
            new SpellChance(SpellId.ManaMasterySelf1, 0.1f),
        };

        // melee and missile weapons
        private static readonly List<SpellChance> spellSelectionGroup6 = new List<SpellChance>()
        {
            new SpellChance(SpellId.StrengthSelf1, 0.25f),
            new SpellChance(SpellId.EnduranceSelf1, 0.25f),
            new SpellChance(SpellId.CoordinationSelf1, 0.25f),
            new SpellChance(SpellId.QuicknessSelf1, 0.25f),
        };

        // bracers, breastplates, coats, cuirasses, girths, hauberks, pauldrons, chest armor, sleeves
        private static readonly List<SpellChance> spellSelectionGroup7 = new List<SpellChance>()
        {
            new SpellChance(SpellId.StrengthSelf1, 0.3f),
            new SpellChance(SpellId.EnduranceSelf1, 0.3f),
            new SpellChance(SpellId.MagicResistanceSelf1, 0.15f),
            new SpellChance(SpellId.FealtySelf1, 0.05f),
            new SpellChance(SpellId.RejuvenationSelf1, 0.1f),
            new SpellChance(SpellId.RegenerationSelf1, 0.1f),
        };

        // shields
        private static readonly List<SpellChance> spellSelectionGroup8 = new List<SpellChance>()
        {
            new SpellChance(SpellId.StrengthSelf1, 0.1f),
            new SpellChance(SpellId.EnduranceSelf1, 0.1f),
            new SpellChance(SpellId.ImpregnabilitySelf1, 0.2f),
            new SpellChance(SpellId.InvulnerabilitySelf1, 0.2f),
            new SpellChance(SpellId.FealtySelf1, 0.15f),
            new SpellChance(SpellId.RejuvenationSelf1, 0.15f),
            new SpellChance(SpellId.MagicResistanceSelf1, 0.1f),
        };

        // gauntlets, sollerets, sandals, boots
        private static readonly List<SpellChance> spellSelectionGroup9 = new List<SpellChance>()
        {
            new SpellChance(SpellId.CoordinationSelf1, 0.16f),
            new SpellChance(SpellId.LightWeaponsMasterySelf1, 0.1f),
            new SpellChance(SpellId.FinesseWeaponsMasterySelf1, 0.08f),
            new SpellChance(SpellId.LightWeaponsMasterySelf1, 0.1f),
            new SpellChance(SpellId.LightWeaponsMasterySelf1, 0.1f),
            new SpellChance(SpellId.LightWeaponsMasterySelf1, 0.08f),
            new SpellChance(SpellId.HeavyWeaponsMasterySelf1, 0.1f),
            new SpellChance(SpellId.LightWeaponsMasterySelf1, 0.08f),
            new SpellChance(SpellId.MissileWeaponsMasterySelf1, 0.05f),
            new SpellChance(SpellId.MissileWeaponsMasterySelf1, 0.05f),
            new SpellChance(SpellId.ThrownWeaponMasterySelf1, 0.05f),
            new SpellChance(SpellId.HealingMasterySelf1, 0.05f),
        };

        // helms, basinets, helmets, coifs, cowls, heaumes, kabutons
        private static readonly List<SpellChance> spellSelectionGroup10 = new List<SpellChance>()
        {
            new SpellChance(SpellId.MagicResistanceSelf1, 0.3f),
            new SpellChance(SpellId.ArmorExpertiseSelf1, 0.1f),
            new SpellChance(SpellId.WeaponExpertiseSelf1, 0.1f),
            new SpellChance(SpellId.MonsterAttunementSelf1, 0.1f),
            new SpellChance(SpellId.FealtySelf1, 0.1f),
            new SpellChance(SpellId.RejuvenationSelf1, 0.1f),
            new SpellChance(SpellId.ManaRenewalSelf1, 0.1f),
            new SpellChance(SpellId.HealingMasterySelf1, 0.1f),
        };

        // boots, sandals, shoes, slippers, sollerets
        private static readonly List<SpellChance> spellSelectionGroup11 = new List<SpellChance>()
        {
            new SpellChance(SpellId.QuicknessSelf1, 0.15f),
            new SpellChance(SpellId.JumpingMasterySelf1, 0.06f),
            new SpellChance(SpellId.SprintSelf1, 0.06f),
            new SpellChance(SpellId.CoordinationSelf1, 0.15f),
            new SpellChance(SpellId.LightWeaponsMasterySelf1, 0.06f),
            new SpellChance(SpellId.FinesseWeaponsMasterySelf1, 0.04f),
            new SpellChance(SpellId.LightWeaponsMasterySelf1, 0.06f),
            new SpellChance(SpellId.LightWeaponsMasterySelf1, 0.06f),
            new SpellChance(SpellId.LightWeaponsMasterySelf1, 0.04f),
            new SpellChance(SpellId.HeavyWeaponsMasterySelf1, 0.08f),
            new SpellChance(SpellId.LightWeaponsMasterySelf1, 0.04f),
            new SpellChance(SpellId.MissileWeaponsMasterySelf1, 0.08f),
            new SpellChance(SpellId.MissileWeaponsMasterySelf1, 0.06f),
            new SpellChance(SpellId.ThrownWeaponMasterySelf1, 0.06f),
        };

        // breeches, jerkins, shirts, pants, tunics, doublets, trousers, pantaloons
        private static readonly List<SpellChance> spellSelectionGroup12 = new List<SpellChance>()
        {
            new SpellChance(SpellId.AcidProtectionSelf1, 0.1f),
            new SpellChance(SpellId.BludgeonProtectionSelf1, 0.1f),
            new SpellChance(SpellId.ColdProtectionSelf1, 0.1f),
            new SpellChance(SpellId.LightningProtectionSelf1, 0.1f),
            new SpellChance(SpellId.FireProtectionSelf1, 0.1f),
            new SpellChance(SpellId.BladeProtectionSelf1, 0.1f),
            new SpellChance(SpellId.PiercingProtectionSelf1, 0.1f),
            new SpellChance(SpellId.ArmorSelf1, 0.3f),
        };

        // caps, qafiyas, turbans, fezs, berets
        private static readonly List<SpellChance> spellSelectionGroup13 = new List<SpellChance>()
        {
            new SpellChance(SpellId.FocusSelf1, 0.2f),
            new SpellChance(SpellId.MagicResistanceSelf1, 0.1f),
            new SpellChance(SpellId.ArcaneEnlightenmentSelf1, 0.05f),
            new SpellChance(SpellId.ArmorExpertiseSelf1, 0.05f),
            new SpellChance(SpellId.ItemExpertiseSelf1, 0.05f),
            new SpellChance(SpellId.MagicItemExpertiseSelf1, 0.05f),
            new SpellChance(SpellId.WeaponExpertiseSelf1, 0.05f),
            new SpellChance(SpellId.MonsterAttunementSelf1, 0.05f),
            new SpellChance(SpellId.PersonAttunementSelf1, 0.05f),
            new SpellChance(SpellId.DeceptionMasterySelf1, 0.1f),
            new SpellChance(SpellId.ManaRenewalSelf1, 0.1f),
            new SpellChance(SpellId.AlchemyMasterySelf1, 0.05f),
            new SpellChance(SpellId.FletchingMasterySelf1, 0.05f),
            new SpellChance(SpellId.CookingMasterySelf1, 0.05f),
        };

        // cloth gloves (1 entry?)
        private static readonly List<SpellChance> spellSelectionGroup14 = new List<SpellChance>()
        {
            new SpellChance(SpellId.CoordinationSelf1, 0.1f),
            new SpellChance(SpellId.HealingMasterySelf1, 0.1f),
            new SpellChance(SpellId.LockpickMasterySelf1, 0.1f),
            new SpellChance(SpellId.ArmorExpertiseSelf1, 0.1f),
            new SpellChance(SpellId.ItemExpertiseSelf1, 0.1f),
            new SpellChance(SpellId.MagicItemExpertiseSelf1, 0.1f),
            new SpellChance(SpellId.WeaponExpertiseSelf1, 0.1f),
            new SpellChance(SpellId.AlchemyMasterySelf1, 0.1f),
            new SpellChance(SpellId.FletchingMasterySelf1, 0.1f),
            new SpellChance(SpellId.CookingMasterySelf1, 0.1f),
        };

        // greaves, leggings, tassets, leather pants
        private static readonly List<SpellChance> spellSelectionGroup15 = new List<SpellChance>()
        {
            new SpellChance(SpellId.QuicknessSelf1, 0.3f),
            new SpellChance(SpellId.JumpingMasterySelf1, 0.15f),
            new SpellChance(SpellId.SprintSelf1, 0.15f),
            new SpellChance(SpellId.StrengthSelf1, 0.3f),
            new SpellChance(SpellId.EnduranceSelf1, 0.1f),
        };

        // dinnerware
        private static readonly List<SpellChance> spellSelectionGroup16 = new List<SpellChance>()
        {
            new SpellChance(SpellId.StrengthSelf1, 0.03f),
            new SpellChance(SpellId.EnduranceSelf1, 0.03f),
            new SpellChance(SpellId.CoordinationSelf1, 0.04f),
            new SpellChance(SpellId.QuicknessSelf1, 0.03f),
            new SpellChance(SpellId.FocusSelf1, 0.04f),
            new SpellChance(SpellId.WillpowerSelf1, 0.03f),
            new SpellChance(SpellId.ArmorExpertiseSelf1, 0.1f),
            new SpellChance(SpellId.ItemExpertiseSelf1, 0.1f),
            new SpellChance(SpellId.MagicItemExpertiseSelf1, 0.1f),
            new SpellChance(SpellId.WeaponExpertiseSelf1, 0.1f),
            new SpellChance(SpellId.AlchemyMasterySelf1, 0.1f),
            new SpellChance(SpellId.FletchingMasterySelf1, 0.1f),
            new SpellChance(SpellId.CookingMasterySelf1, 0.1f),
            new SpellChance(SpellId.LockpickMasterySelf1, 0.1f),
        };

        /// <summary>
        /// Key is PropertyInt.TsysMutationData >> 24
        /// </summary>
        public static readonly Dictionary<int, List<SpellChance>> Group = new Dictionary<int, List<SpellChance>>()
        {
            { 1, spellSelectionGroup1 },
            { 2, spellSelectionGroup2 },
            { 3, spellSelectionGroup3 },
            { 4, spellSelectionGroup4 },
            { 5, spellSelectionGroup5 },
            { 6, spellSelectionGroup6 },
            { 7, spellSelectionGroup7 },
            { 8, spellSelectionGroup8 },
            { 9, spellSelectionGroup9 },
            { 10, spellSelectionGroup10 },
            { 11, spellSelectionGroup11 },
            { 12, spellSelectionGroup12 },
            { 13, spellSelectionGroup13 },
            { 14, spellSelectionGroup14 },
            { 15, spellSelectionGroup15 },
            { 16, spellSelectionGroup16 },
        };
    }
}
