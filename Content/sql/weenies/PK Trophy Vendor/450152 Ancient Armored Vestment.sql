DELETE FROM `weenie` WHERE `class_Id` = 450152;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450152, 'shirtfalatacot3tailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450152,   1,          2) /* ItemType - Armor */
     , (450152,   3,         85) /* PaletteTemplate - DyeDarkRed */
     , (450152,   4,       1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms */
     , (450152,   5,        0) /* EncumbranceVal */
     , (450152,   8,       1000) /* Mass */
     , (450152,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor */
     , (450152,  16,          1) /* ItemUseable - No */
     , (450152,  19,      20) /* Value */
     , (450152,  27,          8) /* ArmorType - Scalemail */
     , (450152,  28,        0) /* ArmorLevel */
     , (450152,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450152,  22, True ) /* Inscribable */
     , (450152,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450152,   5,  -0.033) /* ManaRate */
     , (450152,  12,    0.66) /* Shade */
     , (450152,  13,     1.3) /* ArmorModVsSlash */
     , (450152,  14,     0.8) /* ArmorModVsPierce */
     , (450152,  15,     1.3) /* ArmorModVsBludgeon */
     , (450152,  16,       1) /* ArmorModVsCold */
     , (450152,  17,       1) /* ArmorModVsFire */
     , (450152,  18,     1.1) /* ArmorModVsAcid */
     , (450152,  19,     0.5) /* ArmorModVsElectric */
     , (450152, 110,     1.2) /* BulkMod */
     , (450152, 111,       4) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450152,   1, 'Ancient Armored Vestment') /* Name */
     , (450152,  15, 'This armored vestment appears to have been an ornamental piece. Obviously this is only one part of a complete suit of armor.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450152,   1, 0x020000D2) /* Setup */
     , (450152,   3, 0x20000014) /* SoundTable */
     , (450152,   6, 0x0400007E) /* PaletteBase */
     , (450152,   7, 0x10000536) /* ClothingBase */
     , (450152,   8, 0x060030BB) /* Icon */
     , (450152,  22, 0x3400002B) /* PhysicsEffectTable */;
