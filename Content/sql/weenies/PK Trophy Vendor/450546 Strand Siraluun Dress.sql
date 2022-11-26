DELETE FROM `weenie` WHERE `class_Id` = 450546;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450546, 'dresssiraluunstrandtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450546,   1,          4) /* ItemType - Clothing */
     , (450546,   3,         10) /* PaletteTemplate - LightBlue */
     , (450546,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450546,   5,       0) /* EncumbranceVal */
     , (450546,   8,        150) /* Mass */
     , (450546,   9,      512) /* ValidLocations - Armor */
     , (450546,  16,          1) /* ItemUseable - No */
     , (450546,  18,          1) /* UiEffects - Magical */
     , (450546,  19,       20) /* Value */
     , (450546,  27,          1) /* ArmorType - Cloth */
     , (450546,  28,        0) /* ArmorLevel */
     , (450546,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450546, 150,        103) /* HookPlacement - Hook */
     , (450546, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450546,  22, True ) /* Inscribable */
     , (450546,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450546,   5,   -0.03) /* ManaRate */
     , (450546,  12,       1) /* Shade */
     , (450546,  13,     1.4) /* ArmorModVsSlash */
     , (450546,  14,     1.4) /* ArmorModVsPierce */
     , (450546,  15,       1) /* ArmorModVsBludgeon */
     , (450546,  16,       1) /* ArmorModVsCold */
     , (450546,  17,       1) /* ArmorModVsFire */
     , (450546,  18,       1) /* ArmorModVsAcid */
     , (450546,  19,     1.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450546,   1, 'Strand Siraluun Dress') /* Name */
     , (450546,  16, 'A formal gown woven from the plumes of a Strand Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450546,   1, 0x020001A6) /* Setup */
     , (450546,   3, 0x20000014) /* SoundTable */
     , (450546,   6, 0x0400007E) /* PaletteBase */
     , (450546,   7, 0x1000030C) /* ClothingBase */
     , (450546,   8, 0x060036AB) /* Icon */
     , (450546,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450546,  36, 0x0E000016) /* MutateFilter */;

