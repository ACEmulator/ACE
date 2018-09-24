using System;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.Database;
using ACE.Server.Entity;
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
            bool targetSelf = false || (int)Math.Floor(spell.BaseRangeConstant) == 0;
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
            uint mana_conversion_skill = caster.GetCreatureSkill(Skill.ManaConversion).Current;
            uint difficulty = spell.PowerMod;   // modified power difficulty

            double baseManaPercent = 1.0;
            if (mana_conversion_skill > difficulty)
                baseManaPercent = (double)difficulty / mana_conversion_skill;

            uint preCost = 0;

            if ((spell.School == MagicSchool.ItemEnchantment) && (spell.MetaSpellType == SpellType.Enchantment))
            {
                var targetPlayer = target as Player;

                int numTargetItems = 1;
                if (targetPlayer != null)
                {
                    var targetItems = targetPlayer.GetAllWieldedItems();
                    numTargetItems = targetItems.Count;
                }
                preCost = (uint)Math.Round((spell.BaseMana + (spell.ManaMod * numTargetItems)) * baseManaPercent);
            }
            else
                preCost = (uint)Math.Round(spell.BaseMana * baseManaPercent);

            if (preCost < 1) preCost = 1;

            uint manaUsed = Physics.Common.Random.RollDice(1, preCost);
            return manaUsed;
        }
    }
}
