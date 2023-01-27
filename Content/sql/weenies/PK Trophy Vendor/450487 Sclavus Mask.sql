DELETE FROM `weenie` WHERE `class_Id` = 450487;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450487, 'masksclavustailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450487,   1,          4) /* ItemType - Armor */
     , (450487,   3,          4) /* PaletteTemplate - Brown */
     , (450487,   4,      16384) /* ClothingPriority - Head */
     , (450487,   5,        0) /* EncumbranceVal */
     , (450487,   8,         75) /* Mass */
     , (450487,   9,          1) /* ValidLocations - HeadWear */
     , (450487,  16,          1) /* ItemUseable - No */
     , (450487,  19,        20) /* Value */
     , (450487,  27,          2) /* ArmorType - Leather */
     , (450487,  28,         0) /* ArmorLevel */
     , (450487,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450487, 150,        103) /* HookPlacement - Hook */
     , (450487, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450487,  22, True ) /* Inscribable */
     , (450487,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450487,  12,    0.66) /* Shade */
     , (450487,  13,     0.5) /* ArmorModVsSlash */
     , (450487,  14,   0.375) /* ArmorModVsPierce */
     , (450487,  15,    0.25) /* ArmorModVsBludgeon */
     , (450487,  16,     0.5) /* ArmorModVsCold */
     , (450487,  17,   0.375) /* ArmorModVsFire */
     , (450487,  18,   0.125) /* ArmorModVsAcid */
     , (450487,  19,   0.125) /* ArmorModVsElectric */
     , (450487, 110,       1) /* BulkMod */
     , (450487, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450487,   1, 'Sclavus Mask') /* Name */
     , (450487,  16, 'A finely sewed and oiled Sclavus head, patched with utmost precision, and conveniently fitted for use.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450487,   1, 0x02000B72) /* Setup */
     , (450487,   3, 0x20000014) /* SoundTable */
     , (450487,   6, 0x0400007E) /* PaletteBase */
     , (450487,   7, 0x1000032B) /* ClothingBase */
     , (450487,   8, 0x060022A3) /* Icon */
     , (450487,  22, 0x3400002B) /* PhysicsEffectTable */;
