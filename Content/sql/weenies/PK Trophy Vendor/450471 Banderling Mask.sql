DELETE FROM `weenie` WHERE `class_Id` = 450471;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450471, 'maskbanderlingtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450471,   1,          4) /* ItemType - Armor */
     , (450471,   3,          4) /* PaletteTemplate - Brown */
     , (450471,   4,      16384) /* ClothingPriority - Head */
     , (450471,   5,        0) /* EncumbranceVal */
     , (450471,   8,         75) /* Mass */
     , (450471,   9,          1) /* ValidLocations - HeadWear */
     , (450471,  16,          1) /* ItemUseable - No */
     , (450471,  19,        20) /* Value */
     , (450471,  27,          2) /* ArmorType - Leather */
     , (450471,  28,         10) /* ArmorLevel */
     , (450471,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450471, 150,        103) /* HookPlacement - Hook */
     , (450471, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450471,  22, True ) /* Inscribable */
     , (450471,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450471,  12,    0.66) /* Shade */
     , (450471,  13,    0.45) /* ArmorModVsSlash */
     , (450471,  14,     0.5) /* ArmorModVsPierce */
     , (450471,  15,       1) /* ArmorModVsBludgeon */
     , (450471,  16,    0.45) /* ArmorModVsCold */
     , (450471,  17,    0.35) /* ArmorModVsFire */
     , (450471,  18,     0.5) /* ArmorModVsAcid */
     , (450471,  19,     0.3) /* ArmorModVsElectric */
     , (450471, 110,       1) /* BulkMod */
     , (450471, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450471,   1, 'Banderling Mask') /* Name */
     , (450471,  16, 'A finely sewed and maintained Banderling head, patched with utmost precision, and conveniently fitted for use.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450471,   1, 0x02000956) /* Setup */
     , (450471,   3, 0x20000014) /* SoundTable */
     , (450471,   6, 0x0400007E) /* PaletteBase */
     , (450471,   7, 0x10000252) /* ClothingBase */
     , (450471,   8, 0x06001E2E) /* Icon */
     , (450471,  22, 0x3400002B) /* PhysicsEffectTable */;
