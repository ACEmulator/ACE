DELETE FROM `weenie` WHERE `class_Id` = 480552;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480552, 'hatvisorpk', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480552,   1,          4) /* ItemType - Clothing */
     , (480552,   3,          8) /* PaletteTemplate - Green */
     , (480552,   4,      16384) /* ClothingPriority - Head */
     , (480552,   5,         0) /* EncumbranceVal */
     , (480552,   8,         15) /* Mass */
     , (480552,   9,          1) /* ValidLocations - HeadWear */
     , (480552,  16,          1) /* ItemUseable - No */
     , (480552,  19,          20) /* Value */
     , (480552,  27,          1) /* ArmorType - Cloth */
     , (480552,  28,          0) /* ArmorLevel */
     , (480552,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480552, 150,        103) /* HookPlacement - Hook */
     , (480552, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480552,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480552,  12,    0.66) /* Shade */
     , (480552,  13,     0.8) /* ArmorModVsSlash */
     , (480552,  14,     0.8) /* ArmorModVsPierce */
     , (480552,  15,       1) /* ArmorModVsBludgeon */
     , (480552,  16,     0.2) /* ArmorModVsCold */
     , (480552,  17,     0.2) /* ArmorModVsFire */
     , (480552,  18,     0.1) /* ArmorModVsAcid */
     , (480552,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480552,   1, 'Visor') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480552,   1, 0x02000A2D) /* Setup */
     , (480552,   3, 0x20000014) /* SoundTable */
     , (480552,   6, 0x0400007E) /* PaletteBase */
     , (480552,   7, 0x100002D7) /* ClothingBase */
     , (480552,   8, 0x06001357) /* Icon */
     , (480552,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480552,  36, 0x0E000016) /* MutateFilter */;
