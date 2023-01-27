DELETE FROM `weenie` WHERE `class_Id` = 450276;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450276, 'tattooroyalfavorlowtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450276,   1,          2) /* ItemType - Armor */
     , (450276,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450276,   4,      12288) /* ClothingPriority - OuterwearUpperArms, OuterwearLowerArms */
     , (450276,   5,        0) /* EncumbranceVal */
     , (450276,   8,        180) /* Mass */
     , (450276,   9,       6144) /* ValidLocations - UpperArmArmor, LowerArmArmor */
     , (450276,  16,          1) /* ItemUseable - No */
     , (450276,  19,       20) /* Value */
     , (450276,  27,          2) /* ArmorType - Leather */
     , (450276,  28,        0) /* ArmorLevel */
     , (450276,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450276,  22, True ) /* Inscribable */
     , (450276,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450276,   5,  -0.033) /* ManaRate */
     , (450276,  12,    0.66) /* Shade */
     , (450276,  13,       1) /* ArmorModVsSlash */
     , (450276,  14,     1.2) /* ArmorModVsPierce */
     , (450276,  15,       1) /* ArmorModVsBludgeon */
     , (450276,  16,     0.8) /* ArmorModVsCold */
     , (450276,  17,     0.6) /* ArmorModVsFire */
     , (450276,  18,     0.8) /* ArmorModVsAcid */
     , (450276,  19,     0.6) /* ArmorModVsElectric */
     , (450276, 110,       1) /* BulkMod */
     , (450276, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450276,   1, 'Royal Paint') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450276,   1, 0x020000D1) /* Setup */
     , (450276,   3, 0x20000014) /* SoundTable */
     , (450276,   6, 0x0400007E) /* PaletteBase */
     , (450276,   7, 0x1000059B) /* ClothingBase */
     , (450276,   8, 0x060013FC) /* Icon */
     , (450276,  22, 0x3400002B) /* PhysicsEffectTable */;

