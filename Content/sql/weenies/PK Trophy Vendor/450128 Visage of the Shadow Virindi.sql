DELETE FROM `weenie` WHERE `class_Id` = 450128;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450128, 'ace450128-visageoftheshadowvirinditailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450128,   1,          2) /* ItemType - Armor */
     , (450128,   3,         39) /* PaletteTemplate - Black */
     , (450128,   4,      16384) /* ClothingPriority - Head */
     , (450128,   5,        0) /* EncumbranceVal */
     , (450128,   9,          1) /* ValidLocations - HeadWear */
     , (450128,  16,          1) /* ItemUseable - No */
     , (450128,  18,          1) /* UiEffects - Magical */
     , (450128,  19,       20) /* Value */
     , (450128,  28,        0) /* ArmorLevel */
     , (450128,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450128, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450128,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450128,   5,   -0.05) /* ManaRate */
     , (450128,  12,       0) /* Shade */
     , (450128,  13,     0.6) /* ArmorModVsSlash */
     , (450128,  14,     0.6) /* ArmorModVsPierce */
     , (450128,  15,     0.6) /* ArmorModVsBludgeon */
     , (450128,  16,     1.5) /* ArmorModVsCold */
     , (450128,  17,     0.6) /* ArmorModVsFire */
     , (450128,  18,     1.5) /* ArmorModVsAcid */
     , (450128,  19,     1.5) /* ArmorModVsElectric */
     , (450128, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450128,   1, 'Visage of the Shadow Virindi') /* Name */
     , (450128,  16, 'This mask was fashioned for its wielder from the defeated essence of Aerbax left within Claude the Archmage. While it lacks in physical form, it radiates magical power beyond most articles of clothing or armor you have encountered.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450128,   1, 0x0200174C) /* Setup */
     , (450128,   3, 0x20000014) /* SoundTable */
     , (450128,   6, 0x0400007E) /* PaletteBase */
     , (450128,   7, 0x100006E8) /* ClothingBase */
     , (450128,   8, 0x060066FC) /* Icon */
     , (450128,  22, 0x3400002B) /* PhysicsEffectTable */;


