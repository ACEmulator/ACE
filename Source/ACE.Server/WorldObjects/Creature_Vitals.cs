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

        public void DeltaVital(CreatureVital vital, long delta)
        {
            EnqueueAction(new ActionEventDelegate(() => DeltaVitalInternal(vital, delta)));
        }

        private void DeltaVitalInternal(CreatureVital vital, long delta)
        {
            uint absVal;

            if (delta < 0 && Math.Abs(delta) > vital.Current)
                absVal = (uint)(-1 * vital.Current);
            else if (delta + vital.Current > vital.MaxValue)
                absVal = vital.MaxValue - vital.Current;
            else
                absVal = (uint)(vital.Current + delta);

            UpdateVitalInternal(vital, absVal);
        }

        /// <summary>
        /// Updates a vital, returns true if vital is now &lt; max
        /// </summary>
        public void UpdateVital(CreatureVital vital, uint newVal)
        {
            EnqueueAction(new ActionEventDelegate(() => UpdateVitalInternal(vital, newVal)));
        }

        protected virtual void UpdateVitalInternal(CreatureVital vital, uint newVal)
        {
            uint old = vital.Current;

            if (newVal > vital.MaxValue)
                newVal = (vital.MaxValue - vital.Current);

            vital.Current = newVal;

            // Check for amount
            if (vital.Current != vital.MaxValue)
            {
                // Start up a vital ticker
                new ActionChain(this, () => VitalTickInternal(vital)).EnqueueChain();
            }
        }

        private void VitalTick(CreatureVital vital)
        {
            double tickTime = vital.NextTickTime;

            if (double.IsNegativeInfinity(tickTime))
                tickTime = vital.RegenRate;
            else
                tickTime -= WorldManager.PortalYearTicks;

            // Set up our next tick
            ActionChain tickChain = new ActionChain();
            tickChain.AddDelayTicks(tickTime);
            tickChain.AddAction(this, () => VitalTickInternal(vital));
            tickChain.EnqueueChain();
        }

        protected virtual void VitalTickInternal(CreatureVital vital)
        {
            vital.Tick(WorldManager.PortalYearTicks);

            if (vital.Current != vital.MaxValue)
                VitalTick(vital);
        }
    }
}
