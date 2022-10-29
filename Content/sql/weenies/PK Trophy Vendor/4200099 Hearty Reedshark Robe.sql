DELETE FROM `weenie` WHERE `class_Id` = 4200099;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200099, 'robereedsharkreapertailorr', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200099,   1,          2) /* ItemType - Armor */
     , (4200099,   3,          8) /* PaletteTemplate - Green */
     , (4200099,   4,       1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (4200099,   5,          1) /* EncumbranceVal */
     , (4200099,   8,        340) /* Mass */
     , (4200099,   9,        512) /* ValidLocations - Armor */
     , (4200099,  16,          1) /* ItemUseable - No */
     , (4200099,  19,         20) /* Value */
     , (4200099,  27,          1) /* ArmorType - Cloth */
     , (4200099,  28,          1) /* ArmorLevel */
     , (4200099,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200099, 150,        103) /* HookPlacement - Hook */
     , (4200099, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200099,  22, True ) /* Inscribable */
     , (4200099, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200099,  12,       0) /* Shade */
     , (4200099,  13,    0.25) /* ArmorModVsSlash */
     , (4200099,  14,    0.75) /* ArmorModVsPierce */
     , (4200099,  15,     0.6) /* ArmorModVsBludgeon */
     , (4200099,  16,    0.25) /* ArmorModVsCold */
     , (4200099,  17,    0.65) /* ArmorModVsFire */
     , (4200099,  18,    0.75) /* ArmorModVsAcid */
     , (4200099,  19,    0.75) /* ArmorModVsElectric */
     , (4200099, 110,       1) /* BulkMod */
     , (4200099, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200099,   1, 'Hearty Reedshark Robe') /* Name */
     , (4200099,  15, 'A robe crafted from the leathery hide of a Reedshark Reaper. The hide has been treated and crafted into a fairly useful robe.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200099,   1, 0x020001A6) /* Setup */
     , (4200099,   3, 0x20000014) /* SoundTable */
     , (4200099,   6, 0x0400007E) /* PaletteBase */
     , (4200099,   7, 0x100004D6) /* ClothingBase */
     , (4200099,   8, 0x06002DD5) /* Icon */
     , (4200099,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200099,  36, 0x0E000016) /* MutateFilter */;
