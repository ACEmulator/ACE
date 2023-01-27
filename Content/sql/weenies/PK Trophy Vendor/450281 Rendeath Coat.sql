DELETE FROM `weenie` WHERE `class_Id` = 450281;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450281, 'coatshrethrendeathtsilot', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450281,   1,          2) /* ItemType - Armor */
     , (450281,   3,         14) /* PaletteTemplate - Red */
     , (450281,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (450281,   5,        0) /* EncumbranceVal */
     , (450281,   8,        270) /* Mass */
     , (450281,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (450281,  16,          1) /* ItemUseable - No */
     , (450281,  19,       20) /* Value */
     , (450281,  27,          2) /* ArmorType - Leather */
     , (450281,  28,        0) /* ArmorLevel */
     , (450281,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450281, 150,        103) /* HookPlacement - Hook */
     , (450281, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450281,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450281,  12,    0.66) /* Shade */
     , (450281,  13,    0.55) /* ArmorModVsSlash */
     , (450281,  14,    0.75) /* ArmorModVsPierce */
     , (450281,  15,       1) /* ArmorModVsBludgeon */
     , (450281,  16,       1) /* ArmorModVsCold */
     , (450281,  17,     0.5) /* ArmorModVsFire */
     , (450281,  18,     0.5) /* ArmorModVsAcid */
     , (450281,  19,     0.5) /* ArmorModVsElectric */
     , (450281, 110,       1) /* BulkMod */
     , (450281, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450281,   1, 'Rendeath Coat') /* Name */
     , (450281,  16, 'This coat has been reinforced with bone structures and metal strapping. The main body of the coat came from the hide of a Rendeath Shreth.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450281,   1, 0x020001A6) /* Setup */
     , (450281,   3, 0x20000014) /* SoundTable */
     , (450281,   6, 0x0400007E) /* PaletteBase */
     , (450281,   7, 0x100004D8) /* ClothingBase */
     , (450281,   8, 0x06002DE2) /* Icon */
     , (450281,  22, 0x3400002B) /* PhysicsEffectTable */;
