DELETE FROM `weenie` WHERE `class_Id` = 450505;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450505, 'ace450505-giantsnowmanmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450505,   1,          4) /* ItemType - Armor */
     , (450505,   3,          4) /* PaletteTemplate - Brown */
     , (450505,   4,      16384) /* ClothingPriority - Head */
     , (450505,   5,        0) /* EncumbranceVal */
     , (450505,   8,         75) /* Mass */
     , (450505,   9,          1) /* ValidLocations - HeadWear */
     , (450505,  16,          1) /* ItemUseable - No */
     , (450505,  19,        20) /* Value */
     , (450505,  27,          2) /* ArmorType - Leather */
     , (450505,  28,         0) /* ArmorLevel */
     , (450505,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450505, 150,        103) /* HookPlacement - Hook */
     , (450505, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450505,  11, True ) /* IgnoreCollisions */
     , (450505,  13, True ) /* Ethereal */
     , (450505,  14, True ) /* GravityStatus */
     , (450505,  19, True ) /* Attackable */
     , (450505,  22, True ) /* Inscribable */
     , (450505,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450505,  12,    0.66) /* Shade */
     , (450505,  13,     0.5) /* ArmorModVsSlash */
     , (450505,  14,     0.4) /* ArmorModVsPierce */
     , (450505,  15,     0.4) /* ArmorModVsBludgeon */
     , (450505,  16,     0.6) /* ArmorModVsCold */
     , (450505,  17,     0.2) /* ArmorModVsFire */
     , (450505,  18,    0.75) /* ArmorModVsAcid */
     , (450505,  19,    0.35) /* ArmorModVsElectric */
     , (450505, 110,       1) /* BulkMod */
     , (450505, 111,       1) /* SizeMod */
     , (450505, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450505,   1, 'Giant Snowman Mask') /* Name */
     , (450505,  16, 'A mask crafted from the hollowed-out head of a Giant Snowman.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450505,   1, 0x020014DD) /* Setup */
     , (450505,   3, 0x20000014) /* SoundTable */
     , (450505,   6, 0x0400007E) /* PaletteBase */
     , (450505,   7, 0x10000656) /* ClothingBase */
     , (450505,   8, 0x06006237) /* Icon */
     , (450505,  22, 0x3400002B) /* PhysicsEffectTable */;
