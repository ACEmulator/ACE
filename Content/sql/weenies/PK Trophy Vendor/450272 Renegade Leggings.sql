DELETE FROM `weenie` WHERE `class_Id` = 450272;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450272, 'leggingslugianrenegadetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450272,   1,          2) /* ItemType - Armor */
     , (450272,   3,         14) /* PaletteTemplate - Red */
     , (450272,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450272,   5,       0) /* EncumbranceVal */
     , (450272,   8,       1100) /* Mass */
     , (450272,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450272,  16,          1) /* ItemUseable - No */
     , (450272,  19,       20) /* Value */
     , (450272,  27,         32) /* ArmorType - Metal */
     , (450272,  28,        0) /* ArmorLevel */
     , (450272,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450272,  22, True ) /* Inscribable */
     , (450272,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450272,  12,    0.66) /* Shade */
     , (450272,  13,       1) /* ArmorModVsSlash */
     , (450272,  14,       1) /* ArmorModVsPierce */
     , (450272,  15,       1) /* ArmorModVsBludgeon */
     , (450272,  16,    0.75) /* ArmorModVsCold */
     , (450272,  17,    0.75) /* ArmorModVsFire */
     , (450272,  18,     0.8) /* ArmorModVsAcid */
     , (450272,  19,     1.3) /* ArmorModVsElectric */
     , (450272, 110,       1) /* BulkMod */
     , (450272, 111,       2) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450272,   1, 'Renegade Leggings') /* Name */
     , (450272,  33, 'RenegadeLeggingsPickedUp') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450272,   1, 0x020001A8) /* Setup */
     , (450272,   3, 0x20000014) /* SoundTable */
     , (450272,   6, 0x0400007E) /* PaletteBase */
     , (450272,   7, 0x10000556) /* ClothingBase */
     , (450272,   8, 0x06003352) /* Icon */
     , (450272,  22, 0x3400002B) /* PhysicsEffectTable */;
