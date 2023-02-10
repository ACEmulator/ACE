DELETE FROM `weenie` WHERE `class_Id` = 450029;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450029, 'ace450029-festivalrobetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450029,   1,          4) /* ItemType - Clothing */
     , (450029,   3,         39) /* PaletteTemplate - Black */
     , (450029,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450029,   5,        0) /* EncumbranceVal */
     , (450029,   9,      512) /* ValidLocations - Armor */
     , (450029,  16,          1) /* ItemUseable - No */
     , (450029,  19,         20) /* Value */
     , (450029,  28,          20) /* ArmorLevel */
     , (450029,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450029,  11, True ) /* IgnoreCollisions */
     , (450029,  13, True ) /* Ethereal */
     , (450029,  14, True ) /* GravityStatus */
     , (450029,  19, True ) /* Attackable */
     , (450029,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450029,  12,       0) /* Shade */
     , (450029,  13,     0.8) /* ArmorModVsSlash */
     , (450029,  14,     0.8) /* ArmorModVsPierce */
     , (450029,  15,       1) /* ArmorModVsBludgeon */
     , (450029,  16,     0.2) /* ArmorModVsCold */
     , (450029,  17,     0.2) /* ArmorModVsFire */
     , (450029,  18,     0.1) /* ArmorModVsAcid */
     , (450029,  19,     0.2) /* ArmorModVsElectric */
     , (450029,  84,       0) /* Shade2 */
     , (450029, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450029,   1, 'Festival Robe') /* Name */
     , (450029,  16, 'A robe celebrating the Festival Season.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450029,   1, 0x020001A6) /* Setup */
     , (450029,   3, 0x20000014) /* SoundTable */
     , (450029,   6, 0x0400007E) /* PaletteBase */
     , (450029,   7, 0x10000658) /* ClothingBase */
     , (450029,   8, 0x0600626F) /* Icon */
     , (450029,  22, 0x3400002B) /* PhysicsEffectTable */;
