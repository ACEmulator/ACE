DELETE FROM `weenie` WHERE `class_Id` = 450721;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450721, 'gauntletsgromniehidetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450721,   1,          2) /* ItemType - Armor */
     , (450721,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450721,   4,      32768) /* ClothingPriority - Hands */
     , (450721,   5,        0) /* EncumbranceVal */
     , (450721,   8,         90) /* Mass */
     , (450721,   9,         32) /* ValidLocations - HandWear */
     , (450721,  16,          1) /* ItemUseable - No */
     , (450721,  19,        20) /* Value */
     , (450721,  27,          2) /* ArmorType - Leather */
     , (450721,  28,        0) /* ArmorLevel */
     , (450721,  44,          0) /* Damage */
     , (450721,  45,          4) /* DamageType - Bludgeon */
     , (450721,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450721,  22, True ) /* Inscribable */
     , (450721, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450721,  12,    0.66) /* Shade */
     , (450721,  13,       1) /* ArmorModVsSlash */
     , (450721,  14,       1) /* ArmorModVsPierce */
     , (450721,  15,       1) /* ArmorModVsBludgeon */
     , (450721,  16,     0.6) /* ArmorModVsCold */
     , (450721,  17,     0.8) /* ArmorModVsFire */
     , (450721,  18,     1.4) /* ArmorModVsAcid */
     , (450721,  19,     0.8) /* ArmorModVsElectric */
     , (450721,  22,    0.75) /* DamageVariance */
     , (450721, 110,    1.67) /* BulkMod */
     , (450721, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450721,   1, 'Gromnie Hide Gauntlets') /* Name */
     , (450721,  16, 'A pair of gauntlets crafted from the hide of an azure gromnie.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450721,   1, 0x020000D8) /* Setup */
     , (450721,   3, 0x20000014) /* SoundTable */
     , (450721,   6, 0x0400007E) /* PaletteBase */
     , (450721,   7, 0x1000056F) /* ClothingBase */
     , (450721,   8, 0x06000FCC) /* Icon */
     , (450721,  22, 0x3400002B) /* PhysicsEffectTable */;
