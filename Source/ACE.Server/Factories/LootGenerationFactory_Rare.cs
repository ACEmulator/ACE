using System.Linq;

using System.Collections.Generic;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        public static Dictionary<int, int> RareChances = new Dictionary<int, int>()
        {
            { 1, 2500 },
            { 2, 25000 },
            { 3, 250000 },
            { 4, 3120000 },
            { 5, 7542500 },
            { 6, 8750000 }
        };

        public static Dictionary<int, HashSet<int>> RareWCIDs;

        public static void InitRares()
        {
            RareWCIDs = new Dictionary<int, HashSet<int>>();

            var tier1Rares = new HashSet<int>() { 30183, 30184, 30186, 30187, 30188, 30189, 30194, 30195, 30196, 30197, 30198, 30199, 30200, 30202, 30205, 30206, 30209, 30214, 30215, 30216, 30217, 30218, 30221, 30222, 30223, 30224, 30225, 30226, 30228, 30229, 30232, 30233, 30234, 30236, 30238, 30240, 30242, 30243, 30244, 30245, 30246 };
            var tier2Rares = new HashSet<int>() { 30182, 30185, 30193, 30203, 30208, 30220, 30231, 30235, 30181, 30190, 30191, 30192, 30201, 30204, 30207, 30211, 30212, 30213, 30219, 30230, 30237, 30241, 30239, 30227, 30210 };
            var tier3Rares = new HashSet<int>() { 52034, 30258 };

            //tier 4 is missing all med kits
            var tier4Rares = new HashSet<int>() { 30510, 30523, 30520, 30525, 30518, 30513, 30528, 30521, 30516, 30530, 30532, 30367, 30534, 30524, 30529, 30519, 30526, 30517, 30514, 30511, 30522, 30515, 30512, 30533, 30531, 30368, 30369, 30527, 30371, 30373, 30372, 30370, 30366, 30354, 30352, 30356, 30353, 30357, 30359, 30361, 30355, 30358, 30360, 30362, 30363, 30364, 30365 };
            var tier5Rares = new HashSet<int>() { 30936, 30074, 30075, 30076, 30077, 30078, 30079, 30080, 30081, 30082, 30083, 30084, 30085, 30086, 30087, 30088, 30089, 30090, 30091, 30112, 30113, 30114, 30115, 30116, 30117, 30118, 30119, 30120, 30121, 30122, 30123, 30124, 30125, 30126, 30127, 30128, 30129, 30130, 30131, 30132, 30133, 30134, 30135, 30136, 30137, 30318, 30139, 30140, 30141, 30142, 30143, 30144, 30145, 30146, 30147, 30148, 30149, 30150, 30151, 30152, 30153, 30154, 30155, 30156, 30157, 30158, 30159, 30160, 30161, 30162, 30163, 30164, 30165, 30166, 30167, 30168, 30169, 30170, 30171, 30172, 30173, 30174, 30175, 30176, 30177, 30178, 30179, 30180, 30247, 30248, 30249, 30253, 30254, 30092, 30093, 30110, 30111, 30094, 30095, 30096, 30097, 30098, 30099, 30100, 30101, 30102, 30103, 30104, 30105, 30106 };
            var tier6Rares = new HashSet<int>() { 30345, 30346, 30347, 30348, 30349, 30340, 30341, 30342, 30343, 30344, 30304, 30350, 30351, 30302, 30303, 30309, 30305, 30306, 30307, 30308, 30316, 30317, 30318, 30310, 30311, 30312, 30313, 30314, 30315, 30339, 30319, 30320, 30321, 30322, 30323, 30324, 30325, 30326, 30327, 30328, 30329, 30330, 30331, 30332, 30333, 30334, 30335, 30336, 30337, 30338, 30374, 30375, 30376, 30377, 30378 };

            RareWCIDs.Add(1, tier1Rares);
            RareWCIDs.Add(2, tier2Rares);
            RareWCIDs.Add(3, tier3Rares);
            RareWCIDs.Add(4, tier4Rares);
            RareWCIDs.Add(5, tier5Rares);
            RareWCIDs.Add(6, tier6Rares);
        }

        public static WorldObject CreateRare()
        {
            int tier = 0;

            if (ThreadSafeRandom.Next(1, 2500) == 1)   // 1 in 2,500 chance
            {
                tier = 1;
                if (ThreadSafeRandom.Next(1, 10) == 1)  // 1 in 25,000 chance
                {
                    tier = 2;
                }
                if (ThreadSafeRandom.Next(1, 100) == 1)  // 1 in 250,000 chance
                {
                    tier = 3;
                }
                if (ThreadSafeRandom.Next(1, 1250) == 1)  // 1 in 3,120,000 chance
                {
                    tier = 4;
                }
                if (ThreadSafeRandom.Next(1, 3017) == 1)  // 1 in 7,542,500 (wiki avg. 7,543,103)
                {
                    tier = 5;
                }
                if (ThreadSafeRandom.Next(1, 3500) == 1)  // 1 in 8,750,000 chance
                {
                    tier = 6;
                }
            }

            if (tier == 0) return null;

            var tierRares = RareWCIDs[tier].ToList();

            var rng = ThreadSafeRandom.Next(0, tierRares.Count - 1);

            var rareWCID = tierRares[rng];

            var wo = WorldObjectFactory.CreateNewWorldObject((uint)rareWCID);

            if (wo == null)
                log.Error($"LootGenerationFactory_Rare.CreateRare(): failed to generate rare wcid {rareWCID}");

            return wo;
        }

        /// <summary>
        /// Returns the tier for a rare wcid
        /// </summary>
        public static int GetRareTier(uint rareWCID)
        {
            var wcid = (int)rareWCID;

            foreach (var kvp in RareWCIDs)
                if (kvp.Value.Contains(wcid))
                    return kvp.Key;

            return 0;
        }
    }
}
