using System;
using System.Linq;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum FactionBits
    {
        None          = 0x0,
        CelestialHand = 0x1,
        EldrytchWeb   = 0x2,
        RadiantBlood  = 0x4,

        // helper
        ValidFactions = CelestialHand | EldrytchWeb | RadiantBlood
    }

    public static class FactionBitsExtensions
    {
        /// <summary>
        /// Will add a space infront of capital letter words in a string
        /// </summary>
        /// <param name="factionBits"></param>
        /// <returns>string with spaces infront of capital letters</returns>
        public static string ToSentence(this FactionBits factionBits)
        {
            switch (factionBits)
            {
                case FactionBits.None:
                    return "None";
                case FactionBits.CelestialHand:
                    return "Celestial Hand";
                case FactionBits.EldrytchWeb:
                    return "Eldrytch Web";
                case FactionBits.RadiantBlood:
                    return "Radiant Blood";
            }

            // TODO we really should log this as a warning to indicate that we're missing a case up above, and that the inefficient (GC unfriendly) line below will be used
            return new string(factionBits.ToString().ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray());
        }
    }
}
