using ACE.Common.Extensions;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Calculates the amount to add to a creature's current skills and vitals,
    /// based on their primary attribute current values
    /// </summary>
    public static class AttributeFormula
    {
        /// <summary>
        /// Returns the amount to add to a creature's current skill,
        /// based on their primary attribute current values
        /// </summary>
        public static uint GetFormula(Creature creature, Skill skill)
        {
            var skillTable = DatManager.PortalDat.SkillTable;

            if (!skillTable.SkillBaseHash.TryGetValue((uint)skill, out SkillBase skillBase))
                return 0;

            return GetFormula(creature, skillBase.Formula);
        }

        /// <summary>
        /// Returns the amount to add to a creature's current vital,
        /// based on their primary attribute current values
        /// </summary>
        public static uint GetFormula(Creature creature, PropertyAttribute2nd vital)
        {
            var vitalTable = DatManager.PortalDat.SecondaryAttributeTable;

            switch (vital)
            {
                case PropertyAttribute2nd.MaxHealth:
                    return GetFormula(creature, vitalTable.MaxHealth.Formula);
                case PropertyAttribute2nd.MaxStamina:
                    return GetFormula(creature, vitalTable.MaxStamina.Formula);
                case PropertyAttribute2nd.MaxMana:
                    return GetFormula(creature, vitalTable.MaxMana.Formula);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Applies a SkillFormula from the portal.dat,
        /// using the primary attributes for a creature
        /// </summary>
        public static uint GetFormula(Creature creature, DatLoader.Entity.SkillFormula formula)
        {
            if (formula.X == 0) return 0;

            var attr1 = (PropertyAttribute)formula.Attr1;
            var attr2 = (PropertyAttribute)formula.Attr2;
            var divisor = formula.Z;

            var total = creature.GetCreatureAttribute(attr1).Current;
            if (attr2 != PropertyAttribute.Undef)
                total += creature.GetCreatureAttribute(attr2).Current;

            if (divisor != 1)
                total = (uint)((float)total / divisor).Round();

            return total;
        }
    }
}
