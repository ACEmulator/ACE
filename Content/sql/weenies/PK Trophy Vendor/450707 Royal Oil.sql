DELETE FROM `weenie` WHERE `class_Id` = 450707;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450707, 'tattooroyalfavorubertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450707,   1,          2) /* ItemType - Armor */
     , (450707,   3,         14) /* PaletteTemplate - Red */
     , (450707,   4,      12288) /* ClothingPriority - OuterwearUpperArms, OuterwearLowerArms */
     , (450707,   5,        0) /* EncumbranceVal */
     , (450707,   8,        180) /* Mass */
     , (450707,   9,       6144) /* ValidLocations - UpperArmArmor, LowerArmArmor */
     , (450707,  16,          1) /* ItemUseable - No */
     , (450707,  19,       20) /* Value */
     , (450707,  27,          2) /* ArmorType - Leather */
     , (450707,  28,        0) /* ArmorLevel */
     , (450707,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450707,  22, True ) /* Inscribable */
     , (450707,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450707,   5,  -0.033) /* ManaRate */
     , (450707,  12,    0.66) /* Shade */
     , (450707,  13,       1) /* ArmorModVsSlash */
     , (450707,  14,     1.2) /* ArmorModVsPierce */
     , (450707,  15,       1) /* ArmorModVsBludgeon */
     , (450707,  16,     0.9) /* ArmorModVsCold */
     , (450707,  17,     0.7) /* ArmorModVsFire */
     , (450707,  18,     0.9) /* ArmorModVsAcid */
     , (450707,  19,     0.7) /* ArmorModVsElectric */
     , (450707, 110,       1) /* BulkMod */
     , (450707, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450707,   1, 'Royal Oil') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450707,   1, 0x020000D1) /* Setup */
     , (450707,   3, 0x20000014) /* SoundTable */
     , (450707,   6, 0x0400007E) /* PaletteBase */
     , (450707,   7, 0x1000059B) /* ClothingBase */
     , (450707,   8, 0x060013FC) /* Icon */
     , (450707,  22, 0x3400002B) /* PhysicsEffectTable */;


