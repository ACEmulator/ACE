DELETE FROM `weenie` WHERE `class_Id` = 450053;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450053, 'robeharrowermattekarcanescent-xptailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450053,   1,          4) /* ItemType - Armor */
     , (450053,   3,          2) /* PaletteTemplate - Blue */
     , (450053,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450053,   5,        0) /* EncumbranceVal */
     , (450053,   8,        500) /* Mass */
     , (450053,   9,      512) /* ValidLocations - Armor */
     , (450053,  16,          1) /* ItemUseable - No */
     , (450053,  19,          20) /* Value */
     , (450053,  27,          1) /* ArmorType - Cloth */
     , (450053,  28,        0) /* ArmorLevel */
     , (450053,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450053, 150,        103) /* HookPlacement - Hook */
     , (450053, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450053,  22, True ) /* Inscribable */
     , (450053,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450053,  12,   0.732) /* Shade */
     , (450053,  13,       1) /* ArmorModVsSlash */
     , (450053,  14,       1) /* ArmorModVsPierce */
     , (450053,  15,       1) /* ArmorModVsBludgeon */
     , (450053,  16,       1) /* ArmorModVsCold */
     , (450053,  17,       1) /* ArmorModVsFire */
     , (450053,  18,       1) /* ArmorModVsAcid */
     , (450053,  19,       1) /* ArmorModVsElectric */
     , (450053, 110,       1) /* BulkMod */
     , (450053, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450053,   1, 'Canescent Mattekar Robe') /* Name */
     , (450053,  15, 'The Canescent Mattekar Robe, brought to you with the finest care by Britana.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450053,   1, 0x020001A6) /* Setup */
     , (450053,   3, 0x20000014) /* SoundTable */
     , (450053,   6, 0x0400007E) /* PaletteBase */
     , (450053,   7, 0x10000348) /* ClothingBase */
     , (450053,   8, 0x06000FD7) /* Icon */
     , (450053,  22, 0x3400002B) /* PhysicsEffectTable */;
