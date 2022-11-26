DELETE FROM `weenie` WHERE `class_Id` = 450536;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450536, 'coatursuindreadtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450536,   1,          2) /* ItemType - Armor */
     , (450536,   3,         18) /* PaletteTemplate - YellowBrown */
     , (450536,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450536,   5,       0) /* EncumbranceVal */
     , (450536,   8,        260) /* Mass */
     , (450536,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450536,  16,          1) /* ItemUseable - No */
     , (450536,  19,       20) /* Value */
     , (450536,  27,          8) /* ArmorType - Scalemail */
     , (450536,  28,        0) /* ArmorLevel */
     , (450536,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450536, 150,        103) /* HookPlacement - Hook */
     , (450536, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450536,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450536,  12,     0.9) /* Shade */
     , (450536,  13,       1) /* ArmorModVsSlash */
     , (450536,  14,       1) /* ArmorModVsPierce */
     , (450536,  15,       1) /* ArmorModVsBludgeon */
     , (450536,  16,       2) /* ArmorModVsCold */
     , (450536,  17,     0.7) /* ArmorModVsFire */
     , (450536,  18,       1) /* ArmorModVsAcid */
     , (450536,  19,     2.4) /* ArmorModVsElectric */
     , (450536, 110,       1) /* BulkMod */
     , (450536, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450536,   1, 'Heavy Ursuin Coat') /* Name */
     , (450536,  15, 'Some tattered shreds of clothing that you have managed to assemble into a coat.') /* ShortDesc */
     , (450536,  16, 'Some tattered shreds of the Dread Ursuin''s pelt that you have managed to assemble into a coat.  The creature''s healing ability seems to have not gone away with its death, allowing for the coat to seal itself as you watch.  It''s actually quite morbid.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450536,   1, 0x020000D4) /* Setup */
     , (450536,   3, 0x20000014) /* SoundTable */
     , (450536,   6, 0x0400007E) /* PaletteBase */
     , (450536,   7, 0x10000286) /* ClothingBase */
     , (450536,   8, 0x06000FF1) /* Icon */
     , (450536,  22, 0x3400002B) /* PhysicsEffectTable */;
