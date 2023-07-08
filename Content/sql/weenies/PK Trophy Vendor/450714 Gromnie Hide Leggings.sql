DELETE FROM `weenie` WHERE `class_Id` = 450714;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450714, 'pantsgromniehidetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450714,   1,          2) /* ItemType - Armor */
     , (450714,   3,          8) /* PaletteTemplate - Green */
     , (450714,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (450714,   5,        0) /* EncumbranceVal */
     , (450714,   8,       1275) /* Mass */
     , (450714,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (450714,  16,          1) /* ItemUseable - No */
     , (450714,  19,       20) /* Value */
     , (450714,  27,          2) /* ArmorType - Leather */
     , (450714,  28,        0) /* ArmorLevel */
     , (450714,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450714,  22, True ) /* Inscribable */
     , (450714, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450714,  12,     0.5) /* Shade */
     , (450714,  13,       1) /* ArmorModVsSlash */
     , (450714,  14,       1) /* ArmorModVsPierce */
     , (450714,  15,       1) /* ArmorModVsBludgeon */
     , (450714,  16,     0.6) /* ArmorModVsCold */
     , (450714,  17,     0.8) /* ArmorModVsFire */
     , (450714,  18,     0.8) /* ArmorModVsAcid */
     , (450714,  19,     1.4) /* ArmorModVsElectric */
     , (450714, 110,     1.1) /* BulkMod */
     , (450714, 111,     1.5) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450714,   1, 'Gromnie Hide Leggings') /* Name */
     , (450714,  16, 'A pair of leggings crafted from the hide of a jade gromnie.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450714,   1, 0x020001A8) /* Setup */
     , (450714,   3, 0x20000014) /* SoundTable */
     , (450714,   6, 0x0400007E) /* PaletteBase */
     , (450714,   7, 0x10000571) /* ClothingBase */
     , (450714,   8, 0x06001BEB) /* Icon */
     , (450714,  22, 0x3400002B) /* PhysicsEffectTable */;
