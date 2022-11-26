DELETE FROM `weenie` WHERE `class_Id` = 450545;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450545, 'dresssiraluunlittoraltailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450545,   1,          4) /* ItemType - Clothing */
     , (450545,   3,         17) /* PaletteTemplate - Yellow */
     , (450545,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450545,   5,        0) /* EncumbranceVal */
     , (450545,   8,        150) /* Mass */
     , (450545,   9,      512) /* ValidLocations - Armor */
     , (450545,  16,          1) /* ItemUseable - No */
     , (450545,  18,          1) /* UiEffects - Magical */
     , (450545,  19,       20) /* Value */
     , (450545,  27,          1) /* ArmorType - Cloth */
     , (450545,  28,        0) /* ArmorLevel */
     , (450545,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450545, 150,        103) /* HookPlacement - Hook */
     , (450545, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450545,  22, True ) /* Inscribable */
     , (450545,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450545,   5,   -0.03) /* ManaRate */
     , (450545,  12,       1) /* Shade */
     , (450545,  13,     1.4) /* ArmorModVsSlash */
     , (450545,  14,     1.4) /* ArmorModVsPierce */
     , (450545,  15,       1) /* ArmorModVsBludgeon */
     , (450545,  16,       1) /* ArmorModVsCold */
     , (450545,  17,       1) /* ArmorModVsFire */
     , (450545,  18,       1) /* ArmorModVsAcid */
     , (450545,  19,     1.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450545,   1, 'Littoral Siraluun Dress') /* Name */
     , (450545,  16, 'A formal gown woven from the plumes of a Littoral Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450545,   1, 0x020001A6) /* Setup */
     , (450545,   3, 0x20000014) /* SoundTable */
     , (450545,   6, 0x0400007E) /* PaletteBase */
     , (450545,   7, 0x1000030C) /* ClothingBase */
     , (450545,   8, 0x0600369F) /* Icon */
     , (450545,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450545,  36, 0x0E000016) /* MutateFilter */;

