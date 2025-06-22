using System;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static void AssignMagic(WorldObject wo, TreasureDeath profile, TreasureRoll roll, bool isArmor = false)
        {
            AssignSpells(wo, profile, roll);

            wo.UiEffects = UiEffects.Magical;

            var maxBaseMana = GetMaxBaseMana(wo);

            wo.ManaRate = CalculateManaRate(maxBaseMana);

            var maxSpellMana = maxBaseMana;

            if (wo.SpellDID != null)
            {
                var spell = new Server.Entity.Spell(wo.SpellDID.Value);

                var castableMana = (int)spell.BaseMana * 5;

                if (castableMana > maxSpellMana)
                    maxSpellMana = castableMana;
            }

            wo.ItemMaxMana = RollItemMaxMana(wo, roll, maxSpellMana);
            wo.ItemCurMana = wo.ItemMaxMana;

            wo.ItemSpellcraft = RollSpellcraft(wo, roll);

            AddActivationRequirements(wo, roll);
        }

        /// <summary>
        /// Returns the maximum BaseMana from the spells in item's spellbook
        /// </summary>
        private static int GetMaxBaseMana(WorldObject wo)
        {
            var maxBaseMana = 0;

            if (wo.Biota.PropertiesSpellBook != null)
            {
                foreach (var spellId in wo.Biota.PropertiesSpellBook.Keys)
                {
                    var spell = new Server.Entity.Spell(spellId);

                    if (spell.BaseMana > maxBaseMana)
                        maxBaseMana = (int)spell.BaseMana;
                }
            }
            return maxBaseMana;
        }

        /// <summary>
        /// Rolls the ItemMaxMana for an object
        /// </summary>
        private static int RollItemMaxMana(WorldObject wo, TreasureRoll roll, int maxSpellMana)
        {
            // verified matches up with magloot eor logs

            var workmanship = WorkmanshipChance.GetModifier(wo.ItemWorkmanship - 1);

            (int min, int max) range;

            if (roll.IsClothing || roll.IsArmor || roll.IsWeapon || roll.IsDinnerware)
            {
                range.min = 6;
                range.max = 15;
            }
            else if (roll.IsJewelry)
            {
                // includes crowns
                range.min = 12;
                range.max = 20;
            }
            else if (roll.IsGem)
            {
                range.min = 1;
                range.max = 1;
            }
            else
            {
                log.Error($"RollItemMaxMana({wo.Name}, {roll.ItemType}, {maxSpellMana}) - unknown item type");
                return 1;
            }

            var rng = ThreadSafeRandom.Next(range.min, range.max);

            return (int)Math.Ceiling(maxSpellMana * workmanship * rng);
        }

        /// <summary>
        /// Calculates the ManaRate for an item
        /// </summary>
        private static float CalculateManaRate(int maxBaseMana)
        {
            if (maxBaseMana <= 0)
                maxBaseMana = 1;

            // verified with eor data
            return -1.0f / (float)Math.Ceiling(1200.0f / maxBaseMana);
        }

        private static int RollSpellcraft(WorldObject wo, TreasureRoll roll)
        {
            var maxSpellPower = GetMaxSpellPower(wo);

            (float min, float max) range = (1.0f, 1.0f);

            if (roll.IsClothing || roll.IsArmor || roll.IsWeapon || roll.IsJewelry || roll.IsDinnerware)
            {
                range.min = 0.9f;
                range.max = 1.1f;
            }
            else if (!roll.IsGem)
            {
                log.Error($"RollSpellcraft({wo.Name}, {roll.ItemType}) - unknown item type");
            }

            var rng = ThreadSafeRandom.Next(range.min, range.max);

            var spellcraft = (int)Math.Ceiling(maxSpellPower * rng);

            // retail was capped at 370
            spellcraft = Math.Min(spellcraft, 370);

            return spellcraft;
        }

        /// <summary>
        /// Returns the maximum power from the spells in item's SpellDID / spellbook
        /// </summary>
        private static int GetMaxSpellPower(WorldObject wo)
        {
            var maxSpellPower = 0;

            if (wo.SpellDID != null)
            {
                var spell = new Server.Entity.Spell(wo.SpellDID.Value);

                if (spell.Power > maxSpellPower)
                    maxSpellPower = (int)spell.Power;
            }

            if (wo.Biota.PropertiesSpellBook != null)
            {
                foreach (var spellId in wo.Biota.PropertiesSpellBook.Keys)
                {
                    var spell = new Server.Entity.Spell(spellId);

                    if (spell.Power > maxSpellPower)
                        maxSpellPower = (int)spell.Power;
                }
            }
            return maxSpellPower;
        }

        private static void AddActivationRequirements(WorldObject wo, TreasureRoll roll)
        {
            // ItemSkill/LevelLimit
            TryMutate_ItemSkillLimit(wo, roll);

            // Arcane Lore / ItemDifficulty
            wo.ItemDifficulty = CalculateArcaneLore(wo, roll);
        }

        private static bool TryMutate_ItemSkillLimit(WorldObject wo, TreasureRoll roll)
        {
            if (!RollItemSkillLimit(roll))
                return false;

            wo.ItemSkillLevelLimit = wo.ItemSpellcraft + 20;

            var skill = Skill.None;

            if (roll.IsMeleeWeapon || roll.IsMissileWeapon)
            {
                skill = wo.WeaponSkill;
            }
            else if (roll.IsArmor)
            {
                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

                if (rng < 0.5f)
                {
                    skill = Skill.MeleeDefense;
                }
                else
                {
                    skill = Skill.MissileDefense;
                    wo.ItemSkillLevelLimit = (int)(wo.ItemSkillLevelLimit * 0.7f);
                }
            }
            else
            {
                log.Error($"RollItemSkillLimit({wo.Name}, {roll.ItemType}) - unknown item type");
                return false;
            }

            wo.ItemSkillLimit = skill;
            return true;
        }

        private static bool RollItemSkillLimit(TreasureRoll roll)
        {
            if (roll.IsMeleeWeapon || roll.IsMissileWeapon)
            {
                return true;
            }
            else if (roll.IsArmor)
            {
                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

                return rng < 0.55f;
            }
            return false;
        }

        /// <summary>
        /// Calculates the Arcane Lore requirement / ItemDifficulty
        /// </summary>
        private static int CalculateArcaneLore(WorldObject wo, TreasureRoll roll)
        {
            // spellcraft - (itemSkillLevelLimit / 2.0f) + creatureLifeEnchantments + cantrips

            var spellcraft = wo.ItemSpellcraft.Value;

            // - mutates 100% of the time for melee / missile weapons
            // - mutates 55% of the time for armor
            // - mutates 0% of the time for all other item types
            var itemSkillLevelFactor = 0.0f;

            if (wo.ItemSkillLevelLimit > 0)
                itemSkillLevelFactor = wo.ItemSkillLevelLimit.Value / 2.0f;

            var fArcane = spellcraft - itemSkillLevelFactor;

            if (fArcane < 0)
                fArcane = 0;

            return (int)Math.Floor(fArcane + roll.ItemDifficulty);
        }
    }
}
