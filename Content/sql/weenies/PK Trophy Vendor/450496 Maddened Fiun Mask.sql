DELETE FROM `weenie` WHERE `class_Id` = 450496;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450496, 'ace450496-maddenedfiunmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450496,   1,          4) /* ItemType - Armor */
     , (450496,   3,          4) /* PaletteTemplate - Brown */
     , (450496,   4,      16384) /* ClothingPriority - Head */
     , (450496,   5,        0) /* EncumbranceVal */
     , (450496,   8,         75) /* Mass */
     , (450496,   9,          1) /* ValidLocations - HeadWear */
     , (450496,  16,          1) /* ItemUseable - No */
     , (450496,  19,        20) /* Value */
     , (450496,  27,          2) /* ArmorType - Leather */
     , (450496,  28,         0) /* ArmorLevel */
     , (450496,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450496, 150,        103) /* HookPlacement - Hook */
     , (450496, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450496,  11, True ) /* IgnoreCollisions */
     , (450496,  13, True ) /* Ethereal */
     , (450496,  14, True ) /* GravityStatus */
     , (450496,  19, True ) /* Attackable */
     , (450496,  22, True ) /* Inscribable */
     , (450496,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450496,  12,    0.66) /* Shade */
     , (450496,  13,     0.5) /* ArmorModVsSlash */
     , (450496,  14,     0.4) /* ArmorModVsPierce */
     , (450496,  15,     0.4) /* ArmorModVsBludgeon */
     , (450496,  16,     0.6) /* ArmorModVsCold */
     , (450496,  17,     0.2) /* ArmorModVsFire */
     , (450496,  18,    0.75) /* ArmorModVsAcid */
     , (450496,  19,    0.35) /* ArmorModVsElectric */
     , (450496, 110,       1) /* BulkMod */
     , (450496, 111,       1) /* SizeMod */
     , (450496, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450496,   1, 'Maddened Fiun Mask') /* Name */
     , (450496,  16, 'A mask crafted after the sad and tortured visage of the Maddened Fiun.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450496,   1, 0x020014D4) /* Setup */
     , (450496,   3, 0x20000014) /* SoundTable */
     , (450496,   6, 0x0400007E) /* PaletteBase */
     , (450496,   7, 0x1000064D) /* ClothingBase */
     , (450496,   8, 0x06006230) /* Icon */
     , (450496,  22, 0x3400002B) /* PhysicsEffectTable */;
