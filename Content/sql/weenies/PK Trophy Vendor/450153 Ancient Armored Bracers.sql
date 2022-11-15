DELETE FROM `weenie` WHERE `class_Id` = 450153;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450153, 'bracerfalatacot3tailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450153,   1,          2) /* ItemType - Armor */
     , (450153,   3,         85) /* PaletteTemplate - DyeDarkRed */
     , (450153,   4,       8192) /* ClothingPriority - OuterwearLowerArms */
     , (450153,   5,        0) /* EncumbranceVal */
     , (450153,   8,       1000) /* Mass */
     , (450153,   9,       4096) /* ValidLocations - LowerArmArmor */
     , (450153,  16,          1) /* ItemUseable - No */
     , (450153,  19,      20) /* Value */
     , (450153,  27,          8) /* ArmorType - Scalemail */
     , (450153,  28,        0) /* ArmorLevel */
     , (450153,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450153,  22, True ) /* Inscribable */
     , (450153,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450153,   5,  -0.033) /* ManaRate */
     , (450153,  12,    0.66) /* Shade */
     , (450153,  13,     1.3) /* ArmorModVsSlash */
     , (450153,  14,     0.8) /* ArmorModVsPierce */
     , (450153,  15,     1.3) /* ArmorModVsBludgeon */
     , (450153,  16,       1) /* ArmorModVsCold */
     , (450153,  17,       1) /* ArmorModVsFire */
     , (450153,  18,     1.1) /* ArmorModVsAcid */
     , (450153,  19,     0.5) /* ArmorModVsElectric */
     , (450153, 110,     1.2) /* BulkMod */
     , (450153, 111,       4) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450153,   1, 'Ancient Armored Bracers') /* Name */
     , (450153,  15, 'This armored bracer appears to have been an ornamental piece. Obviously this is only one part of a complete suit of armor.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450153,   1, 0x020000D1) /* Setup */
     , (450153,   3, 0x20000014) /* SoundTable */
     , (450153,   6, 0x0400007E) /* PaletteBase */
     , (450153,   7, 0x1000053C) /* ClothingBase */
     , (450153,   8, 0x0600314E) /* Icon */
     , (450153,  22, 0x3400002B) /* PhysicsEffectTable */;
