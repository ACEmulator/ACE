using System;
using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.Database;
using ACE.Server.WorldObjects.Entity;
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

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                creature.IsBusy = false;
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            bool targetSelf = false || (int)Math.Floor(spell.BaseRangeConstant) == 0;
            var target = targetSelf ? this : CurrentLandblock?.GetObject(guidTarget);

            Database.Models.World.Spell spellStatMod = DatabaseManager.World.GetCachedSpell(spellId);
            if (spellStatMod == null)
            {
                creature.IsBusy = false;
                return;
            }

            float scale = SpellAttributes(null, spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            switch (spell.School)
            {
                case MagicSchool.ItemEnchantment:
                    // if (!targetSelf && ResistSpell(Skill.CreatureEnchantment)) break;
                    ItemMagic(target, spell, spellStatMod);
                    EnqueueBroadcast(new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
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
            Creature creature = CurrentLandblock?.GetObject(Guid) as Creature;

            if (creature.IsBusy == true)
                return;
            else
                creature.IsBusy = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                creature.IsBusy = false;
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            float scale = SpellAttributes(null, spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            creature.IsBusy = false;
            return;
        }

        public uint CalculateManaUsage(Creature caster, SpellBase spell, WorldObject target = null)
        {
            uint mana_conversion_skill = caster.GetCreatureSkill(Skill.ManaConversion).Current;
            uint difficulty = Math.Max(25, spell.Power);     // for mana conversion only?

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
