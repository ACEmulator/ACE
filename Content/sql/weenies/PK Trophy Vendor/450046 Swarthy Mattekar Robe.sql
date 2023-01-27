DELETE FROM `weenie` WHERE `class_Id` = 450046;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450046, 'robeswarthymattekartailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450046,   1,          4) /* ItemType - Armor */
     , (450046,   3,         39) /* PaletteTemplate - Black */
     , (450046,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450046,   5,       0) /* EncumbranceVal */
     , (450046,   8,        340) /* Mass */
     , (450046,   9,      512) /* ValidLocations - Armor */
     , (450046,  16,          1) /* ItemUseable - No */
     , (450046,  19,       20) /* Value */
     , (450046,  27,          1) /* ArmorType - Cloth */
     , (450046,  28,        0) /* ArmorLevel */
     , (450046,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450046, 150,        103) /* HookPlacement - Hook */
     , (450046, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450046,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450046,  12,       0) /* Shade */
     , (450046,  13,     0.9) /* ArmorModVsSlash */
     , (450046,  14,     0.9) /* ArmorModVsPierce */
     , (450046,  15,     0.9) /* ArmorModVsBludgeon */
     , (450046,  16,     0.4) /* ArmorModVsCold */
     , (450046,  17,       2) /* ArmorModVsFire */
     , (450046,  18,       1) /* ArmorModVsAcid */
     , (450046,  19,       2) /* ArmorModVsElectric */
     , (450046, 110,       1) /* BulkMod */
     , (450046, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450046,   1, 'Swarthy Mattekar Robe') /* Name */
     , (450046,  15, 'Rare, lightweight, extremely warm robe crafted from the hide of the vile Swarthy Mattekar, rumored only to appear under certain conditions.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450046,   1, 0x020001A6) /* Setup */
     , (450046,   3, 0x20000014) /* SoundTable */
     , (450046,   6, 0x0400007E) /* PaletteBase */
     , (450046,   7, 0x10000315) /* ClothingBase */
     , (450046,   8, 0x06000FD7) /* Icon */
     , (450046,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450046,  36, 0x0E000016) /* MutateFilter */;
