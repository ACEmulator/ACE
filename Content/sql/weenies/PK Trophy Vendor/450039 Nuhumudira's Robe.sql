DELETE FROM `weenie` WHERE `class_Id` = 450039;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450039, 'robeulgrimtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450039,   1,          4) /* ItemType - Clothing */
     , (450039,   3,          2) /* PaletteTemplate - Blue */
     , (450039,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450039,   5,        0) /* EncumbranceVal */
     , (450039,   8,        450) /* Mass */
     , (450039,   9,      512) /* ValidLocations - HeadWear, Armor */
     , (450039,  16,          1) /* ItemUseable - No */
     , (450039,  19,       20) /* Value */
     , (450039,  27,          1) /* ArmorType - Cloth */
     , (450039,  28,         0) /* ArmorLevel */
     , (450039,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450039,  22, True ) /* Inscribable */
     , (450039,  23, True ) /* DestroyOnSell */
     , (450039,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450039,  12,       1) /* Shade */
     , (450039,  13,     0.4) /* ArmorModVsSlash */
     , (450039,  14,     0.4) /* ArmorModVsPierce */
     , (450039,  15,     0.4) /* ArmorModVsBludgeon */
     , (450039,  16,     0.4) /* ArmorModVsCold */
     , (450039,  17,     0.4) /* ArmorModVsFire */
     , (450039,  18,     0.4) /* ArmorModVsAcid */
     , (450039,  19,     0.4) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450039,   1, 'Nuhumudira''s Robe') /* Name */
     , (450039,  15, 'A fine robe shimmering with silk fibers.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450039,   1, 0x020001A6) /* Setup */
     , (450039,   3, 0x20000014) /* SoundTable */
     , (450039,   6, 0x0400007E) /* PaletteBase */
     , (450039,   7, 0x100003E6) /* ClothingBase */
     , (450039,   8, 0x06002292) /* Icon */
     , (450039,  22, 0x3400002B) /* PhysicsEffectTable */;
