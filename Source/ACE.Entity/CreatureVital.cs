using System;

using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class CreatureVital : CreatureAbility
    {
        // because health/stam/mana values are determined from stats, we need a reference to the WeenieCreatureData
        // so we can calculate.  this could be refactored into a better pattern, but it will do for now.
        private ICreatureStats creature;

        private double lastTick = double.NegativeInfinity;

        public double RegenRate { set; get; }

        new public uint UnbuffedValue
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

        new public uint MaxValue { get { return UnbuffedValue; } }

        /// <summary>
        /// only applies to Health/Stam/Mana
        /// </summary>
        public uint Current { get; set; }

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
            }
        }

        public CreatureVital(ICreatureStats creature, Ability ability, double regenRate) : base(ability)
        {
            this.creature = creature;
            this.RegenRate = regenRate;
        }
    }
}
