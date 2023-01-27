DELETE FROM `weenie` WHERE `class_Id` = 450034;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450034, 'robenoblemeleetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450034,   1,          4) /* ItemType - Armor */
     , (450034,   3,         21) /* PaletteTemplate - Gold */
     , (450034,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450034,   5,        0) /* EncumbranceVal */
     , (450034,   8,        450) /* Mass */
     , (450034,   9,      512) /* ValidLocations - Armor */
     , (450034,  16,          1) /* ItemUseable - No */
     , (450034,  19,       20) /* Value */
     , (450034,  27,          1) /* ArmorType - Cloth */
     , (450034,  28,        0) /* ArmorLevel */
     , (450034,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450034,  22, True ) /* Inscribable */
     , (450034, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450034,   5,   -0.01) /* ManaRate */
     , (450034,  12,       1) /* Shade */
     , (450034,  13,     0.4) /* ArmorModVsSlash */
     , (450034,  14,     0.2) /* ArmorModVsPierce */
     , (450034,  15,     0.4) /* ArmorModVsBludgeon */
     , (450034,  16,     1.1) /* ArmorModVsCold */
     , (450034,  17,     0.4) /* ArmorModVsFire */
     , (450034,  18,     0.4) /* ArmorModVsAcid */
     , (450034,  19,     1.1) /* ArmorModVsElectric */
     , (450034, 110,       1) /* BulkMod */
     , (450034, 111,       1) /* SizeMod */
     , (450034, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450034,   1, 'Armsman''s Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450034,   1, 0x020001A6) /* Setup */
     , (450034,   3, 0x20000014) /* SoundTable */
     , (450034,   6, 0x0400007E) /* PaletteBase */
     , (450034,   7, 0x10000590) /* ClothingBase */
     , (450034,   8, 0x0600301D) /* Icon */
     , (450034,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450034,  36, 0x0E000016) /* MutateFilter */;

