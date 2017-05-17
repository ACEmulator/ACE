using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class CreatureAbility
    {
        public Ability Ability { get; private set; }

        /// <summary>
        /// Returns the Base Value for a Creature's Ability, for Players this is set durring Character Creation 
        /// </summary>
        public uint Base { get; set; }

        /// <summary>
        /// Returns the Current Rank for a Creature's Ability
        /// </summary>
        public uint Ranks { get; set; }

        /// <summary>
        /// For Primary Abilities, Returns the Base Value Plus the Ranked Value
        /// For Secondary Abilities, Returns the adjusted Value depending on the current Abiliy formula
        /// </summary>
        public uint UnbuffedValue
        {
            get
            {
                // TODO: buffs?  not sure where they will go
                return this.Ranks + this.Base;
            }
        }

        /// <summary>
        /// Returns the MaxValue of an ability, UnbuffedValue + Additional
        /// </summary>
        public uint MaxValue
        {
            get
            {
                // TODO: once buffs are implemented, make sure we have a max Value wich calculates the buffs in, 
                // as it's needed for the UpdateHealth GameMessage. For now this is just the unbuffed value.

                return UnbuffedValue;
            }
        }

        /// <summary>
        /// Total Experience Spent on an ability
        /// </summary>
        public uint ExperienceSpent { get; set; }

        public CreatureAbility(Ability ability)
        {
            Ability = ability;
        }
    }
}
