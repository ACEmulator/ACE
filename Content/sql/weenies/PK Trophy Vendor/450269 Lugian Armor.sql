DELETE FROM `weenie` WHERE `class_Id` = 450269;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450269, 'hauberklugiantailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450269,   1,          2) /* ItemType - Armor */
     , (450269,   3,          2) /* PaletteTemplate - Blue */
     , (450269,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450269,   5,       0) /* EncumbranceVal */
     , (450269,   8,       1100) /* Mass */
     , (450269,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450269,  16,          1) /* ItemUseable - No */
     , (450269,  19,       20) /* Value */
     , (450269,  27,         32) /* ArmorType - Metal */
     , (450269,  28,        0) /* ArmorLevel */
     , (450269,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450269,  22, True ) /* Inscribable */
     , (450269,  23, True ) /* DestroyOnSell */
     , (450269, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450269,  12,    0.66) /* Shade */
     , (450269,  13,     1.3) /* ArmorModVsSlash */
     , (450269,  14,       1) /* ArmorModVsPierce */
     , (450269,  15,       1) /* ArmorModVsBludgeon */
     , (450269,  16,     0.7) /* ArmorModVsCold */
     , (450269,  17,     0.7) /* ArmorModVsFire */
     , (450269,  18,     0.5) /* ArmorModVsAcid */
     , (450269,  19,     0.3) /* ArmorModVsElectric */
     , (450269, 110,       1) /* BulkMod */
     , (450269, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450269,   1, 'Lugian Armor') /* Name */
     , (450269,  15, 'A chestplate, Lugian sized.') /* ShortDesc */
     , (450269,  16, 'A chestplate with a scuffed seal on the chest.  The armor is brutally simplistic, and sturdily crafted.') /* LongDesc */
     , (450269,  33, 'HauberkLugian') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450269,   1, 0x020000D4) /* Setup */
     , (450269,   3, 0x20000014) /* SoundTable */
     , (450269,   6, 0x0400007E) /* PaletteBase */
     , (450269,   7, 0x100002C8) /* ClothingBase */
     , (450269,   8, 0x0600200C) /* Icon */
     , (450269,  22, 0x3400002B) /* PhysicsEffectTable */;
