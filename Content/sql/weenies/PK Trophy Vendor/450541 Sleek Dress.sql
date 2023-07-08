DELETE FROM `weenie` WHERE `class_Id` = 450541;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450541, 'dressnoirtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450541,   1,          4) /* ItemType - Clothing */
     , (450541,   3,         14) /* PaletteTemplate - Red */
     , (450541,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450541,   5,        0) /* EncumbranceVal */
     , (450541,   8,        150) /* Mass */
     , (450541,   9,      512) /* ValidLocations - Armor */
     , (450541,  16,          1) /* ItemUseable - No */
     , (450541,  19,       20) /* Value */
     , (450541,  27,          1) /* ArmorType - Cloth */
     , (450541,  28,         0) /* ArmorLevel */
     , (450541,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450541,  22, True ) /* Inscribable */
     , (450541,  69, False) /* IsSellable */
     , (450541, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450541,  12,     0.5) /* Shade */
     , (450541,  13,     0.1) /* ArmorModVsSlash */
     , (450541,  14,     0.1) /* ArmorModVsPierce */
     , (450541,  15,     0.1) /* ArmorModVsBludgeon */
     , (450541,  16,     0.1) /* ArmorModVsCold */
     , (450541,  17,     0.1) /* ArmorModVsFire */
     , (450541,  18,     0.1) /* ArmorModVsAcid */
     , (450541,  19,     0.1) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450541,   1, 'Sleek Dress') /* Name */
     , (450541,  15, 'A dress designed by the Gharu''ndim tailor, Xuut. The fibers of the dress look as though they could withstand the dyeing process.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450541,   1, 0x020001A6) /* Setup */
     , (450541,   3, 0x20000014) /* SoundTable */
     , (450541,   6, 0x0400007E) /* PaletteBase */
     , (450541,   7, 0x100004F2) /* ClothingBase */
     , (450541,   8, 0x06001B8D) /* Icon */
     , (450541,  22, 0x3400002B) /* PhysicsEffectTable */;
