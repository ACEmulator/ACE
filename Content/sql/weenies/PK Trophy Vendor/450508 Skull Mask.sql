DELETE FROM `weenie` WHERE `class_Id` = 450508;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450508, 'maskskulltailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450508,   1,          4) /* ItemType - Armor */
     , (450508,   3,          4) /* PaletteTemplate - Brown */
     , (450508,   4,      16384) /* ClothingPriority - Head */
     , (450508,   5,        0) /* EncumbranceVal */
     , (450508,   8,         75) /* Mass */
     , (450508,   9,          1) /* ValidLocations - HeadWear */
     , (450508,  16,          1) /* ItemUseable - No */
     , (450508,  19,        20) /* Value */
     , (450508,  27,          2) /* ArmorType - Leather */
     , (450508,  28,         0) /* ArmorLevel */
     , (450508,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450508, 150,        103) /* HookPlacement - Hook */
     , (450508, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450508,  22, True ) /* Inscribable */
     , (450508,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450508,  12,    0.66) /* Shade */
     , (450508,  13,     0.6) /* ArmorModVsSlash */
     , (450508,  14,     1.5) /* ArmorModVsPierce */
     , (450508,  15,    0.25) /* ArmorModVsBludgeon */
     , (450508,  16,       1) /* ArmorModVsCold */
     , (450508,  17,     0.5) /* ArmorModVsFire */
     , (450508,  18,    0.75) /* ArmorModVsAcid */
     , (450508,  19,       1) /* ArmorModVsElectric */
     , (450508, 110,       1) /* BulkMod */
     , (450508, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450508,   1, 'Skull Mask') /* Name */
     , (450508,  16, 'A very well assembled Skeletal Mask.  It has a movable jaw, and is well padded on the inside to better insulate you from the environment.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450508,   1, 0x02000959) /* Setup */
     , (450508,   3, 0x20000014) /* SoundTable */
     , (450508,   6, 0x0400007E) /* PaletteBase */
     , (450508,   7, 0x10000255) /* ClothingBase */
     , (450508,   8, 0x06001E31) /* Icon */
     , (450508,  22, 0x3400002B) /* PhysicsEffectTable */;
