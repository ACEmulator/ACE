using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    public static class LootHelper
    {

        public static int[] amuliColors = { 0xF00001B, 0xF000022, 0xF00001D, 0xF00001E, 0xF00001C, 0xF00001F, 0xF000020, 0xF00028A, 0xF00028B, 0xF00028C, 0xF0000AE, 0xF00011B, 0xF00011C, 0xF00011D, 0xF00011E, 0xF00016F, 0xF000170, 0xF000171, 0xF0001E2, 0xF0001E3, 0xF0001E4};

        public static int[][] HeadSpells =
{
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 },
////Person Attunement
new int[] { 824, 825, 826, 827, 828, 829, 2293, 4608 },
////Invuln
new int[] { 18, 245, 246, 247, 248, 249, 2245, 4560 },
////Impreg
new int[] { 256, 257, 258, 259, 260, 261, 2243, 4544 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
////Dirty Fighting
new int[] { 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786 },
////Dual Wield
new int[] { 5803, 5804, 5805, 5806, 5807, 5808, 5809, 5810 },
////Recklessness
new int[] { 5827, 5828, 5829, 5830, 5831, 5832, 5833, 5834 },
////Sneak Attack
new int[] { 5867, 5868, 5869, 5870, 5871, 5872, 5881, 5882 },
////Deception Mastery
new int[] { 850, 851, 852, 853, 854, 855, 2226, 4542 },
////Arcane Enlight
new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
////Creature Enchant
new int[] { 557, 558, 559, 560, 561, 562, 2215, 4530 },
////Item Enchant
new int[] { 581, 582, 583, 584, 585, 586, 2249, 4564 },
////Life Magic
new int[] { 605, 606, 607, 608, 609, 610, 2267, 4582 },
////War Magic
new int[] { 629, 630, 631, 632, 633, 634, 2323, 4638 },
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
new int[] { 3499, 3500, 3501, 3502, 3503, 3504, 3519, 4502 },
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
new int[] { 898, 899, 900, 901, 902, 903, 2271, 4586 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
////Armor Self
new int[] { 24, 1308, 1309, 1310, 1311, 1312, 2053, 4291 },
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
};

        public static int[][] ChestSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170, 2185, 4496 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193, 2187, 4498 },
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
};

        public static int[][] UpperArmSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170, 2185, 4496 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193, 2187, 4498 },
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
};

        public static int[][] LowerArmSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170, 2185, 4496 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193, 2187, 4498 },
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
};

        public static int[][] HandSpells =
        {
////Coordination
new int[] { 1373, 1374, 1375, 1376, 1377, 1378, 2059, 4297 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402, 2081, 4319 },
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 },
////Willpower
new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091, 4329 },
////Two Handed
new int[] { 5099, 5100, 5101, 5102, 5103, 5104, 5105, 5032 },
////Finesse Weapon
new int[] { 322, 323, 324, 325, 326, 327, 2223, 4538 },
////Heavy Weapon
new int[] { 418, 419, 420, 421, 422, 423, 2309, 4624 },
////Light Weapon
new int[] { 298, 299, 300, 301, 302, 303, 2275, 4518 },
////Missile Weapon
new int[] { 467, 468, 469, 470, 471, 472, 2243, 4558 },
////Shield
new int[] { 5843, 5844, 5845, 5846, 5847, 5848, 5857, 5858 },
////Arcane Enlight
new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
////Creature Enchant
new int[] { 557, 558, 559, 560, 561, 562, 2215, 4530 },
////Item Enchant
new int[] { 581, 582, 583, 584, 585, 586, 2249, 4564 },
////Life Magic
new int[] { 605, 606, 607, 608, 609, 610, 2267, 4582 },
////War Magic
new int[] { 629, 630, 631, 632, 633, 634, 2323, 4638 },
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
new int[] { 3499, 3500, 3501, 3502, 3503, 3504, 3519, 4502 },
////Armor Tinkering
new int[] { 702, 703, 704, 705, 706, 707, 2197, 4512 },
////Item Tinkering
new int[] { 726, 727, 728, 729, 730, 731, 2251, 4566 },
////Magic Item Tinkering
new int[] { 750, 751, 752, 753, 754, 755, 2277, 4592 },
////Weapon Tinkering
new int[] { 774, 775, 776, 777, 778, 779, 2325, 4640 },
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
};

        public static int[][] AbdomenSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170, 2185, 4496 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193, 2187, 4498 },
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
};

        public static int[][] UpperLegSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402, 2081, 4319 },
