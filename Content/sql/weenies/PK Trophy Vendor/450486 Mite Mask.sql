DELETE FROM `weenie` WHERE `class_Id` = 450486;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450486, 'maskmitetailor	', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450486,   1,          4) /* ItemType - Armor */
     , (450486,   3,          4) /* PaletteTemplate - Brown */
     , (450486,   4,      16384) /* ClothingPriority - Head */
     , (450486,   5,        0) /* EncumbranceVal */
     , (450486,   8,         75) /* Mass */
     , (450486,   9,          1) /* ValidLocations - HeadWear */
     , (450486,  16,          1) /* ItemUseable - No */
     , (450486,  19,        20) /* Value */
     , (450486,  27,          2) /* ArmorType - Leather */
     , (450486,  28,         0) /* ArmorLevel */
     , (450486,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450486, 150,        101) /* HookPlacement - Resting */
     , (450486, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450486,  22, True ) /* Inscribable */
     , (450486,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450486,  12,    0.66) /* Shade */
     , (450486,  13,     0.5) /* ArmorModVsSlash */
     , (450486,  14,     0.4) /* ArmorModVsPierce */
     , (450486,  15,     0.4) /* ArmorModVsBludgeon */
     , (450486,  16,     0.6) /* ArmorModVsCold */
     , (450486,  17,     0.2) /* ArmorModVsFire */
     , (450486,  18,    0.75) /* ArmorModVsAcid */
     , (450486,  19,    0.35) /* ArmorModVsElectric */
     , (450486,  39,       1) /* DefaultScale */
     , (450486, 110,       1) /* BulkMod */
     , (450486, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450486,   1, 'Mite Mask') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450486,   1, 0x020011D1) /* Setup */
     , (450486,   3, 0x20000014) /* SoundTable */
     , (450486,   6, 0x0400007E) /* PaletteBase */
     , (450486,   7, 0x1000057E) /* ClothingBase */
     , (450486,   8, 0x060035DA) /* Icon */
     , (450486,  22, 0x3400002B) /* PhysicsEffectTable */;
