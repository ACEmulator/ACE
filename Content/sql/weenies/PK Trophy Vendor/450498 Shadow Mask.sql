DELETE FROM `weenie` WHERE `class_Id` = 450498;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450498, 'ace450498-shadowmasktailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450498,   1,          4) /* ItemType - Armor */
     , (450498,   3,         39) /* PaletteTemplate - Black */
     , (450498,   4,      16384) /* ClothingPriority - Head */
     , (450498,   5,        0) /* EncumbranceVal */
     , (450498,   9,          1) /* ValidLocations - HeadWear */
     , (450498,  16,          1) /* ItemUseable - No */
     , (450498,  19,        20) /* Value */
     , (450498,  28,         0) /* ArmorLevel */
     , (450498,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450498, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450498,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450498,  13,     0.5) /* ArmorModVsSlash */
     , (450498,  14,     0.4) /* ArmorModVsPierce */
     , (450498,  15,     0.4) /* ArmorModVsBludgeon */
     , (450498,  16,     0.6) /* ArmorModVsCold */
     , (450498,  17,     0.2) /* ArmorModVsFire */
     , (450498,  18,    0.75) /* ArmorModVsAcid */
     , (450498,  19,    0.35) /* ArmorModVsElectric */
     , (450498, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450498,   1, 'Shadow Mask') /* Name */
     , (450498,  16, 'A translucent mask, crafted from the head of a powerful Shadow.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450498,   1, 0x02001525) /* Setup */
     , (450498,   3, 0x20000014) /* SoundTable */
     , (450498,   7, 0x10000674) /* ClothingBase */
     , (450498,   8, 0x060064E3) /* Icon */
     , (450498,  22, 0x3400002B) /* PhysicsEffectTable */;
