using System;
using System.Linq;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// Method used for handling creature targeted spell casts
        /// </summary>
        public void CreateCreatureSpell(ObjectGuid guidTarget, uint spellId)
        {
            Creature creature = CurrentLandblock?.GetObject(Guid) as Creature;

            if (creature.IsBusy == true)
                return;

            creature.IsBusy = true;

            var spell = new Spell(spellId);

            if (spell.NotFound)
            {
                creature.IsBusy = false;
                return;
            }

            // originally was using spell.RangeConstant, bug?
            // is this floor required?
            bool targetSelf = Math.Floor(spell.BaseRangeConstant) == 0;
            var target = targetSelf ? this : CurrentLandblock?.GetObject(guidTarget);

            switch (spell.School)
            {
                case MagicSchool.ItemEnchantment:
                    // if (!targetSelf && ResistSpell(Skill.CreatureEnchantment)) break;
                    ItemMagic(target, spell);
                    EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
                    break;
                case MagicSchool.LifeMagic:

                    break;
                case MagicSchool.CreatureEnchantment:

                    break;
                case MagicSchool.WarMagic:

                    break;
            }

            creature.IsBusy = false;
            return;
        }

        /// <summary>
        /// Method used for handling creature untargeted spell casts
        /// </summary>
        public void CreateCreatureSpell(uint spellId)
        {
            // does this function even do anything??
            var creature = CurrentLandblock?.GetObject(Guid) as Creature;

            if (creature == null || creature.IsBusy) return;

            creature.IsBusy = true;

            var spell = new Spell(spellId);

            if (spell.NotFound)
            {
                creature.IsBusy = false;
                return;
            }

            creature.IsBusy = false;
        }

        public uint CalculateManaUsage(Creature caster, Spell spell, WorldObject target = null)
        {
            var baseCost = spell.BaseMana;

            // for casting spells built into a casting implement, use the ItemManaCost
            var castItem = caster.GetEquippedWand();
            if (castItem != null && (castItem.SpellDID ?? 0) == spell.Id)
                baseCost = (uint)(castItem.ItemManaCost ?? 0);

            if ((spell.School == MagicSchool.ItemEnchantment) && (spell.MetaSpellType == SpellType.Enchantment) &&
                (spell.Category >= SpellCategory.ArmorValueRaising) && (spell.Category <= SpellCategory.AcidicResistanceLowering) && target is Player targetPlayer)
            {
                var numTargetItems = 1;
                if (targetPlayer != null)
                    numTargetItems = targetPlayer.EquippedObjects.Values.Count(i => (i is Clothing || i.IsShield) && i.IsEnchantable);

                baseCost += spell.ManaMod * (uint)numTargetItems;
            }
            else if ((spell.Flags & SpellFlags.FellowshipSpell) != 0)
            {
                var numFellows = 1;
                if (this is Player player && player.Fellowship != null)
                    numFellows = player.Fellowship.FellowshipMembers.Count;

                baseCost += spell.ManaMod * (uint)numFellows;
            }

            var manaConversion = caster.GetCreatureSkill(Skill.ManaConversion);

            if (manaConversion.AdvancementClass < SkillAdvancementClass.Trained || spell.Flags.HasFlag(SpellFlags.IgnoresManaConversion))
                return baseCost;

            var difficulty = spell.PowerMod;   // modified power difficulty

            var mana_conversion_skill = (uint)Math.Round(manaConversion.Current * GetWeaponManaConversionModifier(caster));

            var manaCost = GetManaCost(difficulty, baseCost, mana_conversion_skill);

            return manaCost;
        }

        public static uint GetManaCost(uint difficulty, uint manaCost, uint manaConv)
        {
            // thanks to GDLE for this function!
            if (manaConv == 0)
                return manaCost;

            // Dropping diff by half as Specced ManaC is only 48 with starter Aug so 50 at level 1 means no bonus
            //   easiest change without having to create two different formulas to try to emulate retail
            var successChance = SkillCheck.GetSkillChance(manaConv, difficulty / 2);
            var roll = ThreadSafeRandom.Next(0.0f, 1.0f);

            // Luck lowers the roll value to give better outcome
            // e.g. successChance = 0.83 & roll = 0.71 would still provide some savings.
            //   but a luck roll of 0.19 will lower that 0.71 to 0.13 so the caster would
            //   receive a 60% reduction in mana cost.  without the luck roll, 12%
            //   so players will always have a level of "luck" in manacost if they make skill checks
            var luck = ThreadSafeRandom.Next(0.0f, 1.0f);

            if (roll < successChance)
            {
                manaCost = (uint)Math.Round(manaCost * (1.0f - (successChance - (roll * luck))));
            }

            // above seems to give a good middle of the range
            // seen in pcaps for mana usage for low level chars
            // bug still need a way to give a better reduction for the "lucky"

            // save some calc time if already at 1 mana cost
            if (manaCost > 1)
            {
                successChance = SkillCheck.GetSkillChance(manaConv, difficulty);
                roll = ThreadSafeRandom.Next(0.0f, 1.0f);

                if (roll < successChance)
                    manaCost = (uint)Math.Round(manaCost * (1.0f - (successChance - (roll * luck))));
            }

            return Math.Max(manaCost, 1);
        }

        /// <summary>
        /// Handles equipping an item casting a spell on player or creature
        /// </summary>
        public virtual EnchantmentStatus CreateItemSpell(WorldObject item, uint spellID)
        {
            var enchantmentStatus = new EnchantmentStatus(spellID);

            var spell = enchantmentStatus.Spell;

            if (spell.NotFound)
                return enchantmentStatus;

            switch (spell.School)
            {
                case MagicSchool.CreatureEnchantment:

                    enchantmentStatus = CreatureMagic(this, spell, item, true);
                    if (enchantmentStatus.Message != null)
                        EnqueueBroadcast(new GameMessageScript(Guid, spell.TargetEffect, spell.Formula.Scale));

                    break;

                case MagicSchool.LifeMagic:

                    LifeMagic(spell, out uint damage, out enchantmentStatus, this, item, equip: true);
                    if (enchantmentStatus.Message != null)
                        EnqueueBroadcast(new GameMessageScript(Guid, spell.TargetEffect, spell.Formula.Scale));

                    break;

                case MagicSchool.ItemEnchantment:

                    if (spell.HasItemCategory || spell.IsPortalSpell)
                        enchantmentStatus = ItemMagic(this, spell, item, item, true);
                    else
                        enchantmentStatus = ItemMagic(item, spell, item, item, true);

                    var playScript = spell.IsPortalSpell && spell.CasterEffect > 0 ? spell.CasterEffect : spell.TargetEffect;
                    EnqueueBroadcast(new GameMessageScript(Guid, playScript, spell.Formula.Scale));

                    break;
            }
            return enchantmentStatus;
        }

        /// <summary>
        /// Removes an item's spell from the appropriate enchantment registry (either the wielder, or the item)
        /// </summary>
        /// <param name="silent">if TRUE, silently removes the spell, without sending a message to the target player</param>
        public void RemoveItemSpell(WorldObject item, uint spellId, bool silent = false)
        {
            if (item == null) return;

            var spell = new Spell(spellId);

            if (spell._spellBase == null)
            {
                if (this is Player player)
                    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"SpellId {spellId} Invalid."));

                return;
            }

            var target = spell.School == MagicSchool.ItemEnchantment && !spell.HasItemCategory ? item : this;

            // Retrieve enchantment on target and remove it, if present
            var propertiesEnchantmentRegistry = target.EnchantmentManager.GetEnchantment(spellId, item.Guid.Full);

            if (propertiesEnchantmentRegistry != null)
            {
                if (!silent)
                    target.EnchantmentManager.Remove(propertiesEnchantmentRegistry);
                else
                    target.EnchantmentManager.Dispel(propertiesEnchantmentRegistry);
            }
        }

        /// <summary>
        /// Returns the creature's effective magic defense skill
        /// with item.WeaponMagicDefense and imbues factored in
        /// </summary>
        public uint GetEffectiveMagicDefense()
        {
            var current = GetCreatureSkill(Skill.MagicDefense).Current;
            var weaponDefenseMod = GetWeaponMagicDefenseModifier(this);
            var defenseImbues = (uint)GetDefenseImbues(ImbuedEffectType.MagicDefense);

            var effectiveMagicDefense = (uint)Math.Round((current * weaponDefenseMod) + defenseImbues);

            //Console.WriteLine($"EffectiveMagicDefense: {effectiveMagicDefense}");

            return effectiveMagicDefense;
        }
    }
}
