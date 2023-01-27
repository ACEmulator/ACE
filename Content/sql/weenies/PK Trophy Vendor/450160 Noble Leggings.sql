DELETE FROM `weenie` WHERE `class_Id` = 450160;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450160, 'leggingsnobletailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450160,   1,          2) /* ItemType - Armor */
     , (450160,   3,         21) /* PaletteTemplate - Gold */
     , (450160,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (450160,   5,       0) /* EncumbranceVal */
     , (450160,   8,       1150) /* Mass */
     , (450160,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (450160,  16,          1) /* ItemUseable - No */
     , (450160,  19,       20) /* Value */
     , (450160,  27,          2) /* ArmorType - Leather */
     , (450160,  28,        0) /* ArmorLevel */
     , (450160,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450160,  22, True ) /* Inscribable */
     , (450160, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450160,   5,  -0.017) /* ManaRate */
     , (450160,  12,    0.66) /* Shade */
     , (450160,  13,     1.2) /* ArmorModVsSlash */
     , (450160,  14,     1.2) /* ArmorModVsPierce */
     , (450160,  15,     1.4) /* ArmorModVsBludgeon */
     , (450160,  16,     1.4) /* ArmorModVsCold */
     , (450160,  17,       1) /* ArmorModVsFire */
     , (450160,  18,     0.8) /* ArmorModVsAcid */
     , (450160,  19,     0.8) /* ArmorModVsElectric */
     , (450160, 110,       1) /* BulkMod */
     , (450160, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450160,   1, 'Noble Leggings') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450160,   1, 0x020001A8) /* Setup */
     , (450160,   3, 0x20000014) /* SoundTable */
     , (450160,   6, 0x0400007E) /* PaletteBase */
     , (450160,   7, 0x1000058E) /* ClothingBase */
     , (450160,   8, 0x06002DE3) /* Icon */
     , (450160,  22, 0x3400002B) /* PhysicsEffectTable */;

