using System;
using System.Linq;
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

            uint mana_conversion_skill = (uint)Math.Round(caster.GetCreatureSkill(Skill.ManaConversion).Current * GetWeaponManaConversionModifier(caster));

            uint difficulty = spell.PowerMod;   // modified power difficulty

            double baseManaPercent = 1.0;
            if (mana_conversion_skill > difficulty)
                baseManaPercent = (double)difficulty / mana_conversion_skill;

            uint preCost = 0;

            if ((spell.School == MagicSchool.ItemEnchantment) && (spell.MetaSpellType == SpellType.Enchantment) &&
                (spell.Category >= SpellCategory.ArmorValueRaising) && (spell.Category <= SpellCategory.AcidicResistanceLowering) && target is Player targetPlayer)
            {
                int numTargetItems = 1;
                if (targetPlayer != null)
                    numTargetItems = targetPlayer.EquippedObjects.Values.Count(i => (i is Clothing || i.IsShield) && i.IsEnchantable);

                preCost = (uint)Math.Round((baseCost + (spell.ManaMod * numTargetItems)) * baseManaPercent);
            }
            else if ((spell.Flags & SpellFlags.FellowshipSpell) != 0)
            {
                int numFellows = 1;
                var player = this as Player;
                if (player != null && player.Fellowship != null)
                    numFellows = player.Fellowship.FellowshipMembers.Count;

                preCost = (uint)Math.Round((baseCost + (spell.ManaMod * numFellows)) * baseManaPercent);
            }
            else
                preCost = (uint)Math.Round(baseCost * baseManaPercent);

            if (preCost < 1) preCost = 1;

            uint manaUsed = ThreadSafeRandom.Next(1, preCost);
            return manaUsed;
        }

        /// <summary>
        /// Handles an item casting a spell on player or creature
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

                    enchantmentStatus = CreatureMagic(this, spell, item);
                    if (enchantmentStatus.Message != null)
                        EnqueueBroadcast(new GameMessageScript(Guid, spell.TargetEffect, spell.Formula.Scale));

                    break;

                case MagicSchool.LifeMagic:

                    LifeMagic(this, spell, out uint damage, out bool critical, out enchantmentStatus, item);
                    if (enchantmentStatus.Message != null)
                        EnqueueBroadcast(new GameMessageScript(Guid, spell.TargetEffect, spell.Formula.Scale));

                    break;

                case MagicSchool.ItemEnchantment:

                    if (spell.HasItemCategory || spell.IsPortalSpell)
                        enchantmentStatus = ItemMagic(this, spell, item);
                    else
                        enchantmentStatus = ItemMagic(item, spell, item);

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
            if (target.EnchantmentManager.HasSpell(spellId))
            {
                if (!silent)
                    target.EnchantmentManager.Remove(target.EnchantmentManager.GetEnchantment(spellId, item.Guid.Full));
                else
                    target.EnchantmentManager.Dispel(target.EnchantmentManager.GetEnchantment(spellId, item.Guid.Full));
            }
        }
    }
}
