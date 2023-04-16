DELETE FROM `weenie` WHERE `class_Id` = 480579;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480579, 'sleevesolthoifighterpk', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480579,   1,          2) /* ItemType - Armor */
     , (480579,   3,          2) /* PaletteTemplate - Blue */
     , (480579,   4,      12288) /* ClothingPriority - OuterwearUpperArms, OuterwearLowerArms */
     , (480579,   5,        0) /* EncumbranceVal */
     , (480579,   8,        360) /* Mass */
     , (480579,   9,       6144) /* ValidLocations - UpperArmArmor, LowerArmArmor */
     , (480579,  16,          1) /* ItemUseable - No */
     , (480579,  19,        20) /* Value */
     , (480579,  27,         16) /* ArmorType - Chainmail */
     , (480579,  28,         0) /* ArmorLevel */
     , (480579,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480579,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480579,  12,    0.66) /* Shade */
     , (480579,  13,     1.2) /* ArmorModVsSlash */
     , (480579,  14,       1) /* ArmorModVsPierce */
     , (480579,  15,     0.8) /* ArmorModVsBludgeon */
     , (480579,  16,     0.6) /* ArmorModVsCold */
     , (480579,  17,     0.6) /* ArmorModVsFire */
     , (480579,  18,     0.5) /* ArmorModVsAcid */
     , (480579,  19,     0.4) /* ArmorModVsElectric */
     , (480579, 110,    1.33) /* BulkMod */
     , (480579, 111,    1.75) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480579,   1, 'Olthoi Fighter Sleeves') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480579,   1, 0x020000DF) /* Setup */
     , (480579,   3, 0x20000014) /* SoundTable */
     , (480579,   6, 0x0400007E) /* PaletteBase */
     , (480579,   7, 0x1000047F) /* ClothingBase */
     , (480579,   8, 0x06001582) /* Icon */
     , (480579,  22, 0x3400002B) /* PhysicsEffectTable */;
