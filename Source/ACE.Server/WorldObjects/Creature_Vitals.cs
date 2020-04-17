using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        public virtual void SetMaxVitals()
        {
            var missingHealth = Health.Missing;

            Health.Current = Health.MaxValue;
            Stamina.Current = Stamina.MaxValue;
            Mana.Current = Mana.MaxValue;

            DamageHistory.OnHeal(missingHealth);
        }

        public CreatureVital GetCreatureVital(PropertyAttribute2nd vital)
        {
            switch (vital)
            {
                case PropertyAttribute2nd.Health:
                    return Health;
                case PropertyAttribute2nd.Stamina:
                    return Stamina;
                case PropertyAttribute2nd.Mana:
                    return Mana;
                default:
                    log.Error($"{Name}.GetCreatureVital({vital}): unexpected vital");
                    return null;
            }
        }

        /// <summary>
        /// Sets the current vital to a new value
        /// </summary>
        /// <returns>The actual change in the vital, after clamping between 0 and MaxVital</returns>
        public virtual int UpdateVital(CreatureVital vital, int newVal)
        {
            var before = vital.Current;
            vital.Current = (uint)Math.Clamp(newVal, 0, vital.MaxValue);
            return (int)(vital.Current - before);
        }

        public virtual int UpdateVital(CreatureVital vital, uint newVal)
        {
            return UpdateVital(vital, (int)newVal);
        }

        /// <summary>
        /// Updates a vital relative to current value
        /// </summary>
        public int UpdateVitalDelta(CreatureVital vital, int delta)
        {
            var newVital = (int)vital.Current + delta;

            return UpdateVital(vital, newVital);
        }

        public int UpdateVitalDelta(CreatureVital vital, uint delta)
        {
            return UpdateVitalDelta(vital, (int)delta);
        }

        /// <summary>
        /// Called every ~5 secs to regenerate vitals
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool VitalHeartBeat()
        {
            if (IsDead)
                return false;

            var vitalUpdate = false;

            vitalUpdate |= VitalHeartBeat(Health);

            vitalUpdate |= VitalHeartBeat(Stamina);

            vitalUpdate |= VitalHeartBeat(Mana);

            return vitalUpdate;
        }

        /// <summary>
        /// Updates a particular vital according to regeneration rate
        /// </summary>
        /// <param name="vital">The vital stat to update (health/stamina/mana)</param>
        /// <returns>TRUE if vital has changed</returns>
        public bool VitalHeartBeat(CreatureVital vital)
        {
            // Current and MaxValue are properties and include overhead in getting their values. We cache them so we only hit the overhead once.
            var vitalCurrent = vital.Current;
            var vitalMax = vital.MaxValue;

            if (vitalCurrent == vitalMax)
                return false;

            if (vitalCurrent > vitalMax)
            {
                UpdateVital(vital, vitalMax);
                return true;
            }

            if (vital.RegenRate == 0.0) return false;

            // take attributes into consideration (strength, endurance)
            var attributeMod = GetAttributeMod(vital);

            // take stance into consideration (combat, crouch, sitting, sleeping)
            var stanceMod = GetStanceMod(vital);

            // take enchantments into consideration:
            // (regeneration / rejuvenation / mana renewal / etc.)
            var enchantmentMod = EnchantmentManager.GetRegenerationMod(vital);

            var augMod = 1.0f;
            if (this is Player player && player.AugmentationFasterRegen > 0)
                augMod += player.AugmentationFasterRegen;

            // cap rate?
            var currentTick = vital.RegenRate * attributeMod * stanceMod * enchantmentMod * augMod;

            // add in partially accumulated / rounded vitals from previous tick(s)
            var totalTick = currentTick + vital.PartialRegen;

            // accumulate partial vital rates between ticks
            var intTick = (int)totalTick;
            vital.PartialRegen = totalTick - intTick;

            if (intTick > 0)
            {
                //if (this is Player)
                    //Console.WriteLine($"VitalTick({vital.Vital.ToSentence()}): attributeMod={attributeMod}, stanceMod={stanceMod}, enchantmentMod={enchantmentMod}, regenRate={vital.RegenRate}, currentTick={currentTick}, totalTick={totalTick}, accumulated={vital.PartialRegen}");

                UpdateVitalDelta(vital, intTick);
                if (vital.Vital == PropertyAttribute2nd.MaxHealth)
                    DamageHistory.OnHeal((uint)intTick);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the vital regeneration modifier based on attributes
        /// (strength, endurance for health, stamina)
        /// </summary>
        public float GetAttributeMod(CreatureVital vital)
        {
            // only applies to players
            if (!(this is Player)) return 1.0f;

            // only applies for health?
            if (vital.Vital != PropertyAttribute2nd.MaxHealth) return 1.0f;

            // The combination of strength and endurance (with endurance being more important) allows one to regenerate hit points 
            // at a faster rate the higher one's endurance is. This bonus is in addition to any regeneration spells one may have placed upon themselves.
            // This regeneration bonus caps at around 110%.

            var strength = (int)Strength.Base;
            var endurance = (int)Endurance.Base;

            var strAndEnd = strength + (endurance * 2);

            //var modifier = 1.0 + (0.0494 * Math.Pow(strAndEnd, 1.179) / 100.0f);    // formula deduced from values present in the client pdb
            //var attributeMod = Math.Clamp(modifier, 1.0, 2.1);      // cap between + 0-110%

            if (strAndEnd <= 200)
                return 1.0f;

            var modifier = 1.0f + (float)(strAndEnd - 200) / 600;
            var attributeMod = Math.Clamp(modifier, 1.0f, 2.1f);

            return attributeMod;
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

            var forwardCommand = CurrentMovementData.MovementType == MovementType.Invalid && CurrentMovementData.Invalid != null ? CurrentMovementData.Invalid.State.ForwardCommand : MotionCommand.Invalid;

            // combat mode / running
            if (CombatMode != CombatMode.NonCombat || forwardCommand == MotionCommand.RunForward)
                return 0.5f;

            switch (forwardCommand)
            {
                // TODO: verify multipliers
                default:
                    return 1.0f;
                case MotionCommand.Crouch:
                    return 2.0f;
                case MotionCommand.Sitting:
                    return 2.5f;
                case MotionCommand.Sleeping:
                    return 3.0f;
            }
        }
    }
}
