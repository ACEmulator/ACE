DELETE FROM `weenie` WHERE `class_Id` = 450271;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450271, 'hauberklugianrenegadetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450271,   1,          2) /* ItemType - Armor */
     , (450271,   3,         14) /* PaletteTemplate - Red */
     , (450271,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450271,   5,       0) /* EncumbranceVal */
     , (450271,   8,       1100) /* Mass */
     , (450271,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450271,  16,          1) /* ItemUseable - No */
     , (450271,  19,       20) /* Value */
     , (450271,  27,         32) /* ArmorType - Metal */
     , (450271,  28,        0) /* ArmorLevel */
     , (450271,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450271,  22, True ) /* Inscribable */
     , (450271,  69, False) /* IsSellable */
     , (450271, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450271,  12,    0.66) /* Shade */
     , (450271,  13,       1) /* ArmorModVsSlash */
     , (450271,  14,       1) /* ArmorModVsPierce */
     , (450271,  15,       1) /* ArmorModVsBludgeon */
     , (450271,  16,    0.75) /* ArmorModVsCold */
     , (450271,  17,    0.75) /* ArmorModVsFire */
     , (450271,  18,     0.8) /* ArmorModVsAcid */
     , (450271,  19,     1.3) /* ArmorModVsElectric */
     , (450271, 110,       1) /* BulkMod */
     , (450271, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450271,   1, 'Renegade Hauberk') /* Name */
     , (450271,  16, 'A chestplate worn by Lugian Renegades.  The armor is brutally simplistic, and sturdily crafted.') /* LongDesc */
     , (450271,  33, 'RenegadeHauberkPickedUp') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450271,   1, 0x020000D4) /* Setup */
     , (450271,   3, 0x20000014) /* SoundTable */
     , (450271,   6, 0x0400007E) /* PaletteBase */
     , (450271,   7, 0x100002C8) /* ClothingBase */
     , (450271,   8, 0x06003351) /* Icon */
     , (450271,  22, 0x3400002B) /* PhysicsEffectTable */;
