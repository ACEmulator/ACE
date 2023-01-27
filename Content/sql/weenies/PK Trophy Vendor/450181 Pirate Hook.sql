DELETE FROM `weenie` WHERE `class_Id` = 450181;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450181, 'glovehooktailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450181,   1,          4) /* ItemType - Clothing */
     , (450181,   3,          4) /* PaletteTemplate - Brown */
     , (450181,   4,      32768) /* ClothingPriority - Hands */
     , (450181,   5,        0) /* EncumbranceVal */
     , (450181,   8,         25) /* Mass */
     , (450181,   9,         32) /* ValidLocations - HandWear */
     , (450181,  16,          1) /* ItemUseable - No */
     , (450181,  19,        20) /* Value */
     , (450181,  27,          1) /* ArmorType - Cloth */
     , (450181,  28,         10) /* ArmorLevel */
     , (450181,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450181,  22, True ) /* Inscribable */
     , (450181,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450181,  12,    0.66) /* Shade */
     , (450181,  13,       1) /* ArmorModVsSlash */
     , (450181,  14,     0.8) /* ArmorModVsPierce */
     , (450181,  15,       1) /* ArmorModVsBludgeon */
     , (450181,  16,     0.5) /* ArmorModVsCold */
     , (450181,  17,     0.5) /* ArmorModVsFire */
     , (450181,  18,     0.3) /* ArmorModVsAcid */
     , (450181,  19,     0.6) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450181,   1, 'Pirate Hook') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450181,   1, 0x020011E9) /* Setup */
     , (450181,   3, 0x20000014) /* SoundTable */
     , (450181,   6, 0x0400007E) /* PaletteBase */
     , (450181,   7, 0x10000587) /* ClothingBase */
     , (450181,   8, 0x060035F2) /* Icon */
     , (450181,  22, 0x3400002B) /* PhysicsEffectTable */;
