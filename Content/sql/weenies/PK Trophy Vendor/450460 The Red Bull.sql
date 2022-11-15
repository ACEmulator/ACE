DELETE FROM `weenie` WHERE `class_Id` = 450460;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450460, 'ace450460-theredbulltailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450460,   1,          2) /* ItemType - Armor */
     , (450460,   3,         14) /* PaletteTemplate - Red */
     , (450460,   4,      16384) /* ClothingPriority - Head */
     , (450460,   5,        0) /* EncumbranceVal */
     , (450460,   9,          1) /* ValidLocations - HeadWear */
     , (450460,  16,          1) /* ItemUseable - No */
     , (450460,  18,          1) /* UiEffects - Magical */
     , (450460,  19,       20) /* Value */
     , (450460,  28,        0) /* ArmorLevel */
     , (450460,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450460, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450460,  22, True ) /* Inscribable */
     , (450460,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450460,   5,  -0.033) /* ManaRate */
     , (450460,  13,       1) /* ArmorModVsSlash */
     , (450460,  14,     1.2) /* ArmorModVsPierce */
     , (450460,  15,     1.2) /* ArmorModVsBludgeon */
     , (450460,  16,    1.35) /* ArmorModVsCold */
     , (450460,  17,    1.35) /* ArmorModVsFire */
     , (450460,  18,    1.35) /* ArmorModVsAcid */
     , (450460,  19,    1.35) /* ArmorModVsElectric */
     , (450460, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450460,   1, 'The Red Bull') /* Name */
     , (450460,  16, 'This amazingly well-crafted mask, made in the manner of Viamontian High Heraldry, is a stylized representation of the Red Bull of Sanamar.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450460,   1, 0x0200162A) /* Setup */
     , (450460,   3, 0x20000014) /* SoundTable */
     , (450460,   7, 0x100006A2) /* ClothingBase */
     , (450460,   8, 0x060064FE) /* Icon */
     , (450460,  22, 0x3400002B) /* PhysicsEffectTable */;
