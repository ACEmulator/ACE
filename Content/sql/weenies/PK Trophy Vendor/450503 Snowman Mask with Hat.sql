DELETE FROM `weenie` WHERE `class_Id` = 450503;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450503, 'ace450503-snowmanmaskwithhattailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450503,   1,          4) /* ItemType - Armor */
     , (450503,   3,          4) /* PaletteTemplate - Brown */
     , (450503,   4,      16384) /* ClothingPriority - Head */
     , (450503,   5,        0) /* EncumbranceVal */
     , (450503,   8,         75) /* Mass */
     , (450503,   9,          1) /* ValidLocations - HeadWear */
     , (450503,  16,          1) /* ItemUseable - No */
     , (450503,  19,        20) /* Value */
     , (450503,  27,          2) /* ArmorType - Leather */
     , (450503,  28,         0) /* ArmorLevel */
     , (450503,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450503, 150,        103) /* HookPlacement - Hook */
     , (450503, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450503,  11, True ) /* IgnoreCollisions */
     , (450503,  13, True ) /* Ethereal */
     , (450503,  14, True ) /* GravityStatus */
     , (450503,  19, True ) /* Attackable */
     , (450503,  22, True ) /* Inscribable */
     , (450503,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450503,  12,    0.66) /* Shade */
     , (450503,  13,    0.29) /* ArmorModVsSlash */
     , (450503,  14,     0.5) /* ArmorModVsPierce */
     , (450503,  15,    0.29) /* ArmorModVsBludgeon */
     , (450503,  16,    0.29) /* ArmorModVsCold */
     , (450503,  17,    0.43) /* ArmorModVsFire */
     , (450503,  18,    0.29) /* ArmorModVsAcid */
     , (450503,  19,    0.29) /* ArmorModVsElectric */
     , (450503, 110,       1) /* BulkMod */
     , (450503, 111,       1) /* SizeMod */
     , (450503, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450503,   1, 'Snowman Mask with Hat') /* Name */
     , (450503,  16, 'A Snowman Mask accessorized with a stylish hat.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450503,   1, 0x020014DE) /* Setup */
     , (450503,   3, 0x20000014) /* SoundTable */
     , (450503,   6, 0x0400007E) /* PaletteBase */
     , (450503,   7, 0x10000657) /* ClothingBase */
     , (450503,   8, 0x06006236) /* Icon */
     , (450503,  22, 0x3400002B) /* PhysicsEffectTable */;
