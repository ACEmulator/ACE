DELETE FROM `weenie` WHERE `class_Id` = 450038;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450038, 'robeharrowermattekarcanescentoldtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450038,   1,          4) /* ItemType - Armor */
     , (450038,   3,          2) /* PaletteTemplate - Blue */
     , (450038,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450038,   5,        0) /* EncumbranceVal */
     , (450038,   8,        500) /* Mass */
     , (450038,   9,      512) /* ValidLocations - Armor */
     , (450038,  16,          1) /* ItemUseable - No */
     , (450038,  19,          20) /* Value */
     , (450038,  27,          1) /* ArmorType - Cloth */
     , (450038,  28,        0) /* ArmorLevel */
     , (450038,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450038, 150,        103) /* HookPlacement - Hook */
     , (450038, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450038,  22, True ) /* Inscribable */
     , (450038,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450038,  12,       1) /* Shade */
     , (450038,  13,       1) /* ArmorModVsSlash */
     , (450038,  14,       1) /* ArmorModVsPierce */
     , (450038,  15,       1) /* ArmorModVsBludgeon */
     , (450038,  16,       1) /* ArmorModVsCold */
     , (450038,  17,       1) /* ArmorModVsFire */
     , (450038,  18,       1) /* ArmorModVsAcid */
     , (450038,  19,       1) /* ArmorModVsElectric */
     , (450038, 110,       1) /* BulkMod */
     , (450038, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450038,   1, 'Canescent Mattekar Robe') /* Name */
     , (450038,  15, 'The Canescent Mattekar Robe, brought to you with the finest care by Britana.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450038,   1, 0x020001A6) /* Setup */
     , (450038,   3, 0x20000014) /* SoundTable */
     , (450038,   6, 0x0400007E) /* PaletteBase */
     , (450038,   7, 0x10000315) /* ClothingBase */
     , (450038,   8, 0x06000FD7) /* Icon */
     , (450038,  22, 0x3400002B) /* PhysicsEffectTable */;
