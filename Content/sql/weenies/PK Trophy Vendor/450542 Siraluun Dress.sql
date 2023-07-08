DELETE FROM `weenie` WHERE `class_Id` = 450542;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450542, 'dresssiraluun-xptailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450542,   1,          4) /* ItemType - Clothing */
     , (450542,   3,         14) /* PaletteTemplate - Red */
     , (450542,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450542,   5,       0) /* EncumbranceVal */
     , (450542,   8,        150) /* Mass */
     , (450542,   9,      512) /* ValidLocations - Armor */
     , (450542,  16,          1) /* ItemUseable - No */
     , (450542,  18,          1) /* UiEffects - Magical */
     , (450542,  19,       20) /* Value */
     , (450542,  27,          1) /* ArmorType - Cloth */
     , (450542,  28,         0) /* ArmorLevel */
     , (450542,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450542, 150,        103) /* HookPlacement - Hook */
     , (450542, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450542,  22, True ) /* Inscribable */
     , (450542,  23, True ) /* DestroyOnSell */
     , (450542,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450542,   5,   -0.03) /* ManaRate */
     , (450542,  12,       1) /* Shade */
     , (450542,  13,     1.3) /* ArmorModVsSlash */
     , (450542,  14,     1.4) /* ArmorModVsPierce */
     , (450542,  15,     1.1) /* ArmorModVsBludgeon */
     , (450542,  16,       1) /* ArmorModVsCold */
     , (450542,  17,       1) /* ArmorModVsFire */
     , (450542,  18,       1) /* ArmorModVsAcid */
     , (450542,  19,     1.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450542,   1, 'Siraluun Dress') /* Name */
     , (450542,  15, 'A formal gown woven from the plumes of a Kithless Siraluun.') /* ShortDesc */
     , (450542,  16, 'A formal gown woven from the plumes of a Kithless Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450542,   1, 0x020001A6) /* Setup */
     , (450542,   3, 0x20000014) /* SoundTable */
     , (450542,   6, 0x0400007E) /* PaletteBase */
     , (450542,   7, 0x1000030C) /* ClothingBase */
     , (450542,   8, 0x060021FE) /* Icon */
     , (450542,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450542,  36, 0x0E000016) /* MutateFilter */;

