DELETE FROM `weenie` WHERE `class_Id` = 450043;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450043, 'robereedsharkslashertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450043,   1,          4) /* ItemType - Armor */
     , (450043,   3,         14) /* PaletteTemplate - Red */
     , (450043,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450043,   5,        0) /* EncumbranceVal */
     , (450043,   8,        340) /* Mass */
     , (450043,   9,      512) /* ValidLocations - Armor */
     , (450043,  16,          1) /* ItemUseable - No */
     , (450043,  19,       20) /* Value */
     , (450043,  27,          1) /* ArmorType - Cloth */
     , (450043,  28,        20) /* ArmorLevel */
     , (450043,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450043, 150,        103) /* HookPlacement - Hook */
     , (450043, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450043,  22, True ) /* Inscribable */
     , (450043, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450043,  12,       1) /* Shade */
     , (450043,  13,    0.25) /* ArmorModVsSlash */
     , (450043,  14,    0.75) /* ArmorModVsPierce */
     , (450043,  15,     0.6) /* ArmorModVsBludgeon */
     , (450043,  16,    0.25) /* ArmorModVsCold */
     , (450043,  17,    0.65) /* ArmorModVsFire */
     , (450043,  18,    0.75) /* ArmorModVsAcid */
     , (450043,  19,    0.75) /* ArmorModVsElectric */
     , (450043, 110,       1) /* BulkMod */
     , (450043, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450043,   1, 'Sturdy Reedshark Robe') /* Name */
     , (450043,  15, 'A robe crafted from the leathery hide of a Reedshark Slasher. The hide has been treated and crafted into a fairly useful robe.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450043,   1, 0x020001A6) /* Setup */
     , (450043,   3, 0x20000014) /* SoundTable */
     , (450043,   6, 0x0400007E) /* PaletteBase */
     , (450043,   7, 0x100004D7) /* ClothingBase */
     , (450043,   8, 0x06002DE1) /* Icon */
     , (450043,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450043,  36, 0x0E000016) /* MutateFilter */;
