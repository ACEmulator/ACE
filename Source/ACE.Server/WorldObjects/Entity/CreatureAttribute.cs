using System;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects.Entity
{
    public class CreatureAttribute
    {
        private readonly Creature creature;
        //public readonly Ability Ability;
        public readonly PropertyAttribute Attribute;

        // This is the underlying database record
        private readonly BiotaPropertiesAttribute biotaPropertiesAttribute;

        /// <summary>
        /// If the creatures biota does not contain this attribute, a new record will be created.
        /// </summary>
        public CreatureAttribute(Creature creature, PropertyAttribute attribute)
        {
            this.creature = creature;
            Attribute = attribute;

            biotaPropertiesAttribute = creature.Biota.BiotaPropertiesAttribute.FirstOrDefault(x => x.Type == (uint)Attribute);

            if (biotaPropertiesAttribute == null)
            {
                biotaPropertiesAttribute = new BiotaPropertiesAttribute { ObjectId = creature.Biota.Id, Type = (ushort)Attribute };
                creature.Biota.BiotaPropertiesAttribute.Add(biotaPropertiesAttribute);
            }
        }

        /// <summary>
        /// Total Experience Spent on an attribute
        /// </summary>
        public uint ExperienceSpent
        {
            get => biotaPropertiesAttribute.CPSpent;
            set => biotaPropertiesAttribute.CPSpent = value;
        }

        /// <summary>
        /// Returns the Base Value for a Creature's attribute, for Players this is set durring Character Creation 
        /// </summary>
        public uint StartingValue
        {
            get => biotaPropertiesAttribute.InitLevel;
            set => biotaPropertiesAttribute.InitLevel = value;
        }

        /// <summary>
        /// Returns the Current Rank for a Creature's attribute
        /// </summary>
        public uint Ranks
        {
            get => biotaPropertiesAttribute.LevelFromCP;
            set => biotaPropertiesAttribute.LevelFromCP = value;
        }

        /// <summary>
        /// Returns the Base Value Plus the Ranked Value
        /// </summary>
        public uint Base
        {
            get
            {
                uint total = Ranks + StartingValue;

                // TODO: augs

                return total;
            }
        }

        public uint Current
        {
            get
            {
                var total = (int)Base;

                var attributeMod = creature.EnchantmentManager.GetAttributeMod(Attribute);
                total += attributeMod;

                return (uint)Math.Max(total, 10);    // minimum value for an attribute: 10
            }
        }

        public ModifierType ModifierType
        {
            get
            {
                var attrMod = creature.EnchantmentManager.GetAttributeMod(Attribute);

                if (attrMod > 0)
                    return ModifierType.Buffed;
                else if (attrMod < 0)
                    return ModifierType.Debuffed;
                else
                    return ModifierType.None;
            }
        }
    }
}
