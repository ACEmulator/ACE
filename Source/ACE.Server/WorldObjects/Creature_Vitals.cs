using System;
using System.Collections.Generic;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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

            if (vital.RegenRate == 0.0) return;

            // take attributes into consideration (strength, endurance)
            var attributeMod = GetAttributeMod(vital);

            // take stance into consideration (combat, crouch, sitting, sleeping)
            var stanceMod = GetStanceMod(vital);

            // take enchantments into consideration:
            // (regeneration / rejuvenation / mana renewal / etc.)
            var enchantmentMod = EnchantmentManager.GetRegenerationMod(vital);

            // cap rate?
            var currentTick = vital.RegenRate * attributeMod * stanceMod * enchantmentMod;

            // add in partially accumulated / rounded vitals from previous tick(s)
            var totalTick = currentTick + vital.PartialRegen;

            // accumulate partial vital rates between ticks
            var intTick = (int)totalTick;
            vital.PartialRegen = totalTick - intTick;

            if (intTick > 0)
                UpdateVitalDelta(vital, intTick);

            //Console.WriteLine($"VitalTick({vital.Vital.ToSentence()}): attributeMod={attributeMod}, stanceMod={stanceMod}, enchantmentMod={enchantmentMod}, regenRate={vital.RegenRate}, currentTick={currentTick}, totalTick={totalTick}, accumulated={vital.PartialRegen}");
        }

        /// <summary>
        /// Returns the vital regeneration modifier based on attributes
        /// (strength, endurance for health, stamina)
        /// </summary>
        public float GetAttributeMod(CreatureVital vital)
        {
            // only applies to players
            if ((this as Player) == null) return 1.0f;

            // only applies for health?
            if (vital.Vital != PropertyAttribute2nd.MaxHealth) return 1.0f;

            // The combination of strength and endurance (with endurance being more important) allows one to regenerate hit points 
            // at a faster rate the higher one's endurance is. This bonus is in addition to any regeneration spells one may have placed upon themselves.
            // This regeneration bonus caps at around 110%.

            var strength = GetCreatureAttribute(PropertyAttribute.Strength).Base;
            var endurance = GetCreatureAttribute(PropertyAttribute.Endurance).Base;

            var strAndEnd = strength + (endurance * 2);

            var modifier = 1.0 + (0.0494 * Math.Pow(strAndEnd, 1.179) / 100.0f);    // formula deduced from values present in the client pdb
            var attributeMod = Math.Clamp(modifier, 1.0, 2.1);      // cap between + 0-110%

            return (float)attributeMod;
        }

        /// <summary>
        /// Returns the vital regeneration modifier based on player stance
        /// (combat, crouch, sitting, sleeping)
        /// </summary>
        public float GetStanceMod(CreatureVital vital)
        {
            // only applies to players
            if ((this as Player) == null) return 1.0f;

            // does not apply for mana?
            if (vital.Vital == PropertyAttribute2nd.MaxMana) return 1.0f;

            // combat mode / running
            if (CombatMode != CombatMode.NonCombat || CurrentMotionCommand == (uint)MotionCommand.RunForward)
                return 0.5f;

            switch (CurrentMotionCommand)
            {
                // TODO: verify multipliers
                default:
                    return 1.0f;
                case 0x12:  // MotionCommand.Crouch
                    return 2.0f;
                case 0x13:  // MotionCommand.Sitting
                    return 2.5f;
                case 0x14:  // MotionCommand.Sleeping
                    return 3.0f;
            }
        }
    }
}
