DELETE FROM `weenie` WHERE `class_Id` = 450042;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450042, 'robeplaguefangtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450042,   1,         4) /* ItemType - Armor */
     , (450042,   3,         14) /* PaletteTemplate - Red */
     , (450042,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450042,   5,       0) /* EncumbranceVal */
     , (450042,   8,        340) /* Mass */
     , (450042,   9,      512) /* ValidLocations - Armor */
     , (450042,  16,          1) /* ItemUseable - No */
     , (450042,  19,      20) /* Value */
     , (450042,  27,          1) /* ArmorType - Cloth */
     , (450042,  28,        0) /* ArmorLevel */
     , (450042,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450042,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450042,   5,  -0.033) /* ManaRate */
     , (450042,  12,       1) /* Shade */
     , (450042,  13,    0.25) /* ArmorModVsSlash */
     , (450042,  14,    0.75) /* ArmorModVsPierce */
     , (450042,  15,     0.6) /* ArmorModVsBludgeon */
     , (450042,  16,    0.25) /* ArmorModVsCold */
     , (450042,  17,    0.65) /* ArmorModVsFire */
     , (450042,  18,    0.75) /* ArmorModVsAcid */
     , (450042,  19,    0.75) /* ArmorModVsElectric */
     , (450042, 110,       1) /* BulkMod */
     , (450042, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450042,   1, 'Plaguefang''s Robe') /* Name */
     , (450042,  15, 'A robe crafted from the hide of the vile doomshark, Plaguefang.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450042,   1, 0x020001A6) /* Setup */
     , (450042,   3, 0x20000014) /* SoundTable */
     , (450042,   6, 0x0400007E) /* PaletteBase */
     , (450042,   7, 0x10000513) /* ClothingBase */
     , (450042,   8, 0x0600301D) /* Icon */
     , (450042,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450042,  36, 0x0E000016) /* MutateFilter */;


