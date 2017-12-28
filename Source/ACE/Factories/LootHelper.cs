using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    public static class LootHelper
    {
        public static int[][] HeadSpells =
{
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426 },
////Person Attunement
new int[] { 824, 825, 826, 827, 828, 829 },
////Invuln
new int[] { 18, 245, 246, 247, 248, 249 },
////Impreg
new int[] { 256, 257, 258, 259, 260, 261 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279 },
////Dirty Fighting
new int[] { 5779, 5780, 5781, 5782, 5783, 5784 },
////Dual Wield
new int[] { 5803, 5804, 5805, 5806, 5807, 5808 },
////Recklessness
new int[] { 5827, 5828, 5829, 5830, 5831, 5832 },
////Sneak Attack
new int[] { 5867, 5868, 5869, 5870, 5871, 5872 },
////Deception Mastery
new int[] { 850, 851, 852, 853, 854, 855 },
////Arcane Enlight
new int[] { 678, 679, 680, 681, 682, 683 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658 },
////Creature Enchant
new int[] { 557, 558, 559, 560, 561, 562 },
////Item Enchant
new int[] { 581, 582, 583, 584, 585, 586 },
////Life Magic
new int[] { 605, 606, 607, 608, 609, 610 },
////War Magic
new int[] { 629, 630, 631, 632, 633, 634 },
////Cooking Mastery
new int[] { 1715, 1716, 1717, 1718, 1719, 1720 },
////Fletching
new int[] { 1739, 1740, 1741, 1742, 1743, 1744 },
////Alchemy
new int[] { 1763, 1764, 1765, 1766, 1767, 1768 },
////Healing Mastery
new int[] { 874, 875, 876, 877, 878, 879 },
////Lockpick
new int[] { 922, 923, 924, 925, 926, 927 },
////Salvaging
new int[] { 3499, 3500, 3501, 3502, 3503, 3504 },
////Armor Tinkering
new int[] { 702, 703, 704, 705, 706, 707 },
////Item Tinkering
new int[] { 726, 727, 728, 729, 730, 731 },
////Magic Item Tinkering
new int[] { 750, 751, 752, 753, 754, 755 },
////Weapon Tinkering
new int[] { 774, 775, 776, 777, 778, 779 },
////Monster Attunement
new int[] { 798, 799, 800, 801, 802, 803 },
////Leadership
new int[] { 898, 899, 900, 901, 902, 903 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951 },
////Armor Self
new int[] { 24, 1308, 1309, 1310, 1311, 1312 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193 },
////Mana Renewal
new int[] { 212, 213, 214, 215, 216, 217 },
////Imp
new int[] { 51, 1483, 1484, 1485, 1486, 1487 },
////Blade Bane
new int[] { 37, 1558, 1559, 1560, 1561, 1562 },
////Acid Bane
new int[] { 1493, 1494, 1495, 1496, 1497, 1498 },
////Bludge Bane
new int[] { 1511, 1512, 1513, 1514, 1515, 1516 },
////Frost Bane
new int[] { 1523, 1524, 1525, 1526, 1527, 1528 },
////Lightning Bane
new int[] { 1535, 1536, 1537, 1538, 1539, 1540 },
////Flame Bane
new int[] { 1547, 1548, 1549, 1550, 1551, 1552 },
////Pierce Bane
new int[] { 1569, 1570, 1571, 1572, 1573, 1574 },
};

        public static int[][] ChestSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193 },
////Imp
new int[] { 51, 1483, 1484, 1485, 1486, 1487 },
////Blade Bane
new int[] { 37, 1558, 1559, 1560, 1561, 1562 },
////Acid Bane
new int[] { 1493, 1494, 1495, 1496, 1497, 1498 },
////Bludge Bane
new int[] { 1511, 1512, 1513, 1514, 1515, 1516 },
////Frost Bane
new int[] { 1523, 1524, 1525, 1526, 1527, 1528 },
////Lightning Bane
new int[] { 1535, 1536, 1537, 1538, 1539, 1540 },
////Flame Bane
new int[] { 1547, 1548, 1549, 1550, 1551, 1552 },
////Pierce Bane
new int[] { 1569, 1570, 1571, 1572, 1573, 1574 },
};

        public static int[][] UpperArmSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193 },
