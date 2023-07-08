DELETE FROM `weenie` WHERE `class_Id` = 450720;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450720, 'gauntletsopaltailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450720,   1,          2) /* ItemType - Armor */
     , (450720,   3,          2) /* PaletteTemplate - Blue */
     , (450720,   4,      32768) /* ClothingPriority - Hands */
     , (450720,   5,        0) /* EncumbranceVal */
     , (450720,   8,        460) /* Mass */
     , (450720,   9,         32) /* ValidLocations - HandWear */
     , (450720,  16,          1) /* ItemUseable - No */
     , (450720,  19,       20) /* Value */
     , (450720,  27,         32) /* ArmorType - Metal */
     , (450720,  28,        0) /* ArmorLevel */
     , (450720,  44,          0) /* Damage */
     , (450720,  45,          4) /* DamageType - Bludgeon */
     , (450720,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450720,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450720,   5,  -0.017) /* ManaRate */
     , (450720,  12,    0.66) /* Shade */
     , (450720,  13,     0.8) /* ArmorModVsSlash */
     , (450720,  14,     0.8) /* ArmorModVsPierce */
     , (450720,  15,     0.8) /* ArmorModVsBludgeon */
     , (450720,  16,     1.2) /* ArmorModVsCold */
     , (450720,  17,       1) /* ArmorModVsFire */
     , (450720,  18,       1) /* ArmorModVsAcid */
     , (450720,  19,     1.2) /* ArmorModVsElectric */
     , (450720,  22,    0.75) /* DamageVariance */
     , (450720, 110,       1) /* BulkMod */
     , (450720, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450720,   1, 'Opal Gauntlets') /* Name */
     , (450720,  16, 'These gauntlets have been carved from Opal to fit the human hand. They are mystical in nature.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450720,   1, 0x020000D8) /* Setup */
     , (450720,   3, 0x20000014) /* SoundTable */
     , (450720,   6, 0x0400007E) /* PaletteBase */
     , (450720,   7, 0x1000049B) /* ClothingBase */
     , (450720,   8, 0x06002B28) /* Icon */
     , (450720,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450720,  36, 0x0E000016) /* MutateFilter */;

