using System;
using System.Collections.Generic;

using ACE.Entity.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static class LootTables
    {
        /// <summary>
        /// A mapping of MaterialTypes to value modifiers
        /// </summary>
        private static Dictionary<int, double> materialModifier = new Dictionary<int, double>()
        {
            { 1,  1.0 }, // Ceramic
            { 2,  1.5 }, // Porcelain
            { 3,  1.0 }, // Cloth ====
            { 4,  1.0 }, // Linen
            { 5,  1.4 }, // Satin
            { 6,  1.8 }, // Silk
            { 7,  1.8 }, // Velvet
            { 8,  1.0 }, // Wool
            { 10, 1.2 }, // Agate
            { 11, 1.4 }, // Amber
            { 12, 1.6 }, // Amethyst
            { 13, 1.8 }, // Aquamarine
            { 14, 1.2 }, // Azurite
            { 15, 1.6 }, // Black Garnet
            { 16, 2.0 }, // Black Opal
            { 17, 1.4 }, // Bloodstone
            { 18, 1.4 }, // Carnelian
            { 19, 1.4 }, // Citrine
            { 20, 2.5 }, // Diamond
            { 21, 1.2 }, // Emerald
            { 22, 2.0 }, // Fire Opal
            { 23, 1.0 }, // Green Garnet
            { 24, 1.6 }, // Green Jade
            { 25, 1.4 }, // Hematite
            { 26, 2.0 }, // Imperial Topaz
            { 27, 1.6 }, // Jet
            { 28, 1.2 }, // Lapis Lazuli
            { 29, 2.0 }, // Lavender Jade
            { 30, 1.2 }, // Malachite
            { 31, 1.4 }, // Moonstone
            { 32, 1.4 }, // Onyx
            { 33, 1.2 }, // Opal
            { 34, 1.8 }, // Peridot
            { 35, 1.0 }, // Red Garnet
            { 36, 2.0 }, // Red Jade
            { 37, 1.4 }, // Rose Quartz
            { 38, 2.5 }, // Ruby
            { 39, 2.5 }, // Sapphire
            { 40, 1.2 }, // Smokey Quartz
            { 41, 2.0 }, // Sunstone
            { 42, 1.2 }, // Tiger Eye
            { 43, 1.6 }, // Tourmaline
            { 44, 1.2 }, // Turquoise
            { 45, 1.6 }, // White Jade
            { 46, 1.2 }, // White Quartz
            { 47, 2.0 }, // White Sapphire
            { 48, 1.6 }, // Yellow Garnet
            { 49, 2.0 }, // Yellow Topaz
            { 50, 1.5 }, // Zircon
            { 51, 1.0 }, // Ivory
            { 52, 1.0 }, // Leather
            { 53, 1.2 }, // Armoredillo Hide
            { 54, 1.2 }, // Gromnie Hide
            { 55, 1.2 }, // Reedshark Hide
            { 56, 1.0 }, // Metal ====
            { 57, 1.2 }, // Brass
            { 58, 1.2 }, // Bronze
            { 59, 1.1 }, // Copper
            { 60, 1.8 }, // Gold
            { 61, 1.3 }, // Iron
            { 62, 2.0 }, // Pyreal
            { 63, 1.6 }, // Silver
            { 64, 1.4 }, // Steel
            { 65, 1.0 }, // Stone ====
            { 66, 1.4 }, // Alabaster
            { 67, 1.2 }, // Granite
            { 68, 1.6 }, // Marble
            { 69, 1.8 }, // Obsidian
            { 70, 1.0 }, // Sandstone
            { 71, 2.0 }, // Serpentine
            { 72, 1.0 }, // Wood ====
            { 73, 2.0 }, // Ebony
            { 74, 1.8 }, // Mahogany
            { 75, 1.4 }, // Oak
            { 76, 1.0 }, // Pine
            { 77, 1.2 }, // Teak
        };

        public static double getMaterialValueModifier(WorldObject wo)
        {
            if (wo.MaterialType != null && materialModifier.TryGetValue((int)wo.MaterialType, out var materialMod))
                return materialMod;
            else
                return 1.0;
        }

        public static double getGemMaterialValueModifier(WorldObject wo)
        {
            return getMaterialValueModifier(wo);
        }

        public static Dictionary<int, int> gemValues = new Dictionary<int, int>()
        {
            { 10, 100 },    // Agate
            { 11, 500 },    // Amber
            { 12, 1000 },   // Amethyst
            { 13, 2500 },   // Aquamarine
            { 14, 100 },    // Azurite
            { 15, 1000 },   // Black Garnet
            { 16, 5000 },   // Black Opal
            { 17, 500 },    // Bloodstone
            { 18, 500 },    // Carnelian
            { 19, 500 },    // Citrine
            { 20, 10000 },  // Diamond
            { 21, 10000 },  // Emerald
            { 22, 5000 },   // Fire Opal
            { 23, 2500 },   // Green Garnet
            { 24, 1000 },   // Green Jade
            { 25, 500 },    // Hematite
            { 26, 5000 },   // Imperial Topaz
            { 27, 1000 },   // Jet
            { 28, 100 },    // Lapis Lazuli
            { 29, 5000 },   // Lavender Jade
            { 30, 100 },    // Malachite
            { 31, 500 },    // Moonstone
            { 32, 500 },    // Onyx
            { 33, 2500 },   // Opal
            { 34, 2500 },   // Peridot
            { 35, 1000 },   // Red Garnet
            { 36, 5000 },   // Red Jade
            { 37, 500 },    // Rose Quartz
            { 38, 10000 },  // Ruby
            { 39, 10000 },  // Sapphire
            { 40, 100 },    // Smokey Quartz
            { 41, 5000 },   // Sunstone
            { 42, 100 },    // Tiger Eye
            { 43, 1000 },   // Tourmaline
            { 44, 100 },    // Turquoise
            { 45, 1000 },   // White Jade
            { 46, 100 },    // White Quartz
            { 47, 5000 },   // White Sapphire
            { 48, 1000 },   // Yellow Garnet
            { 49, 2500 },   // Yellow Topaz
            { 50, 1000 },   // Zircon
            { 51, 100 }     // Ivory
        };

        public static readonly int[][] SummoningEssencesMatrix =
        {
            new int[] { 48942, 48944, 48945, 48946, 48947, 48948, 48956 },
            new int[] { 49213, 49214, 49215, 49216, 49217, 49218, 49219 },
            new int[] { 49220, 49221, 49222, 49223, 49224, 49225, 49226 },
            new int[] { 49227, 49228, 49229, 49230, 49231, 49232, 49212 },
            new int[] { 48972, 49234, 49235, 49236, 49237, 49238, 49239 },
            new int[] { 49240, 49241, 49242, 49243, 49244, 49245, 49246 },
            new int[] { 49247, 49248, 49249, 49250, 49251, 49252, 49253 },
            new int[] { 49254, 49255, 49256, 49257, 49258, 49259, 49233 },
            new int[] { 49421, 49422, 49423, 49424, 49425, 49426, 49427 },
            new int[] { 49428, 49429, 49430, 49431, 49432, 49433, 49434 },
            new int[] { 49435, 49436, 49437, 49438, 49439, 49440, 49441 },
            new int[] { 49442, 49443, 49444, 49445, 49446, 49447, 49448 },
            new int[] { 48959, 48961, 48963, 48965, 48967, 48969, 48957 },
            new int[] { 49261, 49262, 49263, 49264, 49265, 49266, 49267 },
            new int[] { 49268, 49269, 49270, 49271, 49272, 49273, 49274 },
            new int[] { 49275, 49276, 49277, 49278, 49279, 49280, 49260 },
            new int[] { 49282, 49283, 49284, 49285, 49286, 49287, 49281 },
            new int[] { 49289, 49290, 49291, 49292, 49293, 49294, 49288 },
            new int[] { 49296, 49297, 49298, 49299, 49300, 49301, 49295 },
            new int[] { 49303, 49304, 49305, 49306, 49307, 49308, 49302 },
            new int[] { 49310, 49311, 49312, 49313, 49314, 49315, 49316 },
            new int[] { 49317, 49318, 49319, 49320, 49321, 49322, 49323 },
            new int[] { 49324, 49325, 49326, 49327, 49328, 49329, 49330 },
            new int[] { 49331, 49332, 49333, 49334, 49335, 49336, 49309 },
            new int[] { 49338, 49339, 49340, 49341, 49342, 49343, 49344 },
            new int[] { 49345, 49346, 49347, 49348, 49349, 49350, 49351 },
            new int[] { 49352, 49353, 49354, 49355, 49356, 49357, 49358 },
            new int[] { 49359, 49360, 49361, 49362, 49363, 49364, 49337 },
            new int[] { 49366, 49367, 49368, 49369, 49370, 49371, 49372 },
            new int[] { 49373, 49374, 49375, 49376, 49377, 49378, 49379 },
            new int[] { 49380, 49381, 49382, 49383, 49384, 49385, 49386 },
            new int[] { 49387, 49388, 49389, 49390, 49391, 49392, 49365 },
            new int[] { 49524, 49525, 49526, 49527, 49528, 49529, 49530 },
            new int[] { 49531, 49532, 49533, 49534, 49535, 49536, 49537 },
            new int[] { 49538, 49539, 49540, 49541, 49542, 49543, 49544 },
            new int[] { 49545, 49546, 49547, 49548, 49549, 49550, 49551 }
        };

        public static readonly int[][] HeavyWeaponsMatrix =
        {
            new int[] { 301, 3750, 3751, 3752, 3753 },       //  0 - Battle Axe
            new int[] { 344, 3865, 3866, 3867, 3868 },       //  1 - Silifi
            new int[] { 31769, 31770, 31771, 31772, 31768 }, //  2 - War Axe
            new int[] { 31764, 31765, 31766, 31767, 31763 }, //  3 - Lugian Hammer
            new int[] { 22440, 22441, 22442, 22443, 22444 }, //  4 - Dirk
            new int[] { 30601, 30602, 30603, 30604, 30605 }, //  5 - Stiletto (MS)
            new int[] { 319, 3794, 3795, 3796, 3797 },       //  6 - Jambiya (MS)
            new int[] { 30586, 30587, 30588, 30589, 30590 }, //  7 - Flanged Mace
            new int[] { 331, 3834, 3835, 3836, 3837 },       //  8 - Mace
            new int[] { 30581, 30582, 30583, 30584, 30585 }, //  9 - Mazule
            new int[] { 332, 3937, 3938, 3939, 3940 },       // 10 - Morning Star
            new int[] { 31778, 31779, 31780, 31781, 31782 }, // 11 - Spine Glaive
            new int[] { 30591, 30592, 30593, 30594, 30595 }, // 12 - Partizan
            new int[] { 7772, 7791, 7792, 7793, 7794 },      // 13 - Trident
            new int[] { 333, 22159, 22160, 22161, 22162 },   // 14 - Nabut
            new int[] { 31788, 31789, 31790, 31791, 31792 }, // 15 - Stick
            new int[] { 30576, 30577, 30578, 30579, 30580 }, // 16 - Flamberge
            new int[] { 327, 3822, 3823, 3824, 3825 },       // 17 - Ken
            new int[] { 351, 3881, 3882, 3883, 3884 },       // 18 - Long Sword
            new int[] { 353, 3889, 3890, 3891, 3892 },       // 19 - Tachi
            new int[] { 354, 3893, 3894, 3895, 3896 },       // 20 - Takuba
            new int[] { 45108, 45109, 45110, 45111, 45112 }, // 21 - Schlager (MS)
            new int[] { 4190, 4191, 4192, 4193, 4194 },      // 22 - Cestus
            new int[] { 4195, 4196, 4197, 4198, 4199 },      // 23 - Nekode
        };

        public static readonly int[][] LightWeaponsMatrix =
        {
            new int[] { 30561, 30562, 30563, 30564, 30565 }, //  0 - Dolabra
            new int[] { 303, 3754, 3755, 3756, 3757 },       //  1 - Hand Axe
            new int[] { 336, 3842, 3843, 3844, 3845 },       //  2 - Ono
            new int[] { 359, 3905, 3906, 3907, 3908 },       //  3 - War Hammer
            new int[] { 314, 3778, 3779, 3780, 3781 },       //  4 - Dagger (MS)
            new int[] { 328, 3826, 3827, 3828, 3829 },       //  5 - Khanjar
            new int[] { 309, 3766, 3767, 3768, 3769 },       //  6 - Club
            new int[] { 325, 3814, 3815, 3816, 3817 },       //  7 - Kasrullah
            new int[] { 7768, 7787, 7788, 7789, 7790 },      //  8 - Spiked Club
            new int[] { 348, 3873, 3874, 3875, 3876 },       //  9 - Spear
            new int[] { 362, 3913, 3914, 3915, 3916 },       // 10 - Yari
            new int[] { 338, 22164, 22165, 22166, 22167 },   // 11 - Quarter Staff
            new int[] { 350, 3877, 3878, 3879, 3880 },       // 12 - Broad Sword
            new int[] { 31759, 31760, 31761, 31762, 31758 }, // 13 - Dericost Blade
            new int[] { 45099, 45099, 45099, 45099, 45099 }, // 14 - Epee (MS)
            new int[] { 324, 3810, 3811, 3812, 3813 },       // 15 - Kaskara
            new int[] { 30571, 30572, 30573, 30574, 30575 }, // 16 - Spada
            new int[] { 340, 3853, 3854, 3855, 3856 },       // 17 - Shamshir
            new int[] { 30611, 30612, 30613, 30614, 30615 }, // 18 - Knuckles
            new int[] { 326, 3818, 3819, 3820, 3821 }        // 19 - Katar
        };

        public static readonly int[][] FinesseWeaponsMatrix =
        {
            new int[] { 45113, 45114, 45115, 45116, 45117 }, //  0 - Hammer
            new int[] { 30556, 30557, 30558, 30559, 30560 }, //  1 - Hatchet
            new int[] { 342, 3857, 3858, 3859, 3860 },       //  2 - Shou-ono
            new int[] { 357, 3901, 3902, 3903, 3904 },       //  3 - Tungi
            new int[] { 329, 3830, 3831, 3832, 3833 },       //  4 - Knife (MS)
            new int[] { 31794, 31795, 31796, 31797, 31793 }, //  5 - Lancet (MS)
            new int[] { 30596, 30597, 30598, 30599, 30600 }, //  6 - Poniard
            new int[] { 31774, 31775, 31776, 31777, 31773 }, //  7 - Board with Nail
            new int[] { 313, 3774, 3775, 3776, 3777 },       //  8 - Dabus
            new int[] { 356, 3897, 3898, 3899, 3900 },       //  9 - Tofun
            new int[] { 321, 3802, 3803, 3804, 3805 },       // 10 - Jitte
            new int[] { 308, 3762, 3763, 3764, 3765 },       // 11 - Budiaq
            new int[] { 7771, 7795, 7796, 7797, 7798 },      // 12 - Naginata
            new int[] { 30606, 30607, 30608, 30609, 30610 }, // 13 - Bastone
            new int[] { 322, 3806, 3807, 3808, 3809 },       // 14 - Jo
            new int[] { 6853, 45104, 45105, 45106, 45107 },  // 15 - Rapier (MS)
            new int[] { 30566, 30567, 30568, 30569, 30570 }, // 16 - Sabra
            new int[] { 339, 3849, 3850, 3851, 3852 },       // 17 - Scimitar
            new int[] { 352, 3885, 3886, 3887, 3888 },       // 18 - Short Sword
            new int[] { 345, 3869, 3870, 3871, 3872 },       // 19 - Simi
            new int[] { 361, 3909, 3910, 3911, 3912 },       // 20 - Yaoji
            new int[] { 31784, 31785, 31786, 31787, 31783 }, // 21 - Claw
            new int[] { 45118, 45119, 45120, 45121, 45122 }  // 22 - Hand Wraps
        };

        public static readonly int[][] TwoHandedWeaponsMatrix =
        {
            new int[] { 40760, 40761, 40762, 40763, 40764 }, //  0 - Nodachi
            new int[] { 41067, 41068, 41069, 41070, 41071 }, //  1 - Shashqa
            new int[] { 40618, 40619, 40620, 40621, 40622 }, //  2 - Spadone
            new int[] { 41057, 41058, 41059, 41060, 41061 }, //  3 - Great Star Mace
            new int[] { 40623, 40624, 40625, 40626, 40627 }, //  4 - Quadrelle
            new int[] { 41062, 41063, 41064, 41065, 41066 }, //  5 - Khanda-handled Mace
            new int[] { 40635, 40636, 40637, 40638, 40639 }, //  6 - Tetsubo
            new int[] { 41052, 41053, 41054, 41055, 41056 }, //  7 - Great Axe
            new int[] { 41036, 41037, 41038, 41039, 41040 }, //  8 - Assagai
            new int[] { 41046, 41047, 41048, 41049, 41050 }, //  9 - Pike
            new int[] { 40818, 40819, 40820, 40821, 40822 }, // 10 - Corsesca
            new int[] { 41041, 41042, 41043, 41044, 41045 }  // 11 - Magari Yari
        };

        public static readonly List<int[][]> MeleeWeaponsMatrices = new List<int[][]>()
        {
            HeavyWeaponsMatrix,
            LightWeaponsMatrix,
            FinesseWeaponsMatrix,
            TwoHandedWeaponsMatrix
        };

        public static readonly HashSet<uint> AetheriaWcids = new HashSet<uint>()
        {
            Server.Entity.Aetheria.AetheriaBlue,
            Server.Entity.Aetheria.AetheriaYellow,
            Server.Entity.Aetheria.AetheriaRed,
        };

        public static readonly int[,] HeavyWeaponDamageTable =
        {      //  0|250|300|325|350|370|400|420|430
                { 26, 33, 40, 47, 54, 61, 68, 71, 74 }, // Axe
                { 24, 31, 38, 45, 51, 58, 65, 68, 71 }, // Dagger
                { 13, 16, 20, 23, 26, 30, 33, 36, 38 }, // MultiDagger
                { 22, 29, 36, 43, 49, 56, 63, 66, 69 }, // Mace
                { 25, 32, 39, 46, 52, 59, 66, 69, 72 }, // Spear
                { 24, 31, 38, 45, 51, 58, 65, 68, 71 }, // Sword
                { 12, 16, 19, 23, 26, 30, 33, 36, 38 }, // MultiSword
                { 23, 30, 36, 43, 50, 56, 63, 66, 70 }, // Staff
                { 20, 26, 31, 37, 43, 48, 54, 56, 59 }  // UA
        };

        public static readonly int[,] LightWeaponDamageTable =
        {      //  0|250|300|325|350|370|400|420|430
                { 22, 28, 33, 39, 44, 50, 55, 58, 61 }, // Axe
                { 18, 24, 29, 35, 40, 46, 51, 54, 58 }, // Dagger
                {  7, 10, 13, 16, 18, 21, 24, 27, 28 }, // MultiDagger
                { 19, 24, 29, 35, 40, 45, 50, 53, 57 }, // Mace
                { 21, 26, 32, 37, 43, 48, 53, 56, 60 }, // Spear
                { 20, 25, 31, 36, 41, 47, 52, 55, 58 }, // Sword
                {  7, 10, 13, 16, 18, 21, 24, 25, 28 }, // MultiSword
                { 19, 24, 30, 35, 40, 46, 51, 54, 57 }, // Staff
                { 17, 22, 26, 31, 35, 40, 44, 46, 48 }  // UA
        };

        public static readonly int[,] TwoHandedWeaponDamageTable =
        {      //  0|250|300|325|350|370|400|420|430
                { 13, 17, 22, 26, 30, 35, 39, 42, 45 }, // Cleaving
                { 14, 19, 23, 28, 33, 37, 42, 45, 48 }  // Spears
        };

        public static readonly int[][] CasterWeaponsMatrix =
        {
            new int[] { 2366, 2548, 2547, 2472 }, // Orb, Sceptre, Staff, Wand
            new int[] { 29265, 29264, 29260, 29263, 29262, 29259, 29261, 43381 }, // Sceptre: Slashing, Piercing, Blunt, Frost, Fire, Acid, Electric, Nether
            new int[] { 31819, 31825, 31821, 31824, 31823, 31820, 31822, 43382 }, // Baton: Slashing, Piercing, Blunt, Frost, Fire, Acid, Electric, Nether
            new int[] { 37223, 37222, 37225, 37221, 37220, 37224, 37219, 43383 }  // Staff: Slashing, Piercing, Blunt, Frost, Fire, Acid, Electric, Nether
        };

        public static readonly float[][] MissileDamageMod =
        {
            new float[] { 2.1f, 2.1f, 2.2f, 2.3f, 2.4f, 2.4f, 2.4f, 2.4f, 2.4f }, // Bow
            new float[] { 2.4f, 2.4f, 2.5f, 2.55f, 2.65f, 2.65f, 2.65f, 2.65f, 2.65f }, // Crossbow
            new float[] { 2.3f, 2.3f, 2.4f, 2.5f, 2.6f, 2.6f, 2.6f, 2.6f, 2.6f }  // Thrown
        };

        public static readonly int[][] NonElementalMissileWeaponsMatrix =
        {
            new int[] { 306, 363, 334, 307, 341, 30625, 360 }, // Longbow, Yumi, Nayin, Shortbow, Shouyumi, War Bow, Yag
            new int[] { 30616, 311, 312 }, // Arbalest, Heavy Crossbow, Light Crossbow
            new int[] { 12463, 20640 } // Atlatl, Royal Atlatl
        };

        public static readonly int[][] ElementalMissileWeaponsMatrix =
        {
            new int[] { 29244, 29239, 29243, 29241, 29242, 29238, 29240 },
            new int[] { 29251, 29246, 29250, 29248, 29249, 29245, 29247 },
            new int[] { 29258, 29253, 29257, 29255, 29256, 29252, 29254 },
            new int[] { 31812, 31814, 31818, 31816, 31817, 31813, 31815 },
            new int[] { 31798, 31800, 31804, 31802, 31803, 31799, 31801 },
            new int[] { 31805, 31807, 31811, 31809, 31810, 31806, 31808 }
        };

        public static readonly List<int[][]> MissileWeaponsMatrices = new List<int[][]>()
        {
            NonElementalMissileWeaponsMatrix,
            ElementalMissileWeaponsMatrix,
        };

        public static readonly int[] DinnerwareLootMatrix = { 141, 142, 148, 149, 150, 154, 161, 163, 168, 243, 254, 7940 };

        /*public static readonly int[][] GemCreatureSpellMatrix =
        {
            new int[] { 2, 18, 256, 274, 298, 322, 5099, 418, 467, 557, 581, 605, 629, 653, 678, 702, 726, 750, 774, 798, 824, 850, 874, 898, 922, 946, 970, 982, 1349, 1373, 1397, 1421, 1445, 1715, 1739, 1763, 5779, 5803, 5827, 5851, 5875, 6116, 5411, 3499 },
            new int[] { 1328, 245, 257, 275, 299, 323, 5100, 419, 468, 558, 582, 606, 630, 654, 679, 703, 727, 751, 775, 799, 825, 851, 875, 899, 923, 947, 971, 983, 1350, 1374, 1398, 1422, 1446, 1716, 1740, 1764, 5780, 5804, 5828, 5852, 5876, 6117, 5412, 3500 },
            new int[] { 1329, 246, 258, 276, 300, 324, 5101, 420, 469, 559, 583, 607, 631, 655, 680, 704, 728, 752, 776, 800, 826, 852, 876, 900, 924, 948, 972, 984, 1351, 1375, 1399, 1423, 1447, 1717, 1741, 1765, 5781, 5805, 5829, 5853, 5877, 6118, 5413, 3501 },
            new int[] { 1330, 247, 259, 277, 301, 325, 5102, 421, 470, 560, 584, 608, 632, 656, 681, 705, 729, 753, 777, 801, 827, 853, 877, 901, 925, 949, 973, 985, 1352, 1376, 1400, 1424, 1448, 1718, 1742, 1766, 5782, 5806, 5830, 5854, 5878, 6119, 5414, 3502 },
            new int[] { 1331, 248, 260, 278, 302, 326, 5103, 422, 471, 561, 585, 609, 633, 657, 682, 706, 730, 754, 778, 802, 828, 854, 878, 902, 926, 950, 974, 986, 1353, 1377, 1401, 1425, 1449, 1719, 1743, 1767, 5783, 5807, 5831, 5855, 5879, 6120, 5415, 3503 },
            new int[] { 1332, 249, 261, 279, 303, 327, 5104, 423, 472, 562, 586, 610, 634, 658, 683, 707, 731, 755, 779, 803, 829, 855, 879, 903, 927, 951, 975, 987, 1354, 1378, 1402, 1426, 1450, 1720, 1744, 1768, 5784, 5808, 5832, 5856, 5880, 6121, 5416, 3504 },
            new int[] { 2087, 2245, 2243, 2281, 2203, 2223, 5105, 2309, 2207, 2215, 2249, 2267, 2323, 2287, 2195, 2197, 2251, 2277, 2325, 2289, 2293, 2227, 2241, 2263, 2271, 2233, 2257, 2301, 2061, 2059, 2081, 2067, 2091, 2211, 2237, 2191, 5785, 5809, 5833, 5857, 5881, 6122, 5417, 3505 },
            new int[] { 4325, 4560, 4558, 4596, 4518, 4538, 5032, 4624, 4522, 4530, 4564, 4582, 4638, 4602, 4510, 4512, 4566, 4592, 4640, 4604, 4608, 4542, 4556, 4578, 4586, 4548, 4572, 4616, 4299, 4297, 4319, 4305, 4329, 4526, 4552, 4506, 5786, 5810, 5834, 5858, 5882, 6123, 5418, 4499 }
        };

        public static readonly int[][] GemLifeSpellMatrix =
        {
            new int[] { 165, 54, 212, 515, 1018, 1030, 1066, 20, 1109, 1133, 24 },
            new int[] { 166, 189, 213, 516, 1019, 1031, 1067, 1090, 1110, 1134, 1308 },
            new int[] { 167, 190, 214, 517, 1020, 1032, 1068, 1091, 1111, 1135, 1309 },
            new int[] { 168, 191, 215, 518, 1021, 1033, 1069, 1092, 1112, 1136, 1310 },
            new int[] { 169, 192, 216, 519, 1022, 1034, 1070, 1093, 1113, 1137, 1311 },
            new int[] { 170, 193, 217, 520, 1023, 1035, 1071, 1094, 1114, 1138, 1312 },
            new int[] { 2185, 2187, 2183, 2149, 2153, 2155, 2159, 2157, 2151, 2161, 2053 },
            new int[] { 4496, 4498, 4494, 4460, 4464, 4466, 4470, 4468, 4462, 4472, 4291 }
        };*/

        public static readonly int[][] GemsWCIDsMatrix =
{
            new int[] { 2413, 2426, 2414, 2427, 2428, 2429, 2430, 2415, 2405, 2416, 2431, 2423, 2424, 2406, 2433, 2417, 2418, 2419, 2420 },
            new int[] { 2413, 2426, 2414, 2427, 2428, 2429, 2430, 2415, 2405, 2416, 2431, 2423, 2424, 2406, 2433, 2417, 2418, 2419, 2420, 2393, 2394, 2395, 2396, 2397, 2398, 2399, 2400, 2401 },
            new int[] { 2413, 2426, 2414, 2427, 2428, 2429, 2430, 2415, 2405, 2416, 2431, 2423, 2424, 2406, 2433, 2417, 2418, 2419, 2420, 2393, 2394, 2395, 2396, 2397, 2398, 2399, 2400, 2401, 2421, 2422, 2432, 2425},
            new int[] { 2413, 2426, 2414, 2427, 2428, 2429, 2430, 2415, 2405, 2416, 2431, 2423, 2424, 2406, 2433, 2417, 2418, 2419, 2420, 2393, 2394, 2395, 2396, 2397, 2398, 2399, 2400, 2401, 2421, 2422, 2432, 2425, 2402, 2403, 2404, 2407, 2408 },
            new int[] { 2413, 2426, 2414, 2427, 2428, 2429, 2430, 2415, 2405, 2416, 2431, 2423, 2424, 2406, 2433, 2417, 2418, 2419, 2420, 2393, 2394, 2395, 2396, 2397, 2398, 2399, 2400, 2401, 2421, 2422, 2432, 2425, 2402, 2403, 2404, 2407, 2408, 2409, 2410, 2411, 2412 }
        };

        public static readonly int[][] GemsMatrix =
        {
            new int[] { 10, 11, 14, 17, 18, 19, 25, 28, 29, 30, 31, 33, 34, 36, 37, 40, 42, 44, 46 },
            new int[] { 10, 11, 14, 17, 18, 19, 25, 28, 29, 30, 31, 33, 34, 36, 37, 40, 42, 44, 46, 12, 15, 24, 27, 35, 43, 45, 48, 50 },
            new int[] { 10, 11, 14, 17, 18, 19, 25, 28, 29, 30, 31, 33, 34, 36, 37, 40, 42, 44, 46, 12, 15, 24, 27, 35, 43, 45, 48, 50, 13, 23, 32, 49},
            new int[] { 10, 11, 14, 17, 18, 19, 25, 28, 29, 30, 31, 33, 34, 36, 37, 40, 42, 44, 46, 12, 15, 24, 27, 35, 43, 45, 48, 50, 13, 23, 32, 49, 16, 22, 26, 41, 47 },
            new int[] { 10, 11, 14, 17, 18, 19, 25, 28, 29, 30, 31, 33, 34, 36, 37, 40, 42, 44, 46, 12, 15, 24, 27, 35, 43, 45, 48, 50, 13, 23, 32, 49, 16, 22, 26, 41, 47, 20, 21, 38, 39 }
        };

        public static readonly int[][] GemSpellIndexMatrix =
        {
            new int[] { 0,1 },
            new int[] { 1,2 },
            new int[] { 2,3 },
            new int[] { 3,4 },
            new int[] { 4,5 },
            new int[] { 5,6 },
            new int[] { 6,7 },
            new int[] { 6,7 }
        };

        public static readonly int[] food = { 258, 4746, 259, 547, 260, 5758, 261, 262, 263, 264, 265 };

        public static readonly int[][] GenericLootMatrix =
        {
            new int[] { 8329, 27331, 2434, 378, 377, 379, 2457, 2460, 27326, 628, 629, 513, 545, 42518 },
            new int[] { 8329, 8328, 27331, 2434, 2435, 378, 377, 379, 2457, 2460, 27326, 2470, 27319, 27322, 629, 630, 513, 545, 512, 42518 },
            new int[] { 8329, 8328, 8326, 2434, 2435, 27330, 27326, 377, 27322, 27319, 2460, 2470, 27324, 2458, 2461, 629, 630, 631, 545, 512, 514, 42518, 42517 },
            new int[] { 8329, 8328, 8326, 2434, 2435, 27330, 27326, 377, 27322, 27319, 2460, 2470, 27324, 2458, 2461, 629, 630, 631, 545, 512, 514, 42518, 42517, 42516 },
            new int[] { 8326, 8331, 8327, 27330, 2436, 27328, 27324, 2458, 2461, 27320, 27323, 27327, 27325, 27318, 27321, 631, 632, 9229, 514, 515, 516 },
            new int[] { 8331, 8327, 8330, 2436, 27328, 27324, 2458, 2461, 27320, 27323, 27327, 27325, 27318, 27321, 631, 632, 9229, 514, 515, 516 },
            new int[] { 8331, 8327, 8330, 2436, 27328, 27324, 2458, 2461, 27320, 27323, 27327, 27325, 27318, 27321, 631, 632, 9229, 514, 515, 516 },
            new int[] { 8331, 8327, 8330, 2436, 27328, 27324, 2458, 2461, 27320, 27323, 27327, 27325, 27318, 27321, 631, 632, 9229, 514, 515, 516 }
        };

        // Level 8 spell components
        public static readonly int[] Level8SpellComps = { 37363, 37365, 37362, 37364, 37360, 37361, 37353, 37354, 37355, 37357, 37358, 37356, 37359, 37343, 37344, 37345, 37346, 37347, 37349, 37350,
                         37342, 37351, 43379, 37352, 45370, 45371, 37300, 37373, 37301, 37302, 37303, 37348, 37304, 37305, 37369, 37309, 37310, 37311, 37312, 37313, 37339,
                         37314, 37315, 37316, 37317, 38760, 37318, 37319, 37321, 37323, 37324, 37338, 37325, 43387, 37326, 37327, 37328, 45372, 37307, 37329, 37330, 37331,
                         45373, 37332, 45374, 37333, 37336, 37337, 49455, 41747, 43380, 37340, 37341 };

        public static readonly int[][] ScrollLootMatrix =
        {
            // Spell level equals # value in array + 1
            new int[] { 0, 2 },
            new int[] { 2, 4 },
            new int[] { 3, 5 },
            new int[] { 4, 6 },
            new int[] { 5, 6 },
            new int[] { 6, 6 },
            new int[] { 6, 6 }
        };

        /*public static readonly int[][] ScrollSpells =
        {
            /// CREATURE SPELLS ///

            // ATTRIBUTES //

            ////Strength
            new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087 },
            ////Strength Other
            new int[] { 1, 1333, 1334, 1335, 1336, 1337, 2086 },
            ////Weakness Other
            new int[] { 3, 1339, 1340, 1341, 1342, 1343, 2088 },
            ////Endurance
            new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061 },
            ////Endurance Other
            new int[] { 1355, 1356, 1357, 1358, 1359, 1360, 2060 },
            ////Fraility Other
            new int[] { 1367, 1368, 1369, 1370, 1371, 1372, 2068 },
            ////Coordination
            new int[] { 1373, 1374, 1375, 1376, 1377, 1378, 2059 },
            ////Coordination Other
            new int[] { 1379, 1380, 1381, 1382, 1383, 1384, 2058 },
            ////Clumsy Other
            new int[] { 1391, 1392, 1393, 1394, 1395, 1396, 2056 },
            ////Quickness
            new int[] { 1397, 1398, 1399, 1400, 1401, 1402, 2081 },
            ////Quickness Other
            new int[] { 1403, 1404, 1405, 1406, 1407, 1408, 2080 },
            ////Slowness Other
            new int[] { 1415, 1416, 1417, 1418, 1419, 1420, 2084 },
            ////Focus
            new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067 },
            ////Focus Other
            new int[] { 1427, 1428, 1429, 1430, 1431, 1432, 2066 },
            ////Bafflement Other
            new int[] { 1439, 1440, 1441, 1442, 1443, 1444, 2054 },
            ////Willpower
            new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091 },
            ////Willpower Other
            new int[] { 1451, 1452, 1453, 1454, 1455, 1456, 2090 },
            ////Feeblemind Other
            new int[] { 1463, 1464, 1465, 1466, 1467, 1468, 2064 },

            // DEFENSES //

            ////Invuln
            new int[] { 18, 245, 246, 247, 248, 249, 2245 },
            ////Invuln Other
            new int[] { 17, 240, 241, 242, 243, 244, 2244 },
            ////Vuln Other
            new int[] { 15, 230, 231, 232, 233, 234, 2318 },
            ////Impreg
            new int[] { 256, 257, 258, 259, 260, 261, 2243 },
            ////Impreg Other
            new int[] { 250, 251, 252, 253, 254, 255, 2242 },
            ////Defenselessness Other
            new int[] { 262, 263, 264, 265, 266, 267, 2228 },
            ////Magic Resist
            new int[] { 274, 275, 276, 277, 278, 279, 2281 },
            ////Magic Resist Other
            new int[] { 268, 269, 270, 271, 272, 273, 2280 },
            ////Magic Yield Other
            new int[] { 280, 281, 282, 283, 284, 285, 2282 },

            // MASTERIES //

            ////Light Weapon
            new int[] { 298, 299, 300, 301, 302, 303, 2203 },
            ////Light Weapon Other
            new int[] { 292, 293, 294, 295, 296, 297, 2202 },
            ////Finesse Weapon
            new int[] { 322, 323, 324, 325, 326, 327, 2223 },
            ////Finesse Weapon Other
            new int[] { 316, 317, 318, 319, 320, 321, 2222 },
            ////Two Handed
            new int[] { 5099, 5100, 5101, 5102, 5103, 5104, 5105 },
            ////Two Handed Other
            new int[] { 5091, 5092, 5093, 5094, 5095, 5096, 5097 },
            ////Heavy Weapon
            new int[] { 418, 419, 420, 421, 422, 423, 2309 },
            ////Heavy Weapon Other
            new int[] { 412, 413, 414, 415, 416, 417, 2308 },
            ////Missile Weapon
            new int[] { 467, 468, 469, 470, 471, 472, 2207 },
            ////Missile Weapon Other
            new int[] { 461, 462, 463, 464, 465, 466, 2206 },
            ////Creature Enchant
            new int[] { 557, 558, 559, 560, 561, 562, 2215 },
            ////Creature Enchant Other
            new int[] { 563, 564, 565, 566, 567, 568, 2214 },
            ////Item Enchant
            new int[] { 581, 582, 583, 584, 585, 586, 2249 },
            ////Item Enchant Other
            new int[] { 587, 588, 589, 590, 591, 592, 2248 },
            ////Life Magic
            new int[] { 605, 606, 607, 608, 609, 610, 2267 },
            ////Life Magic Other
            new int[] { 611, 612, 613, 614, 615, 616, 2266 },
            ////War Magic
            new int[] { 629, 630, 631, 632, 633, 634, 2323 },
            ////War Magic Other
            new int[] { 635, 636, 637, 638, 639, 640, 2322 },
            ////Mana C
            new int[] { 653, 654, 655, 656, 657, 658, 2287 },
            ////Mana C Other
            new int[] { 659, 660, 661, 662, 663, 664, 2286 },
            ////Arcane Enlight
            new int[] { 678, 679, 680, 681, 682, 683, 2195 },
            ////Arcane Enlight Other
            new int[] { 684, 685, 686, 687, 688, 689, 2194 },
            ////Armor Tinkering
            new int[] { 702, 703, 704, 705, 706, 707, 2197 },
            ////Armor Tinkering Other
            new int[] { 708, 709, 710, 711, 712, 713, 2196 },
            ////Item Tinkering
            new int[] { 726, 727, 728, 729, 730, 731, 2251 },
            ////Item Tinkering Other
            new int[] { 732, 733, 734, 735, 736, 737, 2250 },
            ////Magic Item Tinkering
            new int[] { 750, 751, 752, 753, 754, 755, 2277 },
            ////Magic Item Tinkering Other
            new int[] { 756, 757, 758, 759, 760, 761, 2276 },
            ////Weapon Tinkering
            new int[] { 774, 775, 776, 777, 778, 779, 2325 },
            ////Weapon Tinkering Other
            new int[] { 780, 781, 782, 783, 784, 785, 2324 },
            ////Monster Attunement
            new int[] { 798, 799, 800, 801, 802, 803, 2289 },
            ////Monster Attunement Other
            new int[] { 804, 805, 806, 807, 808, 809, 2288 },
            ////Person Attunement
            new int[] { 824, 825, 826, 827, 828, 829, 2293 },
            ////Person Attunement Other
            new int[] { 830, 831, 832, 833, 834, 835, 2292 },
            ////Deception Mastery
            new int[] { 850, 851, 852, 853, 854, 855, 2227 },
            ////Deception Mastery Other
            new int[] { 856, 857, 858, 859, 860, 861, 2226 },
            ////Healing Mastery
            new int[] { 874, 875, 876, 877, 878, 879, 2241 },
            ////Healing Mastery Other
            new int[] { 880, 881, 882, 883, 884, 885, 2240 },
            ////Leadership
            new int[] { 898, 899, 900, 901, 902, 903, 2263 },
            ////Leadership Other
            new int[] { 904, 905, 906, 907, 908, 909, 2262 },
            ////Lockpick
            new int[] { 922, 923, 924, 925, 926, 927, 2271 },
            ////Lockpick Other
            new int[] { 928, 929, 930, 931, 932, 933, 2270 },
            ////Fealty
            new int[] { 946, 947, 948, 949, 950, 951, 2233 },
            ////Fealty Other
            new int[] { 952, 953, 954, 955, 956, 957, 2232 },
            ////Jumping
            new int[] { 970, 971, 972, 973, 974, 975, 2257 },
            ////Jumping Other
            new int[] { 976, 977, 978, 979, 980, 981, 2256 },
            ////Sprint
            new int[] { 982, 983, 984, 985, 986, 987, 2301 },
            ////Sprint Other
            new int[] { 988, 989, 990, 991, 992, 993, 2300 },
            ////Cooking Mastery
            new int[] { 1715, 1716, 1717, 1718, 1719, 1720, 2211 },
            ////Cooking Mastery Other
            new int[] { 1709, 1710, 1711, 1712, 1713, 1714, 2210 },
            ////Fletching
            new int[] { 1739, 1740, 1741, 1742, 1743, 1744, 2237 },
            ////Fletching Other
            new int[] { 1733, 1734, 1735, 1736, 1737, 1738, 2236 },
            ////Alchemy
            new int[] { 1763, 1764, 1765, 1766, 1767, 1768, 2191 },
            ////Alchemy Other
            new int[] { 1757, 1758, 1759, 1760, 1761, 1762, 2190 },
            ////Dirty Fighting
            new int[] { 5779, 5780, 5781, 5782, 5783, 5784, 5785 },
            ////Dirty Fighting Other
            new int[] { 5771, 5772, 5773, 5774, 5775, 5776, 5777 },
            ////Dual Wield
            new int[] { 5803, 5804, 5805, 5806, 5807, 5808, 5809 },
            ////Dual Wield Other
            new int[] { 5795, 5796, 5797, 5798, 5799, 5800, 5801 },
            ////Recklessness
            new int[] { 5827, 5828, 5829, 5830, 5831, 5832, 5833 },
            ////Recklessness Other
            new int[] { 5819, 5820, 5821, 5822, 5823, 5824, 5825 },
            ////Shield
            new int[] { 5851, 5852, 5853, 5854, 5855, 5856, 5857 },
            ////Shield Other
            new int[] { 5843, 5844, 5845, 5846, 5847, 5848, 5849 },
            ////Sneak Attack
            new int[] { 5875, 5876, 5877, 5878, 5879, 5880, 5881 },
            ////Sneak Attack Other
            new int[] { 5867, 5868, 5869, 5870, 5871, 5872, 5873 },
            ////Summoning
            new int[] { 6116, 6117, 6118, 6119, 6120, 6121, 6122 },
            ////Summoning Other
            new int[] { 6108, 6109, 6110, 6111, 6112, 6113, 6114 },
            ////Void Magic
            new int[] { 5411, 5412, 5413, 5414, 5415, 5416, 5417 },
            ////Void Magic Other
            new int[] { 5403, 5404, 5405, 5406, 5407, 5408, 5409 },
            ////Salvaging
            new int[] { 3499, 3500, 3501, 3502, 3503, 3504, 3505 },
            ////Salvaging Other
            new int[] { 3506, 3507, 3508, 3509, 3510, 3511, 3512 },

            // INEPTITUDES //

            ////Alchemy Ineptitude                  Bottle Breaker
            new int[] { 1769, 1770, 1771, 1772, 1773, 1774, 2188 },
            ////Arcane Benightedness                Hands of Chorizite
            new int[] { 696, 697, 698, 699, 700, 701, 2192 },
            ////Armor Tinkering Ignorance   	    Jibril's Vitae
            new int[] { 720, 721, 722, 723, 724, 725, 2198 },
            ////Cooking Ineptitude  	            Challenger's Legacy
            new int[] { 1721, 1722, 1723, 1724, 1725, 1726, 2208 },
            ////Creature Enchantment Ineptitude 	Wrath of Adja
            new int[] { 569, 570, 571, 572, 573, 574, 2212 },
            ////Deception Ineptitude                Heart on Sleeves
            new int[] { 868, 869, 870, 871, 872, 873, 2224 },
            ////Dirty Fighting Ineptitude	        Dirty Fighting Ineptitude Other
            new int[] { 5763, 5764, 5765, 5766, 5767, 5768, 5769 },
            ////Dual Wield Ineptitude               Dual Wield Ineptitude Other
            new int[] { 5787, 5788, 5789, 5790, 5791, 5792, 5793 },
            ////Faithlessness   	                Sashi Mu's Kiss
            new int[] { 964, 965, 966, 967, 968, 969, 2230 },
            ////Finesse Weapon Ineptitude           Finesse Weapon Ineptitude Other
            new int[] { 328, 329, 330, 331, 332, 333, 2220 },
            ////Fletching Ineptitude    	        Twisted Digits
            new int[] { 1745, 1746, 1747, 1748, 1749, 1750, 2234 },
            ////Healing Ineptitude          	    Unsteady Hands
            new int[] { 892, 893, 894, 895, 896, 897, 2238 },
            ////Heavy Weapon Ineptitude         	Heavy Weapon Ineptitude Other
            new int[] { 424, 425, 426, 427, 428, 429, 2306 },
            ////Item Enchantment Ineptitude         Wrath of Celcynd
            new int[] { 593, 594, 595, 596, 597, 598, 2246 },
            ////Item Tinkering Ignorance            Unfortunate Appraisal - has a duplicate spell ID 5043?
            new int[] { 744, 745, 746, 747, 748, 749, 2252 },   
            ////Jumping Ineptitude                  Feat of Radaz
            new int[] { 1012, 1013, 1014, 1015, 1016, 1017, 2254 },
            ////Leaden Feet Run	                    Gears Unwound
            new int[] { 1000, 1001, 1002, 1003, 1004, 1005, 2258 },
            ////Leadership Ineptitude               Kwipetian Vision
            new int[] { 916, 917, 918, 919, 920, 921, 2260 },
            ////Life Magic Ineptitude               Wrath of Harlune
            new int[] { 623, 624, 625, 626, 627, 628, 2264 },
            ////Light Weapon Ineptitude             Light Weapon Ineptitude Other - lots of duplicates probably from the original weapon skills
            new int[] { 304, 305, 306, 307, 308, 309, 2200 },
            ////Lockpick Ineptitude                 Fat Fingers
            new int[] { 940, 941, 942, 943, 944, 945, 2268 },
            ////Magic Item Tinkering Ignorance      Eyes Clouded
            new int[] { 768, 769, 770, 771, 772, 773, 2278 },
            ////Mana Conversion Ineptitude          Inefficient Investment
            new int[] { 672, 673, 674, 675, 676, 677, 2284 },
            ////Missile Weapon Ineptitude           Missile Weapon Ineptitude Other - more duplicates... bow, xbow, thrown
            new int[] { 473, 474, 475, 476, 477, 478, 2204 },
            ////Monster Unfamiliarity               Ignorance's Bliss
            new int[] { 817, 818, 819, 820, 821, 822, 2290 },
            ////Person Unfamiliarity                Introversion
            new int[] { 843, 844, 845, 846, 847, 848, 2294 },
            ////Recklessness Ineptitude             Recklessness Ineptitude Other
            new int[] { 5811, 5812, 5813, 5814, 5815, 5816, 5817 },
            ////Shield Ineptitude                   Shield Ineptitude Other
            new int[] { 5835, 5836, 5837, 5838, 5839, 5840, 5841 },
            ////Sneak Attack Ineptitude             Sneak Attack Ineptitude Other
            new int[] { 5859, 5860, 5861, 5862, 5863, 5864, 5865 },
            ////Two Handed Ineptitude               Greased Palms
            new int[] { 5075, 5076, 5077, 5078, 5079, 5080, 5081 }, 
            ////Void Magic Ineptitude               Void Magic Ineptitude Other
            new int[] { 5419, 5420, 5421, 5422, 5423, 5424, 5425 },
            ////War Magic Ineptitude                Wrath of the Hieromancer
            new int[] { 647, 648, 649, 650, 651, 652, 2320 },
            ////Weapon Tinkering Ignorance          Eye of the Grunt
            new int[] { 792, 793, 794, 795, 796, 797, 2326 },
            ////Summoning Inept                     Inept Other VII
            new int[] { 6129, 6130, 6131, 6132, 6133, 6134, 6135 },

            // DISPELLS //

            ////Dispell
            new int[] { 1888, 1894, 1900, 1906, 1912, 1918, 3185 },
            ////Dispell Other
            new int[] { 1885, 1891, 1897, 1903, 1909, 1915, 3184 },

            /// LIFE SPELLS ///

            // VITALS //

            ////Regeneration
            new int[] { 165, 166, 167, 168, 169, 170, 2185 },
            ////Regeneration Other
            new int[] { 159, 160, 161, 162, 163, 164, 2184 },
            ////Fester Other
            new int[] { 171, 172, 173, 174, 175, 176, 2178 },
            ////Rejuvenation
            new int[] { 54, 189, 190, 191, 192, 193, 2187 },
            ////Rejuvenation Other
            new int[] { 53, 184, 185, 186, 187, 188, 2186 },
            ////Exhaustion Other
            new int[] { 194, 195, 196, 197, 198, 199, 2176 },
            ////Mana Renewal
            new int[] { 212, 213, 214, 215, 216, 217, 2183 },
            ////Mana Renewal Other
            new int[] { 206, 207, 208, 209, 210, 211, 2182 },
            ////Mana Depletion Other
            new int[] { 218, 219, 220, 221, 222, 223, 2180 },
            ////Infuse Health
            new int[] { 1225, 1226, 1227, 1228, 1229, 1230, 2335 },
            ////Infuse Stamina
            new int[] { 1243, 1244, 1245, 1246, 1247, 1248, 2337 },
            ////Infuse Mana
            new int[] { 9, 1255, 1256, 1257, 1258, 1259, 2336 },
            ////Stamina to Mana
            new int[] { 1676, 1677, 1678, 1679, 1680, 1681, 2345 },
            ////Stamina to Health
            new int[] { 1664, 1665, 1666, 1667, 1668, 1669, 2343 },
            ////Health to Stamina
            new int[] { 1272, 1273, 1274, 1275, 1276, 1277, 2334 },
            ////Health to Mana
            new int[] { 1278, 1279, 1280, 1702, 1703, 1704, 2332 },
            ////Mana to Health
            new int[] { 1290, 1291, 1292, 1293, 1294, 1295, 2339 },
            ////Mana to Stamina
            new int[] { 1296, 1297, 1298, 1299, 1300, 1301, 2341 },
            ////Heal Self
            new int[] { 6, 1157, 1158, 1159, 1160, 1161, 2073 },
            ////Heal Other
            new int[] { 5, 1162, 1163, 1164, 1165, 1166, 2072 },
            ////Harm Other
            new int[] { 7, 1172, 1173, 1174, 1175, 1176, 2070 },
            ////Revit Self
            new int[] { 1177, 1178, 1179, 1180, 1181, 1182, 2083 },
            ////Revit Other
            new int[] { 1183, 1184, 1185, 1186, 1187, 1188, 2082 },
            ////Enfeeble Other
            new int[] { 1195, 1196, 1197, 1198, 1199, 1200, 2062 },
            ////Mana Drain
            new int[] { 1219, 1220, 1221, 1222, 1223, 1224, 2078 },
            ////Drain Health
            new int[] { 1237, 1238, 1239, 1240, 1241, 1242, 2328 },
            ////Drain Stamina
            new int[] { 1249, 1250, 1251, 1252, 1253, 1254, 2330 },
            ////Drain Mana
            new int[] { 1260, 1261, 1262, 1263, 1264, 1265, 2329 },
            ////Martyrs Hetacomb
            new int[] { 2760, 2761, 2762, 2763, 2764, 2765, 2766 },
            ////Martyrs Tenacity
            new int[] { 2767, 2768, 2769, 2770, 2771, 2772, 2773 },
            ////Martyrs Blight
            new int[] { 2774, 2775, 2776, 2777, 2778, 2779, 2780 },

            // PROTECTIONS & VULNS //

            ////Acid Prot
            new int[] { 515, 516, 517, 518, 519, 520, 2149 },
            ////Acid Prot Other
            new int[] { 509, 510, 511, 512, 513, 514, 2148 },
            ////Acid Vuln Other
            new int[] { 521, 522, 523, 524, 525, 526, 2162 },
            ////Blud Prot
            new int[] { 1018, 1019, 1020, 1021, 1022, 1023, 2153 },
            ////Blud Prot Other
            new int[] { 1024, 1025, 1026, 1027, 1028, 1029, 2152 },
            ////Blud Vuln Other
            new int[] { 1048, 1049, 1050, 1051, 1052, 1053, 2166 },
            ////Cold Prot
            new int[] { 1030, 1031, 1032, 1033, 1034, 1035, 2155 },
            ////Cold Prot Other
            new int[] { 1036, 1037, 1038, 1039, 1040, 1041, 2154 },
            ////Cold Vuln Other
            new int[] { 1060, 1061, 1062, 1063, 1064, 1065, 2168 },
            ////Lightning Prot
            new int[] { 1066, 1067, 1068, 1069, 1070, 1071, 2159 },
            ////Lightning Prot Other
            new int[] { 1072, 1073, 1074, 1075, 1076, 1077, 2158 },
            ////Lightning Vuln Other
            new int[] { 1084, 1085, 1086, 1087, 1088, 1089, 2172 },
            ////Fire Prot
            new int[] { 20, 1090,1091, 1092, 1093, 1094, 2157 },
            ////Fire Prot Other
            new int[] { 19, 810, 836, 849, 1095, 1096, 2156 },
            ////Fire Vuln Other
            new int[] { 21, 1104, 1105, 1106, 1107, 1108, 2170 },
            ////Blade Prot
            new int[] { 1109, 1110, 1111, 1112, 1113, 1114, 2151 },
            ////Blade Prot Other
            new int[] { 1115, 1116, 1117, 1118, 1119, 1120, 2150 },
            ////Blade Vuln Other
            new int[] { 1127, 1128, 1129, 1130, 1131, 1132, 2164 },
            ////Pierce Prot
            new int[] { 1133, 1134, 1135, 1136, 1137, 1138, 2161 },
            ////Pierce Prot Other
            new int[] { 1139, 1140, 1141, 1142, 1143, 1144, 2160 },
            ////Pierce Vuln Other
            new int[] { 1151, 1152, 1153, 1154, 1155, 1156, 2174 },
            ////Armor Self
            new int[] { 24, 1308, 1309, 1310, 1311, 1312, 2053 },
            ////Armor Other
            new int[] { 23, 1313, 1314, 1315, 1316, 1317, 2052 },
            ////Imperil Other
            new int[] { 25, 1323, 1324, 1325, 1326, 1327, 2074 },

            // DISPELLS //

            ////Dispell
            new int[] { 1960, 1966, 1972, 1978, 1984, 1990, 3194 },
            ////Dispell Other
            new int[] { 1957, 1963, 1969, 1975, 1981, 1987, 3193 },

            /// ITEM SPELLS ///

            // ARMOR //

            ////Impen
            new int[] { 51, 1482, 1483, 1484, 1485, 1486, 2108 },
            ////Brittlemail
            new int[] { 1487, 1488, 1489, 1490, 1491, 1492, 2100 },
            ////Blade Bane
            new int[] { 37, 1558, 1559, 1560, 1561, 1562, 2094 },
            ////Blade Lure
            new int[] { 38, 1553, 1554, 1555, 1556, 1557, 2095 },
            ////Pierce Bane
            new int[] { 1569, 1570, 1571, 1572, 1573, 1574, 2113 },
            ////Pierce Lure
            new int[] { 1563, 1564, 1565, 1566, 1567, 1568, 2114 },
            ////Blud Bane
            new int[] { 1511, 1512, 1513, 1514, 1515, 1516, 2098 },
            ////Blud Lure
            new int[] { 1505, 1506, 1507, 1508, 1509, 1510, 2099 },
            ////Acid Bane
            new int[] { 1493, 1494, 1495, 1496, 1497, 1498, 2092 },
            ////Acid Lure
            new int[] { 1499, 1500, 1501, 1502, 1503, 1504, 2093 },
            ////Frost Bane
            new int[] { 1523, 1524, 1525, 1526, 1527, 1528, 2104 },
            ////Frost Lure
            new int[] { 1517, 1518, 1519, 1520, 1521, 1522, 2105 },
            ////Lightning Bane
            new int[] { 1535, 1536, 1537, 1538, 1539, 1540, 2110 },
            ////Lightning Lure
            new int[] { 1529, 1530, 1531, 1532, 1533, 1534, 2111 },
            ////Flame Bane
            new int[] { 1547, 1548, 1549, 1550, 1551, 1552, 2102 },
            ////Flame Lure
            new int[] { 1541, 1542, 1543, 1544, 1545, 1546, 2103 },

            // WEAPONS //

            ////Blooddrinker Self
            new int[] { 35, 1612, 1613, 1614, 1615, 1616, 2096 },
            ////Blooddrinker Other
            new int[] { 5990, 5991, 5992, 5993, 5994, 5995, 5996 },
            ////Blood Loather
            new int[] { 36, 1617, 1618, 1619, 1620, 1621, 2097 },
            ////Heart Seeker Self
            new int[] { 1587, 1588, 1589, 1590, 1591, 1592, 2106 },
            ////Heart Seeker Other
            new int[] { 6007, 6008, 6009, 6010, 6011, 6012, 6013 },
            ////Turn Blade
            new int[] { 1593, 1594, 1595, 1596, 1597, 1598, 2118 },
            ////Defender Self
            new int[] { 1599, 1601, 1602, 1603, 1604, 1605, 2101 },
            ////Defender Other
            new int[] { 5999, 6000, 6001, 6002, 6003, 6004, 6005 },
            ////Lure Blade
            new int[] { 1606, 1607, 1608, 1609, 1610, 1611, 2112 },
            ////SwiftKiller Self
            new int[] { 49, 1623, 1624, 1625, 1626, 1627, 2116 },
            ////SwiftKiller Other
            new int[] { 6024, 6025, 6026, 6027, 6028, 6029, 6030 },
            ////Leaden Weapon
            new int[] { 50, 1629, 1630, 1631, 1632, 1633, 2109 },
            ////Hermetic Link Self
            new int[] { 1475, 1476, 1477, 1478, 1479, 1480, 2117 },
            ////Hermetic Link Other
            new int[] { 5982, 5983, 5984, 5985, 5986, 5987, 5988 },
            ////Hermetic Void
            new int[] { 1469, 1470, 1471, 1472, 1473, 1474, 2107 },
            ////Spirit Drinker Self
            new int[] { 3253, 3254, 3255, 3256, 3257, 3258, 3259 },
            ////Spirit Drinker Other
            new int[] { 6015, 6016, 6017, 6018, 6019, 6020, 6021 },
            ////Spirit Loather
            new int[] { 3260, 3261, 3262, 3263, 3264, 3265, 3266 },

            // MISC //

            ////Strengthen Lock
            new int[] { 1575, 1576, 1577, 1578, 1579, 1580, 2115 },
            ////Weaken Lock
            new int[] { 1581, 1582, 1583, 1584, 1585, 1586, 2119 },
            ////Dispells
            new int[] { 1921, 1927, 1933, 1939, 1945, 1951, 3190 },
            ////Portal Spells - Dont typically find these in loot.

            /// WAR SPELLS ///

            // Flame Bolt
            new int[] { 27, 81, 82, 83, 84, 85, 2128 },
            // Frost Bolt
            new int[] { 28, 70, 71, 72, 73, 74, 2136 },
            // Acid Stream
            new int[] { 58, 59, 60, 61, 62, 63, 2122 },
            // Shock Wave
            new int[] { 64, 65, 66, 67, 68, 69, 2144 },
            // Lightning Bolt
            new int[] { 75, 76, 77, 78, 79, 80, 2140 },
            // Force Bolt
            new int[] { 86, 87, 88, 89, 90, 91, 2132 },
            // Whirling Blade
            new int[] { 92, 93, 94, 95, 96, 97, 2146 },
            // Acid Streak
            new int[] { 1790, 1791, 1792, 1793, 1794, 1795, 2121 },
            // Flame Streak
            new int[] { 1796, 1797, 1798, 1799, 1800, 1801, 2129 },
            // Force Streak
            new int[] { 1802, 1803, 1804, 1805, 1806, 1807, 2133 },
            // Frost Streak
            new int[] { 1808, 1809, 1810, 1811, 1812, 1813, 2137 },
            // Lightning Streak
            new int[] { 1814, 1815, 1816, 1817, 1818, 1819, 2141 },
            // Shock Wave Streak
            new int[] { 1820, 1821, 1822, 1823, 1824, 1825, 2145 },
            // Whirling Blade Streak
            new int[] { 1826, 1827, 1828, 1829, 1830, 1831, 2147 },
            // Acid Arc 
            new int[] { 2711, 2712, 2713, 2714, 2715, 2716, 2717 },
            // Force Arc
            new int[] { 2718, 2719, 2720, 2721, 2722, 2723, 2724 },
            // Frost Arc
            new int[] { 2725, 2726, 2727, 2728, 2729, 2730, 2731 },
            // Lightning Arc
            new int[] { 2732, 2733, 2734, 2735, 2736, 2737, 2738 },
            // Flame Arc
            new int[] { 2739, 2740, 2741, 2742, 2743, 2744, 2745 },
            // Shock Arc
            new int[] { 2746, 2747, 2748, 2749, 2750, 2751, 2752 },
            // Blade Arc
            new int[] { 2753, 2754, 2755, 2756, 2757, 2758, 2759 },
            // Acid Blast
            new int[] { 0, 0, 99, 100, 101, 102, 2120 },
            // Shock Blast
            new int[] { 0, 0, 103, 104, 105, 106, 2143 },
            // Frost Blast
            new int[] { 0, 0, 107, 108, 109, 110, 2135 },
            // Lightning Blast
            new int[] { 0, 0, 111, 112, 113, 114, 2139 },
            // Flame Blast
            new int[] { 0, 0, 115, 116, 117, 118, 2127 },
            // Force Blast
            new int[] { 0, 0, 119, 120, 121, 122, 2131 },
            // Blade Blast
            new int[] { 0, 0, 123, 124, 125, 126, 2124 },
            // Acid Volley
            new int[] { 0, 0, 127, 128, 129, 130, 2123 },
            // Bludgeoning Volley
            new int[] { 0, 0, 131, 132, 133, 134, 2126 },
            // Frost Volley
            new int[] { 0, 0, 135, 136, 137, 138, 2138 },
            // Lightning Volley
            new int[] { 0, 0, 139, 140, 141, 142, 2142 },
            // Flame Volley
            new int[] { 0, 0, 143, 144, 145, 146, 2130 },
            // Force Volley
            new int[] { 0, 0, 147, 148, 149, 150, 2134 },
            // Blade Volley
            new int[] { 0, 0, 151, 152, 153, 154, 2125 },

            /// VOID SPELLS ///

            // Nether Bolt
            new int[] { 5349, 5350, 5351, 5352, 5353, 5354, 5355 },
            // Nether Streak
            new int[] { 5357, 5358, 5359, 5360, 5345, 5346, 5347 },
            // Nether Arc
            new int[] { 5369, 5362, 5363, 5364, 5365, 5366, 5367 },
            // Corrosion
            new int[] { 5387, 5388, 5389, 5390, 5391, 5392, 5393 },
            // Corruption
            new int[] { 5395, 5396, 5397, 5398, 5399, 5400, 5401 },
            // Destructive Curse
            new int[] { 5339, 5340, 5341, 5342, 5343, 5344, 5337 },
            // Festering Curse
            new int[] { 5371, 5372, 5373, 5374, 5375, 5376, 5377 },
            // Weakening Curse
            new int[] { 5379, 5380, 5381, 5382, 5383, 5384, 5385 }
        };*/

        public static readonly int[] WarVoidRingScrollSpells = { 1783, 1784, 1785, 1786, 1787, 1788, 1789, 5361 };

        public static readonly int[] WarWallScrollSpells = { 1839, 1840, 1841, 1842, 1843, 1844, 1845 };

        // TODO: replace with GetMaterialName()
        public static readonly Dictionary<int, string> gemNames = new Dictionary<int, string>()
        {
            { 10, "Agate" },
            { 11, "Amber" },
            { 12, "Amethyst" },
            { 13, "Aquamarine" },
            { 14, "Azurite" },
            { 15, "Black Garnet" },
            { 16, "Black Opal" },
            { 17, "Bloodstone" },
            { 18, "Carnelian" },
            { 19, "Citrine" },
            { 20, "Diamond" },
            { 21, "Emerald" },
            { 22, "Fire Opal" },
            { 23, "Green Garnet" },
            { 24, "Green Jade" },
            { 25, "Hematite" },
            { 26, "Imperial Topaz" },
            { 27, "Jet" },
            { 28, "Lapis Lazuli" },
            { 29, "Lavender Jade" },
            { 30, "Malachite" },
            { 31, "Moonstone" },
            { 32, "Onyx" },
            { 33, "Opal" },
            { 34, "Peridot" },
            { 35, "Red Garnet" },
            { 36, "Red Jade" },
            { 37, "Rose Quartz" },
            { 38, "Ruby" },
            { 39, "Sapphire" },
            { 40, "Smokey Quartz" },
            { 41, "Sunstone" },
            { 42, "Tiger Eye" },
            { 43, "Tourmaline" },
            { 44, "Turquoise" },
            { 45, "White Jade" },
            { 46, "White Quartz" },
            { 47, "White Sapphire" },
            { 48, "Yellow Garnet" },
            { 49, "Yellow Topaz" },
            { 50, "Zircon" }
        };

        /*public static readonly int[][] ArmorSpells =
        {
            ////Strength
            new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
            ////Endurance
            new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
            ////Coordination
            new int[] { 1373, 1374, 1375, 1376, 1377, 1378, 2059, 4297 },
            ////Quickness
            new int[] { 1397, 1398, 1399, 1400, 1401, 1402, 2081, 4319 },
            ////Focus
            new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 },
            ////Willpower
            new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091, 4329 },
            ////Person Attunement
            new int[] { 824, 825, 826, 827, 828, 829, 2293, 4608 },
            ////Invuln
            new int[] { 18, 245, 246, 247, 248, 249, 2245, 4560 },
            ////Impreg
            new int[] { 256, 257, 258, 259, 260, 261, 2243, 4558 },
            ////Magic Resist
            new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
            ////Dirty Fighting
            new int[] { 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786 },
            ////Dual Wield
            new int[] { 5803, 5804, 5805, 5806, 5807, 5808, 5809, 5810 },
            ////Recklessness
            new int[] { 5827, 5828, 5829, 5830, 5831, 5832, 5833, 5834 },
            ////Sneak Attack
            new int[] { 5875, 5876, 5877, 5878, 5879, 5880, 5881, 5882 },
            ////Deception Mastery
            new int[] { 850, 851, 852, 853, 854, 855, 2227, 4542 },
            ////Two Handed
            new int[] { 5099, 5100, 5101, 5102, 5103, 5104, 5105, 5032 },
            ////Finesse Weapon
            new int[] { 322, 323, 324, 325, 326, 327, 2223, 4538 },
            ////Heavy Weapon
            new int[] { 418, 419, 420, 421, 422, 423, 2309, 4624 },
            ////Light Weapon
            new int[] { 298, 299, 300, 301, 302, 303, 2203, 4518 },
            ////Missile Weapon
            new int[] { 467, 468, 469, 470, 471, 472, 2207, 4522 },
            ////Shield
            new int[] { 5851, 5852, 5853, 5854, 5855, 5856, 5857, 5858 },
            ////Arcane Enlight
            new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 },
            ////Mana C
            new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
            ////Summoning Mastery
            new int[] { 6116, 6117, 6118, 6119, 6120, 6121, 6122, 6123 },
            ////Creature Enchant
            new int[] { 557, 558, 559, 560, 561, 562, 2215, 4530 },
            ////Item Enchant
            new int[] { 581, 582, 583, 584, 585, 586, 2249, 4564 },
            ////Life Magic
            new int[] { 605, 606, 607, 608, 609, 610, 2267, 4582 },
            ////War Magic
            new int[] { 629, 630, 631, 632, 633, 634, 2323, 4638 },
            ////Void Magic
            new int[] { 5411, 5412, 5413, 5414, 5415, 5416, 5417, 5418 },
            ////Cooking Mastery
            new int[] { 1715, 1716, 1717, 1718, 1719, 1720, 2211, 4526 },
            ////Fletching
            new int[] { 1739, 1740, 1741, 1742, 1743, 1744, 2237, 4552 },
            ////Alchemy
            new int[] { 1763, 1764, 1765, 1766, 1767, 1768, 2191, 4506 },
            ////Healing Mastery
            new int[] { 874, 875, 876, 877, 878, 879, 2241, 4556 },
            ////Lockpick
            new int[] { 922, 923, 924, 925, 926, 927, 2271, 4586 },
            ////Salvaging
            new int[] { 3499, 3500, 3501, 3502, 3503, 3504, 3505, 4499 },
            ////Armor Tinkering
            new int[] { 702, 703, 704, 705, 706, 707, 2197, 4512 },
            ////Item Tinkering
            new int[] { 726, 727, 728, 729, 730, 731, 2251, 4566 },
            ////Magic Item Tinkering
            new int[] { 750, 751, 752, 753, 754, 755, 2277, 4592 },
            ////Weapon Tinkering
            new int[] { 774, 775, 776, 777, 778, 779, 2325, 4640 },
            ////Monster Attunement
            new int[] { 798, 799, 800, 801, 802, 803, 2289, 4604 },
            ////Leadership
            new int[] { 898, 899, 900, 901, 902, 903, 2263, 4578 },
            ////Fealty
            new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
            ////Jumping
            new int[] { 970, 971, 972, 973, 974, 975, 2257, 4572 },
            ////Sprint
            new int[] { 982, 983, 984, 985, 986, 987, 2301, 4616 },
            ////Regeneration
            new int[] { 165, 166, 167, 168, 169, 170, 2185, 4496 },
            ////Rejuvenation
            new int[] { 54, 189, 190, 191, 192, 193, 2187, 4498 },
            ////Mana Renewal
            new int[] { 212, 213, 214, 215, 216, 217, 2183, 4494 },
            ////Imp
            new int[] { 51, 1482, 1483, 1484, 1485, 1486, 2108, 4407 },
            ////Blade Bane
            new int[] { 37, 1558, 1559, 1560, 1561, 1562, 2094, 4393 },
            ////Acid Bane
            new int[] { 1493, 1494, 1495, 1496, 1497, 1498, 2092, 4391 },
            ////Bludge Bane
            new int[] { 1511, 1512, 1513, 1514, 1515, 1516, 2098, 4397 },
            ////Frost Bane
            new int[] { 1523, 1524, 1525, 1526, 1527, 1528, 2104, 4403 },
            ////Lightning Bane
            new int[] { 1535, 1536, 1537, 1538, 1539, 1540, 2110, 4409 },
            ////Flame Bane
            new int[] { 1547, 1548, 1549, 1550, 1551, 1552, 2102, 4401 },
            ////Pierce Bane
            new int[] { 1569, 1570, 1571, 1572, 1573, 1574, 2113, 4412 },
            ////Armor Self
            new int[] { 24, 1308, 1309, 1310, 1311, 1312, 2053, 4291 },
            ////Acid Prot
            new int[] { 515, 516, 517, 518, 519, 520, 2149, 4460 },
            ////Brudge Prot
            new int[] { 1018, 1019, 1020, 1021, 1022, 1023, 2153, 4464 },
            ////Cold Prot
            new int[] { 1030, 1031, 1032, 1033, 1034, 1035, 2155, 4466 },
            ////Lightning Prot
            new int[] { 1066, 1067, 1068, 1069, 1070, 1071, 2159, 4470 },
            ////Fire Prot
            new int[] { 20, 1090, 1091, 1092, 1093, 1094, 2157, 4468 },
            ////Blade Prot
            new int[] { 1109, 1110, 1111, 1112, 1113, 1114, 2151, 4462 },
            ////Pierce Prot
            new int[] { 1133, 1134, 1135, 1136, 1137, 1138, 2161, 4472 },
        };*/

        /*public static readonly int[][] WandSpells =
        {
                new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 }, // Focus
                new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091, 4329 }, // Willpower
                new int[] { 5875, 5876, 5877, 5878, 5879, 5880, 5881, 5882 }, // Sneak Attack Mastery
                new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 }, // Arcane Enlightenment
                new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 }, // Mana Conversion Mastery
                new int[] { 557, 558, 559, 560, 561, 562 , 2215, 4530 }, // Creature Enchantment Mastery
                new int[] { 581, 582, 583, 584, 585, 586, 2249, 4564 }, // Item Enchantment Mastery
                new int[] { 605, 606, 607, 608, 609, 610, 2267, 4582 }, // Life Magic Mastery
                new int[] { 629, 630, 631, 632, 633, 634 , 2323, 4638 }, // War Magic Mastery
                new int[] { 1599, 1601, 1602, 1603, 1604, 1605, 2101, 4400 }, // Defender
                new int[] { 1475, 1476, 1477, 1478, 1479, 1480, 2117, 4418 }, // Hermetic Link
        };*/

        /*public static readonly int[][] JewelrySpells =
        {
            ////Strength
            new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
            ////Endurance
            new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
            ////Coordination
            new int[] { 1373, 1374, 1375, 1376, 1377, 1378, 2059, 4297 },
            ////Quickness
            new int[] { 1397, 1398, 1399, 1400, 1401, 1402, 2081, 4319 },
            ////Focus
            new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 },
            ////Willpower
            new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091, 4329 },
            ////Person Attunement
            new int[] { 824, 825, 826, 827, 828, 829, 2293, 4608 },
            ////Invuln
            new int[] { 18, 245, 246, 247, 248, 249, 2245, 4560 },
            ////Impreg
            new int[] { 256, 257, 258, 259, 260, 261, 2243, 4558 },
            ////Magic Resist
            new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
            ////Dirty Fighting
            new int[] { 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786 },
            ////Dual Wield
            new int[] { 5803, 5804, 5805, 5806, 5807, 5808, 5809, 5810 },
            ////Recklessness
            new int[] { 5827, 5828, 5829, 5830, 5831, 5832, 5833, 5834 },
            ////Sneak Attack
            new int[] { 5875, 5876, 5877, 5878, 5879, 5880, 5881, 5882 },
            ////Deception Mastery
            new int[] { 850, 851, 852, 853, 854, 855, 2227, 4542 },
            ////Two Handed
            new int[] { 5099, 5100, 5101, 5102, 5103, 5104, 5105, 5032 },
            ////Finesse Weapon
            new int[] { 322, 323, 324, 325, 326, 327, 2223, 4538 },
            ////Heavy Weapon
            new int[] { 418, 419, 420, 421, 422, 423, 2309, 4624 },
            ////Light Weapon
            new int[] { 298, 299, 300, 301, 302, 303, 2203, 4518 },
            ////Missile Weapon
            new int[] { 467, 468, 469, 470, 471, 472, 2207, 4522 },
            ////Shield
            new int[] { 5851, 5852, 5853, 5854, 5855, 5856, 5857, 5858 },
            ////Arcane Enlight
            new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 },
            ////Mana C
            new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
            ////Summoning Mastery
            new int[] { 6116, 6117, 6118, 6119, 6120, 6121, 6122, 6123 },
            ////Creature Enchant
            new int[] { 557, 558, 559, 560, 561, 562, 2215, 4530 },
            ////Item Enchant
            new int[] { 581, 582, 583, 584, 585, 586, 2249, 4564 },
            ////Life Magic
            new int[] { 605, 606, 607, 608, 609, 610, 2267, 4582 },
            ////War Magic
            new int[] { 629, 630, 631, 632, 633, 634, 2323, 4638 },
            ////Void Magic
            new int[] { 5411, 5412, 5413, 5414, 5415, 5416, 5417, 5418 },
            ////Cooking Mastery
            new int[] { 1715, 1716, 1717, 1718, 1719, 1720, 2211, 4526 },
            ////Fletching
            new int[] { 1739, 1740, 1741, 1742, 1743, 1744, 2237, 4552 },
            ////Alchemy
            new int[] { 1763, 1764, 1765, 1766, 1767, 1768, 2191, 4506 },
            ////Healing Mastery
            new int[] { 874, 875, 876, 877, 878, 879, 2241, 4556 },
            ////Lockpick
            new int[] { 922, 923, 924, 925, 926, 927, 2271, 4586 },
            ////Salvaging
            new int[] { 3499, 3500, 3501, 3502, 3503, 3504, 3505, 4499 },
            ////Armor Tinkering
            new int[] { 702, 703, 704, 705, 706, 707, 2197, 4512 },
            ////Item Tinkering
            new int[] { 726, 727, 728, 729, 730, 731, 2251, 4566 },
            ////Magic Item Tinkering
            new int[] { 750, 751, 752, 753, 754, 755, 2277, 4592 },
            ////Weapon Tinkering
            new int[] { 774, 775, 776, 777, 778, 779, 2325, 4640 },
            ////Monster Attunement
            new int[] { 798, 799, 800, 801, 802, 803, 2289, 4604 },
            ////Leadership
            new int[] { 898, 899, 900, 901, 902, 903, 2263, 4578 },
            ////Fealty
            new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
            ////Jumping
            new int[] { 970, 971, 972, 973, 974, 975, 2257, 4572 },
            ////Sprint
            new int[] { 982, 983, 984, 985, 986, 987, 2301, 4616 },
            ////Regeneration
            new int[] { 165, 166, 167, 168, 169, 170, 2185, 4496 },
            ////Rejuvenation
            new int[] { 54, 189, 190, 191, 192, 193, 2187, 4498 },
            ////Mana Renewal
            new int[] { 212, 213, 214, 215, 216, 217, 2183, 4494 },
            ////Armor Self
            new int[] { 24, 1308, 1309, 1310, 1311, 1312, 2053, 4291 },
            ////Acid Prot
            new int[] { 515, 516, 517, 518, 519, 520, 2149, 4460 },
            ////Brudge Prot
            new int[] { 1018, 1019, 1020, 1021, 1022, 1023, 2153, 4464 },
            ////Cold Prot
            new int[] { 1030, 1031, 1032, 1033, 1034, 1035, 2155, 4466 },
            ////Lightning Prot
            new int[] { 1066, 1067, 1068, 1069, 1070, 1071, 2159, 4470 },
            ////Fire Prot
            new int[] { 20, 1090, 1091, 1092, 1093, 1094, 2157, 4468 },
            ////Blade Prot
            new int[] { 1109, 1110, 1111, 1112, 1113, 1114, 2151, 4462 },
            ////Pierce Prot
            new int[] { 1133, 1134, 1135, 1136, 1137, 1138, 2161, 4472 },
        };*/

        /*public static readonly int[][] GemSpells =
        {
            ////Strength
            new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
            ////Invuln
            new int[] { 18, 245, 246, 247, 248, 249, 2245, 4560 },
            ////Impreg
            new int[] { 256, 257, 258, 259, 260, 261, 2243, 4558 },
            ////Magic Resist
            new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
            ////Light Weapon
            new int[] { 298, 299, 300, 301, 302, 303, 2203, 4518 },
            ////Finesse Weapon
            new int[] { 322, 323, 324, 325, 326, 327, 2223, 4538 },
            ////Two Handed
            new int[] { 5099, 5100, 5101, 5102, 5103, 5104, 5105, 5032 },
            ////Heavy Weapon
            new int[] { 418, 419, 420, 421, 422, 423, 2309, 4624 },
            ////Missile Weapon
            new int[] { 467, 468, 469, 470, 471, 472, 2207, 4522 },
            ////Creature Enchant
            new int[] { 557, 558, 559, 560, 561, 562, 2215, 4530 },
            ////Item Enchant
            new int[] { 581, 582, 583, 584, 585, 586, 2249, 4564 },
            ////Life Magic
            new int[] { 605, 606, 607, 608, 609, 610, 2267, 4582 },
            ////War Magic
            new int[] { 629, 630, 631, 632, 633, 634, 2323, 4638 },
            ////Mana C
            new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
            ////Arcane Enlight
            new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 },
            ////Armor Tinkering
            new int[] { 702, 703, 704, 705, 706, 707, 2197, 4512 },
            ////Item Tinkering
            new int[] { 726, 727, 728, 729, 730, 731, 2251, 4566 },
            ////Magic Item Tinkering
            new int[] { 750, 751, 752, 753, 754, 755, 2277, 4592 },
            ////Weapon Tinkering
            new int[] { 774, 775, 776, 777, 778, 779, 2325, 4640 },
            ////Monster Attunement
            new int[] { 798, 799, 800, 801, 802, 803, 2289, 4604 },
            ////Person Attunement
            new int[] { 824, 825, 826, 827, 828, 829, 2293, 4608 },
            ////Deception Mastery
            new int[] { 850, 851, 852, 853, 854, 855, 2227, 4542 },
            ////Healing Mastery
            new int[] { 874, 875, 876, 877, 878, 879, 2241, 4556 },
            ////Leadership
            new int[] { 898, 899, 900, 901, 902, 903, 2263, 4578 },
            ////Lockpick
            new int[] { 922, 923, 924, 925, 926, 927, 2271, 4586 },
            ////Fealty
            new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
            ////Jumping
            new int[] { 970, 971, 972, 973, 974, 975, 2257, 4572 },
            ////Sprint
            new int[] { 982, 983, 984, 985, 986, 987, 2301, 4616 },
            ////Endurance
            new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
            ////Coordination
            new int[] { 1373, 1374, 1375, 1376, 1377, 1378, 2059, 4297 },
            ////Quickness
            new int[] { 1397, 1398, 1399, 1400, 1401, 1402, 2081, 4319 },
            ////Focus
            new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 },
            ////Willpower
            new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091, 4329 },
            ////Cooking Mastery
            new int[] { 1715, 1716, 1717, 1718, 1719, 1720, 2211, 4526 },
            ////Fletching
            new int[] { 1739, 1740, 1741, 1742, 1743, 1744, 2237, 4552 },
            ////Alchemy
            new int[] { 1763, 1764, 1765, 1766, 1767, 1768, 2191, 4506 },
            ////Dirty Fighting
            new int[] { 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786 },
            ////Dual Wield
            new int[] { 5803, 5804, 5805, 5806, 5807, 5808, 5809, 5810 },
            ////Recklessness
            new int[] { 5827, 5828, 5829, 5830, 5831, 5832, 5833, 5834 },
            ////Shield
            new int[] { 5851, 5852, 5853, 5854, 5855, 5856, 5857, 5858 },
            ////Sneak Attack
            new int[] { 5875, 5876, 5877, 5878, 5879, 5880, 5881, 5882 },
            ////Summoning
            new int[] { 6116, 6117, 6118, 6119, 6120, 6121, 6122, 6123 },
            ////Void Magic
            new int[] { 5411, 5412, 5413, 5414, 5415, 5416, 5417, 5418 },
            ////Salvaging
            new int[] { 3499, 3500, 3501, 3502, 3503, 3504, 3505, 4499 },
            ////Regeneration
            new int[] { 165, 166, 167, 168, 169, 170, 2185, 4496 },
            ////Rejuvenation
            new int[] { 54, 189, 190, 191, 192, 193, 2187, 4498 },
            ////Mana Renewal
            new int[] { 212, 213, 214, 215, 216, 217, 2183, 4494 },
            ////Acid Prot
            new int[] { 515, 516, 517, 518, 519, 520, 2149, 4460 },
            ////Brudge Prot
            new int[] { 1018, 1019, 1020, 1021, 1022, 1023, 2153, 4464 },
            ////Cold Prot
            new int[] { 1030, 1031, 1032, 1033, 1034, 1035, 2155, 4466 },
            ////Lightning Prot
            new int[] { 1066, 1067, 1068, 1069, 1070, 1071, 2159, 4470 },
            ////Fire Prot
            new int[] { 20, 1090, 1091, 1092, 1093, 1094, 2157, 4468 },
            ////Blade Prot
            new int[] { 1109, 1110, 1111, 1112, 1113, 1114, 2151, 4462 },
            ////Pierce Prot
            new int[] { 1133, 1134, 1135, 1136, 1137, 1138, 2161, 4472 },
            ////Armor Self
            new int[] { 24, 1308, 1309, 1310, 1311, 1312, 2053, 4291 },
        };*/

        /*public static readonly int[][] MeleeSpells =
        {
            ////Strength
            new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
            ////Endurance
            new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
            ////Coordination
            new int[] { 1373, 1374, 1375, 1376, 1377, 1378, 2059, 4297 },
            ////Dirty Fighting
            new int[] { 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786 },
            ////Dual Wield
            new int[] { 5803, 5804, 5805, 5806, 5807, 5808, 5809, 5810 },
            ////Recklessness
            new int[] { 5827, 5828, 5829, 5830, 5831, 5832, 5833, 5834 },
            ////Sneak Attack
            new int[] { 5875, 5876, 5877, 5878, 5879, 5880, 5881, 5882 },
            ////Heart Seeker
            new int[] { 1587, 1588, 1589, 1590, 1591, 1592, 2106, 4405 },
            ////Defender
            new int[] { 1599, 1601, 1602, 1603, 1604, 1605, 2101, 4400 },
            ////SwiftKiller
            new int[] { 49, 1623, 1624, 1625, 1626, 1627, 2116, 4417 },
            ////Blooddrinker
            new int[] { 35, 1612, 1613, 1614, 1615, 1616, 2096, 4395 },
        };*/

        /*public static readonly int[][] MissileSpells =
        {
            ////Strength
            new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
            ////Endurance
            new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
            ////Coordination
            new int[] { 1373, 1374, 1375, 1376, 1377, 1378, 2059, 4297 },
            ////Dirty Fighting
            new int[] { 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786 },
            ////Recklessness
            new int[] { 5827, 5828, 5829, 5830, 5831, 5832, 5833, 5834 },
            ////Sneak Attack
            new int[] { 5875, 5876, 5877, 5878, 5879, 5880, 5881, 5882 },
            ////Heart Seeker
            new int[] { 1587, 1588, 1589, 1590, 1591, 1592, 2106, 4405 },
            ////Defender
            new int[] { 1599, 1601, 1602, 1603, 1604, 1605, 2101, 4400 },
            ////SwiftKiller
            new int[] { 49, 1623, 1624, 1625, 1626, 1627, 2116, 4417 },
            ////Blooddrinker
            new int[] { 35, 1612, 1613, 1614, 1615, 1616, 2096, 4395 },
        };*/

        /*public static readonly int[][] ArmorCantrips =
        {
            ////Strength
            new int[] { 2583, 2576, 3965, 6107},
            ////Endurance
            new int[] { 2580, 2573, 4226, 6104},
            ////Coordination
            new int[] { 2579, 2572, 3963, 6103},
            ////Quickness
            new int[] { 2582, 2575, 4019, 6106},
            ////Focus
            new int[] { 2581, 2574, 3964, 6105},
            ////Willpower
            new int[] { 2584, 2577, 4227, 6101},
            ////Invuln
            new int[] { 2550, 2515, 4696, 6055},
            ////Impreg
            new int[] { 2549, 2514, 4695, 6054},
            ////Magic Resist
            new int[] { 2559, 2524, 4704, 6063},
            ////Dirty Fighting
            new int[] { 5883, 5888, 5893, 6049 },
            ////Dual Wield
            new int[] { 5884, 5889, 5894, 6050},
            ////Recklessness
            new int[] { 5885, 5890,5895, 6067},
            ////Sneak Attack
            new int[] {5887, 5892, 5897, 6070 },
            ////Deception Mastery
            new int[] { 2545, 2510, 4020, 6048},
            ////Person Attunement
            new int[] { 2562, 2527, 4707, 6066 },
            ////Arcane Enlight
            new int[] { 2537, 2502, 4684, 6041},
            ////Mana C
            new int[] { 2560, 2525, 4705, 6064},
            ////Two Handed
            new int[] { 5072, 5070, 5034, 6073},
            ////Finesse Weapon
            new int[] { 2544, 2509, 4691, 6047},
            ////Heavy Weapon
            new int[] { 2566, 2531, 4712, 6072},
            ////Light Weapon
            new int[] { 2539, 2504, 4686, 6043},
            ////Missile Weapon
            new int[] { 2540, 2505, 4687, 6044},
            ////Shield
            new int[] { 5885, 5890, 5895, 6067},
            ////Summoning Mastery
            new int[] { 6127, 6126, 6124, 6125},
            ////Creature Enchant
            new int[] { 2542, 2507, 4689, 6046},
            ////Item Enchant
            new int[] { 2551, 2516, 4697, 6056},
            ////Life Magic
            new int[] { 2555, 2520, 4700, 6060},
            ////War Magic
            new int[] { 2569, 2534, 4715, 6075},
            ////Void Magic
            new int[] { 5427, 5428, 5429, 6074},
            ////Cooking Mastery
            new int[] { 2541, 2506, 4688, 6045},
            ////Fletching
            new int[] { 2547, 2512, 4693, 6052},
            ////Alchemy
            new int[] { 2536, 2501, 4683, 6040},
            ////Healing Mastery
            new int[] { 2548, 2513, 4694, 6053},
            ////Lockpick
            new int[] { 2556, 2521, 4701, 6061},
            ////Salvaging
            new int[] { 3833, 3834, 4708, 6068},
            ////Armor Tinkering
            new int[] { 2538, 2503, 4685, 6042},
            ////Item Tinkering
            new int[] { 2552, 2517, 4698, 6057},
            ////Magic Item Tinkering
            new int[] { 2558, 2523, 4703, 6062},
            ////Weapon Tinkering
            new int[] { 2570, 2535, 4912, 6039},
            ////Monster Attunement
            new int[] { 2561, 2526, 4706, 6065},
            ////Leadership
            new int[] { 2554, 2519, 4232, 6059},
            ////Fealty
            new int[] { 2546, 2511, 4692, 6051},
            ////Armor Self
            new int[] { 2578, 2571, 4911, 6102},
            ////Imp
            new int[] { 2604, 2592, 4667, 6095},
            ////Blade Bane
            new int[] { 2606, 2594, 4669, 6097},
            ////Acid Bane
            new int[] { 2597, 2585, 4660, 6088},
            ////Bludge Bane
            new int[] { 2599, 2587, 4662, 6090},
            ////Frost Bane
            new int[] { 2602, 2590, 4665, 6093},
            ////Lightning Bane
            new int[] { 2607, 2595, 4671, 6099},
            ////Flame Bane
            new int[] { 2601, 2589, 4664, 6092},
            ////Pierce Bane
            new int[] { 2605, 2593, 4668, 6096},
            ////Armor Self
            new int[] { 2578, 2571, 4911, 6102},
            ////Acid Prot
            new int[] { 2616, 2609, 4673, 6080},
            ////Brudge Prot
            new int[] { 2617, 2610, 4674, 6081},
            ////Cold Prot
            new int[] { 2619, 2612, 4676, 6083},
            ////Lightning Prot
            new int[] { 2622, 2615, 4679, 6079},
            ////Fire Prot
            new int[] { 2618, 2611, 4675, 6082},
            ////Blade Prot
            new int[] { 2621, 2614, 4678, 6085},
            ////Pierce Prot
            new int[] { 2620, 2613, 4677, 6084},
            ///Shield
            new int[] { 5886, 5891, 5896, 6069},
        };*/

        /*public static readonly int[][] JewelryCantrips =
        {
            ////Invuln
            new int[] { 2550, 2515, 4696, 6055},
            ////Impreg
            new int[] { 2549, 2514, 4695, 6054},
            ////Magic Resist
            new int[] { 2559, 2524, 4704, 6063},
            ////Dirty Fighting
            new int[] { 5883, 5888, 5893, 6049 },
            ////Dual Wield
            new int[] { 5884, 5889, 5894, 6050},
            ////Recklessness
            new int[] { 5885, 5890,5895, 6067},
            ////Sneak Attack
            new int[] {5887, 5892, 5897, 6070 },
            ////Deception Mastery
            new int[] { 2545, 2510, 4020, 6048},
            ////Person Attunement
            new int[] { 2562, 2527, 4707, 6066 },
            ////Arcane Enlight
            new int[] { 2537, 2502, 4684, 6041},
            ////Mana C
            new int[] { 2560, 2525, 4705, 6064},
            ////Two Handed
            new int[] { 5072, 5070, 5034, 6073},
            ////Finesse Weapon
            new int[] { 2544, 2509, 4691, 6047},
            ////Heavy Weapon
            new int[] { 2566, 2531, 4712, 6072},
            ////Light Weapon
            new int[] { 2539, 2504, 4686, 6043},
            ////Missile Weapon
            new int[] { 2540, 2505, 4687, 6044},
            ////Shield
            new int[] { 5885, 5890, 5895, 6067},
            ////Summoning Mastery
            new int[] { 6127, 6126, 6124, 6125},
            ////Creature Enchant
            new int[] { 2542, 2507, 4689, 6046},
            ////Item Enchant
            new int[] { 2551, 2516, 4697, 6056},
            ////Life Magic
            new int[] { 2555, 2520, 4700, 6060},
            ////War Magic
            new int[] { 2569, 2534, 4715, 6075},
            ////Void Magic
            new int[] { 5427, 5428, 5429, 6074},
            ////Cooking Mastery
            new int[] { 2541, 2506, 4688, 6045},
            ////Fletching
            new int[] { 2547, 2512, 4693, 6052},
            ////Alchemy
            new int[] { 2536, 2501, 4683, 6040},
            ////Healing Mastery
            new int[] { 2548, 2513, 4694, 6053},
            ////Lockpick
            new int[] { 2556, 2521, 4701, 6061},
            ////Salvaging
            new int[] { 3833, 3834, 4708, 6068},
            ////Armor Tinkering
            new int[] { 2538, 2503, 4685, 6042},
            ////Item Tinkering
            new int[] { 2552, 2517, 4698, 6057},
            ////Magic Item Tinkering
            new int[] { 2558, 2523, 4703, 6062},
            ////Weapon Tinkering
            new int[] { 2570, 2535, 4912, 6039},
            ////Monster Attunement
            new int[] { 2561, 2526, 4706, 6065},
            ////Leadership
            new int[] { 2554, 2519, 4232, 6059},
            ////Fealty
            new int[] { 2546, 2511, 4692, 6051},
            ////Jump
            new int[] { 2553, 2518, 4699, 6058},
            ////Sprint
            new int[] { 2564, 2529, 4710, 6071},
            ////Armor Self
            new int[] { 2578, 2571, 4911, 6102},
            ////Armor Self
            new int[] { 2578, 2571, 4911, 6102},
            ////Acid Prot
            new int[] { 2616, 2609, 4673, 6080},
            ////Brudge Prot
            new int[] { 2617, 2610, 4674, 6081},
            ////Cold Prot
            new int[] { 2619, 2612, 4676, 6083},
            ////Lightning Prot
            new int[] { 2622, 2615, 4679, 6079},
            ////Fire Prot
            new int[] { 2618, 2611, 4675, 6082},
            ////Blade Prot
            new int[] { 2621, 2614, 4678, 6085},
            ////Pierce Prot
            new int[] { 2620, 2613, 4677, 6084},
            ///Shield
            new int[] { 5886, 5891, 5896, 6069},
        };*/

        /*public static readonly int[][] WandCantrips =
        {
            ////Focus
            new int[] { 2581, 2574, 3964, 6105},
            ////Willpower
            new int[] { 2584, 2577, 4227, 6101},
            ////Sneak Attack
            new int[] { 5887, 5892, 5897, 6070},
            ////Arcane Enlight
            new int[] { 2537, 2502, 4684, 6041},
            ////Mana C
            new int[] { 2560, 2525, 4705, 6064},
            ////Creature Enchant
            new int[] { 2542, 2507, 4689, 6046},
            ////Item Enchant
            new int[] { 2551, 2516, 4697, 6056},
            ////Life Magic
            new int[] { 2555, 2520, 4700, 6060},
            ////War Magic
            new int[] { 2569, 2534, 4715, 6075},
            ////Defender
            new int[] { 2600, 2588, 4663, 6091},
            ////Hermetic Link
            new int[] { 3199, 3200, 6086, 6087},
            ////Spirit Thirst
            new int[] { 3251, 3250, 4670, 6098},
        };*/

        /*public static readonly int[][] MeleeCantrips =
        {
            ////Strength
            new int[] { 2583, 2576, 3965, 6107},
            ////Endurance
            new int[] { 2580, 2573, 4226, 6104},
            ////Coordination
            new int[] { 2579, 2572, 3963, 6103},
            ////Dirty Fighting
            new int[] { 5883, 5888, 5893, 6049 },
            ////Dual Wield
            new int[] { 5884, 5889, 5894, 6050},
            ////Recklessness
            new int[] { 5885, 5890,5895, 6067},
            ////Sneak Attack
            new int[] {5887, 5892, 5897, 6070 },
            ////Heart Seeker
            new int[] { 2603, 2591, 4666, 6094},
            ////Defender
            new int[] { 2600, 2588, 4663, 6091},
            ////SwiftKiller
            new int[] { 2608, 2596, 4672, 6100},
            ////Blooddrinker
            new int[] { 2598, 2586, 4661, 6089},
        };*/

        /*public static readonly int[][] MissileCantrips =
        {
            ////Strength
            new int[] { 2583, 2576, 3965, 6107},
            ////Endurance
            new int[] { 2580, 2573, 4226, 6104},
            ////Coordination
            new int[] { 2579, 2572, 3963, 6103},
            ////Dirty Fighting
            new int[] { 5883, 5888, 5893, 6049 },
            ////Recklessness
            new int[] { 5885, 5890,5895, 6067},
            ////Sneak Attack
            new int[] {5887, 5892, 5897, 6070 },
            ////Heart Seeker
            new int[] { 2603, 2591, 4666, 6094},
            ////Defender
            new int[] { 2600, 2588, 4663, 6091},
            ////SwiftKiller
            new int[] { 2608, 2596, 4672, 6100},
            ////Blooddrinker
            new int[] { 2598, 2586, 4661, 6089},
        };*/

        public static readonly int[][] DefaultMaterial =
        {
            new int[] { (int)MaterialType.Copper, (int)MaterialType.Bronze, (int)MaterialType.Iron, (int)MaterialType.Steel, (int)MaterialType.Silver },            // Armor
            new int[] { (int)MaterialType.Oak, (int)MaterialType.Teak, (int)MaterialType.Mahogany, (int)MaterialType.Pine, (int)MaterialType.Ebony },               // Missile
            new int[] { (int)MaterialType.Brass, (int)MaterialType.Ivory, (int)MaterialType.Gold, (int)MaterialType.Steel, (int)MaterialType.Diamond },             // Melee
            new int[] { (int)MaterialType.RedGarnet, (int)MaterialType.Jet, (int)MaterialType.BlackOpal, (int)MaterialType.FireOpal, (int)MaterialType.Emerald },   // Caster
            new int[] { (int)MaterialType.Granite, (int)MaterialType.Ceramic, (int)MaterialType.Porcelain, (int)MaterialType.Alabaster, (int)MaterialType.Marble }, // Dinnerware
            new int[] { (int)MaterialType.Linen, (int)MaterialType.Wool, (int)MaterialType.Velvet, (int)MaterialType.Satin, (int)MaterialType.Silk }                // Clothes
        };

        public enum ArmorType
        {
            Undef,
            MiscClothing,
            Helms,
            Shields,
            LeatherArmor,
            StuddedLeatherArmor,
            ChainmailArmor,
            PlatemailArmor,
            ScalemailArmor,
            YoroiArmor,
            DiforsaArmor,
            CeldonArmor,
            AmuliArmor,
            KoujiaArmor,
            TenassaArmor,
            OverRobes,
            CovenantArmor,
            LoricaArmor,
            NariyidArmor,
            ChiranArmor,
            AlduressaArmor,
            KnorrAcademyArmor,
            SedgemailLeatherArmor,
            HaebreanArmor,
            OlthoiArmor,
            OlthoiAmuliArmor,
            OlthoiCeldonArmor,
            OlthoiKoujiaArmor,
            OlthoiAlduressaArmor,
            SocietyArmor
        }

        public static readonly int[] OverRobes =
        {
            44799,  // Faran Over-robe
            44800,  // Dho Vest and Over-robe
            44801,  // Suikan Over-robe
            44802,  // Vestiri Over-robe
            44803   // Empyrean Over-robe
        };

        public static readonly int[] MiscClothing =
        {
            107,    // Sollerets
            117,    // Breeches
            118,    // Cloth Cap
            119,    // Cowl
            121,    // Cloth gloves
            124,    // Gloves
            127,    // Pants
            128,    // Qafiya
            129,    // Sandals
            130,    // Shirt
            132,    // Shoes
            133,    // Slippers
            134,    // Tunic
            135,    // Turban
            2587,   // Shirt
            2588,   // Flared Shirt
            2589,   // Smock
            2590,   // Baggy Shirt
            2591,   // Puffy Shirt
            2592,   // Tunic
            2593,   // Tunic
            2594,   // Tunic
            2595,   // Tunic
            2596,   // Doublet
            2597,   // Pants
            2598,   // Pants
            2599,   // Trousers
            2600,   // Pantaloons
            2601,   // Pants
            2602,   // Breeches
            2603,   // Breeches
            2604,   // Breeches
            5894,   // Fez
            5901,   // Kasa
            7897,   // Steel Toed Boots
            28605,  // Beret
            28606,  // Viamontian Pants
            28607,  // Lace Shirt
            28608,  // Poet's Shirt
            28609,  // Vest
            28610,  // Loafers
            28611,  // Viamontian Laced Boots
            28612,  // Bandana
            44975   // Hood
        };

        public static readonly int[] ringItems =
        {
            297,    // Ring - Type 1
            624     // Ring - Type 2
        };

        public static readonly int[] braceletItems =
        {
            295,    // Bracelet
            621     // Heavy Bracelet
        };

        public static readonly int[] necklaceItems =
        {
            294,    // Amulet
            622,    // Necklace
            623,    // Heavy Necklace
            2367    // Gorget
        };

        public static readonly int[] trinketItems =
{
            41483,    // Compass
            41484,    // Goggles
            41487,    // Mechanical Scarab
            41486,    // Puzzle Box
            41485,    // PocketWatch
            41488     // Top
        };

        public static readonly List<int[]> jewelryTables = new List<int[]>()
        {
            ringItems,
            braceletItems,
            necklaceItems,
            trinketItems
        };

        public static readonly int[] Helms =
        {
            46,     // Metal Cap
            75,     // Helmet
            76,     // Horned Helm
            77,     // Kabuton
            296,    // Crown
            550,    // Baigha
            8488,   // Armet
            8489,   // Heaume
            31865   // Circlet
        };

        public static readonly int[] Shields =
        {
            44,     // Buckler
            91,     // Kite Shield
            92,     // Large Kite Shield
            93,     // Round Shield
            94,     // Large Round Shield
            95      // Round Tower Shield
        };

        public static readonly int[] LeatherArmor =
        {
            25636,  // Leather Helm
            25637,  // Leather Bracers
            25638,  // Leather Vest
            25639,  // Leather Jerkin
            25640,  // Leather Cowl
            25641,  // Leather Cuirass
            25642,  // Leather Gauntlets
            25643,  // Leather Girth
            25644,  // Leather Greaves
            25645,  // Leather Leggings
            25646,  // Long Leather Gauntlets
            25647,  // Leather Pants
            25648,  // Leather Pauldrons
            25649,  // Leather Shirt
            25650,  // Leather Shorts
            25651,  // Leather Sleeves
            25652,  // Leather Tassets
            25661   // Leather Boots
        };

        public static readonly int[] StuddedLeatherArmor =
        {
            38,     // Studded Leather Bracers
            42,     // Studded Leather Breastplate
            48,     // Studded Leather Coat
            53,     // Studded Leather Cuirass
            59,     // Studded Leather Gauntlets
            63,     // Studded Leather Girth
            68,     // Studded Leather Greaves
            89,     // Studded Leather Pauldrons
            99,     // Studded Leather Shirt
            105,    // Studded Leather Sleeves
            112,    // Studded Leather Tassets
            116,    // Studded Leather Boots
            554,    // Studded Leather Bassinet
            723     // Studded Leather Cowl
        };

        public static readonly int[] ChainmailArmor =
        {
            35,     // Chainmail Basinet
            55,     // Chainmail Gauntlets
            71,     // Chainmail Hauberk
            80,     // Chainmail Leggings
            85,     // Chainmail Coif
            96,     // Chainmail Shirt
            101,    // Chainmail Sleeves
            108,    // Chainmail Tassets
            413,    // Chainmail Bracers
            414,    // Chainmail Breastplate
            415,    // Chainmail Girth
            416,    // Chainmail Pauldrons
            2605    // Chainmail Greaves
        };

        public static readonly int[] PlatemailArmor =
        {
            40,     // Platemail Breastplate
            51,     // Platemail Cuirass
            57,     // Platemail Gauntlets
            61,     // Platemail Girth
            66,     // Platemail Greaves
            72,     // Platemail Hauberk
            82,     // Platemail Leggings
            87,     // Platemail Pauldrons
            103,    // Platemail Sleeves
            110,    // Platemail Tassets
            114     // Platemail Vambraces
        };

        public static readonly int[] ScalemailArmor =
        {
            37,     // Scalemail Bracers
            41,     // Scalemail Breastplate
            52,     // Scalemail Cuirass
            58,     // Scalemail Gauntlets
            62,     // Scalemail Girth
            67,     // Scalemail Greaves
            73,     // Scalemail Hauberk
            83,     // Scalemail Leggings
            88,     // Scalemail Pauldrons
            98,     // Scalemail Shirt
            104,    // Scalemail Sleeves
            111,    // Scalemail Tassets
            552,    // Scalemail Bassinet
            793     // Scalemail Coif
        };

        public static readonly int[] YoroiArmor =
        {
            43,     // Yoroi Breastplate
            54,     // Yoroi Cuirass
            64,     // Yoroi Girth
            69,     // Yoroi Greaves
            78,     // Kote
            90,     // Yoroi Pauldrons
            106,    // Yoroi Sleeves
            113,    // Yoroi Tassets
            2437    // Yoroi Leggings
        };

        public static readonly int[] DiforsaArmor =
        {
            28618,  // Diforsa Helm
            28621,  // Diforsa Leggings
            28623,  // Diforsa Pauldrons
            28625,  // Diforsa Sollerets
            28626,  // Diforsa Tassets
            28627,  // Diforsa Bracers
            28628,  // Diforsa Breastplate
            28630,  // Diforsa Cuirass
            28632,  // Diforsa Gauntlets
            28633,  // Diforsa Girth
            28634,  // Diforsa Greaves
            30948,  // Diforsa Hauberk
            30949,  // Diforsa Sleeves
        };

        public static readonly int[] CeldonArmor =
        {
            6043,   // Celdon Girth
            6044,   // Celdon Breastplate
            6045,   // Celdon Leggings
            6048    // Celdon Sleeves
        };

        public static readonly int[] AmuliArmor =
        {
            6046,   // Amuli Coat
            6047    // Amuli Leggings
        };

        public static readonly int[] KoujiaArmor =
        {
            6003,   // Koujia Breastplate
            6004,   // Koujia Leggings
            6005    // Koujia Sleeves
        };

        public static readonly int[] TenassaArmor =
        {
            28622,  // Tenassa Leggings
            28624,  // Tenassa Sleeves
            31026   // Tenassa Breastplate
        };

        public static readonly int[] CovenantArmor =
        {
            21150,  // Covenant Sollerets
            21151,  // Covenant Bracers
            21152,  // Covenant Breastplate
            21153,  // Covenant Gauntlets
            21154,  // Covenant Girth
            21155,  // Covenant Greaves
            21156,  // Covenant Helm
            21157,  // Covenant Pauldrons
            21158,  // Covenant Shield
            21159   // Covenant Tassets
        };

        public static readonly int[] LoricaArmor =
        {
            27220,  // Lorica Boots
            27221,  // Lorica Breastplate
            27222,  // Lorica Gauntlets
            27223,  // Lorica Helm
            27224,  // Lorica Leggings
            27225   // Lorica Sleeves
        };

        public static readonly int[] NariyidArmor =
        {
            27226,  // Nariyid Boots
            27227,  // Nariyid Breastplate
            27228,  // Nariyid Gauntlets
            27229,  // Nariyid Girth
            27230,  // Nariyid Helm
            27231,  // Nariyid Leggings
            27232   // Nariyid Sleeves
        };

        public static readonly int[] ChiranArmor =
        {
            27215,  // Chiran Coat
            27216,  // Chiran Gauntlets
            27217,  // Chiran Helm
            27218,  // Chiran Leggings
            27219   // Chiran Sandals
        };

        public static readonly int[] AlduressaArmor =
        {
            28617,  // Alduressa Helm
            28620,  // Alduressa Leggings
            28629,  // Alduressa Coat
            30950,  // Alduressa Boots
            30951   // Alduressa Gauntlets
        };

        public static readonly int[] KnorrAcademyArmor =
        {
            43048,  // Knorr Academy Breastplate
            43049,  // Knorr Academy Gauntlets
            43050,  // Knorr Academy Girth
            43051,  // Knorr Academy Greaves
            43052,  // Knorr Academy Pauldrons
            43053,  // Knorr Academy Boots
            43054,  // Knorr Academy Tassets
            43055,  // Knorr Academy Vambraces
            43068   // Knorr Academy Helm
        };

        public static readonly int[] SedgemailLeatherArmor =
        {
            43828,  // Sedgemail Leather Vest
            43829,  // Sedgemail Leather Cowl
            43830,  // Sedgemail Leather Gauntlets
            43831,  // Sedgemail Leather Pants
            43832,  // Sedgemail Leather Shoes
            43833   // Sedgemail Leather Sleeves
        };

        public static readonly int[] HaebreanArmor =
        {
            42749,  // Haebrean Breastplate
            42750,  // Haebrean Gauntlets
            42751,  // Haebrean Girth
            42752,  // Haebrean Greaves
            42753,  // Haebrean Helm
            42754,  // Haebrean Pauldrons
            42755,  // Haebrean Boots
            42756,  // Haebrean Tassets
            42757   // Haebrean Vambraces
        };

        public static readonly int[] OlthoiArmor =
        {
            37191,  // Olthoi Gauntlets
            37193,  // Olthoi Girth
            37194,  // Olthoi Greaves
            37199,  // Olthoi Helm
            37204,  // Olthoi Pauldrons
            37211,  // Olthoi Shoes
            37212,  // Olthoi Tassets
            37213,  // Olthoi Bracers
            37216,  // Olthoi Breastplate
            37291   // Olthoi Shield
        };

        public static readonly int[] OlthoiAmuliArmor =
        {
            37188,  // Olthoi Amuli Gauntlets
            37196,  // Olthoi Amuli Helm
            37201,  // Olthoi Amuli Leggings
            37208,  // Olthoi Amuli Sollerets
            37299   // Olthoi Amuli Coat
        };

        public static readonly int[] OlthoiCeldonArmor =
        {
            37189,  // Olthoi Celdon Gauntlets
            37192,  // Olthoi Celdon Girth
            37197,  // Olthoi Celdon Helm
            37202,  // Olthoi Celdon Leggings
            37205,  // Olthoi Celdon Sleeves
            37209,  // Olthoi Celdon Sollerets
            37214   // Olthoi Celdon Breastplate
        };

        public static readonly int[] OlthoiKoujiaArmor =
        {
            37190,  // Olthoi Koujia Gauntlets
            37198,  // Olthoi Koujia Kabuton
            37203,  // Olthoi Koujia Leggings
            37206,  // Olthoi Koujia Sleeves
            37215   // Olthoi Koujia Breastplate
        };

        public static readonly int[] OlthoiAlduressaArmor =
        {
            37187,  // Olthoi Alduressa Gauntlets
            37195,  // Olthoi Alduressa Helm
            37200,  // Olthoi Alduressa Leggings
            37207,  // Olthoi Alduressa Boots
            37217   // Olthoi Alduressa Coat
        };

        public static readonly int[] Cloaks =  // Cloak WCIDs Total of 11
        {
            44840,  // Cloak
            44849,  // Chevron Cloak
            44850,  // Chevron Cloak
            44851,  // Chevron Cloak
            44852,  // Chevron Cloak
            44853,  // Bordered Cloak
            44854,  // Halved Cloak
            44855,  // Halved Cloak
            44856,  // Trimmed Cloak
            44857,  // Quartered Cloak
            44858  // Quartered Cloak
        };

        public static readonly int[] CloakSpells =  // Cloak SpellIDs 12 of them (Damage Reduction is not a spell, but an Integer Property)
        {
            1784,  // Horizon's Blades
            1789,  // Tectonic Rifts
            1786,  // Nuhmudira's Spines
            1783,  // Searing Disc
            1785,  // Cassius' Ring of Fire
            1787,  // Halo of Frost
            1788,  // Eye of the Storm
            5361,  // Clouded Soul
            5754,  // Shroud of Darkness (Magic)
            5755,  // Shroud of Darkness (Melee)
            5756,  // Shroud of Darkness (Missile)
            5753   // Cloaked in Skill
        };

        public static readonly int[] CloakSets =  // Total of 35 different sets, Duplicates were removed (MoA update)
        {
            49,  // Alchemy
            50,  // Arcane Lore
            51,  // Armor Tinkering
            52,  // Assess Person
            53,  // Light Weapons
            54,  // Missile Weapons
            55,  // Cooking
            56,  // Creature Enchantment
            58,  // Finesse Weapons
            59,  // Deception
            60,  // Fletching
            61,  // Healing
            62,  // Item Enchantment
            63,  // Item Tinkering
            64,  // Leadership
            65,  // Life Magic
            66,  // Loyalty
            68,  // Magic Defense
            69,  // Magic Item Tinkering
            70,  // Mana Conversion
            71,  // Melee Defense
            72,  // Missile Defense
            73,  // Salvaging
            76,  // Heavy Weapons
            78,  // Two Handed Combat
            80,  // Void Magic
            81,  // War Magic
            82,  // Weapon Tinkering
            83,  // Assess Creature
            84,  // Dirty Fighting
            85,  // Dual Wield
            86,  // Recklessness
            87,  // Shield
            88,  // Sneak Attack
            90   // Summoning
        };

        // for logging epic/legendary drops
        public static HashSet<int> MinorCantrips;
        public static HashSet<int> MajorCantrips;
        public static HashSet<int> EpicCantrips;
        public static HashSet<int> LegendaryCantrips;

        private static List<SpellId[][]> cantripTables = new List<SpellId[][]>()
        {
            ArmorCantrips.Table,
            JewelryCantrips.Table,
            WandCantrips.Table,
            MeleeCantrips.Table,
            MissileCantrips.Table
        };

        static LootTables()
        {
            BuildCantripsTable(ref MinorCantrips, 0);
            BuildCantripsTable(ref MajorCantrips, 1);
            BuildCantripsTable(ref EpicCantrips, 2);
            BuildCantripsTable(ref LegendaryCantrips, 3);
        }

        private static void BuildCantripsTable(ref HashSet<int> table, int tier)
        {
            table = new HashSet<int>();

            foreach (var cantripTable in cantripTables)
            {
                foreach (var category in cantripTable)
                    table.Add((int)category[tier]);
            }
        }

        public static readonly Dictionary<ArmorType, int[]> armorTypeMap = new Dictionary<ArmorType, int[]>()
        {
            { ArmorType.MiscClothing,          MiscClothing },
            { ArmorType.Helms,                 Helms },
            { ArmorType.Shields,               Shields },
            { ArmorType.LeatherArmor,          LeatherArmor },
            { ArmorType.StuddedLeatherArmor,   StuddedLeatherArmor },
            { ArmorType.ChainmailArmor,        ChainmailArmor },
            { ArmorType.PlatemailArmor,        PlatemailArmor },
            { ArmorType.ScalemailArmor,        ScalemailArmor },
            { ArmorType.YoroiArmor,            YoroiArmor },
            { ArmorType.DiforsaArmor,          DiforsaArmor },
            { ArmorType.CeldonArmor,           CeldonArmor },
            { ArmorType.AmuliArmor,            AmuliArmor },
            { ArmorType.KoujiaArmor,           KoujiaArmor },
            { ArmorType.TenassaArmor,          TenassaArmor },
            { ArmorType.OverRobes,             OverRobes },
            { ArmorType.CovenantArmor,         CovenantArmor },
            { ArmorType.LoricaArmor,           LoricaArmor },
            { ArmorType.NariyidArmor,          NariyidArmor },
            { ArmorType.ChiranArmor,           ChiranArmor },
            { ArmorType.AlduressaArmor,        AlduressaArmor },
            { ArmorType.KnorrAcademyArmor,     KnorrAcademyArmor },
            { ArmorType.SedgemailLeatherArmor, SedgemailLeatherArmor },
            { ArmorType.HaebreanArmor,         HaebreanArmor },
            { ArmorType.OlthoiArmor,           OlthoiArmor },
            { ArmorType.OlthoiAmuliArmor,      OlthoiAmuliArmor },
            { ArmorType.OlthoiCeldonArmor,     OlthoiCeldonArmor },
            { ArmorType.OlthoiKoujiaArmor,     OlthoiKoujiaArmor },
            { ArmorType.OlthoiAlduressaArmor,  OlthoiAlduressaArmor }
        };

        public static readonly int[][] SocietyArmorMatrix =
        {           //   CH     EW     RB 
            new int[] { 38463, 38472, 38481 },  // BP
            new int[] { 38464, 38473, 38482 },  // Gauntlets
            new int[] { 38465, 38474, 38483 },  // Girth
            new int[] { 38466, 38475, 38484 },  // Greaves
            new int[] { 38467, 38476, 38485 },  // Helm
            new int[] { 38468, 38477, 38486 },  // Pauldrons
            new int[] { 38469, 38478, 38487 },  // Tassets
            new int[] { 38470, 38479, 38488 },  // Vambraces
            new int[] { 38471, 38480, 38489 }   // Sollerets
        };

        public static int[] GetLootTable(ArmorType armorType)
        {
            return armorTypeMap[armorType];
        }
    }
}
