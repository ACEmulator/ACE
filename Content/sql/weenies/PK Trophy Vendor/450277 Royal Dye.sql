DELETE FROM `weenie` WHERE `class_Id` = 450277;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450277, 'tattooroyalfavormidtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450277,   1,          2) /* ItemType - Armor */
     , (450277,   3,          8) /* PaletteTemplate - Green */
     , (450277,   4,      12288) /* ClothingPriority - OuterwearUpperArms, OuterwearLowerArms */
     , (450277,   5,        0) /* EncumbranceVal */
     , (450277,   8,        180) /* Mass */
     , (450277,   9,       6144) /* ValidLocations - UpperArmArmor, LowerArmArmor */
     , (450277,  16,          1) /* ItemUseable - No */
     , (450277,  19,       20) /* Value */
     , (450277,  27,          2) /* ArmorType - Leather */
     , (450277,  28,        0) /* ArmorLevel */
     , (450277,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450277,  22, True ) /* Inscribable */
     , (450277,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450277,   5,  -0.033) /* ManaRate */
     , (450277,  12,    0.66) /* Shade */
     , (450277,  13,       1) /* ArmorModVsSlash */
     , (450277,  14,     1.2) /* ArmorModVsPierce */
     , (450277,  15,       1) /* ArmorModVsBludgeon */
     , (450277,  16,     0.8) /* ArmorModVsCold */
     , (450277,  17,     0.6) /* ArmorModVsFire */
     , (450277,  18,     0.9) /* ArmorModVsAcid */
     , (450277,  19,     0.7) /* ArmorModVsElectric */
     , (450277, 110,       1) /* BulkMod */
     , (450277, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450277,   1, 'Royal Dye') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450277,   1, 0x020000D1) /* Setup */
     , (450277,   3, 0x20000014) /* SoundTable */
     , (450277,   6, 0x0400007E) /* PaletteBase */
     , (450277,   7, 0x1000059B) /* ClothingBase */
     , (450277,   8, 0x060013FC) /* Icon */
     , (450277,  22, 0x3400002B) /* PhysicsEffectTable */;


