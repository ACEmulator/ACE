DELETE FROM `weenie` WHERE `class_Id` = 450549;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450549, 'dresssiraluununtamedtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450549,   1,          4) /* ItemType - Clothing */
     , (450549,   3,          2) /* PaletteTemplate - Blue */
     , (450549,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450549,   5,       0) /* EncumbranceVal */
     , (450549,   8,        150) /* Mass */
     , (450549,   9,      512) /* ValidLocations - Armor */
     , (450549,  16,          1) /* ItemUseable - No */
     , (450549,  18,          1) /* UiEffects - Magical */
     , (450549,  19,       20) /* Value */
     , (450549,  27,          1) /* ArmorType - Cloth */
     , (450549,  28,        0) /* ArmorLevel */
     , (450549,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450549, 150,        103) /* HookPlacement - Hook */
     , (450549, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450549,  22, True ) /* Inscribable */
     , (450549,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450549,   5,   -0.03) /* ManaRate */
     , (450549,  12,       1) /* Shade */
     , (450549,  13,     1.4) /* ArmorModVsSlash */
     , (450549,  14,     1.4) /* ArmorModVsPierce */
     , (450549,  15,       1) /* ArmorModVsBludgeon */
     , (450549,  16,       1) /* ArmorModVsCold */
     , (450549,  17,       1) /* ArmorModVsFire */
     , (450549,  18,       1) /* ArmorModVsAcid */
     , (450549,  19,     1.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450549,   1, 'Untamed Siraluun Dress') /* Name */
     , (450549,  16, 'A formal gown woven from the plumes of an Untamed Siraluun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450549,   1, 0x020001A6) /* Setup */
     , (450549,   3, 0x20000014) /* SoundTable */
     , (450549,   6, 0x0400007E) /* PaletteBase */
     , (450549,   7, 0x1000030C) /* ClothingBase */
     , (450549,   8, 0x060036A8) /* Icon */
     , (450549,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450549,  36, 0x0E000016) /* MutateFilter */;

