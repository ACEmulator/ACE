using System;
using System.IO;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
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
        public double WeaponDefense;
        public uint MaxVelocityEstimated;   // ??

        public int Enchantment_WeaponTime;
        public int Enchantment_Damage;
        public double Enchantment_DamageVariance;
        public double Enchantment_DamageMod;
        public double Enchantment_WeaponOffense;

        public double Enchantment_WeaponDefense;    // gets sent elsewhere, calculating here for consistency

        public WeaponProfile(WorldObject weapon, WorldObject wielder)
        {
            Weapon = weapon;
            Wielder = wielder;

            WeaponDefense = GetWeaponDefense(weapon, wielder);

            if (weapon is Caster)
                return;

            DamageType = (DamageType)(weapon.GetProperty(PropertyInt.DamageType) ?? 0);
            //if (DamageType == 0)
                //Console.WriteLine($"Warning: WeaponProfile undefined damage type for {weapon.Name} ({weapon.Guid})");

            WeaponTime = GetWeaponSpeed(weapon, wielder);
            WeaponSkill = (Skill)(weapon.GetProperty(PropertyInt.WeaponSkill) ?? 0);
            Damage = GetDamage(weapon, wielder);
            DamageVariance = GetDamageVariance(weapon, wielder);
            DamageMod = GetDamageMultiplier(weapon, wielder);
            WeaponLength = weapon.GetProperty(PropertyFloat.WeaponLength) ?? 1.0f;
            MaxVelocity = weapon.MaximumVelocity ?? 1.0f;
            WeaponOffense = GetWeaponOffense(weapon, wielder);
            //MaxVelocityEstimated = (uint)Math.Round(MaxVelocity);   // not found in pcaps?
        }

        /// <summary>
        /// Returns the weapon max damage, with enchantments factored in
        /// </summary>
        public uint GetDamage(WorldObject weapon, WorldObject wielder)
        {
            var baseDamage = weapon.GetProperty(PropertyInt.Damage) ?? 0;
            var damageBonus = weapon.EnchantmentManager.GetDamageBonus();
            var auraDamageBonus = wielder != null && (weapon.WeenieType != WeenieType.Ammunition || PropertyManager.GetBool("show_ammo_buff").Item) ? wielder.EnchantmentManager.GetDamageBonus() : 0;
            Enchantment_Damage = weapon.IsEnchantable ? damageBonus + auraDamageBonus : damageBonus;
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
            Enchantment_WeaponTime = weapon.IsEnchantable ? speedMod + auraSpeedMod : speedMod;
            return (uint)Math.Max(0, baseSpeed + Enchantment_WeaponTime);
        }

        /// <summary>
        /// Returns the weapon damage variance, with enchantments factored in
        /// </summary>
        public float GetDamageVariance(WorldObject weapon, WorldObject wielder)
        {
            // are there any spells which modify damage variance?
            var baseVariance = weapon.GetProperty(PropertyFloat.DamageVariance) ?? 0.0f;   // safe to assume defaults here?
            var varianceMod = weapon.EnchantmentManager.GetVarianceMod();
            var auraVarianceMod = wielder != null ? wielder.EnchantmentManager.GetVarianceMod() : 1.0f;
            Enchantment_DamageVariance = weapon.IsEnchantable ? varianceMod * auraVarianceMod : varianceMod;
            return (float)(baseVariance * Enchantment_DamageVariance);
        }

        /// <summary>
        /// Returns the weapon damage multiplier, with enchantments factored in
        /// </summary>
        public float GetDamageMultiplier(WorldObject weapon, WorldObject wielder)
        {
            var baseMultiplier = weapon.GetProperty(PropertyFloat.DamageMod) ?? 1.0f;
            var damageMod = weapon.EnchantmentManager.GetDamageMod();
            var auraDamageMod = wielder != null ? wielder.EnchantmentManager.GetDamageMod() : 0.0f;
            Enchantment_DamageMod = weapon.IsEnchantable ? damageMod + auraDamageMod : damageMod;
            return (float)(baseMultiplier + Enchantment_DamageMod);
        }

        /// <summary>
        /// Returns the attack bonus %, with enchantments factored in
        /// </summary>
        public float GetWeaponOffense(WorldObject weapon, WorldObject wielder)
        {
            if (weapon is Ammunition) return 1.0f;

            var baseOffense = weapon.GetProperty(PropertyFloat.WeaponOffense) ?? 1.0f;
            var offenseMod = weapon.EnchantmentManager.GetAttackMod();
            var auraOffenseMod = wielder != null ? wielder.EnchantmentManager.GetAttackMod() : 0.0f;
            Enchantment_WeaponOffense = weapon.IsEnchantable ? offenseMod + auraOffenseMod : offenseMod;
            return (float)(baseOffense + Enchantment_WeaponOffense);
        }

        /// <summary>
        /// Returns the defense bonus %, with enchantments factored in
        /// </summary>
        public float GetWeaponDefense(WorldObject weapon, WorldObject wielder)
        {
            if (weapon is Ammunition) return 1.0f;

            var baseDefense = weapon.GetProperty(PropertyFloat.WeaponDefense) ?? 1.0f;
            var defenseMod = weapon.EnchantmentManager.GetDefenseMod();
            var auraDefenseMod = wielder != null ? wielder.EnchantmentManager.GetDefenseMod() : 0.0f;
            Enchantment_WeaponDefense = weapon.IsEnchantable ? defenseMod + auraDefenseMod : defenseMod;
            return (float)(baseDefense + Enchantment_WeaponDefense);
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
