using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// Determines the monster behavior for which players are targetted
    /// </summary>
    [Flags]
    public enum TargetingTactic
    {
        // note that this is still trying to be figured out...
        None        = 0x00,
        Random      = 0x01,   // target a random player every now and then
        Focused     = 0x02,   // target 1 player and stick with them
        LastDamager = 0x04,   // target the last player who did damage
        TopDamager  = 0x08,   // target the player who did the most damage
        Weakest     = 0x10,   // target the lowest level player
        Strongest   = 0x20,   // target the highest level player
        Nearest     = 0x40,   // target the player in closest proximity
    }
}
