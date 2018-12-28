using System;
using System.IO;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// Handles the info for weapon appraisal panel
    /// </summary>
    public class WeaponProfile
    {
        public WorldObject Weapon;
        public WorldObject Wielder;

        public DamageType DamageType;
        public uint WeaponTime;             // the weapon speed
        public Skill WeaponSkill;
        public uint Damage;                 // max damage
        public double DamageVariance;       // damage range %
        public double DamageMod;            // the damage modifier of the weapon
        public double WeaponLength;         // ??
        public double MaxVelocity;          // the power of the weapon (this affects range)
        public double WeaponOffense;        // the attack skill bonus of the weapon
        public uint MaxVelocityEstimated;   // ??

        public int Enchantment_WeaponTime;
        public int Enchantment_Damage;
        public double Enchantment_DamageVariance;
        public double Enchantment_DamageMod;
        public double Enchantment_WeaponOffense;

        public WeaponProfile(WorldObject weapon, WorldObject wielder)
        {
            Weapon = weapon;
            Wielder = wielder;

            if (weapon is Caster)
                return;

            DamageType = (DamageType)(weapon.GetProperty(PropertyInt.DamageType) ?? 0);
            if (DamageType == 0)
                Console.WriteLine("Warning: WeaponProfile undefined damage type for " + weapon.Guid);

            WeaponTime = GetWeaponSpeed(weapon, wielder);
            WeaponSkill = (Skill)(weapon.GetProperty(PropertyInt.WeaponSkill) ?? 0);
            Damage = GetDamage(weapon, wielder);
            DamageVariance = GetDamageVariance(weapon, wielder);
            DamageMod = GetDamageMultiplier(weapon, wielder);
            WeaponLength = weapon.GetProperty(PropertyFloat.WeaponLength) ?? 1.0f;
            MaxVelocity = weapon.GetProperty(PropertyFloat.MaximumVelocity) ?? 1.0f;
            WeaponOffense = GetWeaponOffense(weapon, wielder);
            MaxVelocityEstimated = (uint)Math.Round(MaxVelocity);   // ??
        }

        /// <summary>
        /// Returns the weapon max damage, with enchantments factored in
        /// </summary>
        public uint GetDamage(WorldObject weapon, WorldObject wielder)
        {
            var baseDamage = weapon.GetProperty(PropertyInt.Damage) ?? 0;
            var damageMod = weapon.EnchantmentManager.GetDamageMod();
            var auraDamageMod = wielder != null ? wielder.EnchantmentManager.GetDamageMod() : 0;
            Enchantment_Damage = weapon.IsEnchantable ? damageMod + auraDamageMod : 0;
            return (uint)Math.Max(0, baseDamage + Enchantment_Damage);
        }

        /// <summary>
        /// Returns the weapon speed, with enchantments factored in
        /// </summary>
        public uint GetWeaponSpeed(WorldObject weapon, WorldObject wielder)
        {
            var baseSpeed = weapon.GetProperty(PropertyInt.WeaponTime) ?? 0;   // safe to assume defaults here?
            var speedMod = weapon.EnchantmentManager.GetWeaponSpeedMod();
            var auraSpeedMod = wielder != null ? wielder.EnchantmentManager.GetWeaponSpeedMod() : 0;
            Enchantment_WeaponTime = weapon.IsEnchantable ? speedMod + auraSpeedMod : 0;
            return (uint)Math.Max(0, baseSpeed + Enchantment_WeaponTime);
        }

        /// <summary>
        /// Returns the weapon damage variance, with enchantments factored in
        /// </summary>
        public float GetDamageVariance(WorldObject weapon, WorldObject wielder)
        {
            // are there any spells which modify damage variance?
            var baseVariance = weapon.GetProperty(PropertyFloat.DamageVariance) ?? 1.0f;   // safe to assume defaults here?
            var varianceMod = weapon.EnchantmentManager.GetVarianceMod();
            var auraVarianceMod = wielder != null ? wielder.EnchantmentManager.GetVarianceMod() : 1.0f;
            Enchantment_DamageVariance = weapon.IsEnchantable ? varianceMod * auraVarianceMod : 1.0f;
            return (float)(baseVariance * Enchantment_DamageVariance);
        }

        /// <summary>
        /// Returns the weapon damage multiplier, with enchantments factored in
        /// </summary>
        public float GetDamageMultiplier(WorldObject weapon, WorldObject wielder)
        {
            var baseMultiplier = weapon.GetProperty(PropertyFloat.DamageMod) ?? 1.0f;
            var multiplierMod = weapon.EnchantmentManager.GetDamageModifier();
            var auraMultiplierMod = wielder != null ? wielder.EnchantmentManager.GetDamageModifier() : 1.0f;
            Enchantment_DamageMod = weapon.IsEnchantable ? multiplierMod * auraMultiplierMod : 1.0f;
            return (float)(baseMultiplier * Enchantment_DamageMod);
        }

        /// <summary>
        /// Returns the attack bonus %, with enchantments factored in
        /// </summary>
        public float GetWeaponOffense(WorldObject weapon, WorldObject wielder)
        {
            if (weapon is Ammunition) return 1.0f;

            // always aura?
            var baseOffense = weapon.GetProperty(PropertyFloat.WeaponOffense) ?? 1.0f;
            var offenseMod = wielder != null ? wielder.EnchantmentManager.GetAttackMod() : 0.0f;
            Enchantment_WeaponOffense = weapon.IsEnchantable ? offenseMod : 0.0f;
            return (float)(baseOffense + offenseMod);
        }
    }

    public static class WeaponProfileExtensions
    {
        /// <summary>
        /// Writes the weapon appraisal info to the network stream
        /// </summary>
        public static void Write(this BinaryWriter writer, WeaponProfile profile)
        {
            writer.Write((uint)profile.DamageType);
            writer.Write(profile.WeaponTime);
            writer.Write((uint)profile.WeaponSkill);
            writer.Write(profile.Damage);
            writer.Write(profile.DamageVariance);
            writer.Write(profile.DamageMod);
            writer.Write(profile.WeaponLength);
            writer.Write(profile.MaxVelocity);
            writer.Write(profile.WeaponOffense);
            writer.Write(profile.MaxVelocityEstimated);
        }
    }

}
