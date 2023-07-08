DELETE FROM `weenie` WHERE `class_Id` = 450011;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450011, 'ace450011-festivalrobetailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450011,   1,          4) /* ItemType - Clothing */
     , (450011,   3,         76) /* PaletteTemplate - Orange */
     , (450011,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450011,   5,        10) /* EncumbranceVal */
     , (450011,   9,      512) /* ValidLocations - Armor */
     , (450011,  16,          1) /* ItemUseable - No */
     , (450011,  19,         20) /* Value */
     , (450011,  28,          0) /* ArmorLevel */
     , (450011,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450011,  11, True ) /* IgnoreCollisions */
     , (450011,  13, True ) /* Ethereal */
     , (450011,  14, True ) /* GravityStatus */
     , (450011,  19, True ) /* Attackable */
     , (450011,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450011,  12,       0) /* Shade */
     , (450011,  13,     0.8) /* ArmorModVsSlash */
     , (450011,  14,     0.8) /* ArmorModVsPierce */
     , (450011,  15,       1) /* ArmorModVsBludgeon */
     , (450011,  16,     0.2) /* ArmorModVsCold */
     , (450011,  17,     0.2) /* ArmorModVsFire */
     , (450011,  18,     0.1) /* ArmorModVsAcid */
     , (450011,  19,     0.2) /* ArmorModVsElectric */
     , (450011,  84,       0) /* Shade2 */
     , (450011, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450011,   1, 'Festival Robe') /* Name */
     , (450011,  16, 'A robe celebrating the Festival Season.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450011,   1, 0x020001A6) /* Setup */
     , (450011,   3, 0x20000014) /* SoundTable */
     , (450011,   6, 0x0400007E) /* PaletteBase */
     , (450011,   7, 0x10000658) /* ClothingBase */
     , (450011,   8, 0x0600626E) /* Icon */
     , (450011,  22, 0x3400002B) /* PhysicsEffectTable */;
