DELETE FROM `weenie` WHERE `class_Id` = 450711;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450711, 'tattooswamplordtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450711,   1,          2) /* ItemType - Armor */
     , (450711,   3,         17) /* PaletteTemplate - Yellow */
     , (450711,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450711,   5,        0) /* EncumbranceVal */
     , (450711,   8,        910) /* Mass */
     , (450711,   9,      31232) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor, UpperLegArmor, LowerLegArmor */
     , (450711,  16,          1) /* ItemUseable - No */
     , (450711,  19,       20) /* Value */
     , (450711,  27,         16) /* ArmorType - Chainmail */
     , (450711,  28,        0) /* ArmorLevel */
     , (450711,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450711,  22, True ) /* Inscribable */
     , (450711,  23, True ) /* DestroyOnSell */
     , (450711, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450711,   5,  -0.033) /* ManaRate */
     , (450711,  12,    0.66) /* Shade */
     , (450711,  13,       1) /* ArmorModVsSlash */
     , (450711,  14,       1) /* ArmorModVsPierce */
     , (450711,  15,       1) /* ArmorModVsBludgeon */
     , (450711,  16,     0.4) /* ArmorModVsCold */
     , (450711,  17,     0.6) /* ArmorModVsFire */
     , (450711,  18,     0.6) /* ArmorModVsAcid */
     , (450711,  19,     0.4) /* ArmorModVsElectric */
     , (450711, 110,    1.33) /* BulkMod */
     , (450711, 111,     4.5) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450711,   1, 'Swamp Lord''s War Paint') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450711,   1, 0x0200090F) /* Setup */
     , (450711,   6, 0x0400007E) /* PaletteBase */
     , (450711,   7, 0x10000560) /* ClothingBase */
     , (450711,   8, 0x060033F7) /* Icon */
     , (450711,  22, 0x3400002B) /* PhysicsEffectTable */;

