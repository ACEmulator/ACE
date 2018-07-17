using System;
using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.Server.WorldObjects.Entity;

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
            var items = new List<WorldObject>();

            if ((target as Player) != null)
                items = (target as Player).GetAllWieldedItems();

            CreatureSkill mc = caster.GetCreatureSkill(Skill.ManaConversion);
            double z = mc.Current;
            double baseManaPercent = 1;
            if (z > spell.Power)
            {
                baseManaPercent = spell.Power / z;
            }
            double preCost = 0;
            uint manaUsed = 0;
            if ((int)Math.Floor(baseManaPercent) == 1)
            {
                preCost = spell.BaseMana;
                manaUsed = (uint)preCost;
            }
            else
            {
                if ((spell.School == MagicSchool.ItemEnchantment) && (spell.MetaSpellType == SpellType.Enchantment))
                {
                    int count = 1;
                    if ((target as Player) != null)
                        count = items.Count;

                    preCost = (spell.BaseMana + (spell.ManaMod * items.Count)) * baseManaPercent;
                }
                else
                    preCost = spell.BaseMana * baseManaPercent;
                if (preCost < 1)
                    preCost = 1;
                manaUsed = (uint)Physics.Common.Random.RollDice(1, (int)preCost);
            }

            return manaUsed;
        }
    }
}
