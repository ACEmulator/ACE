using System.Collections.Generic;

using ACE.Common;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class AetheriaWcids
    {
        private static readonly List<WeenieClassName> aetheriaColors = new List<WeenieClassName>()
        {
            WeenieClassName.ace42635_aetheria,  // blue
            WeenieClassName.ace42637_aetheria,  // yellow
            WeenieClassName.ace42636_aetheria,  // red
        };

        public static WeenieClassName Roll(int tier)
        {
            switch (tier)
            {
                // blue only
                case 5:
                    return aetheriaColors[0];

                // even chance between blue / yellow
                case 6:
                    var rng = ThreadSafeRandom.Next(0, 1);
                    return aetheriaColors[rng];

                // even chance between blue / yellow / red
                case 7:
                case 8:
                    rng = ThreadSafeRandom.Next(0, 2);
                    return aetheriaColors[rng];
            }
            return WeenieClassName.undef;
        }
    }
}
