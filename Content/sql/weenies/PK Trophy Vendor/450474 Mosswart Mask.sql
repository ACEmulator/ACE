DELETE FROM `weenie` WHERE `class_Id` = 450474;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450474, 'maskmosswartnewtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450474,   1,          4) /* ItemType - Armor */
     , (450474,   3,          4) /* PaletteTemplate - Brown */
     , (450474,   4,      16384) /* ClothingPriority - Head */
     , (450474,   5,        0) /* EncumbranceVal */
     , (450474,   8,         75) /* Mass */
     , (450474,   9,          1) /* ValidLocations - HeadWear */
     , (450474,  16,          1) /* ItemUseable - No */
     , (450474,  19,        20) /* Value */
     , (450474,  27,          2) /* ArmorType - Leather */
     , (450474,  28,         0) /* ArmorLevel */
     , (450474,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450474, 150,        103) /* HookPlacement - Hook */
     , (450474, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450474,  22, True ) /* Inscribable */
     , (450474,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450474,  12,    0.66) /* Shade */
     , (450474,  13,     0.5) /* ArmorModVsSlash */
     , (450474,  14,     0.4) /* ArmorModVsPierce */
     , (450474,  15,     0.4) /* ArmorModVsBludgeon */
     , (450474,  16,     0.6) /* ArmorModVsCold */
     , (450474,  17,     0.2) /* ArmorModVsFire */
     , (450474,  18,    0.75) /* ArmorModVsAcid */
     , (450474,  19,    0.35) /* ArmorModVsElectric */
     , (450474, 110,       1) /* BulkMod */
     , (450474, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450474,   1, 'Mosswart Mask') /* Name */
     , (450474,  16, 'A finely sewed and cured Mosswart head, complete with cushions around the neck for active use.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450474,   1, 0x02000958) /* Setup */
     , (450474,   3, 0x20000014) /* SoundTable */
     , (450474,   6, 0x0400007E) /* PaletteBase */
     , (450474,   7, 0x100003FA) /* ClothingBase */
     , (450474,   8, 0x0600288F) /* Icon */
     , (450474,  22, 0x3400002B) /* PhysicsEffectTable */;