////Imp
new int[] { 51, 1483, 1484, 1485, 1486, 1487 },
////Blade Bane
new int[] { 37, 1558, 1559, 1560, 1561, 1562 },
////Acid Bane
new int[] { 1493, 1494, 1495, 1496, 1497, 1498 },
////Bludge Bane
new int[] { 1511, 1512, 1513, 1514, 1515, 1516 },
////Frost Bane
new int[] { 1523, 1524, 1525, 1526, 1527, 1528 },
////Lightning Bane
new int[] { 1535, 1536, 1537, 1538, 1539, 1540 },
////Flame Bane
new int[] { 1547, 1548, 1549, 1550, 1551, 1552 },
////Pierce Bane
new int[] { 1569, 1570, 1571, 1572, 1573, 1574 },
};

        public static int[][] LowerArmSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193 },
////Imp
new int[] { 51, 1483, 1484, 1485, 1486, 1487 },
////Blade Bane
new int[] { 37, 1558, 1559, 1560, 1561, 1562 },
////Acid Bane
new int[] { 1493, 1494, 1495, 1496, 1497, 1498 },
////Bludge Bane
new int[] { 1511, 1512, 1513, 1514, 1515, 1516 },
////Frost Bane
new int[] { 1523, 1524, 1525, 1526, 1527, 1528 },
////Lightning Bane
new int[] { 1535, 1536, 1537, 1538, 1539, 1540 },
////Flame Bane
new int[] { 1547, 1548, 1549, 1550, 1551, 1552 },
////Pierce Bane
new int[] { 1569, 1570, 1571, 1572, 1573, 1574 },
};

        public static int[][] HandSpells =
        {
////Coordination
new int[] { 1373, 1374, 1375, 1376, 1377, 1378 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402 },
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426 },
////Willpower
new int[] { 1445, 1446, 1447, 1448, 1449, 1450 },
////Two Handed
new int[] { 5099, 5100, 5101, 5102, 5103, 5104 },
////Finesse Weapon
new int[] { 322, 323, 324, 325, 326, 327 },
////Heavy Weapon
new int[] { 418, 419, 420, 421, 422, 423 },
////Light Weapon
new int[] { 298, 299, 300, 301, 302, 303 },
////Missile Weapon
new int[] { 467, 468, 469, 470, 471, 472 },
////Shield
new int[] { 5843, 5844, 5845, 5846, 5847, 5848 },
////Arcane Enlight
new int[] { 678, 679, 680, 681, 682, 683 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658 },
////Creature Enchant
new int[] { 557, 558, 559, 560, 561, 562 },
////Item Enchant
new int[] { 581, 582, 583, 584, 585, 586 },
////Life Magic
new int[] { 605, 606, 607, 608, 609, 610 },
////War Magic
new int[] { 629, 630, 631, 632, 633, 634 },
////Cooking Mastery
new int[] { 1715, 1716, 1717, 1718, 1719, 1720 },
////Fletching
new int[] { 1739, 1740, 1741, 1742, 1743, 1744 },
////Alchemy
new int[] { 1763, 1764, 1765, 1766, 1767, 1768 },
////Healing Mastery
new int[] { 874, 875, 876, 877, 878, 879 },
////Lockpick
new int[] { 922, 923, 924, 925, 926, 927 },
////Salvaging
new int[] { 3499, 3500, 3501, 3502, 3503, 3504 },
////Armor Tinkering
new int[] { 702, 703, 704, 705, 706, 707 },
////Item Tinkering
new int[] { 726, 727, 728, 729, 730, 731 },
////Magic Item Tinkering
new int[] { 750, 751, 752, 753, 754, 755 },
////Weapon Tinkering
new int[] { 774, 775, 776, 777, 778, 779 },
////Imp
new int[] { 51, 1483, 1484, 1485, 1486, 1487 },
////Blade Bane
new int[] { 37, 1558, 1559, 1560, 1561, 1562 },
////Acid Bane
new int[] { 1493, 1494, 1495, 1496, 1497, 1498 },
////Bludge Bane
new int[] { 1511, 1512, 1513, 1514, 1515, 1516 },
////Frost Bane
new int[] { 1523, 1524, 1525, 1526, 1527, 1528 },
////Lightning Bane
new int[] { 1535, 1536, 1537, 1538, 1539, 1540 },
////Flame Bane
new int[] { 1547, 1548, 1549, 1550, 1551, 1552 },
////Pierce Bane
new int[] { 1569, 1570, 1571, 1572, 1573, 1574 },
};

        public static int[][] AbdomenSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193 },
