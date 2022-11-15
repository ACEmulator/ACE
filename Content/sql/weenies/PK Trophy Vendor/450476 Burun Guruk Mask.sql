DELETE FROM `weenie` WHERE `class_Id` = 450476;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450476, 'maskburunguruktailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450476,   1,          4) /* ItemType - Armor */
     , (450476,   3,          4) /* PaletteTemplate - Brown */
     , (450476,   4,      16384) /* ClothingPriority - Head */
     , (450476,   5,        150) /* EncumbranceVal */
     , (450476,   8,         75) /* Mass */
     , (450476,   9,          1) /* ValidLocations - HeadWear */
     , (450476,  16,          1) /* ItemUseable - No */
     , (450476,  19,        20) /* Value */
     , (450476,  27,          2) /* ArmorType - Leather */
     , (450476,  28,         0) /* ArmorLevel */
     , (450476,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450476, 150,        101) /* HookPlacement - Resting */
     , (450476, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450476,  22, True ) /* Inscribable */
     , (450476,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450476,  12,    0.66) /* Shade */
     , (450476,  13,     0.5) /* ArmorModVsSlash */
     , (450476,  14,     0.4) /* ArmorModVsPierce */
     , (450476,  15,     0.4) /* ArmorModVsBludgeon */
     , (450476,  16,     0.6) /* ArmorModVsCold */
     , (450476,  17,     0.2) /* ArmorModVsFire */
     , (450476,  18,    0.75) /* ArmorModVsAcid */
     , (450476,  19,    0.35) /* ArmorModVsElectric */
     , (450476,  39,       1) /* DefaultScale */
     , (450476, 110,       1) /* BulkMod */
     , (450476, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450476,   1, 'Burun Guruk Mask') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450476,   1, 0x020011D3) /* Setup */
     , (450476,   3, 0x20000014) /* SoundTable */
     , (450476,   6, 0x0400007E) /* PaletteBase */
     , (450476,   7, 0x1000057D) /* ClothingBase */
     , (450476,   8, 0x060035D6) /* Icon */
     , (450476,  22, 0x3400002B) /* PhysicsEffectTable */;
