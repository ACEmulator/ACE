using System;
using System.Collections.Generic;

using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public readonly Dictionary<PropertyAttribute2nd, CreatureVital> Vitals = new Dictionary<PropertyAttribute2nd, CreatureVital>();

        public CreatureVital Health => Vitals[PropertyAttribute2nd.MaxHealth];
        public CreatureVital Stamina => Vitals[PropertyAttribute2nd.MaxStamina];
        public CreatureVital Mana => Vitals[PropertyAttribute2nd.MaxMana];

        public CreatureVital GetCreatureVital(PropertyAttribute2nd vital)
        {
            Vitals.TryGetValue(vital, out var value);
            return value;
        }

        public uint GetCurrentCreatureVital(PropertyAttribute2nd vital)
        {
            switch (vital)
            {
                case PropertyAttribute2nd.Mana:
                    return Mana.Current;
                case PropertyAttribute2nd.Stamina:
                    return Stamina.Current;
                default:
                    return Health.Current;
            }
        }

        /// <summary>
        /// Sets the current vital to a new value
        /// </summary>
        public virtual void UpdateVital(CreatureVital vital, uint newVal)
        {
            vital.Current = Math.Clamp(newVal, 0, vital.MaxValue);
        }

        /// <summary>
        /// Updates a vital relative to current value
        /// </summary>
        public void UpdateVitalDelta(CreatureVital vital, long delta)
        {
            var newVital = vital.Current + delta;
            newVital = Math.Clamp(newVital, 0, vital.MaxValue);

            UpdateVital(vital, (uint)newVital);
        }

        /// <summary>
        /// Called every ~5 secs to regenerate vitals
        /// </summary>
        public void VitalTick()
        {
            if (Health.Current < Health.MaxValue)
                VitalTick(Health);

            if (Stamina.Current < Stamina.MaxValue)
                VitalTick(Stamina);

            if (Mana.Current < Mana.MaxValue)
                VitalTick(Mana);
        }

        /// <summary>
        /// Updates a particular vital according to regeneration rate
        /// </summary>
        /// <param name="vital">The vital stat to update (health/stamina/mana)</param>
        public void VitalTick(CreatureVital vital)
        {
            if (vital.Current >= vital.MaxValue)
                return;

            var amount = (uint)Math.Ceiling(vital.RegenRate * 0.01f * vital.MaxValue);

            UpdateVitalDelta(vital, amount);
        }
    }
}