////Imp
new int[] { 51, 1483, 1484, 1485, 1486, 1487 },
////Blade Bane
new int[] { 37, 1558, 1559, 1560, 1561, 1562 },
////Acid Bane
new int[] { 1493, 1494, 1495, 1496, 1497, 1498 },
////Bludge Bane
new int[] { 1511, 1512, 1513, 1514, 1515, 1516 },
////Frost Bane
new int[] { 1523, 1524, 1525, 1526, 1527, 1528 },
////Lightning Bane
new int[] { 1535, 1536, 1537, 1538, 1539, 1540 },
////Flame Bane
new int[] { 1547, 1548, 1549, 1550, 1551, 1552 },
////Pierce Bane
new int[] { 1569, 1570, 1571, 1572, 1573, 1574 },
};

        public static int[][] UpperLegSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402 },
////Jumping
new int[] { 970, 971, 972, 973, 974, 975 },
////Sprint
new int[] { 982, 983, 984, 985, 986, 986 },
////Imp
new int[] { 51, 1483, 1484, 1485, 1486, 1487 },
////Blade Bane
new int[] { 37, 1558, 1559, 1560, 1561, 1562 },
////Acid Bane
new int[] { 1493, 1494, 1495, 1496, 1497, 1498 },
////Bludge Bane
new int[] { 1511, 1512, 1513, 1514, 1515, 1516 },
////Frost Bane
new int[] { 1523, 1524, 1525, 1526, 1527, 1528 },
////Lightning Bane
new int[] { 1535, 1536, 1537, 1538, 1539, 1540 },
////Flame Bane
new int[] { 1547, 1548, 1549, 1550, 1551, 1552 },
////Pierce Bane
new int[] { 1569, 1570, 1571, 1572, 1573, 1574 },
};

        public static int[][] LowerLegSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402 },
////Jumping
new int[] { 970, 971, 972, 973, 974, 975 },
////Sprint
new int[] { 982, 983, 984, 985, 986, 986 },
////Imp
new int[] { 51, 1483, 1484, 1485, 1486, 1487 },
////Blade Bane
new int[] { 37, 1558, 1559, 1560, 1561, 1562 },
////Acid Bane
new int[] { 1493, 1494, 1495, 1496, 1497, 1498 },
////Bludge Bane
new int[] { 1511, 1512, 1513, 1514, 1515, 1516 },
////Frost Bane
new int[] { 1523, 1524, 1525, 1526, 1527, 1528 },
////Lightning Bane
new int[] { 1535, 1536, 1537, 1538, 1539, 1540 },
////Flame Bane
new int[] { 1547, 1548, 1549, 1550, 1551, 1552 },
////Pierce Bane
new int[] { 1569, 1570, 1571, 1572, 1573, 1574 },
};

        public static int[][] FeetSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Coordination
