﻿using System;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class CreatureAbility : ICloneable
    {
        // because health/stam/mana values are determined from stats, we need a reference to the WeenieCreatureData
        // so we can calculate.  this could be refactored into a better pattern, but it will do for now.
        private ICreatureStats creature;

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
        /// only applies to Health/Stam/Mana
        /// </summary>
        public uint Current { get; set; }

        /// <summary>
        /// For Primary Abilities, Returns the Base Value Plus the Ranked Value
        /// For Secondary Abilities, Returns the adjusted Value depending on the current Abiliy formula
        /// </summary>
        public uint UnbuffedValue
        {
            get
            {
                // TODO: buffs?  not sure where they will go

                var formula = this.Ability.GetFormula();

                uint derivationTotal = 0;
                uint abilityTotal = 0;

                if (formula != null)
                {
                    // restricted to endurance and self because those are the only 2 used by abilities

                    Ability abilities = formula.Abilities;
                    uint end = (uint)((abilities & Ability.Endurance) > 0 ? 1 : 0);
                    uint wil = (uint)((abilities & Ability.Self) > 0 ? 1 : 0);

                    derivationTotal += end * this.creature.Endurance;
                    derivationTotal += wil * this.creature.Self;

                    derivationTotal *= formula.AbilityMultiplier;
                    abilityTotal = derivationTotal / formula.Divisor;
                }

                abilityTotal += this.Ranks + this.Base;

                return abilityTotal;
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

        public CreatureAbility(ICreatureStats creature, Ability ability)
        {
            this.creature = creature;
            Ability = ability;
            Base = 10;
        }

        public CreatureAbility(ICreatureStats stats, AceObjectPropertiesAttribute attrib)
        {
            this.creature = stats;
            Ability = (Ability)attrib.AttributeId;
            Ranks = attrib.AttributeRanks;
            Base = attrib.AttributeBase;
            ExperienceSpent = attrib.AttributeXpSpent;
        }

        public CreatureAbility(ICreatureStats stats, AceObjectPropertiesAttribute2nd vital)
        {
            this.creature = stats;
            Ability = (Ability)vital.Attribute2ndId;
            Ranks = vital.Attribute2ndRanks;
            Current = vital.Attribute2ndValue;
            ExperienceSpent = vital.Attribute2ndXpSpent;
            Base = 0;
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

        public AceObjectPropertiesAttribute2nd GetVital(uint objId)
        {
            var ret = new AceObjectPropertiesAttribute2nd();

            ret.AceObjectId = objId;
            ret.Attribute2ndId = (ushort)Ability;
            ret.Attribute2ndValue = Current;
            ret.Attribute2ndRanks = (ushort)Ranks;
            ret.Attribute2ndXpSpent = ExperienceSpent;

            return ret;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
