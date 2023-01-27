DELETE FROM `weenie` WHERE `class_Id` = 450504;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450504, 'ace450504-snowmanmaskwithfez', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450504,   1,          4) /* ItemType - Armor */
     , (450504,   3,          4) /* PaletteTemplate - Brown */
     , (450504,   4,      16384) /* ClothingPriority - Head */
     , (450504,   5,        0) /* EncumbranceVal */
     , (450504,   9,          1) /* ValidLocations - HeadWear */
     , (450504,  16,          1) /* ItemUseable - No */
     , (450504,  19,        20) /* Value */
     , (450504,  28,         0) /* ArmorLevel */
     , (450504,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450504, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450504,  11, True ) /* IgnoreCollisions */
     , (450504,  13, True ) /* Ethereal */
     , (450504,  14, True ) /* GravityStatus */
     , (450504,  19, True ) /* Attackable */
     , (450504,  22, True ) /* Inscribable */
     , (450504,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450504,  12,       0) /* Shade */
     , (450504,  13,     0.3) /* ArmorModVsSlash */
     , (450504,  14,     0.5) /* ArmorModVsPierce */
     , (450504,  15,     0.4) /* ArmorModVsBludgeon */
     , (450504,  16,     0.3) /* ArmorModVsCold */
     , (450504,  17,     0.4) /* ArmorModVsFire */
     , (450504,  18,     0.3) /* ArmorModVsAcid */
     , (450504,  19,     0.3) /* ArmorModVsElectric */
     , (450504, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450504,   1, 'Snowman Mask with Fez') /* Name */
     , (450504,  16, 'A Snowman Mask accessorized with a jaunty fez.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450504,   1, 0x020014DC) /* Setup */
     , (450504,   3, 0x20000014) /* SoundTable */
     , (450504,   7, 0x10000655) /* ClothingBase */
     , (450504,   8, 0x06006235) /* Icon */
     , (450504,  22, 0x3400002B) /* PhysicsEffectTable */;
