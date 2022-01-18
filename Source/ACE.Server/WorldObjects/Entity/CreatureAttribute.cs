using System;

using ACE.Common.Extensions;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;

namespace ACE.Server.WorldObjects.Entity
{
    public class CreatureAttribute
    {
        private readonly Creature creature;

        public readonly PropertyAttribute Attribute;

        // the underlying database record
        private readonly PropertiesAttribute propertiesAttribute;

        /// <summary>
        /// If the creature's biota does not contain this attribute, a new record will be created.
        /// </summary>
        public CreatureAttribute(Creature creature, PropertyAttribute attribute)
        {
            this.creature = creature;
            Attribute = attribute;

            if (!creature.Biota.PropertiesAttribute.TryGetValue(attribute, out propertiesAttribute))
            {
                propertiesAttribute = new PropertiesAttribute();
                creature.Biota.PropertiesAttribute[attribute] = propertiesAttribute;
            }
        }

        /// <summary>
        /// Returns the Base Value for a Creature's attribute, for Players this is set during Character Creation 
        /// </summary>
        public uint StartingValue
        {
            get => propertiesAttribute.InitLevel;
            set => propertiesAttribute.InitLevel = value;
        }

        /// <summary>
        /// Total Experience Spent on an attribute
        /// </summary>
        public uint ExperienceSpent
        {
            get => propertiesAttribute.CPSpent;
            set => propertiesAttribute.CPSpent = value;
        }

        /// <summary>
        /// Returns the amount of attribute experience remaining
        /// until max rank is reached
        /// </summary>
        public uint ExperienceLeft
        {
            get
            {
                var attributeXPTable = DatManager.PortalDat.XpTable.AttributeXpList;

                return attributeXPTable[attributeXPTable.Count - 1] - ExperienceSpent;
            }
        }

        /// <summary>
        /// The number of levels an attribute has been raised,
        /// derived from ExperienceSpent
        /// </summary>
        public uint Ranks
        {
            get => propertiesAttribute.LevelFromCP;
            set => propertiesAttribute.LevelFromCP = value;
        }

        /// <summary>
        /// Returns TRUE if this attribute has been raised the maximum # of times
        /// </summary>
        public bool IsMaxRank
        {
            get
            {
                var attributeXPTable = DatManager.PortalDat.XpTable.AttributeXpList;

                return Ranks >= (attributeXPTable.Count - 1);
            }
        }

        /// <summary>
        /// Returns the Base Value Plus the Ranked Value
        /// </summary>
        public uint Base
        {
            get
            {
                uint total = Ranks + StartingValue;

                // TODO: cap at 10x of these augs across the board elsewhere
                // verify this with client formula

                // NOTE: this has been moved to InitLevel

                /*if (creature is Player player)
                {
                    switch (Attribute)
                    {
                        case PropertyAttribute.Strength:
                            if (player.AugmentationInnateStrength > 0)
                                total += (uint)(player.AugmentationInnateStrength * 5);
                            break;
                        case PropertyAttribute.Endurance:
                            if (player.AugmentationInnateEndurance > 0)
                                total += (uint)(player.AugmentationInnateEndurance * 5);
                            break;
                        case PropertyAttribute.Coordination:
                            if (player.AugmentationInnateCoordination > 0)
                                total += (uint)(player.AugmentationInnateCoordination * 5);
                            break;
                        case PropertyAttribute.Quickness:
                            if (player.AugmentationInnateQuickness > 0)
                                total += (uint)(player.AugmentationInnateQuickness * 5);
                            break;
                        case PropertyAttribute.Focus:
                            if (player.AugmentationInnateFocus > 0)
                                total += (uint)(player.AugmentationInnateFocus * 5);
                            break;
                        case PropertyAttribute.Self:
                            if (player.AugmentationInnateSelf > 0)
                                total += (uint)(player.AugmentationInnateSelf * 5);
                            break;
                    }
                }*/

                return total;
            }
        }

        public uint Current => GetCurrent(true);

        public uint GetCurrent(bool enchanted)
        {
            var multipliers = enchanted ? creature.EnchantmentManager.GetAttributeMod_Multiplier(Attribute) : 1.0f;
            var additives = enchanted ? creature.EnchantmentManager.GetAttributeMod_Additive(Attribute) : 0;

            var total = (int)Base * multipliers + additives;

            total = total.Round();

            // attributes cannot be debuffed below 10 normally,
            // or 1 for creatures with very low starting attributes
            var minimumAttribute = Base >= 10 ? 10 : 1;

            return (uint)Math.Max(minimumAttribute, total);
        }

        public ModifierType ModifierType
        {
            get
            {
                var diff = (int)GetCurrent(true) - (int)GetCurrent(false);

                if (diff > 0)
                    return ModifierType.Buffed;
                else if (diff < 0)
                    return ModifierType.Debuffed;
                else
                    return ModifierType.None;
            }
        }
    }
}
