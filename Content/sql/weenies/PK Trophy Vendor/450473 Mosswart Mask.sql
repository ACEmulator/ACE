DELETE FROM `weenie` WHERE `class_Id` = 450473;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450473, 'maskmosswarttailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450473,   1,          4) /* ItemType - Armor */
     , (450473,   3,          4) /* PaletteTemplate - Brown */
     , (450473,   4,      16384) /* ClothingPriority - Head */
     , (450473,   5,        0) /* EncumbranceVal */
     , (450473,   8,         75) /* Mass */
     , (450473,   9,          1) /* ValidLocations - HeadWear */
     , (450473,  16,          1) /* ItemUseable - No */
     , (450473,  19,        20) /* Value */
     , (450473,  27,          2) /* ArmorType - Leather */
     , (450473,  28,         0) /* ArmorLevel */
     , (450473,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450473, 150,        103) /* HookPlacement - Hook */
     , (450473, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450473,  22, True ) /* Inscribable */
     , (450473,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450473,  12,    0.66) /* Shade */
     , (450473,  13,     0.5) /* ArmorModVsSlash */
     , (450473,  14,     0.4) /* ArmorModVsPierce */
     , (450473,  15,     0.4) /* ArmorModVsBludgeon */
     , (450473,  16,     0.6) /* ArmorModVsCold */
     , (450473,  17,     0.2) /* ArmorModVsFire */
     , (450473,  18,    0.75) /* ArmorModVsAcid */
     , (450473,  19,    0.35) /* ArmorModVsElectric */
     , (450473, 110,       1) /* BulkMod */
     , (450473, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450473,   1, 'Mosswart Mask') /* Name */
     , (450473,  16, 'A finely sewed and cured Mosswart head, complete with cushions around the neck for active use.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450473,   1, 0x02000958) /* Setup */
     , (450473,   3, 0x20000014) /* SoundTable */
     , (450473,   6, 0x0400007E) /* PaletteBase */
     , (450473,   7, 0x10000254) /* ClothingBase */
     , (450473,   8, 0x06001E30) /* Icon */
     , (450473,  22, 0x3400002B) /* PhysicsEffectTable */;
