using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    public class WeaponProfile
    {
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

        public WeaponProfile(WorldObject weapon, WorldObject wielder)
        {
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
            var damageMod = wielder != null ? wielder.EnchantmentManager.GetDamageMod() : 0;
            return (uint)Math.Max(0, baseDamage + damageMod);
        }

        /// <summary>
        /// Returns the weapon speed, with enchantments factored in
        /// </summary>
        public uint GetWeaponSpeed(WorldObject weapon, WorldObject wielder)
        {
            var baseSpeed = weapon.GetProperty(PropertyInt.WeaponTime) ?? 0;   // safe to assume defaults here?
            var speedMod = wielder != null ? wielder.EnchantmentManager.GetWeaponSpeedMod() : 0;
            return (uint)Math.Max(0, baseSpeed + speedMod);
        }

        /// <summary>
        /// Returns the weapon damage variance, with enchantments factored in
        /// </summary>
        public float GetDamageVariance(WorldObject weapon, WorldObject wielder)
        {
            // are there any spells which modify damage variance?
            var baseVariance = weapon.GetProperty(PropertyFloat.DamageVariance) ?? 1.0f;   // safe to assume defaults here?
            var varianceMod = wielder != null ? wielder.EnchantmentManager.GetVarianceMod() : 1.0f;
            return (float)(baseVariance * varianceMod);
        }

        /// <summary>
        /// Returns the weapon damage multiplier, with enchantments factored in
        /// </summary>
        public float GetDamageMultiplier(WorldObject weapon, WorldObject wielder)
        {
            var baseMultiplier = weapon.GetProperty(PropertyFloat.DamageMod) ?? 1.0f;
            var multiplierMod = wielder != null ? wielder.EnchantmentManager.GetDamageModifier() : 1.0f;
            return (float)(baseMultiplier * multiplierMod);
        }

        /// <summary>
        /// Returns the attack bonus %, with enchantments factored in
        /// </summary>
        public float GetWeaponOffense(WorldObject weapon, WorldObject wielder)
        {
            var baseOffense = weapon.GetProperty(PropertyFloat.WeaponOffense) ?? 1.0f;
            var offenseMod = wielder != null ? wielder.EnchantmentManager.GetAttackMod() : 1.0f;
            return (float)(baseOffense * offenseMod);
        }
    }

    public static class WeaponProfileExtensions
    {
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
