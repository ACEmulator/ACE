DELETE FROM `weenie` WHERE `class_Id` = 450540;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450540, 'dressshotailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450540,   1,          4) /* ItemType - Clothing */
     , (450540,   3,          2) /* PaletteTemplate - Blue */
     , (450540,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450540,   5,        0) /* EncumbranceVal */
     , (450540,   8,        150) /* Mass */
     , (450540,   9,      512) /* ValidLocations - Armor */
     , (450540,  16,          1) /* ItemUseable - No */
     , (450540,  19,       20) /* Value */
     , (450540,  27,          1) /* ArmorType - Cloth */
     , (450540,  28,          0) /* ArmorLevel */
     , (450540,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450540,  22, True ) /* Inscribable */
     , (450540, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450540,  12,     0.5) /* Shade */
     , (450540,  13,       1) /* ArmorModVsSlash */
     , (450540,  14,     0.7) /* ArmorModVsPierce */
     , (450540,  15,     0.4) /* ArmorModVsBludgeon */
     , (450540,  16,     0.2) /* ArmorModVsCold */
     , (450540,  17,     0.2) /* ArmorModVsFire */
     , (450540,  18,     0.3) /* ArmorModVsAcid */
     , (450540,  19,     0.4) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450540,   1, 'Yifan Dress') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450540,   1, 0x020001A6) /* Setup */
     , (450540,   3, 0x20000014) /* SoundTable */
     , (450540,   6, 0x0400007E) /* PaletteBase */
     , (450540,   7, 0x1000026D) /* ClothingBase */
     , (450540,   8, 0x06001B8D) /* Icon */
     , (450540,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450540,  36, 0x0E000016) /* MutateFilter */;