new int[] { 1373, 1374, 1375, 1376, 1377, 1378 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402 },
////Two Handed
new int[] { 5099, 5100, 5101, 5102, 5103, 5104 },
////Finesse Weapon
new int[] { 322, 323, 324, 325, 326, 327 },
////Heavy Weapon
new int[] { 418, 419, 420, 421, 422, 423 },
////Light Weapon
new int[] { 298, 299, 300, 301, 302, 303 },
////Missile Weapon
new int[] { 467, 468, 469, 470, 471, 472 },
////Invuln
new int[] { 18, 245, 246, 247, 248, 249 },
////Impreg
new int[] { 256, 257, 258, 259, 260, 261 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279 },
////Arcane Enlight
new int[] { 678, 679, 680, 681, 682, 683 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658 },
////Healing Mastery
new int[] { 874, 875, 876, 877, 878, 879 },
////Jumping
new int[] { 970, 971, 972, 973, 974, 975 },
////Sprint
new int[] { 982, 983, 984, 985, 986, 986 },
////Imp
new int[] { 51, 1483, 1484, 1485, 1486, 1487 },
////Blade Bane
new int[] { 37, 1558, 1559, 1560, 1561, 1562 },
////Acid Bane
new int[] { 1493, 1494, 1495, 1496, 1497, 1498 },
////Bludge Bane
new int[] { 1511, 1512, 1513, 1514, 1515, 1516 },
////Frost Bane
new int[] { 1523, 1524, 1525, 1526, 1527, 1528 },
////Lightning Bane
new int[] { 1535, 1536, 1537, 1538, 1539, 1540 },
////Flame Bane
new int[] { 1547, 1548, 1549, 1550, 1551, 1552 },
////Pierce Bane
new int[] { 1569, 1570, 1571, 1572, 1573, 1574 },
};

        public static int[][] JewelrySpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Coordination
new int[] { 1373, 1374, 1375, 1376, 1377, 1378 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402 },
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426 },
////Willpower
new int[] { 1445, 1446, 1447, 1448, 1449, 1450 },
////Person Attunement
new int[] { 824, 825, 826, 827, 828, 829 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279 },
////Deception Mastery
new int[] { 850, 851, 852, 853, 854, 855 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658 },
////Salvaging
new int[] { 3499, 3500, 3501, 3502, 3503, 3504 },
////Armor Tinkering
new int[] { 702, 703, 704, 705, 706, 707 },
////Item Tinkering
new int[] { 726, 727, 728, 729, 730, 731 },
////Magic Item Tinkering
new int[] { 750, 751, 752, 753, 754, 755 },
////Weapon Tinkering
new int[] { 774, 775, 776, 777, 778, 779 },
////Monster Attunement
new int[] { 798, 799, 800, 801, 802, 803 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951 },
////Armor Self
new int[] { 24, 1308, 1309, 1310, 1311, 1312 },
////Acid Prot
new int[] { 515, 516, 517, 518, 519, 520 },
////Brudge Prot
new int[] { 1018, 1019, 1020, 1021, 1022, 1023 },
////Cold Prot
new int[] { 1030, 1031, 1032, 1033, 1034, 1035 },
////Lightning Prot
new int[] { 1066, 1067, 1068, 1069, 1070, 1071 },
////Fire Prot
new int[] { 20, 1090, 1091, 1092, 1093, 1094 },
////Blade Prot
new int[] { 1109, 1110, 1111, 1112, 1113, 1114 },
////Pierce Prot
new int[] { 1133, 1134, 1135, 1136, 1137, 1138 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193 },
////Mana Renewal
new int[] { 212, 213, 214, 215, 216, 217 },
};

        public static int[][] ClothingSpells =
        {
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426 },
////Willpower
new int[] { 1445, 1446, 1447, 1448, 1449, 1450 },
////Armor Self
new int[] { 24, 1308, 1309, 1310, 1311, 1312 },
////Acid Prot
new int[] { 515, 516, 517, 518, 519, 520 },
////Brudge Prot
new int[] { 1018, 1019, 1020, 1021, 1022, 1023 },
////Cold Prot
new int[] { 1030, 1031, 1032, 1033, 1034, 1035 },
////Lightning Prot
new int[] { 1066, 1067, 1068, 1069, 1070, 1071 },
////Fire Prot
new int[] { 20, 1090, 1091, 1092, 1093, 1094 },
////Blade Prot
new int[] { 1109, 1110, 1111, 1112, 1113, 1114 },
////Pierce Prot
new int[] { 1133, 1134, 1135, 1136, 1137, 1138 },
};

        public static int[][] ShieldSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Invuln
