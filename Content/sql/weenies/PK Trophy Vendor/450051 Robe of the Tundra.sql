DELETE FROM `weenie` WHERE `class_Id` = 450051;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450051, 'robemattekarbosstailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450051,   1,          4) /* ItemType - Clothing */
     , (450051,   3,         46) /* PaletteTemplate - Tan */
     , (450051,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450051,   5,        0) /* EncumbranceVal */
     , (450051,   8,        150) /* Mass */
     , (450051,   9,      512) /* ValidLocations - HeadWear, Armor */
     , (450051,  16,          1) /* ItemUseable - No */
     , (450051,  19,       20) /* Value */
     , (450051,  27,          1) /* ArmorType - Cloth */
     , (450051,  28,          0) /* ArmorLevel */
     , (450051,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450051, 150,        103) /* HookPlacement - Hook */
     , (450051, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450051,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450051,  12,    0.81) /* Shade */
     , (450051,  13,     0.6) /* ArmorModVsSlash */
     , (450051,  14,     0.6) /* ArmorModVsPierce */
     , (450051,  15,     0.8) /* ArmorModVsBludgeon */
     , (450051,  16,     0.5) /* ArmorModVsCold */
     , (450051,  17,    0.01) /* ArmorModVsFire */
     , (450051,  18,     0.1) /* ArmorModVsAcid */
     , (450051,  19,     0.5) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450051,   1, 'Robe of the Tundra') /* Name */
     , (450051,  15, 'A robe crafted from a mattekar hide.') /* ShortDesc */
     , (450051,  16, 'A robe crafted from a mattekar hide.  It has some natural padding in it that makes it more resistant to damage.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450051,   1, 0x020001A6) /* Setup */
     , (450051,   3, 0x20000014) /* SoundTable */
     , (450051,   6, 0x0400007E) /* PaletteBase */
     , (450051,   7, 0x10000327) /* ClothingBase */
     , (450051,   8, 0x06002292) /* Icon */
     , (450051,  22, 0x3400002B) /* PhysicsEffectTable */;
