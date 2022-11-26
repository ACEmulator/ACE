DELETE FROM `weenie` WHERE `class_Id` = 450475;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450475, 'maskmoarsmantailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450475,   1,          4) /* ItemType - Armor */
     , (450475,   3,          4) /* PaletteTemplate - Brown */
     , (450475,   4,      16384) /* ClothingPriority - Head */
     , (450475,   5,         0) /* EncumbranceVal */
     , (450475,   8,         75) /* Mass */
     , (450475,   9,          1) /* ValidLocations - HeadWear */
     , (450475,  16,          1) /* ItemUseable - No */
     , (450475,  19,       20) /* Value */
     , (450475,  28,         0) /* ArmorLevel */
     , (450475,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450475, 150,        103) /* HookPlacement - Hook */
     , (450475, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450475,  22, True ) /* Inscribable */
     , (450475,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450475,  12,    0.66) /* Shade */
     , (450475,  13,    0.45) /* ArmorModVsSlash */
     , (450475,  14,     0.5) /* ArmorModVsPierce */
     , (450475,  15,       1) /* ArmorModVsBludgeon */
     , (450475,  16,    0.45) /* ArmorModVsCold */
     , (450475,  17,    0.35) /* ArmorModVsFire */
     , (450475,  18,     0.5) /* ArmorModVsAcid */
     , (450475,  19,     0.3) /* ArmorModVsElectric */
     , (450475, 110,       1) /* BulkMod */
     , (450475, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450475,   1, 'Moarsman Mask') /* Name */
     , (450475,  16, 'A finely sewed and maintained Moarsman head, patched with utmost precision, and conveniently fitted for use.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450475,   1, 0x02000957) /* Setup */
     , (450475,   3, 0x20000014) /* SoundTable */
     , (450475,   6, 0x0400007E) /* PaletteBase */
     , (450475,   7, 0x100004CC) /* ClothingBase */
     , (450475,   8, 0x06002D83) /* Icon */
     , (450475,  22, 0x3400002B) /* PhysicsEffectTable */;
