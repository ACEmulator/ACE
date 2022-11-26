DELETE FROM `weenie` WHERE `class_Id` = 450547;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450547, 'dresssiraluuntidaltailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450547,   1,          4) /* ItemType - Clothing */
     , (450547,   3,          9) /* PaletteTemplate - Grey */
     , (450547,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450547,   5,        0) /* EncumbranceVal */
     , (450547,   8,        150) /* Mass */
     , (450547,   9,      512) /* ValidLocations - Armor */
     , (450547,  16,          1) /* ItemUseable - No */
     , (450547,  18,          1) /* UiEffects - Magical */
     , (450547,  19,       20) /* Value */
     , (450547,  27,          1) /* ArmorType - Cloth */
     , (450547,  28,         0) /* ArmorLevel */
     , (450547,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450547, 150,        103) /* HookPlacement - Hook */
     , (450547, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450547,  22, True ) /* Inscribable */
     , (450547,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450547,   5,   -0.03) /* ManaRate */
     , (450547,  12,       1) /* Shade */
     , (450547,  13,     1.4) /* ArmorModVsSlash */
     , (450547,  14,     1.4) /* ArmorModVsPierce */
     , (450547,  15,       1) /* ArmorModVsBludgeon */
     , (450547,  16,       1) /* ArmorModVsCold */
     , (450547,  17,       1) /* ArmorModVsFire */
     , (450547,  18,       1) /* ArmorModVsAcid */
     , (450547,  19,     1.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450547,   1, 'Tidal Siraluun Dress') /* Name */
     , (450547,  16, 'A formal gown woven from the plumes of a Tidal Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450547,   1, 0x020001A6) /* Setup */
     , (450547,   3, 0x20000014) /* SoundTable */
     , (450547,   6, 0x0400007E) /* PaletteBase */
     , (450547,   7, 0x1000030C) /* ClothingBase */
     , (450547,   8, 0x060036AA) /* Icon */
     , (450547,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450547,  36, 0x0E000016) /* MutateFilter */;

