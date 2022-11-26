DELETE FROM `weenie` WHERE `class_Id` = 450544;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450544, 'dresssiraluunkithlesstailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450544,   1,          4) /* ItemType - Clothing */
     , (450544,   3,         14) /* PaletteTemplate - Red */
     , (450544,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450544,   5,       0) /* EncumbranceVal */
     , (450544,   8,        150) /* Mass */
     , (450544,   9,      512) /* ValidLocations - Armor */
     , (450544,  16,          1) /* ItemUseable - No */
     , (450544,  18,          1) /* UiEffects - Magical */
     , (450544,  19,       20) /* Value */
     , (450544,  27,          1) /* ArmorType - Cloth */
     , (450544,  28,        0) /* ArmorLevel */
     , (450544,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450544, 150,        103) /* HookPlacement - Hook */
     , (450544, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450544,  22, True ) /* Inscribable */
     , (450544,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450544,   5,   -0.03) /* ManaRate */
     , (450544,  12,       1) /* Shade */
     , (450544,  13,     1.4) /* ArmorModVsSlash */
     , (450544,  14,     1.4) /* ArmorModVsPierce */
     , (450544,  15,       1) /* ArmorModVsBludgeon */
     , (450544,  16,       1) /* ArmorModVsCold */
     , (450544,  17,       1) /* ArmorModVsFire */
     , (450544,  18,       1) /* ArmorModVsAcid */
     , (450544,  19,     1.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450544,   1, 'Kithless Siraluun Dress') /* Name */
     , (450544,  16, 'A formal gown woven from the plumes of a Kithless Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450544,   1, 0x020001A6) /* Setup */
     , (450544,   3, 0x20000014) /* SoundTable */
     , (450544,   6, 0x0400007E) /* PaletteBase */
     , (450544,   7, 0x1000030C) /* ClothingBase */
     , (450544,   8, 0x060021FE) /* Icon */
     , (450544,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450544,  36, 0x0E000016) /* MutateFilter */;


