DELETE FROM `weenie` WHERE `class_Id` = 450518;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450518, 'helmskeletaltailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450518,   1,          2) /* ItemType - Armor */
     , (450518,   3,         14) /* PaletteTemplate - Red */
     , (450518,   4,      16384) /* ClothingPriority - Head */
     , (450518,   5,        0) /* EncumbranceVal */
     , (450518,   8,         75) /* Mass */
     , (450518,   9,          1) /* ValidLocations - HeadWear */
     , (450518,  16,          1) /* ItemUseable - No */
     , (450518,  19,       20) /* Value */
     , (450518,  27,          2) /* ArmorType - Leather */
     , (450518,  28,        0) /* ArmorLevel */
     , (450518,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450518,  22, True ) /* Inscribable */
     , (450518,  23, False) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450518,  12,    0.66) /* Shade */
     , (450518,  13,     1.8) /* ArmorModVsSlash */
     , (450518,  14,     1.5) /* ArmorModVsPierce */
     , (450518,  15,     0.6) /* ArmorModVsBludgeon */
     , (450518,  16,     1.5) /* ArmorModVsCold */
     , (450518,  17,     0.8) /* ArmorModVsFire */
     , (450518,  18,     0.6) /* ArmorModVsAcid */
     , (450518,  19,     1.5) /* ArmorModVsElectric */
     , (450518, 110,       1) /* BulkMod */
     , (450518, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450518,   1, 'Skeletal Helm') /* Name */
     , (450518,  16, 'A skull, treated and properly cleaned, crafted to defend one''s head from deadly blows.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450518,   1, 0x02000FDE) /* Setup */
     , (450518,   3, 0x20000014) /* SoundTable */
     , (450518,   6, 0x0400007E) /* PaletteBase */
     , (450518,   7, 0x100004CF) /* ClothingBase */
     , (450518,   8, 0x06002D88) /* Icon */
     , (450518,  22, 0x3400002B) /* PhysicsEffectTable */;
