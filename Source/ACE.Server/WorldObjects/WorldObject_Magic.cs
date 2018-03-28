using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public enum CastResult
        {
            SpellTargetInvalid,
            SpellNotImplemented,
            ItemMagicSpellSpellNotImplemented,
            CreatureMagicSpellSpellNotImplemented,
            LifeMagicSpellSpellNotImplemented,
            WarMagicSpellSpellNotImplemented,
            VoidMagicSpellSpellNotImplemented,
            InvalidSpell,
            SpellCastCompleted
        }

        /// <summary>
        /// Method used for handling targeted spell casts
        /// </summary>
        public CastResult CreateSpell(ObjectGuid guidTarget, uint spellId)
        {
            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
                return CastResult.InvalidSpell;

            SpellBase spell = spellTable.Spells[spellId];

            if (guidTarget == null)
                return CastResult.SpellTargetInvalid;

            WorldObject target = CurrentLandblock.GetObject(guidTarget);

            if (target == null)
            {
                return CastResult.ItemMagicSpellSpellNotImplemented;
            }

            if (spell.School == MagicSchool.CreatureEnchantment)
                return CastResult.CreatureMagicSpellSpellNotImplemented;

            if (spell.School == MagicSchool.LifeMagic)
                return CastResult.LifeMagicSpellSpellNotImplemented;

            if (spell.School == MagicSchool.WarMagic)
                return CastResult.WarMagicSpellSpellNotImplemented;

            if (spell.School == MagicSchool.VoidMagic)
                return CastResult.VoidMagicSpellSpellNotImplemented;

            return CastResult.SpellNotImplemented;
        }

        /// <summary>
        /// Method used for handling untargeted spell casts
        /// </summary>
        public CastResult CreateSpell(uint spellId)
        {
            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
                return CastResult.InvalidSpell;

            SpellBase spell = spellTable.Spells[spellId];

            return CastResult.SpellNotImplemented;
        }
    }
}
