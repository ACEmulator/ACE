using System.Collections.Generic;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;
using Microsoft.EntityFrameworkCore;

namespace ACE.Server.Factories
{
    public partial class LootGenerationFactory
    {
        private static bool AssignMagic_New(WorldObject wo, TreasureDeath profile, TreasureRoll roll, out int numSpells)
        {
            var spells = RollSpells(wo, profile, roll);

            foreach (var spell in spells)
            {
                wo.Biota.GetOrAddKnownSpell((int)spell, wo.BiotaDatabaseLock, out _);
            }
            numSpells = spells.Count;
            return true;
        }

        private static List<SpellId> RollSpells(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            var spells = new List<SpellId>();

            // crowns, which are classified as TreasureItemType.Jewelry, should also be getting item spells
            // perhaps replace this with wo.ArmorLevel check?
            //if (roll.IsArmor || roll.IsArmorClothing(wo) || roll.IsWeapon)
            if (roll.HasArmorLevel(wo) || roll.IsWeapon)
            {
                var itemSpells = RollItemSpells(wo, profile, roll);

                if (itemSpells != null)
                    spells.AddRange(itemSpells);
            }

            var enchantments = RollEnchantments(wo, profile, roll);

            if (enchantments != null)
                spells.AddRange(enchantments);

            var cantrips = RollCantrips(wo, profile, roll);

            if (cantrips != null)
                spells.AddRange(cantrips);

            return spells;
        }

        private static List<SpellId> RollItemSpells(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            List<SpellId> spells = null;

            //if (roll.IsArmor || roll.IsArmorClothing(wo))
            if (roll.HasArmorLevel(wo))
            {
                spells = ArmorSpells.Roll(profile);
            }
            else if (roll.IsMeleeWeapon)
            {
                spells = MeleeSpells.Roll(profile);
            }
            else if (roll.IsMissileWeapon)
            {
                spells = MissileSpells.Roll(profile);
            }
            else if (roll.IsCaster)
            {
                spells = WandSpells.Roll(wo, profile);
            }
            else
            {
                log.Error($"RollItemSpells({wo.Name}) - item is not clothing / armor / weapon");
                return null;
            }

            return RollSpellLevels(wo, profile, spells);
        }

        private static List<SpellId> RollSpellLevels(WorldObject wo, TreasureDeath profile, IEnumerable<SpellId> spells)
        {
            var finalSpells = new List<SpellId>();

            foreach (var spell in spells)
            {
                var spellLevel = SpellLevelChance.Roll(profile.Tier);

                var spellLevels = SpellLevelProgression.GetSpellLevels(spell);

                if (spellLevels.Count != 8)
                {
                    log.Error($"RollSpellLevels({wo.Name}, {spell}) - spell level progression returned {spellLevels.Count}, expected 8");
                    continue;
                }

                finalSpells.Add(spellLevels[spellLevel - 1]);
            }

            return finalSpells;
        }

        private static List<SpellId> RollEnchantments(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            // TODO: change to ace spell selection tables
            /*if (wo.SpellSelectionCode == null)
            {
                log.Warn($"RollEnchantments({wo.Name}) - missing spell selection code / PropertyInt.TsysMutationData");
                return null;
            }*/

            var numEnchantments = RollNumEnchantments(wo, profile, roll);

            if (numEnchantments <= 0)
                return null;

            var numAttempts = numEnchantments * 3;

            var spells = new HashSet<SpellId>();

            for (var i = 0; i < numAttempts && spells.Count < numEnchantments; i++)
            {
                var spell = RollEnchantment(wo, profile, roll);

                if (spell != SpellId.Undef)
                    spells.Add(spell);
            }

            return RollSpellLevels(wo, profile, spells);
        }

        private static SpellId RollEnchantment(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            // TODO: change to ace spell selection tables
            //return SpellSelectionTable.Roll(wo.SpellSelectionCode.Value);

            if (roll.IsJewelry)
            {
                var rng = ThreadSafeRandom.Next(0, JewelrySpells.Table.Length - 1);
                return JewelrySpells.Table[rng][0];
            }

            List<SpellId> table = null;

            if (roll.IsClothing || roll.IsArmor)
                table = ArmorSpells.CreatureLifeTable;
            else if (roll.IsCaster)
                table = WandSpells.CreatureLifeTable;
            else if (roll.IsMeleeWeapon)
                table = MeleeSpells.CreatureLifeTable;
            else if (roll.IsMissileWeapon)
                table = MissileSpells.CreatureLifeTable;
            else
            {
                log.Error($"RollEnchantment({wo.Name}, {profile.TreasureType}, {roll.ItemType}) - unknown item type");
                return SpellId.Undef;
            }
            var _rng = ThreadSafeRandom.Next(0, table.Count - 1);

            return table[_rng];
        }

        private static int RollNumEnchantments(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            if (roll.IsArmor || roll.IsWeapon)
            {
                return RollNumEnchantments_Armor_Weapon(wo, profile, roll);
            }
            // confirmed:
            // - crowns (classified as TreasureItemType.Jewelry) used this table
            // - clothing w/ al also used this table
            else if (roll.IsClothing || roll.IsJewelry || roll.IsDinnerware)
            {
                return RollNumEnchantments_Clothing_Jewelry_Dinnerware(wo, profile, roll);
            }
            else
            {
                log.Warn($"RollNumEnchantments({wo.Name}, {profile.TreasureType}, {roll.ItemType}) - unknown item type");
                return 1;   // gems?
            }
        }

