DELETE FROM `weenie` WHERE `class_Id` = 450035;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450035, 'robenoblemagictailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450035,   1,          4) /* ItemType - Armor */
     , (450035,   3,         21) /* PaletteTemplate - Gold */
     , (450035,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450035,   5,        0) /* EncumbranceVal */
     , (450035,   8,        450) /* Mass */
     , (450035,   9,      512) /* ValidLocations - Armor */
     , (450035,  16,          1) /* ItemUseable - No */
     , (450035,  19,       20) /* Value */
     , (450035,  27,          1) /* ArmorType - Cloth */
     , (450035,  28,        0) /* ArmorLevel */
     , (450035,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450035,  22, True ) /* Inscribable */
     , (450035, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450035,   5,   -0.01) /* ManaRate */
     , (450035,  12,       1) /* Shade */
     , (450035,  13,     0.4) /* ArmorModVsSlash */
     , (450035,  14,     0.2) /* ArmorModVsPierce */
     , (450035,  15,     0.4) /* ArmorModVsBludgeon */
     , (450035,  16,     1.1) /* ArmorModVsCold */
     , (450035,  17,     0.4) /* ArmorModVsFire */
     , (450035,  18,     0.4) /* ArmorModVsAcid */
     , (450035,  19,     1.1) /* ArmorModVsElectric */
     , (450035, 110,       1) /* BulkMod */
     , (450035, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450035,   1, 'Enscorcelled Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450035,   1, 0x020001A6) /* Setup */
     , (450035,   3, 0x20000014) /* SoundTable */
     , (450035,   6, 0x0400007E) /* PaletteBase */
     , (450035,   7, 0x10000592) /* ClothingBase */
     , (450035,   8, 0x0600301D) /* Icon */
     , (450035,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450035,  36, 0x0E000016) /* MutateFilter */;


