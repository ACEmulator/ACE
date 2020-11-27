using System;

using ACE.Common.Extensions;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;

namespace ACE.Server.WorldObjects.Entity
{
    public class CreatureVital
    {
        private readonly Creature creature;

        public readonly PropertyAttribute2nd Vital;

        // the underlying database record
        private readonly PropertiesAttribute2nd propertiesAttribute2nd;

        /// <summary>
        /// If the creature's biota does not contain this vital, a new record will be created.
        /// </summary>
        public CreatureVital(Creature creature, PropertyAttribute2nd vital)
        {
            this.creature = creature;
            Vital = vital;

            if (!creature.Biota.PropertiesAttribute2nd.TryGetValue(vital, out propertiesAttribute2nd))
            {
                propertiesAttribute2nd = new PropertiesAttribute2nd();
                creature.Biota.PropertiesAttribute2nd[vital] = propertiesAttribute2nd;
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

        public uint StartingValue
        {
            get => propertiesAttribute2nd.InitLevel;
            set => propertiesAttribute2nd.InitLevel = value;
        }

        /// <summary>
        /// Total Experience Spent on this vital
        /// </summary>
        public uint ExperienceSpent
        {
            get => propertiesAttribute2nd.CPSpent;
            set => propertiesAttribute2nd.CPSpent = value;
        }

        /// <summary>
        /// Returns the amount of vital experience remaining
        /// until max rank is reached
        /// </summary>
        public uint ExperienceLeft
        {
            get
            {
                var vitalXPTable = DatManager.PortalDat.XpTable.VitalXpList;

                return vitalXPTable[vitalXPTable.Count - 1] - ExperienceSpent;
            }
        }

        /// <summary>
        /// The number of levels a vital has been raised,
        /// derived from ExperienceSpent
        /// </summary>
        public uint Ranks
        {
            get => propertiesAttribute2nd.LevelFromCP;
            set => propertiesAttribute2nd.LevelFromCP = value;
        }

        /// <summary>
        /// Returns TRUE if this vital has been raised the maximum # of times
        /// </summary>
        public bool IsMaxRank
        {
            get
            {
                var vitalXPTable = DatManager.PortalDat.XpTable.VitalXpList;

                return Ranks >= (vitalXPTable.Count - 1);
            }
        }

        /// <summary>
        /// Returns the adjusted Value depending on the base attribute formula
        /// </summary>
        public uint Base
        {
            get
            {
                var attr = AttributeFormula.GetFormula(creature, Vital, false);

                var total = StartingValue + Ranks + attr;

                if (creature is Player player && Vital == PropertyAttribute2nd.MaxHealth)
                    total += (uint)(player.Enlightenment * 2 + player.GetGearMaxHealth());

                return total;
            }
        }

        public uint Current
        {
            get => propertiesAttribute2nd.CurrentLevel;
            set => propertiesAttribute2nd.CurrentLevel = value;
        }

        public uint MaxValue
        {
            get
            {
                var attr = AttributeFormula.GetFormula(creature, Vital, true);

                uint total = StartingValue + Ranks + attr;

                var player = creature as Player;

                if (player != null)
                {
                    // Enlightenment and GearMaxHealth didn't work like other additives
                    // most additives (ie. from enchantments) were added in *after* multipliers,
                    // but Enlightenment and GearMaxHealth were an exception, and added in beforehand

                    // this means Enlightenment and GearMaxHealth would get scaled by multipliers,
                    // including Asheron's Benediction, and oddly enough, vitae as well

                    // it's also possible these were considered "base"
                    if (Vital == PropertyAttribute2nd.MaxHealth)
                        total += (uint)(player.Enlightenment * 2 + player.GetGearMaxHealth());
                }

                // apply multiplicative enchantments first
                var multiplier = creature.EnchantmentManager.GetVitalMod_Multiplier(this);

                var fTotal = total * multiplier;

                if (player != null)
                {
                    var vitae = player.Vitae;

                    if (vitae != 1.0f)
                        fTotal *= vitae;
                }

                // everything beyond this point does not get scaled by vitae
                var additives = creature.EnchantmentManager.GetVitalMod_Additives(this);

                var iTotal = (fTotal + additives).Round();

                iTotal = Math.Max(iTotal, 5);   // a creature cannot fall below 5 MaxVital from vitae

                return (uint)iTotal;
            }
        }

        public uint Missing => MaxValue - Current;

        public float Percent => (float)Current / MaxValue;

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

        /// <summary>
        /// For tracking partial regeneration between ticks
        /// </summary>
        public double PartialRegen { get; set; }

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
