using System.Collections.Generic;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class CloakChance
    {
        private static ChanceTable<int> T1_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 1.0f )
        };

        private static ChanceTable<int> T2_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.99f ),
            ( 2, 0.01f ),
        };

        private static ChanceTable<int> T3_T4_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.44f ),
            ( 2, 0.55f ),
            ( 3, 0.01f ),
        };

        private static ChanceTable<int> T5_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.04f ),
            ( 2, 0.40f ),
            ( 3, 0.55f ),
            ( 4, 0.01f ),
        };

        private static ChanceTable<int> T6_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.04f ),
            ( 2, 0.30f ),
            ( 3, 0.65f ),
            ( 4, 0.01f ),
        };

        private static ChanceTable<int> T7_T8_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 2, 0.45f ),
            ( 3, 0.50f ),
            ( 4, 0.04f ),
            ( 5, 0.01f ),
        };

        private static List<ChanceTable<int>> cloakLevels = new List<ChanceTable<int>>()
        {
            T1_ItemMaxLevel,
            T2_ItemMaxLevel,
            T3_T4_ItemMaxLevel,
            T3_T4_ItemMaxLevel,
            T5_ItemMaxLevel,
            T6_ItemMaxLevel,
            T7_T8_ItemMaxLevel,
            T7_T8_ItemMaxLevel,
        };

        public static int Roll_ItemMaxLevel(TreasureDeath profile)
        {
            var table = cloakLevels[profile.Tier - 1];

            return table.Roll(profile.LootQualityMod);
        }


        private static readonly List<EquipmentSet> cloakSets = new List<EquipmentSet>()
        {
            EquipmentSet.CloakAlchemy,
            EquipmentSet.CloakArcaneLore,
            EquipmentSet.CloakArmorTinkering,
            EquipmentSet.CloakAssessPerson,
            EquipmentSet.CloakLightWeapons,
            EquipmentSet.CloakMissileWeapons,
            EquipmentSet.CloakCooking,
            EquipmentSet.CloakCreatureEnchantment,
            EquipmentSet.CloakFinesseWeapons,
            EquipmentSet.CloakDeception,
            EquipmentSet.CloakFletching,
            EquipmentSet.CloakHealing,
            EquipmentSet.CloakItemEnchantment,
            EquipmentSet.CloakItemTinkering,
            EquipmentSet.CloakLeadership,
            EquipmentSet.CloakLifeMagic,
            EquipmentSet.CloakLoyalty,
            EquipmentSet.CloakMagicDefense,
            EquipmentSet.CloakMagicItemTinkering,
            EquipmentSet.CloakManaConversion,
            EquipmentSet.CloakMeleeDefense,
            EquipmentSet.CloakMissileDefense,
            EquipmentSet.CloakSalvaging,
            EquipmentSet.CloakHeavyWeapons,
            EquipmentSet.CloakTwoHandedCombat,
            EquipmentSet.CloakVoidMagic,
            EquipmentSet.CloakWarMagic,
            EquipmentSet.CloakWeaponTinkering,
            EquipmentSet.CloakAssessCreature,
            EquipmentSet.CloakDirtyFighting,
            EquipmentSet.CloakDualWield,
            EquipmentSet.CloakRecklessness,
            EquipmentSet.CloakShield,
            EquipmentSet.CloakSneakAttack,
            EquipmentSet.CloakSummoning,
        };

        public static EquipmentSet RollEquipmentSet()
        {
            // verify: even chance per set
            var rng = ThreadSafeRandom.Next(0, cloakSets.Count - 1);

            return cloakSets[rng];
        }

        private static readonly List<SpellId> surgeSpells = new List<SpellId>()
        {
            SpellId.AcidRing,           // Searing Disc
            SpellId.BladeRing,          // Horizon's Blades
            SpellId.FlameRing,          // Cassius' Ring of Fire
            SpellId.ForceRing,          // Nuhmudira's Spines
            SpellId.FrostRing,          // Halo of Frost
            SpellId.LightningRing,      // Eye of the Storm
            SpellId.ShockwaveRing,      // Tectonic Rifts
            SpellId.NetherRing,         // Clouded Soul

            SpellId.CloakAllSkill,      // Cloaked in Skill

            SpellId.CloakMagicDLower,   // Shroud of Darkness (Magic)
            SpellId.CloakMeleeDLower,   // Shroud of Darkness (Melee)
            SpellId.CloakMissileDLower, // Shroud of Darkness (Missile)
        };

        public static SpellId RollProcSpell()
        {
            // verify: even chance for each spell, including Damage Reduction Proc?
            var rng = ThreadSafeRandom.Next(0, surgeSpells.Count);

            // handle Damage Reduction proc
            if (rng == surgeSpells.Count)
                return SpellId.Undef;

            return surgeSpells[rng];
        }
    }
}
