using System;
using System.IO;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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

        public WeaponProfile(WorldObject weapon)
        {
            DamageType = (DamageType)(weapon.GetProperty(PropertyInt.DamageType) ?? 0);
            if (DamageType == 0)
                Console.WriteLine("Warning: WeaponProfile undefined damage type for " + weapon.Guid);

            WeaponTime = (uint)(weapon.GetProperty(PropertyInt.WeaponTime) ?? 0);    // safe to assume defaults here?
            WeaponSkill = (Skill)(weapon.GetProperty(PropertyInt.WeaponSkill) ?? 0);
            Damage = (uint)(weapon.GetProperty(PropertyInt.Damage) ?? 0);
            DamageVariance = weapon.GetProperty(PropertyFloat.DamageVariance) ?? 0;
            DamageMod = weapon.GetProperty(PropertyFloat.DamageMod) ?? 0; // 0 or 1?
            WeaponLength = weapon.GetProperty(PropertyFloat.WeaponLength) ?? 0;
            MaxVelocity = weapon.GetProperty(PropertyFloat.MaximumVelocity) ?? 0;
            WeaponOffense = weapon.GetProperty(PropertyFloat.WeaponOffense) ?? 0;
            MaxVelocityEstimated = (uint)Math.Round(MaxVelocity);   // ??
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
