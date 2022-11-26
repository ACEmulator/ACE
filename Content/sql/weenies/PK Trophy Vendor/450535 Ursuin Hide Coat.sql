DELETE FROM `weenie` WHERE `class_Id` = 450535;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450535, 'coatursuinsummertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450535,   1,          2) /* ItemType - Armor */
     , (450535,   3,          6) /* PaletteTemplate - DeepBrown */
     , (450535,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450535,   5,        0) /* EncumbranceVal */
     , (450535,   8,        270) /* Mass */
     , (450535,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450535,  16,          1) /* ItemUseable - No */
     , (450535,  19,       20) /* Value */
     , (450535,  27,          2) /* ArmorType - Leather */
     , (450535,  28,         0) /* ArmorLevel */
     , (450535,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450535, 150,        103) /* HookPlacement - Hook */
     , (450535, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450535,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450535,  12,    0.66) /* Shade */
     , (450535,  13,     1.1) /* ArmorModVsSlash */
     , (450535,  14,     1.1) /* ArmorModVsPierce */
     , (450535,  15,     1.1) /* ArmorModVsBludgeon */
     , (450535,  16,       2) /* ArmorModVsCold */
     , (450535,  17,     0.8) /* ArmorModVsFire */
     , (450535,  18,     1.1) /* ArmorModVsAcid */
     , (450535,  19,       2) /* ArmorModVsElectric */
     , (450535, 110,       1) /* BulkMod */
     , (450535, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450535,   1, 'Ursuin Hide Coat') /* Name */
     , (450535,  16, 'A coat made out of the hide of an ursuin.  It is thick and vibrant, showing the colors of spring.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450535,   1, 0x020000D4) /* Setup */
     , (450535,   3, 0x20000014) /* SoundTable */
     , (450535,   6, 0x0400007E) /* PaletteBase */
     , (450535,   7, 0x10000286) /* ClothingBase */
     , (450535,   8, 0x06000FF1) /* Icon */
     , (450535,  22, 0x3400002B) /* PhysicsEffectTable */;
