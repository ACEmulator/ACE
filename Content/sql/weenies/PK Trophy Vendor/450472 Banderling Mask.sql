DELETE FROM `weenie` WHERE `class_Id` = 450472;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450472, 'maskbanderlingnewtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450472,   1,          4) /* ItemType - Armor */
     , (450472,   3,          4) /* PaletteTemplate - Brown */
     , (450472,   4,      16384) /* ClothingPriority - Head */
     , (450472,   5,        0) /* EncumbranceVal */
     , (450472,   8,         75) /* Mass */
     , (450472,   9,          1) /* ValidLocations - HeadWear */
     , (450472,  16,          1) /* ItemUseable - No */
     , (450472,  19,        20) /* Value */
     , (450472,  27,          2) /* ArmorType - Leather */
     , (450472,  28,         0) /* ArmorLevel */
     , (450472,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450472, 150,        103) /* HookPlacement - Hook */
     , (450472, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450472,  22, True ) /* Inscribable */
     , (450472,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450472,  12,    0.66) /* Shade */
     , (450472,  13,    0.45) /* ArmorModVsSlash */
     , (450472,  14,     0.5) /* ArmorModVsPierce */
     , (450472,  15,       1) /* ArmorModVsBludgeon */
     , (450472,  16,    0.45) /* ArmorModVsCold */
     , (450472,  17,    0.35) /* ArmorModVsFire */
     , (450472,  18,     0.5) /* ArmorModVsAcid */
     , (450472,  19,     0.3) /* ArmorModVsElectric */
     , (450472, 110,       1) /* BulkMod */
     , (450472, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450472,   1, 'Banderling Mask') /* Name */
     , (450472,  16, 'A finely sewed and maintained Banderling head, patched with utmost precision, and conveniently fitted for use.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450472,   1, 0x02000E0A) /* Setup */
     , (450472,   3, 0x20000014) /* SoundTable */
     , (450472,   6, 0x0400007E) /* PaletteBase */
     , (450472,   7, 0x100003FE) /* ClothingBase */
     , (450472,   8, 0x0600288E) /* Icon */
     , (450472,  22, 0x3400002B) /* PhysicsEffectTable */;
