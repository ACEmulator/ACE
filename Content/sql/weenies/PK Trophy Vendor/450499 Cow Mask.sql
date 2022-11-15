DELETE FROM `weenie` WHERE `class_Id` = 450499;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450499, 'ace450499-cowmasktailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450499,   1,          4) /* ItemType - Armor */
     , (450499,   3,          4) /* PaletteTemplate - Brown */
     , (450499,   4,      16384) /* ClothingPriority - Head */
     , (450499,   5,        0) /* EncumbranceVal */
     , (450499,   9,          1) /* ValidLocations - HeadWear */
     , (450499,  16,          1) /* ItemUseable - No */
     , (450499,  19,        20) /* Value */
     , (450499,  28,         0) /* ArmorLevel */
     , (450499,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450499, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450499,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450499,  13,     0.5) /* ArmorModVsSlash */
     , (450499,  14,     0.4) /* ArmorModVsPierce */
     , (450499,  15,     0.4) /* ArmorModVsBludgeon */
     , (450499,  16,     0.6) /* ArmorModVsCold */
     , (450499,  17,     0.2) /* ArmorModVsFire */
     , (450499,  18,    0.75) /* ArmorModVsAcid */
     , (450499,  19,    0.35) /* ArmorModVsElectric */
     , (450499, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450499,   1, 'Cow Mask') /* Name */
     , (450499,  16, 'A cow mask') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450499,   1, 0x02001740) /* Setup */
     , (450499,   3, 0x20000014) /* SoundTable */
     , (450499,   7, 0x100006E3) /* ClothingBase */
     , (450499,   8, 0x060066D7) /* Icon */
     , (450499,  22, 0x3400002B) /* PhysicsEffectTable */;
