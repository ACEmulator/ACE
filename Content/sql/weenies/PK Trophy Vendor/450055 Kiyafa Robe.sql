DELETE FROM `weenie` WHERE `class_Id` = 450055;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450055, 'dressgharundimtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450055,   1,          4) /* ItemType - Clothing */
     , (450055,   3,         13) /* PaletteTemplate - Purple */
     , (450055,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450055,   5,        0) /* EncumbranceVal */
     , (450055,   8,        150) /* Mass */
     , (450055,   9,      512) /* ValidLocations - Armor */
     , (450055,  16,          1) /* ItemUseable - No */
     , (450055,  19,       20) /* Value */
     , (450055,  27,          1) /* ArmorType - Cloth */
     , (450055,  28,          0) /* ArmorLevel */
     , (450055,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450055,  22, True ) /* Inscribable */
     , (450055, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450055,  12,     0.5) /* Shade */
     , (450055,  13,       1) /* ArmorModVsSlash */
     , (450055,  14,     0.7) /* ArmorModVsPierce */
     , (450055,  15,     0.4) /* ArmorModVsBludgeon */
     , (450055,  16,     0.2) /* ArmorModVsCold */
     , (450055,  17,     0.2) /* ArmorModVsFire */
     , (450055,  18,     0.3) /* ArmorModVsAcid */
     , (450055,  19,     0.4) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450055,   1, 'Kiyafa Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450055,   1, 0x020001A6) /* Setup */
     , (450055,   3, 0x20000014) /* SoundTable */
     , (450055,   6, 0x0400007E) /* PaletteBase */
     , (450055,   7, 0x1000026C) /* ClothingBase */
     , (450055,   8, 0x06001B8D) /* Icon */
     , (450055,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450055,  36, 0x0E000016) /* MutateFilter */;
