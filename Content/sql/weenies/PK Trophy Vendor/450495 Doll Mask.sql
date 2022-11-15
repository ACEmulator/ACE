DELETE FROM `weenie` WHERE `class_Id` = 450495;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450495, 'maskdolltailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450495,   1,          4) /* ItemType - Armor */
     , (450495,   3,         61) /* PaletteTemplate - White */
     , (450495,   4,      16384) /* ClothingPriority - Head */
     , (450495,   5,        0) /* EncumbranceVal */
     , (450495,   8,         75) /* Mass */
     , (450495,   9,          1) /* ValidLocations - HeadWear */
     , (450495,  16,          1) /* ItemUseable - No */
     , (450495,  19,        20) /* Value */
     , (450495,  27,          2) /* ArmorType - Leather */
     , (450495,  28,         0) /* ArmorLevel */
     , (450495,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450495, 150,        103) /* HookPlacement - Hook */
     , (450495, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450495,  22, True ) /* Inscribable */
     , (450495,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450495,  12,    0.66) /* Shade */
     , (450495,  13,    0.25) /* ArmorModVsSlash */
     , (450495,  14,    0.25) /* ArmorModVsPierce */
     , (450495,  15,    0.25) /* ArmorModVsBludgeon */
     , (450495,  16,     0.5) /* ArmorModVsCold */
     , (450495,  17,    0.25) /* ArmorModVsFire */
     , (450495,  18,    0.25) /* ArmorModVsAcid */
     , (450495,  19,     0.5) /* ArmorModVsElectric */
     , (450495,  39,     0.5) /* DefaultScale */
     , (450495, 110,       1) /* BulkMod */
     , (450495, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450495,   1, 'Doll Mask') /* Name */
     , (450495,  16, 'A strange looking doll mask.  From the inside, the mask is completely transparent...') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450495,   1, 0x02000B74) /* Setup */
     , (450495,   3, 0x20000014) /* SoundTable */
     , (450495,   6, 0x0400007E) /* PaletteBase */
     , (450495,   7, 0x10000328) /* ClothingBase */
     , (450495,   8, 0x06001E31) /* Icon */
     , (450495,  22, 0x3400002B) /* PhysicsEffectTable */;
