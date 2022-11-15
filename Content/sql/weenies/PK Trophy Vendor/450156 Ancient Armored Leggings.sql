DELETE FROM `weenie` WHERE `class_Id` = 450156;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450156, 'leggingkiviklir3tailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450156,   1,          2) /* ItemType - Armor */
     , (450156,   3,         14) /* PaletteTemplate - Red */
     , (450156,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (450156,   5,        0) /* EncumbranceVal */
     , (450156,   8,       1350) /* Mass */
     , (450156,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (450156,  16,          1) /* ItemUseable - No */
     , (450156,  19,      20) /* Value */
     , (450156,  27,         32) /* ArmorType - Metal */
     , (450156,  28,        0) /* ArmorLevel */
     , (450156,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450156,  22, True ) /* Inscribable */
     , (450156,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450156,   5,  -0.033) /* ManaRate */
     , (450156,  12,    0.66) /* Shade */
     , (450156,  13,     1.3) /* ArmorModVsSlash */
     , (450156,  14,     0.8) /* ArmorModVsPierce */
     , (450156,  15,     1.3) /* ArmorModVsBludgeon */
     , (450156,  16,       1) /* ArmorModVsCold */
     , (450156,  17,       1) /* ArmorModVsFire */
     , (450156,  18,     1.1) /* ArmorModVsAcid */
     , (450156,  19,     0.5) /* ArmorModVsElectric */
     , (450156, 110,       1) /* BulkMod */
     , (450156, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450156,   1, 'Ancient Armored Leggings') /* Name */
     , (450156,  15, 'These armored leggings appear to have been an ornamental piece. Obviously this is only one part of a complete suit of armor.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450156,   1, 0x020001A8) /* Setup */
     , (450156,   3, 0x20000014) /* SoundTable */
     , (450156,   6, 0x0400007E) /* PaletteBase */
     , (450156,   7, 0x1000057B) /* ClothingBase */
     , (450156,   8, 0x0600358C) /* Icon */
     , (450156,  22, 0x3400002B) /* PhysicsEffectTable */;

