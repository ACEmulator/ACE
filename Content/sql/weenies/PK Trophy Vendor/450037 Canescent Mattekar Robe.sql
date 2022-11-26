DELETE FROM `weenie` WHERE `class_Id` = 450037;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450037, 'robeolthoimattekarcanescentoldtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450037,   1,          4) /* ItemType - Armor */
     , (450037,   3,         14) /* PaletteTemplate - Red */
     , (450037,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450037,   5,        0) /* EncumbranceVal */
     , (450037,   8,        500) /* Mass */
     , (450037,   9,      512) /* ValidLocations - Armor */
     , (450037,  16,          1) /* ItemUseable - No */
     , (450037,  19,          20) /* Value */
     , (450037,  27,          1) /* ArmorType - Cloth */
     , (450037,  28,        0) /* ArmorLevel */
     , (450037,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450037, 150,        103) /* HookPlacement - Hook */
     , (450037, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450037,  22, True ) /* Inscribable */
     , (450037,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450037,  12,       1) /* Shade */
     , (450037,  13,     0.3) /* ArmorModVsSlash */
     , (450037,  14,     0.3) /* ArmorModVsPierce */
     , (450037,  15,     0.3) /* ArmorModVsBludgeon */
     , (450037,  16,     1.3) /* ArmorModVsCold */
     , (450037,  17,     1.3) /* ArmorModVsFire */
     , (450037,  18,     1.3) /* ArmorModVsAcid */
     , (450037,  19,     1.3) /* ArmorModVsElectric */
     , (450037, 110,       1) /* BulkMod */
     , (450037, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450037,   1, 'Canescent Mattekar Robe') /* Name */
     , (450037,  15, 'The Canescent Mattekar Robe, brought to you with the finest care by Britana.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450037,   1, 0x020001A6) /* Setup */
     , (450037,   3, 0x20000014) /* SoundTable */
     , (450037,   6, 0x0400007E) /* PaletteBase */
     , (450037,   7, 0x10000315) /* ClothingBase */
     , (450037,   8, 0x06000FD7) /* Icon */
     , (450037,  22, 0x3400002B) /* PhysicsEffectTable */;
