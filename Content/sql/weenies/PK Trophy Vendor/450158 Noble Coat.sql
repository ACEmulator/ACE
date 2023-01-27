DELETE FROM `weenie` WHERE `class_Id` = 450158;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450158, 'coatnobletailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450158,   1,          2) /* ItemType - Armor */
     , (450158,   3,         21) /* PaletteTemplate - Gold */
     , (450158,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450158,   5,       0) /* EncumbranceVal */
     , (450158,   8,       1250) /* Mass */
     , (450158,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450158,  16,          1) /* ItemUseable - No */
     , (450158,  19,       20) /* Value */
     , (450158,  27,          2) /* ArmorType - Leather */
     , (450158,  28,        0) /* ArmorLevel */
     , (450158,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450158,  22, True ) /* Inscribable */
     , (450158, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450158,   5,  -0.017) /* ManaRate */
     , (450158,  12,    0.66) /* Shade */
     , (450158,  13,     1.2) /* ArmorModVsSlash */
     , (450158,  14,     1.2) /* ArmorModVsPierce */
     , (450158,  15,     1.4) /* ArmorModVsBludgeon */
     , (450158,  16,     1.4) /* ArmorModVsCold */
     , (450158,  17,       1) /* ArmorModVsFire */
     , (450158,  18,     0.8) /* ArmorModVsAcid */
     , (450158,  19,     0.8) /* ArmorModVsElectric */
     , (450158, 110,       1) /* BulkMod */
     , (450158, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450158,   1, 'Noble Coat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450158,   1, 0x020000D2) /* Setup */
     , (450158,   3, 0x20000014) /* SoundTable */
     , (450158,   6, 0x0400007E) /* PaletteBase */
     , (450158,   7, 0x1000058D) /* ClothingBase */
     , (450158,   8, 0x06002DE2) /* Icon */
     , (450158,  22, 0x3400002B) /* PhysicsEffectTable */;

