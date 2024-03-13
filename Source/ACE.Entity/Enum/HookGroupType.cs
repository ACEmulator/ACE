using System;
using System.Linq;

namespace ACE.Entity.Enum
{
    public enum HookGroupType
    {
        Undef                           = 0x0,
        NoisemakingItems                = 0x1,
        TestItems                       = 0x2,
        PortalItems                     = 0x4,
        WritableItems                   = 0x8,
        SpellCastingItems               = 0x10,
        SpellTeachingItems              = 0x20
    }

    public static class HookGroupTypeExtensions
    {
        /// <summary>
        /// Will add a space infront of capital letter words in a string
        /// </summary>
        /// <returns>string with spaces infront of capital letters</returns>
        public static string ToSentence(this HookGroupType hookGroupType)
        {
            switch (hookGroupType)
            {
                case HookGroupType.Undef:
                    return "Undef";
                case HookGroupType.NoisemakingItems:
                    return "Noisemaking Items";
                case HookGroupType.TestItems:
                    return "Test Items";
                case HookGroupType.PortalItems:
                    return "Portal Items";
                case HookGroupType.WritableItems:
                    return "Writable Items";
                case HookGroupType.SpellCastingItems:
                    return "Spell Casting Items";
                case HookGroupType.SpellTeachingItems:
                    return "Spell Teaching Items";
            }

            // TODO we really should log this as a warning to indicate that we're missing a case up above, and that the inefficient (GC unfriendly) line below will be used
            return new string(hookGroupType.ToString().ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray());
        }
    }
}
