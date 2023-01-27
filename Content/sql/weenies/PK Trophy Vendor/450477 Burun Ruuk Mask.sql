DELETE FROM `weenie` WHERE `class_Id` = 450477;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450477, 'maskburunruuktailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450477,   1,          4) /* ItemType - Armor */
     , (450477,   3,          4) /* PaletteTemplate - Brown */
     , (450477,   4,      16384) /* ClothingPriority - Head */
     , (450477,   5,        0) /* EncumbranceVal */
     , (450477,   8,         75) /* Mass */
     , (450477,   9,          1) /* ValidLocations - HeadWear */
     , (450477,  16,          1) /* ItemUseable - No */
     , (450477,  19,        20) /* Value */
     , (450477,  27,          2) /* ArmorType - Leather */
     , (450477,  28,         0) /* ArmorLevel */
     , (450477,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450477, 150,        101) /* HookPlacement - Resting */
     , (450477, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450477,  22, True ) /* Inscribable */
     , (450477,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450477,  12,    0.66) /* Shade */
     , (450477,  13,     0.5) /* ArmorModVsSlash */
     , (450477,  14,     0.4) /* ArmorModVsPierce */
     , (450477,  15,     0.4) /* ArmorModVsBludgeon */
     , (450477,  16,     0.6) /* ArmorModVsCold */
     , (450477,  17,     0.2) /* ArmorModVsFire */
     , (450477,  18,    0.75) /* ArmorModVsAcid */
     , (450477,  19,    0.35) /* ArmorModVsElectric */
     , (450477,  39,       1) /* DefaultScale */
     , (450477, 110,       1) /* BulkMod */
     , (450477, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450477,   1, 'Burun Ruuk Mask') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450477,   1, 0x020011D2) /* Setup */
     , (450477,   3, 0x20000014) /* SoundTable */
     , (450477,   6, 0x0400007E) /* PaletteBase */
     , (450477,   7, 0x1000057F) /* ClothingBase */
     , (450477,   8, 0x060035D8) /* Icon */
     , (450477,  22, 0x3400002B) /* PhysicsEffectTable */;
