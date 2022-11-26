DELETE FROM `weenie` WHERE `class_Id` = 450757;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450757, 'hatminertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450757,   1,          4) /* ItemType - Clothing */
     , (450757,   3,          4) /* PaletteTemplate - Brown */
     , (450757,   4,      16384) /* ClothingPriority - Head */
     , (450757,   5,        0) /* EncumbranceVal */
     , (450757,   8,         15) /* Mass */
     , (450757,   9,          1) /* ValidLocations - HeadWear */
     , (450757,  16,          1) /* ItemUseable - No */
     , (450757,  19,       20) /* Value */
     , (450757,  27,          1) /* ArmorType - Cloth */
     , (450757,  28,        0) /* ArmorLevel */
     , (450757,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450757, 150,        103) /* HookPlacement - Hook */
     , (450757, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450757,  22, True ) /* Inscribable */
     , (450757, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450757,  12,    0.66) /* Shade */
     , (450757,  13,       1) /* ArmorModVsSlash */
     , (450757,  14,       1) /* ArmorModVsPierce */
     , (450757,  15,     0.5) /* ArmorModVsBludgeon */
     , (450757,  16,     0.5) /* ArmorModVsCold */
     , (450757,  17,       1) /* ArmorModVsFire */
     , (450757,  18,       1) /* ArmorModVsAcid */
     , (450757,  19,     0.5) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450757,   1, 'Miner''s Hat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450757,   1, 0x02000F61) /* Setup */
     , (450757,   3, 0x20000014) /* SoundTable */
     , (450757,   6, 0x0400007E) /* PaletteBase */
     , (450757,   7, 0x1000049E) /* ClothingBase */
     , (450757,   8, 0x06001357) /* Icon */
     , (450757,  22, 0x3400002B) /* PhysicsEffectTable */;
