using System;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class CreatureAbility : ICloneable
    {
        public Ability Ability { get; protected set; }

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
        virtual public uint UnbuffedValue
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
        virtual public uint MaxValue
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
            Base = 10;
        }

        public CreatureAbility(AceObjectPropertiesAttribute attrib)
        {
            Ability = (Ability)attrib.AttributeId;
            Ranks = attrib.AttributeRanks;
            Base = attrib.AttributeBase;
            ExperienceSpent = attrib.AttributeXpSpent;
        }

        public AceObjectPropertiesAttribute GetAttribute(uint objId)
        {
            var ret = new AceObjectPropertiesAttribute();

            ret.AceObjectId = objId;
            ret.AttributeId = (ushort)Ability;
            ret.AttributeBase = (ushort)Base;
            ret.AttributeRanks = (ushort)Ranks;
            ret.AttributeXpSpent = ExperienceSpent;

            return ret;
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
