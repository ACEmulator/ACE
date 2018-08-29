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
        /// Mana Conversion skill modifier that can be modified themselves by Item Enchantments
        /// </summary>
        /// <param name="wielder"></param>
        /// <returns></returns>
        public static float GetWeaponManaConversionBonus(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultBonusModifier;

            // TODO: Add EmchantmentManager buff/debuff from Hermetic Link/Void to ManaConversion property
            return (float)(weapon.GetProperty(PropertyFloat.ManaConversionMod) ?? defaultBonusModifier);
        }

        /// <summary>
        /// Melee Defense skill modifier that can be modified themselves by Item Enchantments
        /// </summary>
        /// <param name="wielder"></param>
        /// <returns></returns>
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
        /// Critical chance increase for physical damage
        /// </summary>
        /// <param name="wielder"></param>
        /// <returns></returns>
        public static float GetWeaponPhysicalCritFrequencyBonus(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultPhysicalCritFrequency;

            // TODO: Critial Strike imbue that scales with player's skill
            return (float)(weapon.GetProperty(PropertyFloat.CriticalFrequency) ?? defaultPhysicalCritFrequency);
        }

        /// <summary>
        /// Critical chance increase for magical damage
        /// </summary>
        /// <param name="wielder"></param>
        /// <returns></returns>
        public static float GetWeaponMagicCritFrequencyBonus(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultMagicCritFrequency;

            // TODO: Critial Strike imbue that scales with player's skill
            return (float)(weapon.GetProperty(PropertyFloat.CriticalFrequency) ?? defaultMagicCritFrequency);
        }

        /// <summary>
        /// Critical damage multiplier
        /// </summary>
        /// <param name="wielder"></param>
        /// <returns></returns>
        public static float GetWeaponCritMultiplierBonus(Creature wielder)
        {
            WorldObject weapon = GetWeapon(wielder as Player);

            if (weapon == null)
                return defaultCritMultiplier;

            // TODO: Crippling Blow imbue that scales with player's skill
            return (float)(weapon.GetProperty(PropertyFloat.CriticalMultiplier) ?? defaultCritMultiplier);
        }

        /// <summary>
        /// Fixed 2x damage bonus against specified CreatureType
        /// </summary>
        /// <param name="wielder"></param>
        /// <param name="target"></param>
        /// <returns></returns>
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
        /// Caster weapon only bonus for elemental damage
        /// </summary>
        /// <param name="wielder"></param>
        /// <returns></returns>
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
                    if (elementalDamageModType == (int)damageType)
                    {
                        // TODO: Add EmchantmentManager buff/debuff from Spirit Drinker/Loather to ElementalDamageMod property
                        var elementalDmgMod = (float)(weapon.GetProperty(PropertyFloat.ElementalDamageMod) ?? modifier);
                        if (elementalDmgMod > modifier)
                        {
                            modifier = elementalDmgMod;
                            if (wielderAsPlayer != null && targetAsPlayer != null)
                            {
                                var wholeNumber = (float)Math.Truncate(elementalDmgMod);
                                var decimalComponent = elementalDmgMod - wholeNumber;
                                modifier = wholeNumber + (decimalComponent * elementalDmgBonusPvPReduction);
                            }
                        }
                    }
            }

            return modifier;
        }

        /// <summary>
        /// Quest weapon fixed Resistance Cleaving equivalent to Level 5 Life Vulnerability spell
        /// </summary>
        /// <param name="wielder"></param>
        /// <returns></returns>
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
