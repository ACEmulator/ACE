using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum SquelchMask: uint
    {
        // this is equivalent to 1 << ChatMessageType
        None                = 0x0,
        Speech              = 0x4,
        Tell                = 0x8,
        Combat              = 0x40,
        Magic               = 0x80,
        Emote               = 0x1000,
        Appraisal           = 0x10000,
        Spellcasting        = 0x20000,
        Allegiance          = 0x40000,
        Fellowship          = 0x80000,
        CombatEnemy         = 0x200000,
        CombatSelf          = 0x400000,
        Recall              = 0x800000,
        Craft               = 0x1000000,
        Salvaging           = 0x2000000,
        Combined            = Speech | Tell | Combat | Magic | Emote | Appraisal | Spellcasting | Allegiance | Fellowship | CombatEnemy | CombatSelf | Recall | Craft | Salvaging,
        AllChannels         = 0xFFFFFFFF
    }

    public static class SquelchMaskExtensions
    {
        public static SquelchMask Add(this SquelchMask maskA, SquelchMask maskB)
        {
            if (maskA == SquelchMask.AllChannels || maskB == SquelchMask.AllChannels)
                return SquelchMask.AllChannels;

            var result = maskA | maskB;

            if (result == SquelchMask.Combined)
                return SquelchMask.AllChannels;
            else
                return result;
        }

        public static SquelchMask Remove(this SquelchMask maskA, SquelchMask maskB)
        {
            if (maskB == SquelchMask.AllChannels)
                return SquelchMask.None;

            var result = maskA;

            if (result == SquelchMask.AllChannels)
                result = SquelchMask.Combined;

            result &= ~maskB;

            if (result == SquelchMask.Combined)
                result = SquelchMask.AllChannels;

            return result;
        }
    }
}
