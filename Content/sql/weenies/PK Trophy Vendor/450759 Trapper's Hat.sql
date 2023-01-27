DELETE FROM `weenie` WHERE `class_Id` = 450759;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450759, 'hattrappertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450759,   1,          4) /* ItemType - Clothing */
     , (450759,   3,          4) /* PaletteTemplate - Brown */
     , (450759,   4,      16384) /* ClothingPriority - Head */
     , (450759,   5,        0) /* EncumbranceVal */
     , (450759,   8,         15) /* Mass */
     , (450759,   9,          1) /* ValidLocations - HeadWear */
     , (450759,  16,          1) /* ItemUseable - No */
     , (450759,  19,       20) /* Value */
     , (450759,  27,          1) /* ArmorType - Cloth */
     , (450759,  28,        0) /* ArmorLevel */
     , (450759,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450759, 150,        103) /* HookPlacement - Hook */
     , (450759, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450759,  22, True ) /* Inscribable */
     , (450759, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450759,  12,    0.66) /* Shade */
     , (450759,  13,       1) /* ArmorModVsSlash */
     , (450759,  14,     0.5) /* ArmorModVsPierce */
     , (450759,  15,       1) /* ArmorModVsBludgeon */
     , (450759,  16,     0.5) /* ArmorModVsCold */
     , (450759,  17,       1) /* ArmorModVsFire */
     , (450759,  18,       1) /* ArmorModVsAcid */
     , (450759,  19,     0.5) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450759,   1, 'Trapper''s Hat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450759,   1, 0x02000F62) /* Setup */
     , (450759,   3, 0x20000014) /* SoundTable */
     , (450759,   6, 0x0400007E) /* PaletteBase */
     , (450759,   7, 0x1000049D) /* ClothingBase */
     , (450759,   8, 0x06001357) /* Icon */
     , (450759,  22, 0x3400002B) /* PhysicsEffectTable */;
