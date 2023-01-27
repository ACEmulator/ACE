DELETE FROM `weenie` WHERE `class_Id` = 450534;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450534, 'coatursuintailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450534,   1,          2) /* ItemType - Armor */
     , (450534,   3,         10) /* PaletteTemplate - LightBlue */
     , (450534,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450534,   5,        0) /* EncumbranceVal */
     , (450534,   8,        270) /* Mass */
     , (450534,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450534,  16,          1) /* ItemUseable - No */
     , (450534,  19,       20) /* Value */
     , (450534,  27,          2) /* ArmorType - Leather */
     , (450534,  28,        0) /* ArmorLevel */
     , (450534,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450534, 150,        103) /* HookPlacement - Hook */
     , (450534, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450534,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450534,  12,    0.66) /* Shade */
     , (450534,  13,       1) /* ArmorModVsSlash */
     , (450534,  14,       1) /* ArmorModVsPierce */
     , (450534,  15,       1) /* ArmorModVsBludgeon */
     , (450534,  16,       2) /* ArmorModVsCold */
     , (450534,  17,     0.7) /* ArmorModVsFire */
     , (450534,  18,       1) /* ArmorModVsAcid */
     , (450534,  19,       2) /* ArmorModVsElectric */
     , (450534, 110,       1) /* BulkMod */
     , (450534, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450534,   1, 'Ursuin Hide Coat') /* Name */
     , (450534,  16, 'A coat made out of the hide of an ursuin.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450534,   1, 0x020000D4) /* Setup */
     , (450534,   3, 0x20000014) /* SoundTable */
     , (450534,   6, 0x0400007E) /* PaletteBase */
     , (450534,   7, 0x10000286) /* ClothingBase */
     , (450534,   8, 0x06000FF1) /* Icon */
     , (450534,  22, 0x3400002B) /* PhysicsEffectTable */;