new int[] { 18, 245, 246, 247, 248, 249 },
////Impreg
new int[] { 256, 257, 258, 259, 260, 261 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279 },
////Shield
new int[] { 5843, 5844, 5845, 5846, 5847, 5848 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193 },
////Imp
new int[] { 51, 1483, 1484, 1485, 1486, 1487 },
////Blade Bane
new int[] { 37, 1558, 1559, 1560, 1561, 1562 },
////Acid Bane
new int[] { 1493, 1494, 1495, 1496, 1497, 1498 },
////Bludge Bane
new int[] { 1511, 1512, 1513, 1514, 1515, 1516 },
////Frost Bane
new int[] { 1523, 1524, 1525, 1526, 1527, 1528 },
////Lightning Bane
new int[] { 1535, 1536, 1537, 1538, 1539, 1540 },
////Flame Bane
new int[] { 1547, 1548, 1549, 1550, 1551, 1552 },
////Pierce Bane
new int[] { 1569, 1570, 1571, 1572, 1573, 1574 },
};
        public static int[][] GemSpells =
{
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Invuln
new int[] { 18, 245, 246, 247, 248, 249 },
////Impreg
new int[] { 256, 257, 258, 259, 260, 261 },
////Magic Resist
new int[] { 274, 275, 276, 277, 278, 279 },
////Light Weapon
new int[] { 298, 299, 300, 301, 302, 303 },
////Finesse Weapon
new int[] { 322, 323, 324, 325, 326, 327 },
////Two Handed
new int[] { 5099, 5100, 5101, 5102, 5103, 5104 },
////Heavy Weapon
new int[] { 418, 419, 420, 421, 422, 423 },
////Missile Weapon
new int[] { 467, 468, 469, 470, 471, 472 },
////Creature Enchant
new int[] { 557, 558, 559, 560, 561, 562 },
////Item Enchant
new int[] { 581, 582, 583, 584, 585, 586 },
////Life Magic
new int[] { 605, 606, 607, 608, 609, 610 },
////War Magic
new int[] { 629, 630, 631, 632, 633, 634 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658 },
////Arcane Enlight
new int[] { 678, 679, 680, 681, 682, 683 },
////Armor Tinkering
new int[] { 702, 703, 704, 705, 706, 707 },
////Item Tinkering
new int[] { 726, 727, 728, 729, 730, 731 },
////Magic Item Tinkering
new int[] { 750, 751, 752, 753, 754, 755 },
////Weapon Tinkering
new int[] { 774, 775, 776, 777, 778, 779 },
////Monster Attunement
new int[] { 798, 799, 800, 801, 802, 803 },
////Person Attunement
new int[] { 824, 825, 826, 827, 828, 829 },
////Deception Mastery
new int[] { 850, 851, 852, 853, 854, 855 },
////Healing Mastery
new int[] { 874, 875, 876, 877, 878, 879 },
////Leadership
new int[] { 898, 899, 900, 901, 902, 903 },
////Lockpick
new int[] { 922, 923, 924, 925, 926, 927 },
////Fealty
new int[] { 946, 947, 948, 949, 950, 951 },
////Jumping
new int[] { 970, 971, 972, 973, 974, 975 },
////Sprint
new int[] { 982, 983, 984, 985, 986, 986 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Coordination
new int[] { 1373, 1374, 1375, 1376, 1377, 1378 },
////Quickness
new int[] { 1397, 1398, 1399, 1400, 1401, 1402 },
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426 },
////Willpower
new int[] { 1445, 1446, 1447, 1448, 1449, 1450 },
////Cooking Mastery
new int[] { 1715, 1716, 1717, 1718, 1719, 1720 },
////Fletching
new int[] { 1739, 1740, 1741, 1742, 1743, 1744 },
////Alchemy
new int[] { 1763, 1764, 1765, 1766, 1767, 1768 },
////Dirty Fighting
new int[] { 5779, 5780, 5781, 5782, 5783, 5784 },
////Dual Wield
new int[] { 5803, 5804, 5805, 5806, 5807, 5808 },
////Recklessness
new int[] { 5827, 5828, 5829, 5830, 5831, 5832 },
////Shield
new int[] { 5843, 5844, 5845, 5846, 5847, 5848 },
////Sneak Attack
new int[] { 5867, 5868, 5869, 5870, 5871, 5872 },
////Summoning
new int[] { 6116, 6117, 6118, 6119, 6120, 6121 },
////Void Magic
new int[] { 5411, 5412, 5413, 5414, 5415, 5416 },
////Salvaging
new int[] { 3499, 3500, 3501, 3502, 3503, 3504 },
////Regeneration
new int[] { 165, 166, 167, 168, 169, 170 },
////Rejuvenation
new int[] { 54, 189, 190, 191, 192, 193 },
////Mana Renewal
new int[] { 212, 213, 214, 215, 216, 217 },
////Acid Prot
new int[] { 515, 516, 517, 518, 519, 520 },
////Brudge Prot
new int[] { 1018, 1019, 1020, 1021, 1022, 1023 },
////Cold Prot
new int[] { 1030, 1031, 1032, 1033, 1034, 1035 },
////Lightning Prot
new int[] { 1066, 1067, 1068, 1069, 1070, 1071 },
////Fire Prot
new int[] { 20, 1090, 1091, 1092, 1093, 1094 },
////Blade Prot
new int[] { 1109, 1110, 1111, 1112, 1113, 1114 },
////Pierce Prot
new int[] { 1133, 1134, 1135, 1136, 1137, 1138 },
////Armor Self
new int[] { 24, 1308, 1309, 1310, 1311, 1312 },
};

        public static int[][] WandSpells =
        {
////Focus
new int[] { 1421, 1422, 1423, 1424, 1425, 1426 },
////Willpower
new int[] { 1445, 1446, 1447, 1448, 1449, 1450 },
////Sneak Attack
new int[] { 5867, 5868, 5869, 5870, 5871, 5872 },
////Arcane Enlight
new int[] { 678, 679, 680, 681, 682, 683 },
////Mana C
new int[] { 653, 654, 655, 656, 657, 658 },
////Creature Enchant
new int[] { 557, 558, 559, 560, 561, 562 },
////Item Enchant
new int[] { 581, 582, 583, 584, 585, 586 },
////Life Magic
new int[] { 605, 606, 607, 608, 609, 610 },
////War Magic
new int[] { 629, 630, 631, 632, 633, 634 },
////Defender
new int[] { 1599, 1600, 1601, 1602, 1603, 1604 },
////Hermetic Link
new int[] { 1475, 1476, 1477, 1478, 1479, 1480 },
};

        public static int[][] MeleeSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Coordination
