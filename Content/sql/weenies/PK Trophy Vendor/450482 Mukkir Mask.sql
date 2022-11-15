DELETE FROM `weenie` WHERE `class_Id` = 450482;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450482, 'ace450482-mukkirmasktailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450482,   1,          4) /* ItemType - Armor */
     , (450482,   3,         39) /* PaletteTemplate - Black */
     , (450482,   4,      16384) /* ClothingPriority - Head */
     , (450482,   5,        0) /* EncumbranceVal */
     , (450482,   9,          1) /* ValidLocations - HeadWear */
     , (450482,  16,          1) /* ItemUseable - No */
     , (450482,  19,        20) /* Value */
     , (450482,  28,         0) /* ArmorLevel */
     , (450482,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450482, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450482,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450482,  13,     0.5) /* ArmorModVsSlash */
     , (450482,  14,     0.4) /* ArmorModVsPierce */
     , (450482,  15,     0.4) /* ArmorModVsBludgeon */
     , (450482,  16,     0.6) /* ArmorModVsCold */
     , (450482,  17,     0.2) /* ArmorModVsFire */
     , (450482,  18,    0.75) /* ArmorModVsAcid */
     , (450482,  19,    0.35) /* ArmorModVsElectric */
     , (450482, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450482,   1, 'Mukkir Mask') /* Name */
     , (450482,  16, 'A terrifying mask, crafted from the head of a powerful Mukkir.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450482,   1, 0x02001744) /* Setup */
     , (450482,   3, 0x20000014) /* SoundTable */
     , (450482,   7, 0x100006E6) /* ClothingBase */
     , (450482,   8, 0x060066E1) /* Icon */
     , (450482,  22, 0x3400002B) /* PhysicsEffectTable */;
