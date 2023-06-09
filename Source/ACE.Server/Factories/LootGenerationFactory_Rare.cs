using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
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

            var tier1Rares = new HashSet<int>() { 30183, 30184, 30186, 30187, 30188, 30189, 30194, 30195, 30196, 30197, 30199, 30200, 30202, 30205, 30206, 30209, 30214, 30215, 30216, 30217, 30218, 30221, 30222, 30224, 30225, 30226, 30228, 30229, 30232, 30233, 30234, 30240, 30242, 30245, 30246, 41257, 43407, 45360, 45366, 45367, 45368, 45369 };
            var tier2Rares = new HashSet<int>() { 30107, 30108, 30109, 30181, 30182, 30185, 30190, 30191, 30192, 30193, 30201, 30203, 30204, 30207, 30208, 30210, 30211, 30212, 30213, 30219, 30220, 30227, 30230, 30231, 30235, 30237, 30239, 30241 };
            var tier3Rares = new HashSet<int>() { 30250, 30251, 30252, 30258, 52034 };

            var tier4Rares = new HashSet<int>() { 30352, 30353, 30354, 30355, 30356, 30357, 30358, 30359, 30360, 30361, 30362, 30363, 30364, 30365, 30366, 30367, 30368, 30369, 30370, 30371, 30372, 30373, 30510, 30511, 30512, 30513, 30514, 30515, 30516, 30517, 30518, 30519, 30520, 30521, 30522, 30523, 30524, 30525, 30526, 30527, 30528, 30529, 30530, 30531, 30532, 30533, 30534 };
            var tier5Rares = new HashSet<int>() { 30074, 30075, 30076, 30077, 30078, 30079, 30080, 30081, 30082, 30083, 30084, 30085, 30086, 30087, 30088, 30089, 30090, 30091, 30092, 30093, 30094, 30095, 30096, 30097, 30098, 30099, 30100, 30101, 30102, 30103, 30104, 30105, 30106, 30110, 30111, 30112, 30113, 30114, 30115, 30116, 30117, 30118, 30119, 30120, 30121, 30122, 30123, 30124, 30125, 30126, 30127, 30128, 30129, 30130, 30131, 30132, 30133, 30134, 30135, 30136, 30137, 30139, 30140, 30141, 30142, 30143, 30144, 30145, 30146, 30147, 30148, 30149, 30150, 30151, 30152, 30153, 30154, 30155, 30156, 30157, 30158, 30159, 30160, 30161, 30162, 30163, 30164, 30165, 30166, 30167, 30168, 30169, 30171, 30173, 30174, 30175, 30176, 30179, 30180, 30247, 30248, 30249, 30253, 30254, 30936 };
            var tier6Rares = new HashSet<int>() { 30302, 30303, 30304, 30305, 30306, 30307, 30308, 30309, 30345, 30346, 30347, 30348, 30349, 30350, 30351, 30374, 30375, 30376, 30377, 30378, 42662, 42663, 42664, 42665, 42666, 43848, 45436, 45437, 45438, 45439, 45440, 45441, 45442, 45443, 45444, 45445, 45446, 45447, 45448, 45449, 45450, 45451, 45452, 45453, 45454, 45455, 45456, 45457, 45458, 45459, 45460, 45461, 45462, 45463, 45464, 45465, 45466, 45467, 45468, 45469, 45470 };

            RareWCIDs.Add(1, tier1Rares);
            RareWCIDs.Add(2, tier2Rares);
            RareWCIDs.Add(3, tier3Rares);
            RareWCIDs.Add(4, tier4Rares);
            RareWCIDs.Add(5, tier5Rares);
            RareWCIDs.Add(6, tier6Rares);
        }

        public static WorldObject TryCreateRare(int luck = 0)
        {
            //var t1_chance = 2500; // 1 in 2,500 chance // Old rate. Property default is 0.04 (which is 0.04%, or the same 1/2500)
            double rare_drop_rate_percent = (float)Managers.PropertyManager.GetDouble("rare_drop_rate_percent").Item;

            // Check to make sure there *IS* a chance. Less than/equal to 0 would mean zero chance, so we can stop here
            if (rare_drop_rate_percent <= 0)
                return null;

            rare_drop_rate_percent = Math.Min(rare_drop_rate_percent / 100, 1);
            int t1_chance = (int)Math.Round(1 / rare_drop_rate_percent); // Default PropertyManager value results in a 1 in 2,500 chance
            t1_chance = Math.Max(t1_chance - luck, 1);

            int tier = 0;

            if (ThreadSafeRandom.Next(1, t1_chance) == 1)   // 1 in 2,500 chance
            {
                tier = 1;
                if (ThreadSafeRandom.Next(1, 10) == 1)      // 1 in 25,000 chance
                {
                    tier = 2;
                }
                if (ThreadSafeRandom.Next(1, 100) == 1)     // 1 in 250,000 chance
                {
                    tier = 3;
                }
                if (ThreadSafeRandom.Next(1, 1250) == 1)    // 1 in 3,120,000 chance
                {
                    tier = 4;
                }
                if (ThreadSafeRandom.Next(1, 3017) == 1)    // 1 in 7,542,500 (wiki avg. 7,543,103)
                {
                    tier = 5;
                }
                if (ThreadSafeRandom.Next(1, 3500) == 1)    // 1 in 8,750,000 chance
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
