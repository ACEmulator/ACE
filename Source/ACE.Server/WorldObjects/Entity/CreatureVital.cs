using System;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects.Entity
{
    public class CreatureVital
    {
        private readonly Creature creature;
        //public readonly Ability Ability;
        //public readonly PropertyAttribute Attribute;
        public readonly PropertyAttribute2nd Vital;

        // This is the underlying database record
        private readonly BiotaPropertiesAttribute2nd biotaPropertiesAttribute2nd;

        /// <summary>
        /// If the creatures biota does not contain this vital, a new record will be created.
        /// </summary>
        public CreatureVital(Creature creature, PropertyAttribute2nd vital)
        {
            this.creature = creature;
            Vital = vital;

            biotaPropertiesAttribute2nd = creature.Biota.GetAttribute2nd(Vital);

            if (biotaPropertiesAttribute2nd == null)
            {
                creature.Biota.BiotaPropertiesAttribute2nd.Add(new BiotaPropertiesAttribute2nd { ObjectId = creature.Biota.Id, Type = (ushort)Vital });
                biotaPropertiesAttribute2nd = creature.Biota.GetAttribute2nd(Vital);
            }

            switch (Vital)
            {
                case PropertyAttribute2nd.MaxHealth:
                    RegenRate = creature.GetProperty(PropertyFloat.HealthRate) ?? 0;
                    break;
                case PropertyAttribute2nd.MaxStamina:
                    RegenRate = creature.GetProperty(PropertyFloat.StaminaRate) ?? 0;
                    break;
                case PropertyAttribute2nd.MaxMana:
                    RegenRate = creature.GetProperty(PropertyFloat.ManaRate) ?? 0;
                    break;
            }
        }

        /// <summary>
        /// Total Experience Spent on an attribute
        /// </summary>
        public uint ExperienceSpent
        {
            get => biotaPropertiesAttribute2nd.CPSpent;
            set => biotaPropertiesAttribute2nd.CPSpent = value;
        }

        public uint StartingValue
        {
            get => biotaPropertiesAttribute2nd.InitLevel;
            set => biotaPropertiesAttribute2nd.InitLevel = value;
        }

        public uint Ranks
        {
            get => biotaPropertiesAttribute2nd.LevelFromCP;
            set => biotaPropertiesAttribute2nd.LevelFromCP = value;
        }

        /// <summary>
        /// Returns the adjusted Value depending on the current attribute formula
        /// </summary>
        public uint Base
        {
            get
            {
                var formula = Vital.GetFormula();

                uint derivationTotal = 0;
                uint total = 0;

                if (formula != null)
                {
                    // restricted to endurance and self because those are the only 2 used by vitals

                    var attributeCache = formula.AttributeCache;
                    uint end = (uint)((attributeCache & AttributeCache.Endurance) > 0 ? 1 : 0);
                    uint wil = (uint)((attributeCache & AttributeCache.Self) > 0 ? 1 : 0);

                    derivationTotal += end * creature.Endurance.Current;
                    derivationTotal += wil * creature.Self.Current;

                    derivationTotal *= formula.AbilityMultiplier;
                    total = (uint)Math.Ceiling((double)derivationTotal / (double)formula.Divisor);
                }

                total += StartingValue + Ranks;

                return total;
            }
        }

        public uint Current
        {
            get => biotaPropertiesAttribute2nd.CurrentLevel;
            set => biotaPropertiesAttribute2nd.CurrentLevel = value;
        }

        public uint MaxValue
        {
            get
            {
                uint total = Base;

                // TODO: include all buffs
                if (creature is Player)
                {
                    var player = creature as Player;
                    if (player.HasVitae)
                        total = (uint)Math.Round(total * player.Vitae);
                }
                return total;
            }
        }

        public ModifierType ModifierType
        {
            get
            {
                if (Vital == PropertyAttribute2nd.MaxHealth || Vital == PropertyAttribute2nd.MaxStamina)
                    return creature.Endurance.ModifierType;
                else if (Vital == PropertyAttribute2nd.MaxMana)
                    return creature.Self.ModifierType;
                else
                    return ModifierType.None;
            }
        }


        public double RegenRate { set; get; }

        private double lastTick = double.NegativeInfinity;

        public double NextTickTime
        {
            get
            {
                if (lastTick == double.NegativeInfinity)
                    return double.NegativeInfinity;

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
                return;

            double timeDiff = tickTime - lastTick;

            uint numTicks = (uint)(timeDiff * RegenRate);

            if (numTicks > 0)
            {
                // lastTime is the time at which the last tick would have happened
                lastTick = lastTick + numTicks / RegenRate;

                // Now, update our value
                Current = Math.Min(MaxValue, Current + numTicks);

                // Reset last tick, so when we resume we start ticking properly
                if (Current == MaxValue)
                    lastTick = double.NegativeInfinity;
            }
        }

        public Vital ToEnum()
        {
            switch (Vital)
            {
                case PropertyAttribute2nd.MaxHealth:    return ACE.Entity.Enum.Vital.Health;
                case PropertyAttribute2nd.MaxStamina:   return ACE.Entity.Enum.Vital.Stamina;
                case PropertyAttribute2nd.MaxMana:      return ACE.Entity.Enum.Vital.Mana;
            }
            return ACE.Entity.Enum.Vital.Undefined;
        }
    }
}
