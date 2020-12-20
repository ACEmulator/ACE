using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

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
            {
                spells.AddRange(enchantments);

                roll.ItemDifficulty += RollEnchantmentDifficulty(enchantments);
            }

            var cantrips = RollCantrips(wo, profile, roll);

            if (cantrips != null)
            {
                spells.AddRange(cantrips);

                roll.ItemDifficulty += RollCantripDifficulty(cantrips);
            }

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
            /*if (wo.SpellSelectionCode == null)
            {
                log.Warn($"RollEnchantments({wo.Name}) - missing spell selection code / PropertyInt.TsysMutationData");
                return null;
            }*/

            // test method: determine spell selection code dynamically
            var spellSelectionCode = GetSpellSelectionCode_Dynamic(wo, roll);

            if (spellSelectionCode == 0)
                return null;

            //Console.WriteLine($"Using spell selection code {spellSelectionCode} for {wo.Name}");

            var numEnchantments = RollNumEnchantments(wo, profile, roll);

            if (numEnchantments <= 0)
                return null;

            var numAttempts = numEnchantments * 3;

            var spells = new HashSet<SpellId>();

            for (var i = 0; i < numAttempts && spells.Count < numEnchantments; i++)
            {
                var spell = SpellSelectionTable.Roll(spellSelectionCode);

                if (spell != SpellId.Undef)
                    spells.Add(spell);
            }

            return RollSpellLevels(wo, profile, spells);
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

        private static float RollEnchantmentDifficulty(List<SpellId> spellIds)
        {
            var spells = new List<Server.Entity.Spell>();

            foreach (var spellId in spellIds)
            {
                var spell = new Server.Entity.Spell(spellId);
                spells.Add(spell);
            }

            spells = spells.OrderBy(i => i.Formula.Level).ToList();

            var itemDifficulty = 0.0f;

            // exclude highest spell
            for (var i = 0; i < spells.Count - 1; i++)
            {
                var spell = spells[i];

                var rng = (float)ThreadSafeRandom.Next(0.5f, 1.5f);

                itemDifficulty += spell.Formula.Level * 5.0f * rng;
            }

            return itemDifficulty;
        }

        private static List<SpellId> RollCantrips(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            // no cantrips on dinnerware?
            if (roll.ItemType == TreasureItemType_Orig.ArtObject)
                return null;

            var numCantrips = CantripChance.RollNumCantrips(profile);

            if (numCantrips == 0)
                return null;

            var numAttempts = numCantrips * 3;

            var cantrips = new HashSet<SpellId>();

            for (var i = 0; i < numAttempts && cantrips.Count < numCantrips; i++)
            {
                var cantrip = RollCantrip(wo, profile, roll);

                if (cantrip != SpellId.Undef)
                    cantrips.Add(cantrip);
            }

            var finalCantrips = new List<SpellId>();

            var legendary = false;

            foreach (var cantrip in cantrips)
            {
                var cantripLevel = CantripChance.RollCantripLevel(profile);

                var cantripLevels = SpellLevelProgression.GetSpellLevels(cantrip);

                if (cantripLevels.Count != 4)
                {
                    log.Error($"RollCantrips({wo.Name}, {profile.TreasureType}, {roll.ItemType}) - {cantrip} has {cantripLevels.Count} cantrip levels, expected 4");
                    continue;
                }

                finalCantrips.Add(cantripLevels[cantripLevel - 1]);

                if (!legendary && cantripLevel == 4)
                {
                    // items with legendary cantrips always got bumped up to wield level requirement 180 in retail
                    SetWieldLevelReq(wo, 180);
                    legendary = true;
                }
            }
            return finalCantrips;
        }

        private static SpellId RollCantrip(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            if (roll.HasArmorLevel(wo) || roll.IsClothing)
            {
                // armor / clothing cantrip
                // this table also applies to crowns (treasureitemtype.jewelry w/ al)
                return ArmorCantrips.Roll();
            }
            if (roll.IsMeleeWeapon)
            {
                // melee cantrip
                var meleeCantrip = MeleeCantrips.Roll();

                // adjust for weapon skill
                if (meleeCantrip == SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1)
                    meleeCantrip = AdjustForWeaponMastery(wo);

                return meleeCantrip;
            }
            else if (roll.IsMissileWeapon)
            {
                // missile cantrip
                return MissileCantrips.Roll();
            }
            else if (roll.IsCaster)
            {
                // caster cantrip
                var casterCantrip = WandCantrips.Roll();

                if (casterCantrip == SpellId.CANTRIPWARMAGICAPTITUDE1)
                    casterCantrip = AdjustForDamageType(wo, casterCantrip);

                return casterCantrip;
            }
            else if (roll.IsJewelry)
            {
                // jewelry cantrip
                return JewelryCantrips.Roll();
            }
            else
            {
                log.Error($"RollCantrip({wo.Name}, {profile.TreasureType}, {roll.ItemType}) - unknown item type");
                return SpellId.Undef;
            }
        }

        private static float RollCantripDifficulty(List<SpellId> cantripIds)
        {
            var itemDifficulty = 0.0f;

            foreach (var cantripId in cantripIds)
            {
                var cantripLevels = SpellLevelProgression.GetSpellLevels(cantripId);

                if (cantripLevels == null || cantripLevels.Count != 4)
                {
                    log.Error($"RollCantripDifficulty({cantripId}) - unknown cantrip");
                    continue;
                }

                var cantripLevel = cantripLevels.IndexOf(cantripId);

                if (cantripLevel == 0)
                    itemDifficulty += (float)ThreadSafeRandom.Next(5.0f, 10.0f);
                else
                    itemDifficulty += (float)ThreadSafeRandom.Next(10.0f, 20.0f);
            }
            return itemDifficulty;
        }

        private static SpellId AdjustForWeaponMastery(WorldObject wo)
        {
            // handle two-handed weapons
            if (wo.WeaponSkill == Skill.TwoHandedCombat)
                return SpellId.CANTRIPTWOHANDEDAPTITUDE1;

            // 10% chance to adjust to dual wielding
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            if (rng < 0.1f)
                return SpellId.CantripDualWieldAptitude1;

            // heavy/light/finesse weapons
            switch (wo.WeaponSkill)
            {
                case Skill.HeavyWeapons:
                    return SpellId.CANTRIPHEAVYWEAPONSAPTITUDE1;
                case Skill.LightWeapons:
                    return SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1;
                case Skill.FinesseWeapons:
                    return SpellId.CANTRIPFINESSEWEAPONSAPTITUDE1;
            }
            return SpellId.Undef;
        }

        private static SpellId AdjustForDamageType(WorldObject wo, SpellId spell)
        {
            if (wo.W_DamageType == DamageType.Nether)
                return SpellId.CantripVoidMagicAptitude1;

            if (wo.W_DamageType != DamageType.Undef)
                return SpellId.CANTRIPWARMAGICAPTITUDE1;

            // even split? retail was broken here
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            if (rng < 0.5f)
                return SpellId.CANTRIPWARMAGICAPTITUDE1;
            else
                return SpellId.CantripVoidMagicAptitude1;
        }

        /// <summary>
        /// An alternate method to using the SpellSelectionCode from PropertyInt.TSysMutationdata
        /// </summary>
        private static int GetSpellSelectionCode_Dynamic(WorldObject wo, TreasureRoll roll)
        {
            if (wo is Gem)
            {
                return 1;
            }
            else if (roll.ItemType == TreasureItemType_Orig.Jewelry)
            {
                if (!roll.HasArmorLevel(wo))
                    return 2;
                else
                    return 3;
            }
            else if (roll.Wcid == Enum.WeenieClassName.orb)
            {
                return 4;
            }
            else if (roll.IsCaster && wo.W_DamageType != DamageType.Nether)
            {
                return 5;
            }
            else if (roll.IsMeleeWeapon && wo.WeaponSkill != Skill.TwoHandedCombat)
            {
                return 6;
            }
            else if ((roll.IsArmor || roll.IsClothing) && !wo.IsShield)
            {
                return GetSpellCode_Dynamic_ClothingArmor(wo, roll);
            }
            else if (wo.IsShield)
            {
                return 8;
            }
            else if (roll.IsDinnerware)
            {
                if (roll.Wcid == Enum.WeenieClassName.flasksimple)
                    return 0;
                else
                    return 16;
            }
            else if (roll.IsMissileWeapon || wo.WeaponSkill == Skill.TwoHandedCombat)
            {
                return 17;
            }
            else if (roll.IsCaster && wo.W_DamageType == DamageType.Nether)
            {
                return 19;
            }

            log.Error($"GetSpellCode_Dynamic({wo.Name}) - couldn't determine spell selection code");

            return 0;
        }

        private static readonly CoverageMask upperArmor = CoverageMask.OuterwearChest | CoverageMask.OuterwearUpperArms | CoverageMask.OuterwearLowerArms | CoverageMask.OuterwearAbdomen;
        private static readonly CoverageMask lowerArmor = CoverageMask.OuterwearUpperLegs | CoverageMask.OuterwearLowerLegs;     // check abdomen

        private static readonly CoverageMask clothing = CoverageMask.UnderwearChest | CoverageMask.UnderwearUpperArms | CoverageMask.UnderwearLowerArms |
                CoverageMask.UnderwearAbdomen | CoverageMask.UnderwearUpperLegs | CoverageMask.UnderwearLowerLegs;

        private static int GetSpellCode_Dynamic_ClothingArmor(WorldObject wo, TreasureRoll roll)
        {
            // special cases
            switch (roll.Wcid)
            {
                case Enum.WeenieClassName.glovescloth:
                    return 14;
                case Enum.WeenieClassName.capleather:
                    return 20;
            }

            var coverageMask = wo.ClothingPriority ?? 0;
            var isArmor = roll.IsArmor;

            if ((coverageMask & upperArmor) != 0 && (coverageMask & CoverageMask.OuterwearLowerLegs) == 0)
                return 7;

            if (coverageMask == CoverageMask.Hands && isArmor)
                return 9;

            if (coverageMask == CoverageMask.Head && roll.BaseArmorLevel > 20)
                return 10;

            // base weenie armorLevel > 20
            if ((coverageMask & CoverageMask.Feet) != 0 && roll.BaseArmorLevel > 20)
                return 11;

            if ((coverageMask & clothing) != 0)
                return 12;

            // metal cap?
            if (coverageMask == CoverageMask.Head && !isArmor)
                return 13;

            if (coverageMask == CoverageMask.Hands && !isArmor)
                return 14;

            // leggings
            if ((coverageMask & lowerArmor) != 0)
                return 15;

            if (coverageMask == CoverageMask.Feet)
                return 18;

            log.Error($"GetSpellCode_Dynamic_ClothingArmor({wo.Name}) - couldn't determine spell selection code for {coverageMask}, {isArmor}");
            return 0;
        }
    }
}
