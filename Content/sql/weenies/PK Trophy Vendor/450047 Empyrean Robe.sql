DELETE FROM `weenie` WHERE `class_Id` = 450047;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450047, 'robeempyreantailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450047,   1,          4) /* ItemType - Clothing */
     , (450047,   3,         22) /* PaletteTemplate - Aqua */
     , (450047,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450047,   5,        0) /* EncumbranceVal */
     , (450047,   8,        150) /* Mass */
     , (450047,   9,      512) /* ValidLocations - Armor */
     , (450047,  16,          1) /* ItemUseable - No */
     , (450047,  19,         20) /* Value */
     , (450047,  27,          1) /* ArmorType - Cloth */
     , (450047,  28,          0) /* ArmorLevel */
     , (450047,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450047,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450047,  12,     0.5) /* Shade */
     , (450047,  13,     0.8) /* ArmorModVsSlash */
     , (450047,  14,     0.8) /* ArmorModVsPierce */
     , (450047,  15,       1) /* ArmorModVsBludgeon */
     , (450047,  16,     0.2) /* ArmorModVsCold */
     , (450047,  17,     0.2) /* ArmorModVsFire */
     , (450047,  18,     0.1) /* ArmorModVsAcid */
     , (450047,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450047,   1, 'Empyrean Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450047,   1, 0x020001A6) /* Setup */
     , (450047,   3, 0x20000014) /* SoundTable */
     , (450047,   6, 0x0400007E) /* PaletteBase */
     , (450047,   7, 0x10000402) /* ClothingBase */
     , (450047,   8, 0x06001B8D) /* Icon */
     , (450047,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450047,  36, 0x0E000016) /* MutateFilter */;
