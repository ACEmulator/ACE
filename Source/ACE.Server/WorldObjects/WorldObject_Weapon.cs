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
        const uint defaultSpeed = 40;   // TODO: find default speed

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
        /// Returns the weapon speed, with enchantments factored in
        /// </summary>
        public static uint GetWeaponSpeed(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultSpeed;

            var baseSpeed = weapon.GetProperty(PropertyInt.WeaponTime) ?? (int)defaultSpeed;
            var speedMod = wielder != null ? wielder.EnchantmentManager.GetWeaponSpeedMod() : 0;
            return (uint)Math.Max(0, baseSpeed + speedMod);
        }

        /// <summary>
        /// Returns the Mana Conversion skill modifier for the primary weapon
        /// </summary>
        public static float GetWeaponManaConversionModifier(Creature wielder)
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
        public static float GetWeaponMeleeDefenseModifier(Creature wielder)
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
        public static float GetWeaponOffenseModifier(Creature wielder)
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
        public static float GetWeaponPhysicalCritFrequencyModifier(Creature wielder)
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
        public static float GetWeaponMagicCritFrequencyModifier(Creature wielder)
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
        public static float GetWeaponCritMultiplierModifier(Creature wielder)
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
        public static float GetWeaponCreatureSlayerModifier(Creature wielder, Creature target)
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
        /// Returns a multiplicative elemental damage bonus for the magic caster weapon type
        /// </summary>
        public static float GetCasterElementalDamageModifier(Creature wielder, Creature target, DamageType damageType)
        {
            // A multiplicative bonus, so the default is 1
            float elementalDmgBonusPvPReduction = 0.5f;
            float modifier = defaultBonusModifier;

            var wielderAsPlayer = wielder as Player;
            var targetAsPlayer = target as Player;

            WorldObject weapon = GetWeapon(wielderAsPlayer);

            if (weapon is Caster)
            {
                var elementalDamageModType = weapon.GetProperty(PropertyInt.DamageType) ?? (int)DamageType.Undef;
                if (elementalDamageModType != (int)DamageType.Undef && elementalDamageModType == (int)damageType)
                {
                    // TODO: Add EnchantmentManager buff/debuff from Spirit Drinker/Loather
                    var casterElementalDmgMod = (float)(weapon.GetProperty(PropertyFloat.ElementalDamageMod) ?? modifier);
                    if (casterElementalDmgMod > modifier)
                    {
                        modifier = casterElementalDmgMod;
                        if (wielderAsPlayer != null && targetAsPlayer != null)
                            modifier = 1.0f + (casterElementalDmgMod - 1.0f) * elementalDmgBonusPvPReduction;
                    }
                }
            }

            return modifier;
        }

        /// <summary>
        /// Returns an additive elemental damage bonus for the missile launcher weapon type
        /// </summary>
        public static int GetMissileElementalDamageModifier(Creature wielder, Creature target, DamageType damageType)
        {
            // An additive bonus, so the default is zero
            int modifier = 0;

            var wielderAsPlayer = wielder as Player;
            var targetAsPlayer = target as Player;

            WorldObject weapon = GetWeapon(wielderAsPlayer);

            if (weapon is MissileLauncher)
            {
                var elementalDamageModType = weapon.GetProperty(PropertyInt.DamageType) ?? (int)DamageType.Undef;
                if (elementalDamageModType != (int)DamageType.Undef && elementalDamageModType == (int)damageType)
                {
                    // TODO: Add EnchantmentManager buff/debuff from Spirit Drinker/Loather
                    var launcherElementalDmgMod = weapon.GetProperty(PropertyInt.ElementalDamageBonus) ?? modifier;
                    modifier = launcherElementalDmgMod;
                }
            }

            return modifier;
        }

        /// <summary>
        /// Quest weapon fixed Resistance Cleaving equivalent to Level 5 Life Vulnerability spell
        /// </summary>
        public static float GetWeaponResistanceModifier(Creature wielder, DamageType damageType)
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
