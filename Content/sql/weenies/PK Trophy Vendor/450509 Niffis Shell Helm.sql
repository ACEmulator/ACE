DELETE FROM `weenie` WHERE `class_Id` = 450509;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450509, 'helmetniffistailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450509,   1,          2) /* ItemType - Armor */
     , (450509,   3,         20) /* PaletteTemplate - Silver */
     , (450509,   4,      16384) /* ClothingPriority - Head */
     , (450509,   5,        0) /* EncumbranceVal */
     , (450509,   8,        125) /* Mass */
     , (450509,   9,          1) /* ValidLocations - HeadWear */
     , (450509,  16,          1) /* ItemUseable - No */
     , (450509,  19,       20) /* Value */
     , (450509,  27,          4) /* ArmorType - StuddedLeather */
     , (450509,  28,        0) /* ArmorLevel */
     , (450509,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450509, 150,        103) /* HookPlacement - Hook */
     , (450509, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450509,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450509,  12,    0.66) /* Shade */
     , (450509,  13,     0.8) /* ArmorModVsSlash */
     , (450509,  14,       1) /* ArmorModVsPierce */
     , (450509,  15,       1) /* ArmorModVsBludgeon */
     , (450509,  16,     0.5) /* ArmorModVsCold */
     , (450509,  17,     0.8) /* ArmorModVsFire */
     , (450509,  18,     0.5) /* ArmorModVsAcid */
     , (450509,  19,     0.9) /* ArmorModVsElectric */
     , (450509, 110,       1) /* BulkMod */
     , (450509, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450509,   1, 'Niffis Shell Helm') /* Name */
     , (450509,  15, 'A Niffis Shell Helmet.') /* ShortDesc */
     , (450509,  16, 'A Niffis Shell Helmet.  It is unusally light, and very well ventilated.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450509,   1, 0x02000993) /* Setup */
     , (450509,   3, 0x20000014) /* SoundTable */
     , (450509,   6, 0x0400007E) /* PaletteBase */
     , (450509,   7, 0x100002DA) /* ClothingBase */
     , (450509,   8, 0x06001353) /* Icon */
     , (450509,  22, 0x3400002B) /* PhysicsEffectTable */;