////Jumping
new int[] { 970, 971, 972, 973, 974, 975, 2256, 4572 },
////Sprint
new int[] { 982, 983, 984, 985, 986, 986, 2301, 4616 },
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
};

        public static int[][] LowerLegSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402, 2081, 4319 },
////Jumping
new int[] { 970, 971, 972, 973, 974, 975, 2256, 4572 },
////Sprint
new int[] { 982, 983, 984, 985, 986, 986, 2301, 4616 },
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
};

        public static int[][] FeetSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
////Coordination
new int[] { 1373, 1374, 1375, 1376, 1377, 1378, 2059, 4297 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402, 2081, 4319 },
////Two Handed
new int[] { 5099, 5100, 5101, 5102, 5103, 5104, 5105, 5032 },
////Finesse Weapon
new int[] { 322, 323, 324, 325, 326, 327, 2223, 4538 },
////Heavy Weapon
new int[] { 418, 419, 420, 421, 422, 423, 2309, 4624 },
////Light Weapon
new int[] { 298, 299, 300, 301, 302, 303, 2275, 4518 },
////Missile Weapon
new int[] { 467, 468, 469, 470, 471, 472, 2243, 4558 },
////Invuln
new int[] { 18, 245, 246, 247, 248, 249, 2245, 4560 },
////Impreg
new int[] { 256, 257, 258, 259, 260, 261, 2243, 4544 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
////Arcane Enlight
new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
////Healing Mastery
new int[] { 874, 875, 876, 877, 878, 879, 2241, 4556 },
////Jumping
new int[] { 970, 971, 972, 973, 974, 975, 2256, 4572 },
////Sprint
new int[] { 982, 983, 984, 985, 986, 986, 2301, 4616 },
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
};

        public static int[][] JewelrySpells =
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
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
////Deception Mastery
new int[] { 850, 851, 852, 853, 854, 855, 2226, 4542 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
////Salvaging
new int[] { 3499, 3500, 3501, 3502, 3503, 3504, 3519, 4502 },
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
////Fealty
new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
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
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170, 2185, 4496 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193, 2187, 4498 },
////Mana Renewal
new int[] { 212, 213, 214, 215, 216, 217, 2183, 4494 },
};

        public static int[][] ClothingSpells =
        {
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 },
////Willpower
new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091, 4329 },
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
};

        public static int[][] ShieldSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354, 2061, 4299 },
