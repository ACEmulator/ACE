using System.Collections.Generic;

using ACE.Common;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public class CloakWcids
    {
        private static readonly List<WeenieClassName> cloakWcids = new List<WeenieClassName>()
        {
            WeenieClassName.ace44840_cloak,
            WeenieClassName.ace44849_chevroncloak,
            WeenieClassName.ace44850_chevroncloak,
            WeenieClassName.ace44851_chevroncloak,
            WeenieClassName.ace44852_chevroncloak,
            WeenieClassName.ace44853_borderedcloak,
            WeenieClassName.ace44854_halvedcloak,
            WeenieClassName.ace44855_halvedcloak,
            WeenieClassName.ace44856_trimmedcloak,
            WeenieClassName.ace44857_quarteredcloak,
            WeenieClassName.ace44858_quarteredcloak,
        };

        public static WeenieClassName Roll()
        {
            // verify: even chance for each cloak?
            var rng = ThreadSafeRandom.Next(0, cloakWcids.Count - 1);

            return cloakWcids[rng];
        }

        private static readonly HashSet<WeenieClassName> _combined = new HashSet<WeenieClassName>();

        static CloakWcids()
        {
            foreach (var cloakWcid in cloakWcids)
                _combined.Add(cloakWcid);
        }

        public static bool Contains(WeenieClassName wcid)
        {
            return _combined.Contains(wcid);
        }
    }
}
