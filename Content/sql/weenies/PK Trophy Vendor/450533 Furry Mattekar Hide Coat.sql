DELETE FROM `weenie` WHERE `class_Id` = 450533;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450533, 'coatmattekartailor2', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450533,   1,          2) /* ItemType - Armor */
     , (450533,   3,          9) /* PaletteTemplate - Grey */
     , (450533,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450533,   5,        0) /* EncumbranceVal */
     , (450533,   8,        270) /* Mass */
     , (450533,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450533,  16,          1) /* ItemUseable - No */
     , (450533,  19,       20) /* Value */
     , (450533,  27,          2) /* ArmorType - Leather */
     , (450533,  28,        0) /* ArmorLevel */
     , (450533,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450533, 150,        103) /* HookPlacement - Hook */
     , (450533, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450533,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450533,  12,    0.66) /* Shade */
     , (450533,  13,     1.2) /* ArmorModVsSlash */
     , (450533,  14,     0.9) /* ArmorModVsPierce */
     , (450533,  15,     0.9) /* ArmorModVsBludgeon */
     , (450533,  16,       2) /* ArmorModVsCold */
     , (450533,  17,     0.7) /* ArmorModVsFire */
     , (450533,  18,       1) /* ArmorModVsAcid */
     , (450533,  19,       2) /* ArmorModVsElectric */
     , (450533, 110,       1) /* BulkMod */
     , (450533, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450533,   1, 'Furry Mattekar Hide Coat') /* Name */
     , (450533,  15, 'Coat crafted from the hide of a Mattekar, and energized by Yi Yo-Jin.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450533,   1, 0x020000D4) /* Setup */
     , (450533,   3, 0x20000014) /* SoundTable */
     , (450533,   6, 0x0400007E) /* PaletteBase */
     , (450533,   7, 0x10000286) /* ClothingBase */
     , (450533,   8, 0x06000FF1) /* Icon */
     , (450533,  22, 0x3400002B) /* PhysicsEffectTable */;
