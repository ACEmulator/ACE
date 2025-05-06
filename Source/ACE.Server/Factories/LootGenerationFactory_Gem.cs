using System;

using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        /// <summary>
        /// This is only called by /testlootgen command
        /// The actual lootgen system doesn't use this.
        /// </summary>
        private static WorldObject CreateGem(TreasureDeath profile, bool isMagical)
        {
            var treasureRoll = new TreasureRoll(TreasureItemType.Gem);

            var gemClass = GemClassChance.Roll(profile.Tier);
            var gemResult = GemMaterialChance.Roll(gemClass);

            treasureRoll.Wcid = gemResult.ClassName;

            var wo = WorldObjectFactory.CreateNewWorldObject((uint)treasureRoll.Wcid);

            MutateGem(wo, profile, isMagical, treasureRoll);

            return wo;
        }

        private static void MutateGem(WorldObject wo, TreasureDeath profile, bool isMagical, TreasureRoll roll)
        {
            // workmanship
            wo.ItemWorkmanship = WorkmanshipChance.Roll(profile.Tier);

            // item color
            MutateColor(wo);

            if (!isMagical)
            {
                // TODO: verify if this is needed
                wo.ItemUseable = Usable.No;
                wo.SpellDID = null;
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;
                wo.ItemSkillLevelLimit = null;
                wo.ManaRate = null;
            }
            else
            {
                AssignMagic_Gem(wo, profile, roll);

                wo.UiEffects = UiEffects.Magical;
                wo.ItemUseable = Usable.Contained;
            }

            // item value
            if (wo.HasMutateFilter(MutateFilter.Value))
                MutateValue(wo, profile.Tier, roll);

            // long desc
            wo.LongDesc = GetLongDesc(wo);
        }

        private static bool AssignMagic_Gem(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            // TODO: move to standard AssignMagic() pipeline

            var spell = SpellSelectionTable.Roll(1);

            var spellLevel = SpellLevelChance.Roll(profile.Tier);

            var spellLevels = SpellLevelProgression.GetSpellLevels(spell);

            if (spellLevels == null || spellLevels.Count != 8)
            {
                log.Error($"AssignMagic_Gem({wo.Name}, {profile.TreasureType}, {roll.ItemType}) - unknown spell {spell}");
                return false;
            }

            var finalSpellId = spellLevels[spellLevel - 1];

            wo.SpellDID = (uint)finalSpellId;

            var _spell = new Server.Entity.Spell(finalSpellId);

            // retail spellcraft was capped at 370
            wo.ItemSpellcraft = Math.Min((int)_spell.Power, 370);

            var castableMana = (int)_spell.BaseMana * 5;

            wo.ItemMaxMana = RollItemMaxMana(wo, roll, castableMana);
            wo.ItemCurMana = wo.ItemMaxMana;

            // verified
            wo.ItemManaCost = castableMana;

            return true;
        }

        private static void MutateValue_Gem(WorldObject wo)
        {
            var materialMod = MaterialTable.GetValueMod(wo.MaterialType);

            var workmanshipMod = WorkmanshipChance.GetModifier(wo.ItemWorkmanship);

            wo.Value = (int)(wo.Value * materialMod * workmanshipMod);
        }
    }
}
