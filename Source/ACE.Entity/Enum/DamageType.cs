using System;

using ACE.Common;
using ACE.Common.Extensions;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum DamageType
    {
        Undef       = 0x0,
        Slash       = 0x1,
        Pierce      = 0x2,
        Bludgeon    = 0x4,
        Cold        = 0x8,
        Fire        = 0x10,
        Acid        = 0x20,
        Electric    = 0x40,
        Health      = 0x80,
        Stamina     = 0x100,
        Mana        = 0x200,
        Nether      = 0x400,
        Base        = 0x10000000,

        // helpers
        Physical    = Slash | Pierce | Bludgeon,
        Elemental   = Cold | Fire | Acid | Electric,
    };

    public static class DamageTypeExtensions
    {
        public static string GetName(this DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Undef:    return "Undefined";
                case DamageType.Slash:    return "Slashing";
                case DamageType.Pierce:   return "Piercing";
                case DamageType.Bludgeon: return "Bludgeoning";
                case DamageType.Cold:     return "Cold";
                case DamageType.Fire:     return "Fire";
                case DamageType.Acid:     return "Acid";
                case DamageType.Electric: return "Electric";
                case DamageType.Health:   return "Health";
                case DamageType.Stamina:  return "Stamina";
                case DamageType.Mana:     return "Mana";
                case DamageType.Nether:   return "Nether";
                case DamageType.Base:     return "Base";
                default:
                    return null;
            }
        }

        public static bool IsMultiDamage(this DamageType damageType)
        {
            return EnumHelper.HasMultiple((uint)damageType);
        }

        public static DamageType SelectDamageType(this DamageType damageType, float? powerLevel = null)
        {
            if (powerLevel == null)
            {
                // select random damage type
                var damageTypes = EnumHelper.GetFlags(damageType);

                var rng = ThreadSafeRandom.Next(1, damageTypes.Count - 1);

                return (DamageType)damageTypes[rng];
            }

            var playerTypes = powerLevel < 0.33f ? damageType & DamageType.Physical : damageType & ~DamageType.Physical;

            if (playerTypes == DamageType.Undef)
                playerTypes = damageType;

            return playerTypes.SelectDamageType();
        }
    }
}
