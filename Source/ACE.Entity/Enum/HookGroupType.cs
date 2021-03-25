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
            return new string(hookGroupType.ToString().ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray());
        }
    }
}
