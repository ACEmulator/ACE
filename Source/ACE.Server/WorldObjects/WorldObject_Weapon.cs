using System;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        const float defaultPhysicalCritFrequency = 0.10f;
        const float defaultMagicCritFrequency = 0.02f;
        const float defaultCritMultiplier = 0.0f;
        const float defaultBonusModifier = 1.0f;

        /// <summary>
        /// Returns the primary weapon equipped by a player
        /// (melee, missile, or wand)
        /// </summary>
        private static WorldObject GetWeapon(Player wielder)
        {
            if (wielder == null)
                return null;

            WorldObject weapon = wielder.GetEquippedWeapon();

            if (weapon == null)
                weapon = wielder.GetEquippedWand();

            return weapon;
        }

        /// <summary>
        /// Returns the Mana Conversion skill modifier for the primary weapon
        /// </summary>
        public static float GetWeaponManaConversionBonus(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultBonusModifier;

            // TODO: Add EnchantmentManager buff/debuff from Hermetic Link/Void to ManaConversion property
            return (float)(weapon.GetProperty(PropertyFloat.ManaConversionMod) ?? defaultBonusModifier);
        }

        /// <summary>
        /// Returns the Melee Defense skill modifier for the primary weapon
        /// </summary>
        public static float GetWeaponMeleeDefenseBonus(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultBonusModifier;

            if (wielder.CombatMode != CombatMode.NonCombat)
                return (float)(weapon.GetProperty(PropertyFloat.WeaponDefense) ?? defaultBonusModifier) + wielder.EnchantmentManager.GetDefenseMod();

            return defaultBonusModifier;
        }

        /// <summary>
        /// Returns the attack skill modifier for the primary weapon
        /// </summary>
        public static float GetWeaponOffenseBonus(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultBonusModifier;

            if (wielder.CombatMode != CombatMode.NonCombat)
                return (float)(weapon.GetProperty(PropertyFloat.WeaponOffense) ?? defaultBonusModifier) + wielder.EnchantmentManager.GetAttackMod();

            return defaultBonusModifier;
        }

        /// <summary>
        /// Returns the critical chance modifier for the primary weapon
        /// </summary>
        public static float GetWeaponPhysicalCritFrequencyBonus(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultPhysicalCritFrequency;

            // TODO: Critial Strike imbue that scales with player's skill
            return (float)(weapon.GetProperty(PropertyFloat.CriticalFrequency) ?? defaultPhysicalCritFrequency);
        }

        /// <summary>
        /// Returns the magic critical chance modifier for the primary weapon
        /// </summary>
        public static float GetWeaponMagicCritFrequencyBonus(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultMagicCritFrequency;

            // TODO: Critial Strike imbue that scales with player's skill
            return (float)(weapon.GetProperty(PropertyFloat.CriticalFrequency) ?? defaultMagicCritFrequency);
        }

        /// <summary>
        /// Returns the critical damage multiplier for the primary weapon
        /// </summary>
        public static float GetWeaponCritMultiplierBonus(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultCritMultiplier;

            // TODO: Crippling Blow imbue that scales with player's skill
            return (float)(weapon.GetProperty(PropertyFloat.CriticalMultiplier) ?? defaultCritMultiplier);
        }

        /// <summary>
        /// Returns the slayer 2x+ damage bonus for the primary weapon
        /// against a particular creature type
        /// </summary>
        public static float GetWeaponCreatureSlayerBonus(Creature wielder, Creature target)
        {
            float modifier = defaultBonusModifier;

            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return modifier;

            if (weapon.GetProperty(PropertyInt.SlayerCreatureType) != null && target != null)
                if ((CreatureType)weapon.GetProperty(PropertyInt.SlayerCreatureType) == target.CreatureType)
                    modifier = (float)(weapon.GetProperty(PropertyFloat.SlayerDamageBonus) ?? modifier);
            return modifier;
        }

        /// <summary>
        /// Returns the elemental damage bonus for the magic caster weapon
        /// </summary>
        public static float GetWeaponElementalDamageModBonus(Creature wielder, Creature target, DamageType damageType)
        {
            float elementalDmgBonusPvPReduction = 0.5f;
            float modifier = defaultBonusModifier;

            var wielderAsPlayer = wielder as Player;
            var targetAsPlayer = target as Player;

            WorldObject weapon = GetWeapon(wielderAsPlayer);

            if (weapon is Caster)
            {
                var elementalDamageModType = weapon.GetProperty(PropertyInt.DamageType) ?? (int)DamageType.Undef;
                if (elementalDamageModType != (int)DamageType.Undef)
                {
                    if (elementalDamageModType == (int)damageType)
                    {
                        // TODO: Add EnchantmentManager buff/debuff from Spirit Drinker/Loather to ElementalDamageMod property
                        var elementalDmgMod = (float)(weapon.GetProperty(PropertyFloat.ElementalDamageMod) ?? modifier);
                        if (elementalDmgMod > modifier)
                        {
                            modifier = elementalDmgMod;
                            if (wielderAsPlayer != null && targetAsPlayer != null)
                                modifier = 1.0f + (elementalDmgMod - 1.0f) * elementalDmgBonusPvPReduction;
                        }
                    }
                }
            }

            return modifier;
        }

        /// <summary>
        /// Quest weapon fixed Resistance Cleaving equivalent to Level 5 Life Vulnerability spell
        /// </summary>
        public static float GetWeaponResistanceModifierBonus(Creature wielder, DamageType damageType)
        {
            float modifier = defaultBonusModifier;

            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultBonusModifier;

            // TODO: Resistance Rending imbue that scales with player's skill
            var weaponResistanceModifierType = weapon.GetProperty(PropertyInt.ResistanceModifierType) ?? (int)DamageType.Undef;
            if (weaponResistanceModifierType != (int)DamageType.Undef)
                if (weaponResistanceModifierType == (int)damageType)
                    modifier = (float)(weapon.GetProperty(PropertyFloat.ResistanceModifier) ?? defaultBonusModifier) + 1.0f;
            return modifier;
        }
    }
}
