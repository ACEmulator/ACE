DELETE FROM `weenie` WHERE `class_Id` = 450492;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450492, 'maskscarecrowtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450492,   1,          4) /* ItemType - Armor */
     , (450492,   3,          4) /* PaletteTemplate - Brown */
     , (450492,   4,      16384) /* ClothingPriority - Head */
     , (450492,   5,         0) /* EncumbranceVal */
     , (450492,   8,         75) /* Mass */
     , (450492,   9,          1) /* ValidLocations - HeadWear */
     , (450492,  16,          1) /* ItemUseable - No */
     , (450492,  19,         20) /* Value */
     , (450492,  27,          2) /* ArmorType - Leather */
     , (450492,  28,         0) /* ArmorLevel */
     , (450492,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450492, 150,        103) /* HookPlacement - Hook */
     , (450492, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450492,  22, True ) /* Inscribable */
     , (450492,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450492,  12,    0.66) /* Shade */
     , (450492,  13,    0.45) /* ArmorModVsSlash */
     , (450492,  14,    0.26) /* ArmorModVsPierce */
     , (450492,  15,    0.26) /* ArmorModVsBludgeon */
     , (450492,  16,    0.27) /* ArmorModVsCold */
     , (450492,  17,     0.5) /* ArmorModVsFire */
     , (450492,  18,    0.26) /* ArmorModVsAcid */
     , (450492,  19,    0.45) /* ArmorModVsElectric */
     , (450492, 110,       1) /* BulkMod */
     , (450492, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450492,   1, 'Scarecrow Mask') /* Name */
     , (450492,  16, 'A hollowed out pumpkin that, oddly enough, fits right over your head!') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450492,   1, 0x02000B71) /* Setup */
     , (450492,   3, 0x20000014) /* SoundTable */
     , (450492,   6, 0x0400007E) /* PaletteBase */
     , (450492,   7, 0x1000032A) /* ClothingBase */
     , (450492,   8, 0x060022A2) /* Icon */
     , (450492,  22, 0x3400002B) /* PhysicsEffectTable */;
