DELETE FROM `weenie` WHERE `class_Id` = 450044;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450044, 'robereedsharkreapertailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450044,   1,          4) /* ItemType - Armor */
     , (450044,   3,          8) /* PaletteTemplate - Green */
     , (450044,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450044,   5,        0) /* EncumbranceVal */
     , (450044,   8,        340) /* Mass */
     , (450044,   9,      512) /* ValidLocations - Armor */
     , (450044,  16,          1) /* ItemUseable - No */
     , (450044,  19,       20) /* Value */
     , (450044,  27,          1) /* ArmorType - Cloth */
     , (450044,  28,        20) /* ArmorLevel */
     , (450044,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450044, 150,        103) /* HookPlacement - Hook */
     , (450044, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450044,  22, True ) /* Inscribable */
     , (450044, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450044,  12,       0) /* Shade */
     , (450044,  13,    0.25) /* ArmorModVsSlash */
     , (450044,  14,    0.75) /* ArmorModVsPierce */
     , (450044,  15,     0.6) /* ArmorModVsBludgeon */
     , (450044,  16,    0.25) /* ArmorModVsCold */
     , (450044,  17,    0.65) /* ArmorModVsFire */
     , (450044,  18,    0.75) /* ArmorModVsAcid */
     , (450044,  19,    0.75) /* ArmorModVsElectric */
     , (450044, 110,       1) /* BulkMod */
     , (450044, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450044,   1, 'Hearty Reedshark Robe') /* Name */
     , (450044,  15, 'A robe crafted from the leathery hide of a Reedshark Reaper. The hide has been treated and crafted into a fairly useful robe.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450044,   1, 0x020001A6) /* Setup */
     , (450044,   3, 0x20000014) /* SoundTable */
     , (450044,   6, 0x0400007E) /* PaletteBase */
     , (450044,   7, 0x100004D6) /* ClothingBase */
     , (450044,   8, 0x06002DD5) /* Icon */
     , (450044,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450044,  36, 0x0E000016) /* MutateFilter */;
