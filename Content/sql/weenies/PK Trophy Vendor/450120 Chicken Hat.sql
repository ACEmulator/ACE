DELETE FROM `weenie` WHERE `class_Id` = 450120;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450120, 'hatchickentailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450120,   1,          4) /* ItemType - Clothing */
     , (450120,   3,         61) /* PaletteTemplate - White */
     , (450120,   4,      16384) /* ClothingPriority - Head */
     , (450120,   5,         0) /* EncumbranceVal */
     , (450120,   8,         15) /* Mass */
     , (450120,   9,          1) /* ValidLocations - HeadWear */
     , (450120,  16,          1) /* ItemUseable - No */
     , (450120,  19,       20) /* Value */
     , (450120,  27,          1) /* ArmorType - Cloth */
     , (450120,  28,          0) /* ArmorLevel */
     , (450120,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450120, 150,        103) /* HookPlacement - Hook */
     , (450120, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450120,  22, True ) /* Inscribable */
     , (450120, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450120,  12,    0.66) /* Shade */
     , (450120,  13,       1) /* ArmorModVsSlash */
     , (450120,  14,       1) /* ArmorModVsPierce */
     , (450120,  15,     0.5) /* ArmorModVsBludgeon */
     , (450120,  16,     0.5) /* ArmorModVsCold */
     , (450120,  17,       1) /* ArmorModVsFire */
     , (450120,  18,       1) /* ArmorModVsAcid */
     , (450120,  19,     0.5) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450120,   1, 'Chicken Hat') /* Name */
     , (450120,  16, 'A chicken that you put on your head.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450120,   1, 0x020000D3) /* Setup */
     , (450120,   3, 0x20000014) /* SoundTable */
     , (450120,   6, 0x0400007E) /* PaletteBase */
     , (450120,   7, 0x100004CB) /* ClothingBase */
     , (450120,   8, 0x06002D80) /* Icon */
     , (450120,  22, 0x3400002B) /* PhysicsEffectTable */;
