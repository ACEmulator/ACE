DELETE FROM `weenie` WHERE `class_Id` = 450100;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450100, 'ace450100-undeadcaptainmasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450100,   1,          4) /* ItemType - Armor */
     , (450100,   3,          4) /* PaletteTemplate - Brown */
     , (450100,   4,      16384) /* ClothingPriority - Head */
     , (450100,   5,        0) /* EncumbranceVal */
     , (450100,   8,         75) /* Mass */
     , (450100,   9,          1) /* ValidLocations - HeadWear */
     , (450100,  16,          1) /* ItemUseable - No */
     , (450100,  19,        20) /* Value */
     , (450100,  27,          2) /* ArmorType - Leather */
     , (450100,  28,         0) /* ArmorLevel */
     , (450100,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450100, 150,        103) /* HookPlacement - Hook */
     , (450100, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450100,  11, True ) /* IgnoreCollisions */
     , (450100,  13, True ) /* Ethereal */
     , (450100,  14, True ) /* GravityStatus */
     , (450100,  19, True ) /* Attackable */
     , (450100,  22, True ) /* Inscribable */
     , (450100,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450100,  12,    0.66) /* Shade */
     , (450100,  13,     0.5) /* ArmorModVsSlash */
     , (450100,  14,    0.45) /* ArmorModVsPierce */
     , (450100,  15,    0.45) /* ArmorModVsBludgeon */
     , (450100,  16,     0.6) /* ArmorModVsCold */
     , (450100,  17,     0.2) /* ArmorModVsFire */
     , (450100,  18,     0.8) /* ArmorModVsAcid */
     , (450100,  19,     0.3) /* ArmorModVsElectric */
     , (450100, 110,       1) /* BulkMod */
     , (450100, 111,       1) /* SizeMod */
     , (450100, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450100,   1, 'Undead Captain Mask') /* Name */
     , (450100,  16, 'An Undead Captain mask, complete with jaunty hat.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450100,   1, 0x02001758) /* Setup */
     , (450100,   3, 0x20000014) /* SoundTable */
     , (450100,   6, 0x0400007E) /* PaletteBase */
     , (450100,   7, 0x100006EF) /* ClothingBase */
     , (450100,   8, 0x06006721) /* Icon */
     , (450100,  22, 0x3400002B) /* PhysicsEffectTable */;
