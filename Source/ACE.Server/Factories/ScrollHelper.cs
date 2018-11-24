using ACE.Database;
using ACE.Database.Models.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Factories
{
    public class ScrollHelper
    {
        public List<Weenie> level1Scrolls = new List<Weenie>();
        public List<Weenie> level2Scrolls = new List<Weenie>();
        public List<Weenie> level3Scrolls = new List<Weenie>();
        public List<Weenie> level4Scrolls = new List<Weenie>();
        public List<Weenie> level5Scrolls = new List<Weenie>();
        public List<Weenie> level6Scrolls = new List<Weenie>();

        public ScrollHelper()
        {
            WorldDatabase wDb = new WorldDatabase();
            for (int i = 0; i < ScrollSpells.Length; i++)
            {
                level1Scrolls.Add(wDb.GetScrollBySpellID((uint)ScrollSpells[i][0]));
                level2Scrolls.Add(wDb.GetScrollBySpellID((uint)ScrollSpells[i][1]));
                level3Scrolls.Add(wDb.GetScrollBySpellID((uint)ScrollSpells[i][2]));
                level4Scrolls.Add(wDb.GetScrollBySpellID((uint)ScrollSpells[i][3]));
                level5Scrolls.Add(wDb.GetScrollBySpellID((uint)ScrollSpells[i][4]));
                level6Scrolls.Add(wDb.GetScrollBySpellID((uint)ScrollSpells[i][5]));

            }

        }

        public int[][] ScrollSpells =
{
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
////Strength Other
new int[] { 1, 1333, 1334, 1335, 1336, 1337, 2086, 4324 },
////Weakness Other
new int[] { 3, 1339, 1340, 1341, 1342, 1343, 2088, 4326 },
////Invuln
new int[] { 18, 245, 246, 247, 248, 249, 2245, 4560 },
////Invuln Other
new int[] { 17, 240, 241, 242, 243, 244, 2244, 4559 },
////Vuln Other
new int[] { 15, 230, 231, 232, 233, 234, 2318, 4633 },
////Impreg
new int[] { 256, 257, 258, 259, 260, 261, 2243, 4544 },
////Impreg Other
new int[] { 250, 251, 252, 253, 254, 255, 2242, 4543 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
////Magic Resist Other
new int[] { 268, 269, 270, 271, 272, 273, 2280, 4595 },
////Light Weapon
//new int[] { 298, 299, 300, 301, 302, 303, 2275, 4518 },
////Light Weapon Other
//new int[] { 292, 293, 294, 295, 296, 297, 2274, 4517 },
////Finesse Weapon
//new int[] { 322, 323, 324, 325, 326, 327, 2223, 4538 },
////Finesse Weapon Other
//new int[] { 316, 317, 318, 319, 320, 321, 2222, 4537 },
////Two Handed
///new int[] { 5099, 5100, 5101, 5102, 5103, 5104, 5105, 5032 },
////Two Handed Other
///new int[] { 5091, 5092, 5093, 5094, 5095, 5096, 5097, 5098 },
////Heavy Weapon
//new int[] { 418, 419, 420, 421, 422, 423, 2309, 4624 },
////Heavy Weapon Other
//new int[] { 412, 413, 414, 415, 416, 417, 2308, 4623 },
////Missile Weapon
new int[] { 467, 468, 469, 470, 471, 472, 2243, 4558 },
////Missile Weapon Other
new int[] { 461, 462, 463, 464, 465, 466, 2242, 4557 },
////Creature Enchant
new int[] { 557, 558, 559, 560, 561, 562, 2215, 4530 },
////Creature Enchant Other
new int[] { 563, 564, 565, 566, 567, 568, 2214, 4529 },
////Item Enchant
new int[] { 581, 582, 583, 584, 585, 586, 2249, 4564 },
////Item Enchant Other
new int[] { 587, 588, 589, 590, 591, 592, 2248, 4563 },
////Life Magic
new int[] { 605, 606, 607, 608, 609, 610, 2267, 4582 },
////Life Magic Other
new int[] { 611, 612, 613, 614, 615, 616, 2266, 4581 },
////War Magic
new int[] { 629, 630, 631, 632, 633, 634, 2323, 4638 },
////War Magic Other
new int[] { 635, 636, 637, 638, 639, 640, 2322, 4637 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
////Mana C Other
new int[] { 659, 660, 661, 662, 663, 664, 2286, 4601 },
////Arcane Enlight
new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 },
////Arcane Enlight Other
new int[] { 684, 685, 686, 687, 688, 689, 2194, 4509 },
////Armor Tinkering
new int[] { 702, 703, 704, 705, 706, 707, 2197, 4512 },
////Armor Tinkering Other
new int[] { 708, 709, 710, 711, 712, 713, 2196, 4511 },
////Item Tinkering
new int[] { 726, 727, 728, 729, 730, 731, 2251, 4566 },
////Item Tinkering Other
new int[] { 732, 733, 734, 735, 736, 737, 2250, 4565 },
////Magic Item Tinkering
new int[] { 750, 751, 752, 753, 754, 755, 2277, 4592 },
////Magic Item Tinkering Other
new int[] { 756, 757, 758, 759, 760, 761, 2276, 4591 },
////Weapon Tinkering
new int[] { 774, 775, 776, 777, 778, 779, 2325, 4640 },
////Weapon Tinkering Other
new int[] { 780, 781, 782, 783, 784, 785, 2324, 4639 },
////Monster Attunement
new int[] { 798, 799, 800, 801, 802, 803, 2289, 4604 },
////Monster Attunement Other
new int[] { 804, 805, 806, 807, 808, 809, 2288, 4603 },
////Person Attunement
new int[] { 824, 825, 826, 827, 828, 829, 2293, 4608 },
////Person Attunement Other
new int[] { 830, 831, 832, 833, 834, 835, 2292, 4607 },
////Deception Mastery
new int[] { 850, 851, 852, 853, 854, 855, 2226, 4542 },
////Deception Mastery Other
new int[] { 856, 857, 858, 859, 860, 861, 2225, 4541 },
////Healing Mastery
new int[] { 874, 875, 876, 877, 878, 879, 2241, 4556 },
////Healing Mastery Other
new int[] { 880, 881, 882, 883, 884, 885, 2240, 4555 },
////Leadership
new int[] { 898, 899, 900, 901, 902, 903, 2262, 4586 },
////Leadership Other
new int[] { 904, 905, 906, 907, 908, 909, 2263, 4585 },
////Lockpick
new int[] { 922, 923, 924, 925, 926, 927, 2271, 4586 },
 ////Lockpick Other
new int[] { 928, 929, 930, 931, 932, 933, 2270, 4586 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
////Fealty Other
new int[] { 952, 953, 954, 955, 956, 957, 2232, 4547 },
////Jumping
new int[] { 970, 971, 972, 973, 974, 975, 2256, 4572 },
////Jumping Other
new int[] { 976, 977, 978, 979, 980, 981, 2255, 4571 },
////Sprint
new int[] { 982, 983, 984, 985, 986, 986, 2301, 4616 },
////Sprint Other
new int[] { 988, 989, 990, 991, 992, 993, 2300, 4615 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
////Endurance Other
new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
////Coordination
new int[] { 1373, 1374, 1375, 1376, 1377, 1378, 2059, 4297 },
////Coordination Other
new int[] { 1379, 1380, 1381, 1382, 1383, 1384, 2058, 4296 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402, 2081, 4319 },
////Quickness Other
new int[] { 1403, 1404, 1405, 1406, 1407, 1408, 2080, 4318 },
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 },
////Focus Other
new int[] { 1427, 1428, 1429, 1430, 1431, 1432, 2065, 4304 },
////Willpower
new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091, 4329 },
////Willpower Other
new int[] { 1451, 1452, 1453, 1454, 1455, 1456, 2090, 4328 },
////Cooking Mastery
new int[] { 1715, 1716, 1717, 1718, 1719, 1720, 2211, 4526 },
////Cooking Mastery Other
new int[] { 1709, 1710, 1711, 1712, 1713, 1714, 2210, 4525 },
////Fletching
new int[] { 1739, 1740, 1741, 1742, 1743, 1744, 2237, 4552 },
////Fletching Other
new int[] { 1733, 1734, 1735, 1736, 1737, 1738, 2236, 4551 },
////Alchemy
new int[] { 1763, 1764, 1765, 1766, 1767, 1768, 2191, 4506 },
////Alchemy Other
new int[] { 1757, 1758, 1759, 1760, 1761, 1762, 2190, 4505 },
////Dirty Fighting
//new int[] { 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786 },
////Dirty Fighting Other
//new int[] { 5771, 5772, 5773, 5774, 5775, 5776, 5777, 5778 },
////Dual Wield
//new int[] { 5803, 5804, 5805, 5806, 5807, 5808, 5809, 5810 },
////Dual Wield Other
//new int[] { 5795, 5796, 5797, 5798, 5799, 5800, 5801, 5802 },
////Recklessness
//new int[] { 5827, 5828, 5829, 5830, 5831, 5832, 5833, 5834 },
////Recklessness Other
//new int[] { 5819, 5820, 5821, 5822, 5823, 5824, 5825, 5826 },
////Shield
////new int[] { 5851, 5852, 5853, 5854, 5855, 5856, 5857, 5858 },
////Shield Other
////new int[] { 5843, 5844, 5845, 5846, 5847, 5848, 5849, 5850 },
////Sneak Attack
//new int[] { 5875, 5876, 5877, 5878, 5879, 5880, 5881, 5882 },
 ////Sneak Attack Other
//new int[] { 5867, 5868, 5869, 5870, 5871, 5872, 5873, 5874 },
////Summoning
//new int[] { 6116, 6117, 6118, 6119, 6120, 6121, 6122, 6123 },
////Summoning Other
//new int[] { 6108, 6109, 6110, 6111, 6112, 6113, 6114, 6115 },
////Void Magic
//new int[] { 5411, 5412, 5413, 5414, 5415, 5416, 5417, 5418 },
////Void Magic Other
//new int[] { 5403, 5404, 5405, 5406, 5407, 5408, 5409, 5410 },
////Salvaging
new int[] { 3499, 3500, 3501, 3502, 3503, 3504, 3519, 4502 },
////Salvaging Other
new int[] { 3506, 3507, 3508, 3509, 3510, 3511, 3512, 4513 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170, 2185, 4496 },
////Regeneration Other
new int[] { 159, 160, 161, 162, 163, 164, 2184, 4495 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193, 2187, 4498 },
////Rejuvenation Other
new int[] { 53, 184, 184, 186, 187, 188, 2186, 4497 },
////Mana Renewal
new int[] { 212, 213, 214, 215, 216, 217, 2183, 4494 },
////Mana Renewal Other
new int[] { 206, 207, 208, 209, 210, 211, 2182, 4493 },
////Acid Prot
new int[] { 515, 516, 517, 518, 519, 520, 2149, 4460 },
////Acid Prot Other
new int[] { 509, 510, 511, 512, 513, 514, 2148, 4459 },
////Brudge Prot
new int[] { 1018, 1019, 1020, 1021, 1022, 1023, 2153, 4464 },
////Brudge Prot Other
new int[] { 1024, 1025, 1026, 1027, 1028, 1029, 2152, 4463 },
////Cold Prot
new int[] { 1030, 1031, 1032, 1033, 1034, 1035, 2155, 4466 },
////Cold Prot Other
new int[] { 1036, 1037, 1038, 1039, 1040, 1041, 2154, 4465 },
////Lightning Prot
new int[] { 1066, 1067, 1068, 1069, 1070, 1071, 2159, 4470 },
////Lightning Prot Other
new int[] { 1072, 1073, 1074, 1075, 1076, 1077, 2158, 4469 },
////Fire Prot
new int[] { 20, 1090,1091, 1092, 1093, 1094, 2157, 4468 },
////Fire Prot Other
new int[] { 19, 810, 836, 849, 1095, 1096, 2156, 4467 },
////Blade Prot
new int[] { 1109, 1110, 1111, 1112, 1113, 1114, 2151, 4462 },
////Blade Prot Other
new int[] { 1115, 1116, 1117, 1118, 1119, 1120, 2150, 4461 },
////Pierce Prot
new int[] { 1133, 1134, 1135, 1136, 1137, 1138, 2161, 4472 },
////Pierce Prot Other
new int[] { 1139, 1140, 1141, 1142, 1143, 1144, 2160, 4471 },
////Armor Self
new int[] { 24, 1308, 1309, 1310, 1311, 1312, 2053, 4291 },
////Armor Other
new int[] { 23, 1313, 1314, 1315, 1316, 1317, 2052, 4290 },
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
////Heart Seeker
new int[] { 1587, 1588, 1589, 1590, 1591, 1592, 2106, 4405 },
////Defender
new int[] { 1599, 1601, 1602, 1603, 1604, 1605, 2101, 4400 },
////SwiftKiller
new int[] { 49, 1623, 1624, 1625, 1626, 1627, 2116, 4417 },
////Blooddrinker
new int[] { 35, 1612, 1613, 1614, 1615, 1616, 2096, 4395 },
////Hermetic Link
new int[] { 1475, 1476, 1477, 1478, 1479, 1480, 2117, 4418 },
};
    }
}