new int[] { 1373, 1374, 1375, 1376, 1377, 1378 },
////Dirty Fighting
new int[] { 5779, 5780, 5781, 5782, 5783, 5784 },
////Dual Wield
new int[] { 5803, 5804, 5805, 5806, 5807, 5808 },
////Recklessness
new int[] { 5827, 5828, 5829, 5830, 5831, 5832 },
////Sneak Attack
new int[] { 5867, 5868, 5869, 5870, 5871, 5872 },
////Heart Seeker
new int[] { 1587, 1588, 1589, 1590, 1591, 1592 },
////Defender
new int[] { 1599, 1600, 1601, 1602, 1603, 1604 },
////SwiftKiller
new int[] { 49, 1623, 1624, 1625, 1626, 1627 },
////Blooddrinker
new int[] { 35, 1612, 1613, 1614, 1615, 1616 },
};

        public static int[][] MissileSpells =
        {
////Strength
new int[] { 2, 1328, 1329, 1330, 1331, 1332 },
////Endurance
new int[] { 1349, 1350, 1351, 1352, 1353, 1354 },
////Coordination
new int[] { 1373, 1374, 1375, 1376, 1377, 1378 },
////Dirty Fighting
new int[] { 5779, 5780, 5781, 5782, 5783, 5784 },
////Recklessness
new int[] { 5827, 5828, 5829, 5830, 5831, 5832 },
////Sneak Attack
new int[] { 5867, 5868, 5869, 5870, 5871, 5872 },
////Heart Seeker
new int[] { 1587, 1588, 1589, 1590, 1591, 1592 },
////Defender
new int[] { 1599, 1600, 1601, 1602, 1603, 1604 },
////SwiftKiller
new int[] { 49, 1623, 1624, 1625, 1626, 1627 },
////Blooddrinker
new int[] { 35, 1612, 1613, 1614, 1615, 1616 },
};
    }
}
