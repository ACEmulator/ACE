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
            return new string(factionBits.ToString().ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray());
        }
    }
}
