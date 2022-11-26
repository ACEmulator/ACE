DELETE FROM `weenie` WHERE `class_Id` = 450734;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450734, 'bootswalkingtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450734,   1,          2) /* ItemType - Armor */
     , (450734,   3,         20) /* PaletteTemplate - Silver */
     , (450734,   4,      65536) /* ClothingPriority - Feet */
     , (450734,   5,        0) /* EncumbranceVal */
     , (450734,   8,        360) /* Mass */
     , (450734,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450734,  16,          1) /* ItemUseable - No */
     , (450734,  18,          1) /* UiEffects - Magical */
     , (450734,  19,      20) /* Value */
     , (450734,  27,          4) /* ArmorType - StuddedLeather */
     , (450734,  28,        0) /* ArmorLevel */
     , (450734,  44,         13) /* Damage */
     , (450734,  45,          4) /* DamageType - Bludgeon */
     , (450734,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450734, 150,        103) /* HookPlacement - Hook */
     , (450734, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450734,  22, True ) /* Inscribable */
     , (450734, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450734,   5,   -0.05) /* ManaRate */
     , (450734,  12,    0.66) /* Shade */
     , (450734,  13,     1.5) /* ArmorModVsSlash */
     , (450734,  14,     1.5) /* ArmorModVsPierce */
     , (450734,  15,     1.5) /* ArmorModVsBludgeon */
     , (450734,  16,       1) /* ArmorModVsCold */
     , (450734,  17,       1) /* ArmorModVsFire */
     , (450734,  18,       1) /* ArmorModVsAcid */
     , (450734,  19,       1) /* ArmorModVsElectric */
     , (450734,  22,    0.75) /* DamageVariance */
     , (450734, 110,       1) /* BulkMod */
     , (450734, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450734,   1, 'Walking Boots') /* Name */
     , (450734,  16, 'These boots were made for walking. They can also be dyed.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450734,   1, 0x02000FA8) /* Setup */
     , (450734,   3, 0x20000014) /* SoundTable */
     , (450734,   6, 0x0400007E) /* PaletteBase */
     , (450734,   7, 0x100004C2) /* ClothingBase */
     , (450734,   8, 0x06002D05) /* Icon */
     , (450734,  22, 0x3400002B) /* PhysicsEffectTable */;

