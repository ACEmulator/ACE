DELETE FROM `weenie` WHERE `class_Id` = 450548;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450548, 'dresssiraluuntimbertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450548,   1,          4) /* ItemType - Clothing */
     , (450548,   3,          8) /* PaletteTemplate - Green */
     , (450548,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450548,   5,       0) /* EncumbranceVal */
     , (450548,   8,        150) /* Mass */
     , (450548,   9,      512) /* ValidLocations - Armor */
     , (450548,  16,          1) /* ItemUseable - No */
     , (450548,  18,          1) /* UiEffects - Magical */
     , (450548,  19,       20) /* Value */
     , (450548,  27,          1) /* ArmorType - Cloth */
     , (450548,  28,        0) /* ArmorLevel */
     , (450548,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450548, 150,        103) /* HookPlacement - Hook */
     , (450548, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450548,  22, True ) /* Inscribable */
     , (450548,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450548,   5,   -0.03) /* ManaRate */
     , (450548,  12,       1) /* Shade */
     , (450548,  13,     1.4) /* ArmorModVsSlash */
     , (450548,  14,     1.4) /* ArmorModVsPierce */
     , (450548,  15,       1) /* ArmorModVsBludgeon */
     , (450548,  16,       1) /* ArmorModVsCold */
     , (450548,  17,       1) /* ArmorModVsFire */
     , (450548,  18,       1) /* ArmorModVsAcid */
     , (450548,  19,     1.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450548,   1, 'Timber Siraluun Dress') /* Name */
     , (450548,  16, 'A formal gown woven from the plumes of a Timber Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450548,   1, 0x020001A6) /* Setup */
     , (450548,   3, 0x20000014) /* SoundTable */
     , (450548,   6, 0x0400007E) /* PaletteBase */
     , (450548,   7, 0x1000030C) /* ClothingBase */
     , (450548,   8, 0x060036A9) /* Icon */
     , (450548,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450548,  36, 0x0E000016) /* MutateFilter */;


