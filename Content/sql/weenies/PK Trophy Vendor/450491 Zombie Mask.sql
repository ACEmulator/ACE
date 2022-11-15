DELETE FROM `weenie` WHERE `class_Id` = 450491;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450491, 'maskzombietailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450491,   1,          4) /* ItemType - Armor */
     , (450491,   3,          4) /* PaletteTemplate - Brown */
     , (450491,   4,      16384) /* ClothingPriority - Head */
     , (450491,   5,        0) /* EncumbranceVal */
     , (450491,   8,         75) /* Mass */
     , (450491,   9,          1) /* ValidLocations - HeadWear */
     , (450491,  16,          1) /* ItemUseable - No */
     , (450491,  19,         20) /* Value */
     , (450491,  27,          2) /* ArmorType - Leather */
     , (450491,  28,         0) /* ArmorLevel */
     , (450491,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450491, 150,        103) /* HookPlacement - Hook */
     , (450491, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450491,  22, True ) /* Inscribable */
     , (450491,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450491,  12,    0.66) /* Shade */
     , (450491,  13,     0.5) /* ArmorModVsSlash */
     , (450491,  14,    0.35) /* ArmorModVsPierce */
     , (450491,  15,   0.375) /* ArmorModVsBludgeon */
     , (450491,  16,     0.2) /* ArmorModVsCold */
     , (450491,  17,     0.5) /* ArmorModVsFire */
     , (450491,  18,   0.375) /* ArmorModVsAcid */
     , (450491,  19,    0.43) /* ArmorModVsElectric */
     , (450491, 110,       1) /* BulkMod */
     , (450491, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450491,   1, 'Zombie Mask') /* Name */
     , (450491,  16, 'A mask made from the head of a zombie.  Its skin is dry, though the mask is very well put together.  The odor of undeath still clings to it, however...') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450491,   1, 0x02000B73) /* Setup */
     , (450491,   3, 0x20000014) /* SoundTable */
     , (450491,   6, 0x0400007E) /* PaletteBase */
     , (450491,   7, 0x1000032C) /* ClothingBase */
     , (450491,   8, 0x060022A4) /* Icon */
     , (450491,  22, 0x3400002B) /* PhysicsEffectTable */;
