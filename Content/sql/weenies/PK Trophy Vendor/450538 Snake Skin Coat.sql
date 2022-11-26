DELETE FROM `weenie` WHERE `class_Id` = 450538;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450538, 'coatsclavustailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450538,   1,          2) /* ItemType - Armor */
     , (450538,   3,          8) /* PaletteTemplate - Green */
     , (450538,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450538,   5,       1000) /* EncumbranceVal */
     , (450538,   8,        500) /* Mass */
     , (450538,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450538,  16,          1) /* ItemUseable - No */
     , (450538,  19,       20) /* Value */
     , (450538,  27,          8) /* ArmorType - Scalemail */
     , (450538,  28,        0) /* ArmorLevel */
     , (450538,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450538,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450538,  12,    0.66) /* Shade */
     , (450538,  13,     1.4) /* ArmorModVsSlash */
     , (450538,  14,       1) /* ArmorModVsPierce */
     , (450538,  15,     0.6) /* ArmorModVsBludgeon */
     , (450538,  16,     1.4) /* ArmorModVsCold */
     , (450538,  17,     0.8) /* ArmorModVsFire */
     , (450538,  18,     0.4) /* ArmorModVsAcid */
     , (450538,  19,     0.4) /* ArmorModVsElectric */
     , (450538, 110,       1) /* BulkMod */
     , (450538, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450538,   1, 'Snake Skin Coat') /* Name */
     , (450538,  16, 'A coat made out of the hide of a sclavus.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450538,   1, 0x020000D4) /* Setup */
     , (450538,   3, 0x20000014) /* SoundTable */
     , (450538,   6, 0x0400007E) /* PaletteBase */
     , (450538,   7, 0x100002B1) /* ClothingBase */
     , (450538,   8, 0x06001BE3) /* Icon */
     , (450538,  22, 0x3400002B) /* PhysicsEffectTable */;
