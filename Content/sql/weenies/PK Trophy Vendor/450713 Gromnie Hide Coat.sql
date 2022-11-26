DELETE FROM `weenie` WHERE `class_Id` = 450713;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450713, 'coatgromniehidetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450713,   1,          2) /* ItemType - Armor */
     , (450713,   3,         18) /* PaletteTemplate - YellowBrown */
     , (450713,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450713,   5,        0) /* EncumbranceVal */
     , (450713,   8,       1000) /* Mass */
     , (450713,   9,       6656) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450713,  16,          1) /* ItemUseable - No */
     , (450713,  19,       20) /* Value */
     , (450713,  27,          8) /* ArmorType - Scalemail */
     , (450713,  28,        0) /* ArmorLevel */
     , (450713,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450713,  22, True ) /* Inscribable */
     , (450713, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450713,  12,    0.66) /* Shade */
     , (450713,  13,       1) /* ArmorModVsSlash */
     , (450713,  14,       1) /* ArmorModVsPierce */
     , (450713,  15,       1) /* ArmorModVsBludgeon */
     , (450713,  16,     0.6) /* ArmorModVsCold */
     , (450713,  17,     0.8) /* ArmorModVsFire */
     , (450713,  18,     1.4) /* ArmorModVsAcid */
     , (450713,  19,     0.6) /* ArmorModVsElectric */
     , (450713, 110,     1.1) /* BulkMod */
     , (450713, 111,     1.5) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450713,   1, 'Gromnie Hide Coat') /* Name */
     , (450713,  16, 'A coat crafted from the hide of a swamp gromnie.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450713,   1, 0x020001A6) /* Setup */
     , (450713,   3, 0x20000014) /* SoundTable */
     , (450713,   6, 0x0400007E) /* PaletteBase */
     , (450713,   7, 0x10000573) /* ClothingBase */
     , (450713,   8, 0x06001BE3) /* Icon */
     , (450713,  22, 0x3400002B) /* PhysicsEffectTable */;
