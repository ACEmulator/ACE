DELETE FROM `weenie` WHERE `class_Id` = 450483;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450483, 'maskdrudgetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450483,   1,          4) /* ItemType - Armor */
     , (450483,   3,          4) /* PaletteTemplate - Brown */
     , (450483,   4,      16384) /* ClothingPriority - Head */
     , (450483,   5,        150) /* EncumbranceVal */
     , (450483,   8,         75) /* Mass */
     , (450483,   9,          1) /* ValidLocations - HeadWear */
     , (450483,  16,          1) /* ItemUseable - No */
     , (450483,  19,         20) /* Value */
     , (450483,  27,          2) /* ArmorType - Leather */
     , (450483,  28,         0) /* ArmorLevel */
     , (450483,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450483, 150,        103) /* HookPlacement - Hook */
     , (450483, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450483,  22, True ) /* Inscribable */
     , (450483,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450483,  12,    0.66) /* Shade */
     , (450483,  13,    0.45) /* ArmorModVsSlash */
     , (450483,  14,     0.5) /* ArmorModVsPierce */
     , (450483,  15,    0.55) /* ArmorModVsBludgeon */
     , (450483,  16,     0.3) /* ArmorModVsCold */
     , (450483,  17,     0.3) /* ArmorModVsFire */
     , (450483,  18,     0.5) /* ArmorModVsAcid */
     , (450483,  19,     0.3) /* ArmorModVsElectric */
     , (450483, 110,       1) /* BulkMod */
     , (450483, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450483,   1, 'Drudge Mask') /* Name */
     , (450483,  16, 'A mask made from the head of a Drudge Prowler.  Its skin is very well cured, and the mask is very well put together.  A faint odor still clings to it however...') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450483,   1, 0x02000957) /* Setup */
     , (450483,   3, 0x20000014) /* SoundTable */
     , (450483,   6, 0x0400007E) /* PaletteBase */
     , (450483,   7, 0x10000253) /* ClothingBase */
     , (450483,   8, 0x06001E2F) /* Icon */
     , (450483,  22, 0x3400002B) /* PhysicsEffectTable */;