        private static readonly List<float> EnchantmentChances_Armor_MeleeMissileWeapon = new List<float>()
        {
            0.00f,  // T1
            0.05f,  // T2
            0.10f,  // T3
            0.20f,  // T4
            0.40f,  // T5
            0.60f,  // T6
            0.60f,  // T7
            0.60f,  // T8
        };

        private static readonly List<float> EnchantmentChances_Caster = new List<float>()
        {
            0.60f,  // T1
            0.60f,  // T2
            0.60f,  // T3
            0.60f,  // T4
            0.60f,  // T5
            0.75f,  // T6
            0.75f,  // T7
            0.75f,  // T8
        };

        private static int RollNumEnchantments_Armor_Weapon(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            var tierChances = roll.IsCaster ? EnchantmentChances_Caster : EnchantmentChances_Armor_MeleeMissileWeapon;

            var chance = tierChances[profile.Tier - 1];

            var rng = ThreadSafeRandom.NextInterval(profile.LootQualityMod);

            if (rng < chance)
                return 1;
            else
                return 0;
        }

        private static int RollNumEnchantments_Clothing_Jewelry_Dinnerware(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            var chance = 0.1f;

            var rng = ThreadSafeRandom.NextInterval(profile.LootQualityMod);

            if (rng >= chance)
                return 1;
            else if (profile.Tier < 6)
                return 2;

            // tier 6+ has a chance for 3 enchantments
            rng = ThreadSafeRandom.NextInterval(profile.LootQualityMod * 0.1f);

            if (rng >= chance * 0.5f)
                return 2;
            else
                return 3;
        }

        private static List<SpellId> RollCantrips(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            var numCantrips = CantripChance.RollNumCantrips(profile);

            if (numCantrips == 0)
                return null;

            var numAttempts = numCantrips * 3;

            var cantrips = new HashSet<SpellId>();

            for (var i = 0; i < numAttempts && cantrips.Count < numCantrips; i++)
            {
                var cantrip = RollCantrip(wo, profile, roll);

                cantrip = AdjustForWeaponMastery(wo, cantrip);

                if (cantrip != SpellId.Undef)
                    cantrips.Add(cantrip);
            }

            var finalCantrips = new List<SpellId>();

            foreach (var cantrip in cantrips)
            {
                var cantripLevel = CantripChance.RollCantripLevel(profile);

                var cantripLevels = SpellLevelProgression.GetSpellLevels(cantrip);

                if (cantripLevels.Count != 4)
                {
                    log.Error($"RollCantrips({wo.Name}, {profile.TreasureType}, {roll.ItemType}) - {cantrip} has {cantripLevels.Count} cantrip levels, expected 4");
                    continue;
                }

                // TODO: diffAdjust?
                finalCantrips.Add(cantripLevels[cantripLevel - 1]);

            }
            return finalCantrips;
        }

        private static SpellId RollCantrip(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            // TODO: switch to ace cantrip tables
            SpellId[][] table = null;

            if (roll.IsClothing || roll.IsArmor)
            {
                if (wo.IsShield)
                {
                    // shield cantrip
                    //return ArmorCantrips.Roll();
                    table = ArmorCantrips.Table;
                }
                else
                {
                    // armor / clothing cantrip
                    //return ArmorCantrips.Roll();
                    table = ArmorCantrips.Table;
                }
            }
            else if (roll.IsCaster)
            {
                // caster cantrip
                //return WandCantrips.Roll();
                table = WandCantrips.Table;
            }
            else if (roll.IsMeleeWeapon)
            {
                // melee cantrip
                //return MeleeCantrips.Roll();
                table = MeleeCantrips.Table;
            }
            else if (roll.IsMissileWeapon)
            {
                // missile cantrip
                //return MissileCantrips.Roll();
                table = MissileCantrips.Table;
            }
            else if (roll.IsJewelry)
            {
                // jewelry cantrip
                //return JewelryCantrips.Roll();
                table = JewelryCantrips.Table;
            }
            else
            {
                log.Error($"RollCantrip({wo.Name}, {profile.TreasureType}, {roll.ItemType}) - unknown item type");
                return SpellId.Undef;
            }

            var rng = ThreadSafeRandom.Next(0, table.Length - 1);
            return table[rng][0];
        }

        public static SpellId AdjustForWeaponMastery(WorldObject wo, SpellId spell)
        {
            // only weapon aptitude cantrip in tables
            // indicates adjustment
            if (spell != SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1)
                return spell;

            switch (wo.WeaponSkill)
            {
                case Skill.HeavyWeapons:
                case Skill.LightWeapons:
                case Skill.FinesseWeapons:

                    // 10% chance to adjust to dual wielding
                    var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

                    if (rng < 0.1f)
                        return SpellId.CantripDualWieldAptitude1;

                    switch (wo.WeaponSkill)
                    {
                        case Skill.HeavyWeapons:
                            return SpellId.CANTRIPHEAVYWEAPONSAPTITUDE1;
                        case Skill.LightWeapons:
                            return SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1;
                        case Skill.FinesseWeapons:
                            return SpellId.CANTRIPFINESSEWEAPONSAPTITUDE1;
                    }
                    break;

                case Skill.MissileWeapons:
                    return SpellId.CANTRIPMISSILEWEAPONSAPTITUDE1;
                case Skill.TwoHandedCombat:
                    return SpellId.CANTRIPTWOHANDEDAPTITUDE1;
            }
            return spell;
        }
    }
}
