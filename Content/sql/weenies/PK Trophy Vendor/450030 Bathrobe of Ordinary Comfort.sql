DELETE FROM `weenie` WHERE `class_Id` = 450030;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450030, 'ace450030-bathrobeofordinarycomforttailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450030,   1,          4) /* ItemType - Clothing */
     , (450030,   3,         39) /* PaletteTemplate - Black */
     , (450030,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450030,   5,        0) /* EncumbranceVal */
     , (450030,   9,      512) /* ValidLocations - Armor */
     , (450030,  16,          1) /* ItemUseable - No */
     , (450030,  19,       20) /* Value */
     , (450030,  28,         0) /* ArmorLevel */
     , (450030,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450030, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450030,  11, True ) /* IgnoreCollisions */
     , (450030,  13, True ) /* Ethereal */
     , (450030,  14, True ) /* GravityStatus */
     , (450030,  19, True ) /* Attackable */
     , (450030,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450030,  12,       0) /* Shade */
     , (450030,  13,     0.8) /* ArmorModVsSlash */
     , (450030,  14,     0.5) /* ArmorModVsPierce */
     , (450030,  15,       1) /* ArmorModVsBludgeon */
     , (450030,  16,     1.5) /* ArmorModVsCold */
     , (450030,  17,       0) /* ArmorModVsFire */
     , (450030,  18,       0) /* ArmorModVsAcid */
     , (450030,  19,     0.3) /* ArmorModVsElectric */
     , (450030, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450030,   1, 'Bathrobe of Ordinary Comfort') /* Name */
     , (450030,  16, 'A plush and comfy bathrobe. A small label on the inside of the robe appears to have been removed.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450030,   1, 0x020001A6) /* Setup */
     , (450030,   3, 0x20000014) /* SoundTable */
     , (450030,   6, 0x0400007E) /* PaletteBase */
     , (450030,   7, 0x1000053A) /* ClothingBase */
     , (450030,   8, 0x06006271) /* Icon */
     , (450030,  22, 0x3400002B) /* PhysicsEffectTable */;
