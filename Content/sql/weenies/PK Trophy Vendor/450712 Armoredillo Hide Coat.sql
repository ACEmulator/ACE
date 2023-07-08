DELETE FROM `weenie` WHERE `class_Id` = 450712;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450712, 'coatarmoredillohidetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450712,   1,          2) /* ItemType - Armor */
     , (450712,   3,          4) /* PaletteTemplate - Brown */
     , (450712,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450712,   5,        0) /* EncumbranceVal */
     , (450712,   8,        270) /* Mass */
     , (450712,   9,       7680) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450712,  16,          1) /* ItemUseable - No */
     , (450712,  19,       20) /* Value */
     , (450712,  27,          2) /* ArmorType - Leather */
     , (450712,  28,         0) /* ArmorLevel */
     , (450712,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450712,  22, True ) /* Inscribable */
     , (450712, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450712,  12,    0.66) /* Shade */
     , (450712,  13,     1.2) /* ArmorModVsSlash */
     , (450712,  14,     0.7) /* ArmorModVsPierce */
     , (450712,  15,     1.4) /* ArmorModVsBludgeon */
     , (450712,  16,     0.8) /* ArmorModVsCold */
     , (450712,  17,       2) /* ArmorModVsFire */
     , (450712,  18,       1) /* ArmorModVsAcid */
     , (450712,  19,     0.8) /* ArmorModVsElectric */
     , (450712, 110,       1) /* BulkMod */
     , (450712, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450712,   1, 'Armoredillo Hide Coat') /* Name */
     , (450712,  15, 'Coat crafted from the hide of an Armoredillo.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450712,   1, 0x020000D4) /* Setup */
     , (450712,   3, 0x20000014) /* SoundTable */
     , (450712,   6, 0x0400007E) /* PaletteBase */
     , (450712,   7, 0x100004D1) /* ClothingBase */
     , (450712,   8, 0x06002DC0) /* Icon */
     , (450712,  22, 0x3400002B) /* PhysicsEffectTable */;