////Invuln
new int[] { 18, 245, 246, 247, 248, 249, 2245, 4560 },
////Impreg
new int[] { 256, 257, 258, 259, 260, 261, 2243, 4544 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
////Shield
new int[] { 5843, 5844, 5845, 5846, 5847, 5848, 5857, 5858 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193, 2187, 4498 },
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
};
        public static int[][] GemSpells =
{
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332, 2087, 4325 },
////Invuln
new int[] { 18, 245, 246, 247, 248, 249, 2245, 4560 },
////Impreg
new int[] { 256, 257, 258, 259, 260, 261, 2243, 4544 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279, 2281, 4596 },
////Light Weapon
new int[] { 298, 299, 300, 301, 302, 303, 2275, 4518 },
////Finesse Weapon
new int[] { 322, 323, 324, 325, 326, 327, 2223, 4538 },
////Two Handed
new int[] { 5099, 5100, 5101, 5102, 5103, 5104, 5105, 5032 },
////Heavy Weapon
new int[] { 418, 419, 420, 421, 422, 423, 2309, 4624 },
////Missile Weapon
new int[] { 467, 468, 469, 470, 471, 472, 2243, 4558 },
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
new int[] { 850, 851, 852, 853, 854, 855, 2226, 4542 },
////Healing Mastery
new int[] { 874, 875, 876, 877, 878, 879, 2241, 4556 },
////Leadership
new int[] { 898, 899, 900, 901, 902, 903, 2271, 4586 },
////Lockpick
new int[] { 922, 923, 924, 925, 926, 927, 2271, 4586 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951, 2233, 4548 },
////Jumping
new int[] { 970, 971, 972, 973, 974, 975, 2256, 4572 },
////Sprint
new int[] { 982, 983, 984, 985, 986, 986, 2301, 4616 },
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
new int[] { 5843, 5844, 5845, 5846, 5847, 5848, 5857, 5858 },
////Sneak Attack
new int[] { 5867, 5868, 5869, 5870, 5871, 5872, 5881, 5882 },
////Summoning
new int[] { 6116, 6117, 6118, 6119, 6120, 6121, 6122, 6123 },
////Void Magic
new int[] { 5411, 5412, 5413, 5414, 5415, 5416, 5417, 5418 },
////Salvaging
new int[] { 3499, 3500, 3501, 3502, 3503, 3504, 3519, 4502 },
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
};

        public static int[][] WandSpells =
        {
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 },
////Willpower
new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091, 4329 },
////Sneak Attack
new int[] { 5867, 5868, 5869, 5870, 5871, 5872, 5881, 5882 },
////Arcane Enlight
new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
////Creature Enchant
new int[] { 557, 558, 559, 560, 561, 562, 2215, 4530 },
////Item Enchant
new int[] { 581, 582, 583, 584, 585, 586, 2249, 4564 },
////Life Magic
new int[] { 605, 606, 607, 608, 609, 610, 2267, 4582 },
////War Magic
new int[] { 629, 630, 631, 632, 633, 634, 2323, 4638 },
////Defender
new int[] { 1599, 1600, 1601, 1602, 1603, 1604, 2101, 4400 },
////Hermetic Link
new int[] { 1475, 1476, 1477, 1478, 1479, 1480, 2117, 4418 },
};

        public static int[][] MeleeSpells =
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
new int[] { 5867, 5868, 5869, 5870, 5871, 5872, 5881, 5882 },
////Heart Seeker
new int[] { 1587, 1588, 1589, 1590, 1591, 1592, 2106, 4405 },
////Defender
new int[] { 1599, 1600, 1601, 1602, 1603, 1604, 2101, 4400 },
////SwiftKiller
new int[] { 49, 1623, 1624, 1625, 1626, 1627, 2116, 4417 },
////Blooddrinker
new int[] { 35, 1612, 1613, 1614, 1615, 1616, 2096, 4395 },
};

        public static int[][] MissileSpells =
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
new int[] { 5867, 5868, 5869, 5870, 5871, 5872, 5881, 5882 },
////Heart Seeker
new int[] { 1587, 1588, 1589, 1590, 1591, 1592, 2106, 4405 },
////Defender
new int[] { 1599, 1600, 1601, 1602, 1603, 1604, 2101, 4400 },
////SwiftKiller
new int[] { 49, 1623, 1624, 1625, 1626, 1627, 2116, 4417 },
////Blooddrinker
new int[] { 35, 1612, 1613, 1614, 1615, 1616, 2096, 4395 },
};

     public static int[][] HeadCantrips =
{
////Focus
new int[] { 2581, 2574, 3964, 6105},
////Person Attunement
new int[] { 2562, 2527, 4707, 6066 },
////Invuln
new int[] { 2550, 2515, 4696, 6055},
////Impreg
new int[] { 2549, 2514, 4695, 6054},
////Magic Resist
new int[] { 2559, 2524, 4704, 6063},
////Dirty Fighting
new int[] { 5882, 5888, 5893, 6049 },
////Dual Wield
new int[] { 5884, 5889, 5894, 6050},
////Recklessness
new int[] { 5885, 5890,5895, 6067},
////Sneak Attack
new int[] {5887, 5892, 5897, 6070 },
////Deception Mastery
new int[] { 2545, 2510, 4020, 6048},
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
new int[] { 3809, 3834, 4708, 6068},
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
////Regeneration
new int[] { 2626, 2623, 4680, 6077},
////Rejuvenation
new int[] { 2628, 2625, 4682, 6076},
////Mana Renewal
new int[] { 2627, 2624, 4681, 6078 },
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
};

        public static int[][] ChestCantrips =
        {
////Strength
new int[] { 2583, 2576, 3965, 6107},
////Endurance
new int[] { 2580, 2573, 4226, 6104},
////Magic Resist
new int[] { 2559, 2524, 4704, 6063},
////Fealty
new int[] { 2546, 2511, 4692, 6051},
////Regeneration
new int[] { 2626, 2623, 4680, 6077},
////Rejuvenation
new int[] { 2628, 2625, 4682, 6076},
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
};

        public static int[][] UpperArmCantrips =
        {
////Strength
new int[] { 2583, 2576, 3965, 6107},
////Endurance
new int[] { 2580, 2573, 4226, 6104},
////Magic Resist
new int[] { 2559, 2524, 4704, 6063},
////Fealty
new int[] { 2546, 2511, 4692, 6051},
////Regeneration
new int[] { 2626, 2623, 4680, 6077},
////Rejuvenation
new int[] { 2628, 2625, 4682, 6076},
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
};

        public static int[][] LowerArmCantrips =
        {
////Strength
new int[] { 2583, 2576, 3965, 6107},
////Endurance
new int[] { 2580, 2573, 4226, 6104},
////Magic Resist
new int[] { 2559, 2524, 4704, 6063},
////Fealty
new int[] { 2546, 2511, 4692, 6051},
////Regeneration
new int[] { 2626, 2623, 4680, 6077},
////Rejuvenation
new int[] { 2628, 2625, 4682, 6076},
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
};

        public static int[][] HandCantrips =
        {
////Coordination
new int[] { 2579, 2572, 3963, 6103},
////Quickness
new int[] { 2582, 2575, 4019, 6106},
////Focus
new int[] { 2581, 2574, 3964, 6105},
////Willpower
new int[] { 2584, 2577, 4227, 6101},
////Two Handed
new int[] { 2566, 2531, 4712, 6072},
////Finesse Weapon
new int[] { 2544, 2509, 4691, 6047},
////Heavy Weapon
new int[] { 2566, 2531, 4712, 6072},
////Light Weapon
new int[] { 2557, 2522, 4686, 6043},
////Missile Weapon
new int[] { 2540, 2505, 4687, 6044},
////Shield
new int[] { 5885, 5890, 5895, 6067},
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
new int[] { 3809, 3834, 4708, 6068},
////Armor Tinkering
new int[] { 2538, 2503, 4685, 6042},
////Item Tinkering
new int[] { 2552, 2517, 4698, 6057},
////Magic Item Tinkering
new int[] { 2558, 2523, 4703, 6062},
////Weapon Tinkering
new int[] { 2570, 2535, 4912, 6039},
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
};

        public static int[][] AbdomenCantrips =
        {
////Strength
new int[] { 2583, 2576, 3965, 6107},
////Endurance
new int[] { 2580, 2573, 4226, 6104},
////Magic Resist
new int[] { 2559, 2524, 4704, 6063},
////Fealty
new int[] { 2546, 2511, 4692, 6051},
////Regeneration
new int[] { 2626, 2623, 4680, 6077},
////Rejuvenation
new int[] { 2628, 2625, 4682, 6076},
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
};

        public static int[][] UpperLegCantrips =
        {
////Strength
new int[] { 2583, 2576, 3965, 6107},
////Endurance
new int[] { 2580, 2573, 4226, 6104},
////Quickness
new int[] { 2582, 2575, 4019, 6106},
////Jumping
new int[] { 2553, 2518, 4699, 6058},
////Sprint
new int[] { 2564, 2529, 4710, 6071},
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
};

        public static int[][] LowerLegCantrips =
        {
////Strength
new int[] { 2583, 2576, 3965, 6107},
////Endurance
new int[] { 2580, 2573, 4226, 6104},
////Quickness
new int[] { 2582, 2575, 4019, 6106},
////Jumping
new int[] { 2553, 2518, 4699, 6058},
////Sprint
new int[] { 2564, 2529, 4710, 6071},
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
};

        public static int[][] FeetCantrips =
        {
////Strength
new int[] { 2583, 2576, 3965, 6107},
////Endurance
new int[] { 2580, 2573, 4226, 6104},
////Coordination
new int[] { 2579, 2572, 3963, 6103},
////Quickness
new int[] { 2582, 2575, 4019, 6106},
////Two Handed
new int[] { 2566, 2531, 4712, 6072},
////Finesse Weapon
new int[] { 2544, 2509, 4691, 6047},
////Heavy Weapon
new int[] { 2566, 2531, 4712, 6072},
////Light Weapon
new int[] { 2557, 2522, 4686, 6043},
////Missile Weapon
new int[] { 2540, 2505, 4687, 6044},
////Invuln
new int[] { 2550, 2515, 4696, 6055},
////Impreg
new int[] { 2549, 2514, 4695, 6054},
////Magic Resist
new int[] { 2559, 2524, 4704, 6063},
////Arcane Enlight
new int[] { 2537, 2502, 4684, 6041},
////Mana C
new int[] { 2560, 2525, 4705, 6064},
////Healing Mastery
new int[] { 2548, 2513, 4694, 6053},
////Jumping
new int[] { 2553, 2518, 4699, 6058},
////Sprint
new int[] { 2564, 2529, 4710, 6071},
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
};

        public static int[][] JewelryCantrips =
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
////Person Attunement
new int[] { 2562, 2527, 4707, 6066 },
////Magic Resist
new int[] { 2559, 2524, 4704, 6063},
////Deception Mastery
new int[] { 2545, 2510, 4020, 6048},
////Mana C
new int[] { 2560, 2525, 4705, 6064},
////Salvaging
new int[] { 3809, 3834, 4708, 6068},
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
new int[] { 2621, 2614, 3957, 6085},
////Pierce Prot
new int[] { 2620, 2613, 3956, 6084},
////Regeneration
new int[] { 2626, 2623, 4680, 6077},
////Rejuvenation
new int[] { 2628, 2625, 4682, 6076},
////Mana Renewal
new int[] { 2627, 2624, 4681, 6078},
};

        public static int[][] ClothingCantrips =
        {
////Focus
new int[] { 2581, 2574, 3964, 6105},
////Willpower
new int[] { 2584, 2577, 4227, 6101},
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
new int[] { 2621, 2614, 3957, 6085},
////Pierce Prot
new int[] { 2620, 2613, 3956, 6084},
};

        public static int[][] ShieldCantrips =
        {
////Strength
new int[] { 2583, 2576, 3965, 6107},
////Endurance
new int[] { 2580, 2573, 4226, 6104},
////Invuln
new int[] { 2550, 2515, 4696, 6055},
////Impreg
new int[] { 2549, 2514, 4695, 6054},
////Magic Resist
new int[] { 2559, 2524, 4704, 6063},
////Shield
new int[] { 5886, 5891, 5896, 6069},
////Fealty
new int[] { 2546, 2511, 4692, 6051},
////Rejuvenation
new int[] { 2628, 2625, 4682, 6076},
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
};

        public static int[][] WandCantrips =
        {
////Focus
new int[] { 2581, 2574, 3964, 6105},
////Willpower
new int[] { 2584, 2577, 4227, 6101},
////Sneak Attack
new int[] {5887, 5892, 5897, 6070 },
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
new int[] { 3199, 3200, 4686, 6087},
};

        public static int[][] MeleeCantrips =
        {
////Strength
new int[] { 2583, 2576, 3965, 6107},
////Endurance
new int[] { 2580, 2573, 4226, 6104},
////Coordination
new int[] { 2579, 2572, 3963, 6103},
////Dirty Fighting
new int[] { 5882, 5888, 5893, 6049 },
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
};

        public static int[][] MissileCantrips =
        {
////Strength
new int[] { 2583, 2576, 3965, 6107},
////Endurance
new int[] { 2580, 2573, 4226, 6104},
////Coordination
new int[] { 2579, 2572, 3963, 6103},
////Dirty Fighting
new int[] { 5882, 5888, 5893, 6049 },
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
};

    }
}
