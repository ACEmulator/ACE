using System;

using ACE.Entity.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ACE.Entity
{
    public class CreatureVital : ICloneable, ICreatureXpSpendableStat
    {
        private AceObjectPropertiesAttribute2nd _backer;

        // because health/stam/mana values are determined from stats, we need a reference to the WeenieCreatureData
        // so we can calculate.  this could be refactored into a better pattern, but it will do for now.
        private ICreatureStats creature;

        private double lastTick = double.NegativeInfinity;

        [JsonIgnore]
        public double RegenRate { set; get; }

        [JsonProperty("ability")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Ability Ability
        {
            get { return (Ability)_backer.Attribute2ndId; }
            protected set
            {
                _backer.Attribute2ndId = (ushort)value;
            }
        }

        [JsonProperty("startingValue")]
        public uint Base { get; set; }

        [JsonProperty("ranks")]
        public uint Ranks
        {
            get { return _backer.Attribute2ndRanks; }
            set
            {
                _backer.Attribute2ndRanks = (ushort)value;
                _backer.IsDirty = true;
            }
        }

        /// <summary>
        /// Total Experience Spent on an ability
        /// </summary>
        [JsonProperty("experienceSpent")]
        public uint ExperienceSpent
        {
            get { return _backer.Attribute2ndXpSpent; }
            set
            {
                _backer.Attribute2ndXpSpent = value;
                _backer.IsDirty = true;
            }
        }

        [JsonProperty("baseValue")]
        public uint UnbuffedValue
        {
            get
            {
                // TODO: buffs?  not sure where they will go
                // TODO: check if this can be replaced with Ability.GetFormula().CalcBase(creature)
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
                    abilityTotal = (uint)Math.Ceiling((double)derivationTotal / (double)formula.Divisor);
                }

                abilityTotal += this.Ranks + this.Base;

                return abilityTotal;
            }
        }

        [JsonIgnore]
        public uint MaxValue { get { return UnbuffedValue; } }

        /// <summary>
        /// only applies to Health/Stam/Mana
        /// </summary>
        [JsonIgnore]
        public uint Current
        {
            get { return _backer.Attribute2ndValue; }
            set
            {
                _backer.Attribute2ndValue = (ushort)value;
                _backer.IsDirty = true;
            }
        }

        [JsonIgnore]
        public double NextTickTime {
            get
            {
                if (lastTick == double.NegativeInfinity)
                {
                    return double.NegativeInfinity;
                }
                return lastTick + RegenRate;
            }
        }

        /// <summary>
        /// Used to determine if health/stamina/mana updates need to be sent periodically
        /// Returns the "last time" the vitals were updated
        /// Takes the vital to update, the lastTime it was updated, and the update rate
        /// </summary>
        public void Tick(double tickTime)
        {
            if (lastTick == double.NegativeInfinity)
            {
                lastTick = tickTime;
                return;
            }

            // This shouldn't happen?  maybe?
            if (tickTime <= lastTick)
            {
                return;
            }

            double timeDiff = tickTime - lastTick;

            uint numTicks = (uint)(timeDiff * RegenRate);

            if (numTicks > 0)
            {
                // lastTime is the time at which the last tick would have happened
                lastTick = lastTick + numTicks / RegenRate;

                // Now, update our value
                Current = System.Math.Min(MaxValue, Current + numTicks);

                // Reset last tick, so when we resume we start ticking properly
                if (Current == MaxValue)
                {
                    lastTick = double.NegativeInfinity;
                }
            }
        }

        public CreatureVital(ICreatureStats creature, Ability ability, double regenRate)
        {
            this._backer = new AceObjectPropertiesAttribute2nd();
            this.creature = creature;
            this.RegenRate = regenRate;
            Ability = ability;
        }

        public CreatureVital(ICreatureStats creature, AceObjectPropertiesAttribute2nd props) 
        {
            this._backer = props;
            this.creature = creature;
            this.RegenRate = Ability.GetRegenRate();
        }

        public AceObjectPropertiesAttribute2nd GetVital()
        {
            return _backer;
        }

        public void ClearDirtyFlags()
        {
            _backer.IsDirty = false;
            _backer.HasEverBeenSavedToDatabase = true;
        }

        public void SetDirtyFlags()
        {
            _backer.IsDirty = true;
            _backer.HasEverBeenSavedToDatabase = false;
        }

        public object Clone()
        {
            return new CreatureVital(creature, (AceObjectPropertiesAttribute2nd)_backer.Clone());
        }
    }
}
