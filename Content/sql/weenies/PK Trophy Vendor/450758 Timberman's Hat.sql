DELETE FROM `weenie` WHERE `class_Id` = 450758;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450758, 'hattimbermantailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450758,   1,          4) /* ItemType - Clothing */
     , (450758,   3,          4) /* PaletteTemplate - Brown */
     , (450758,   4,      16384) /* ClothingPriority - Head */
     , (450758,   5,        0) /* EncumbranceVal */
     , (450758,   8,         15) /* Mass */
     , (450758,   9,          1) /* ValidLocations - HeadWear */
     , (450758,  16,          1) /* ItemUseable - No */
     , (450758,  19,       20) /* Value */
     , (450758,  27,          1) /* ArmorType - Cloth */
     , (450758,  28,        0) /* ArmorLevel */
     , (450758,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450758, 150,        103) /* HookPlacement - Hook */
     , (450758, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450758,  22, True ) /* Inscribable */
     , (450758, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450758,  12,    0.66) /* Shade */
     , (450758,  13,     0.5) /* ArmorModVsSlash */
     , (450758,  14,       1) /* ArmorModVsPierce */
     , (450758,  15,       1) /* ArmorModVsBludgeon */
     , (450758,  16,       1) /* ArmorModVsCold */
     , (450758,  17,     0.5) /* ArmorModVsFire */
     , (450758,  18,     0.5) /* ArmorModVsAcid */
     , (450758,  19,       1) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450758,   1, 'Timberman''s Hat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450758,   1, 0x02000F63) /* Setup */
     , (450758,   3, 0x20000014) /* SoundTable */
     , (450758,   6, 0x0400007E) /* PaletteBase */
     , (450758,   7, 0x1000049F) /* ClothingBase */
     , (450758,   8, 0x06001357) /* Icon */
     , (450758,  22, 0x3400002B) /* PhysicsEffectTable */;
