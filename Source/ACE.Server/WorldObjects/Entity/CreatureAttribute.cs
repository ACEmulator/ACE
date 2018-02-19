
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;

namespace ACE.Server.WorldObjects.Entity
{
    public class CreatureAttribute
    {
        private readonly Creature creature;
        public readonly Ability Ability;

        // This is the underlying database record
        private readonly BiotaPropertiesAttribute biotaPropertiesAttribute;

        public CreatureAttribute(Creature creature, Ability ability)
        {
            this.creature = creature;
            Ability = ability;

            biotaPropertiesAttribute = creature.Biota.GetAttribute(ability);
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
