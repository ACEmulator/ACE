DELETE FROM `weenie` WHERE `class_Id` = 450494;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450494, 'ace450494-hollowminionmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450494,   1,          4) /* ItemType - Armor */
     , (450494,   3,          4) /* PaletteTemplate - Brown */
     , (450494,   4,      16384) /* ClothingPriority - Head */
     , (450494,   5,        0) /* EncumbranceVal */
     , (450494,   8,         75) /* Mass */
     , (450494,   9,          1) /* ValidLocations - HeadWear */
     , (450494,  16,          1) /* ItemUseable - No */
     , (450494,  19,        20) /* Value */
     , (450494,  27,          2) /* ArmorType - Leather */
     , (450494,  28,         0) /* ArmorLevel */
     , (450494,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450494, 150,        103) /* HookPlacement - Hook */
     , (450494, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450494,  11, True ) /* IgnoreCollisions */
     , (450494,  13, True ) /* Ethereal */
     , (450494,  14, True ) /* GravityStatus */
     , (450494,  19, True ) /* Attackable */
     , (450494,  22, True ) /* Inscribable */
     , (450494,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450494,  12,    0.66) /* Shade */
     , (450494,  13,     0.5) /* ArmorModVsSlash */
     , (450494,  14,     0.4) /* ArmorModVsPierce */
     , (450494,  15,     0.4) /* ArmorModVsBludgeon */
     , (450494,  16,     0.6) /* ArmorModVsCold */
     , (450494,  17,     0.2) /* ArmorModVsFire */
     , (450494,  18,    0.75) /* ArmorModVsAcid */
     , (450494,  19,    0.35) /* ArmorModVsElectric */
     , (450494, 110,       1) /* BulkMod */
     , (450494, 111,       1) /* SizeMod */
     , (450494, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450494,   1, 'Hollow Minion Mask') /* Name */
     , (450494,  16, 'A mask bearing the cold, blank gaze of the Hollow Minion.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450494,   1, 0x020014D5) /* Setup */
     , (450494,   3, 0x20000014) /* SoundTable */
     , (450494,   6, 0x0400007E) /* PaletteBase */
     , (450494,   7, 0x1000064E) /* ClothingBase */
     , (450494,   8, 0x06006231) /* Icon */
     , (450494,  22, 0x3400002B) /* PhysicsEffectTable */;
