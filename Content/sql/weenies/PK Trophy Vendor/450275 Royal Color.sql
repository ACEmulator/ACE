DELETE FROM `weenie` WHERE `class_Id` = 450275;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450275, 'tattooroyalfavorhightailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450275,   1,          2) /* ItemType - Armor */
     , (450275,   3,         17) /* PaletteTemplate - Yellow */
     , (450275,   4,      12288) /* ClothingPriority - OuterwearUpperArms, OuterwearLowerArms */
     , (450275,   5,        0) /* EncumbranceVal */
     , (450275,   8,        180) /* Mass */
     , (450275,   9,       6144) /* ValidLocations - UpperArmArmor, LowerArmArmor */
     , (450275,  16,          1) /* ItemUseable - No */
     , (450275,  19,       20) /* Value */
     , (450275,  27,          2) /* ArmorType - Leather */
     , (450275,  28,        0) /* ArmorLevel */
     , (450275,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450275,  22, True ) /* Inscribable */
     , (450275,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450275,   5,  -0.033) /* ManaRate */
     , (450275,  12,    0.66) /* Shade */
     , (450275,  13,       1) /* ArmorModVsSlash */
     , (450275,  14,     1.2) /* ArmorModVsPierce */
     , (450275,  15,       1) /* ArmorModVsBludgeon */
     , (450275,  16,     0.9) /* ArmorModVsCold */
     , (450275,  17,     0.7) /* ArmorModVsFire */
     , (450275,  18,     0.8) /* ArmorModVsAcid */
     , (450275,  19,     0.6) /* ArmorModVsElectric */
     , (450275, 110,       1) /* BulkMod */
     , (450275, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450275,   1, 'Royal Color') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450275,   1, 0x020000D1) /* Setup */
     , (450275,   3, 0x20000014) /* SoundTable */
     , (450275,   6, 0x0400007E) /* PaletteBase */
     , (450275,   7, 0x1000059B) /* ClothingBase */
     , (450275,   8, 0x060013FC) /* Icon */
     , (450275,  22, 0x3400002B) /* PhysicsEffectTable */;

