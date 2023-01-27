DELETE FROM `weenie` WHERE `class_Id` = 450052;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450052, 'robeolthoimattekarcanescent-xptailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450052,   1,          4) /* ItemType - Armor */
     , (450052,   3,         14) /* PaletteTemplate - Red */
     , (450052,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450052,   5,        0) /* EncumbranceVal */
     , (450052,   8,        500) /* Mass */
     , (450052,   9,      512) /* ValidLocations - Armor */
     , (450052,  16,          1) /* ItemUseable - No */
     , (450052,  19,          20) /* Value */
     , (450052,  27,          1) /* ArmorType - Cloth */
     , (450052,  28,        0) /* ArmorLevel */
     , (450052,  33,          1) /* Bonded - Bonded */
     , (450052,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450052, 150,        103) /* HookPlacement - Hook */
     , (450052, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450052,  22, True ) /* Inscribable */
     , (450052,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450052,  12,    0.09) /* Shade */
     , (450052,  13,     0.3) /* ArmorModVsSlash */
     , (450052,  14,     0.3) /* ArmorModVsPierce */
     , (450052,  15,     0.3) /* ArmorModVsBludgeon */
     , (450052,  16,     1.3) /* ArmorModVsCold */
     , (450052,  17,     1.3) /* ArmorModVsFire */
     , (450052,  18,     1.3) /* ArmorModVsAcid */
     , (450052,  19,     1.3) /* ArmorModVsElectric */
     , (450052, 110,       1) /* BulkMod */
     , (450052, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450052,   1, 'Canescent Mattekar Robe') /* Name */
     , (450052,  15, 'The Canescent Mattekar Robe, brought to you with the finest care by Britana.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450052,   1, 0x020001A6) /* Setup */
     , (450052,   3, 0x20000014) /* SoundTable */
     , (450052,   6, 0x0400007E) /* PaletteBase */
     , (450052,   7, 0x10000348) /* ClothingBase */
     , (450052,   8, 0x06000FD7) /* Icon */
     , (450052,  22, 0x3400002B) /* PhysicsEffectTable */;
