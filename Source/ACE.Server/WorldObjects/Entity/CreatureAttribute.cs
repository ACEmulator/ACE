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

            biotaPropertiesAttribute = creature.Biota.GetAttribute(Attribute);

            if (biotaPropertiesAttribute == null)
            {
                creature.Biota.BiotaPropertiesAttribute.Add(new BiotaPropertiesAttribute {ObjectId = creature.Biota.Id, Type = (ushort)Attribute });
                biotaPropertiesAttribute = creature.Biota.GetAttribute(Attribute);
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
                uint total = Base;

                // TODO: add buffs

                return total;
            }
        }
    }
}
