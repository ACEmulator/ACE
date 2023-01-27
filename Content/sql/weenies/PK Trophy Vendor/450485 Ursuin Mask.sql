DELETE FROM `weenie` WHERE `class_Id` = 450485;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450485, 'maskursuintailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450485,   1,          4) /* ItemType - Armor */
     , (450485,   3,          4) /* PaletteTemplate - Brown */
     , (450485,   4,      16384) /* ClothingPriority - Head */
     , (450485,   5,        0) /* EncumbranceVal */
     , (450485,   8,         75) /* Mass */
     , (450485,   9,          1) /* ValidLocations - HeadWear */
     , (450485,  16,          1) /* ItemUseable - No */
     , (450485,  19,        20) /* Value */
     , (450485,  27,          2) /* ArmorType - Leather */
     , (450485,  28,         0) /* ArmorLevel */
     , (450485,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450485, 150,        103) /* HookPlacement - Hook */
     , (450485, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450485,  22, True ) /* Inscribable */
     , (450485,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450485,  12,    0.66) /* Shade */
     , (450485,  13,    0.29) /* ArmorModVsSlash */
     , (450485,  14,     0.5) /* ArmorModVsPierce */
     , (450485,  15,    0.29) /* ArmorModVsBludgeon */
     , (450485,  16,    0.29) /* ArmorModVsCold */
     , (450485,  17,    0.43) /* ArmorModVsFire */
     , (450485,  18,    0.29) /* ArmorModVsAcid */
     , (450485,  19,    0.29) /* ArmorModVsElectric */
     , (450485, 110,       1) /* BulkMod */
     , (450485, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450485,   1, 'Ursuin Mask') /* Name */
     , (450485,  16, 'A finely stitched and cured Ursuin head, complete with cushions around the neck for active use.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450485,   1, 0x02000B75) /* Setup */
     , (450485,   3, 0x20000014) /* SoundTable */
     , (450485,   6, 0x0400007E) /* PaletteBase */
     , (450485,   7, 0x1000032D) /* ClothingBase */
     , (450485,   8, 0x060022A5) /* Icon */
     , (450485,  22, 0x3400002B) /* PhysicsEffectTable */;
