DELETE FROM `weenie` WHERE `class_Id` = 450543;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450543, 'dresssiraluunbadlandstailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450543,   1,          4) /* ItemType - Clothing */
     , (450543,   3,         16) /* PaletteTemplate - Rose */
     , (450543,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450543,   5,       0) /* EncumbranceVal */
     , (450543,   8,        150) /* Mass */
     , (450543,   9,      512) /* ValidLocations - Armor */
     , (450543,  16,          1) /* ItemUseable - No */
     , (450543,  18,          1) /* UiEffects - Magical */
     , (450543,  19,       20) /* Value */
     , (450543,  27,          1) /* ArmorType - Cloth */
     , (450543,  28,        0) /* ArmorLevel */
     , (450543,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450543, 150,        103) /* HookPlacement - Hook */
     , (450543, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450543,  22, True ) /* Inscribable */
     , (450543,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450543,   5,   -0.03) /* ManaRate */
     , (450543,  12,       1) /* Shade */
     , (450543,  13,     1.4) /* ArmorModVsSlash */
     , (450543,  14,     1.4) /* ArmorModVsPierce */
     , (450543,  15,       1) /* ArmorModVsBludgeon */
     , (450543,  16,       1) /* ArmorModVsCold */
     , (450543,  17,       1) /* ArmorModVsFire */
     , (450543,  18,       1) /* ArmorModVsAcid */
     , (450543,  19,     1.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450543,   1, 'Badlands Siraluun Dress') /* Name */
     , (450543,  16, 'A formal gown woven from the plumes of a Badlands Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450543,   1, 0x020001A6) /* Setup */
     , (450543,   3, 0x20000014) /* SoundTable */
     , (450543,   6, 0x0400007E) /* PaletteBase */
     , (450543,   7, 0x1000030C) /* ClothingBase */
     , (450543,   8, 0x060036A0) /* Icon */
     , (450543,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450543,  36, 0x0E000016) /* MutateFilter */;

