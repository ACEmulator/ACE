DELETE FROM `weenie` WHERE `class_Id` = 450159;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450159, 'gauntletsnobletailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450159,   1,          2) /* ItemType - Armor */
     , (450159,   3,         21) /* PaletteTemplate - Gold */
     , (450159,   4,      32768) /* ClothingPriority - Hands */
     , (450159,   5,        0) /* EncumbranceVal */
     , (450159,   8,        150) /* Mass */
     , (450159,   9,         32) /* ValidLocations - HandWear */
     , (450159,  16,          1) /* ItemUseable - No */
     , (450159,  19,       20) /* Value */
     , (450159,  27,          2) /* ArmorType - Leather */
     , (450159,  28,        0) /* ArmorLevel */
     , (450159,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450159,  22, True ) /* Inscribable */
     , (450159,  69, False) /* IsSellable */
     , (450159, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450159,   5,  -0.017) /* ManaRate */
     , (450159,  12,    0.66) /* Shade */
     , (450159,  13,     1.2) /* ArmorModVsSlash */
     , (450159,  14,     1.2) /* ArmorModVsPierce */
     , (450159,  15,     1.4) /* ArmorModVsBludgeon */
     , (450159,  16,     1.4) /* ArmorModVsCold */
     , (450159,  17,       1) /* ArmorModVsFire */
     , (450159,  18,     0.8) /* ArmorModVsAcid */
     , (450159,  19,     0.8) /* ArmorModVsElectric */
     , (450159, 110,       1) /* BulkMod */
     , (450159, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450159,   1, 'Noble Gauntlets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450159,   1, 0x020000D8) /* Setup */
     , (450159,   3, 0x20000014) /* SoundTable */
     , (450159,   6, 0x0400007E) /* PaletteBase */
     , (450159,   7, 0x1000058B) /* ClothingBase */
     , (450159,   8, 0x06002B2D) /* Icon */
     , (450159,  22, 0x3400002B) /* PhysicsEffectTable */;


