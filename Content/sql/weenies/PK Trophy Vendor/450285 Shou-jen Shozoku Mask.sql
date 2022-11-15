DELETE FROM `weenie` WHERE `class_Id` = 450285;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450285, 'ace450285-shoujenshozokumasktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450285,   1,          2) /* ItemType - Armor */
     , (450285,   3,          9) /* PaletteTemplate - Grey */
     , (450285,   4,      16384) /* ClothingPriority - Head */
     , (450285,   5,        0) /* EncumbranceVal */
     , (450285,   9,          1) /* ValidLocations - HeadWear */
     , (450285,  16,          1) /* ItemUseable - No */
     , (450285,  18,          1) /* UiEffects - Magical */
     , (450285,  19,      20) /* Value */
     , (450285,  28,        320) /* ArmorLevel */
     , (450285,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450285, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450285,  11, True ) /* IgnoreCollisions */
     , (450285,  13, True ) /* Ethereal */
     , (450285,  14, True ) /* GravityStatus */
     , (450285,  19, True ) /* Attackable */
     , (450285,  22, True ) /* Inscribable */
     , (450285,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450285,   5,  -0.017) /* ManaRate */
     , (450285,  13,     0.6) /* ArmorModVsSlash */
     , (450285,  14,     0.6) /* ArmorModVsPierce */
     , (450285,  15,     0.6) /* ArmorModVsBludgeon */
     , (450285,  16,     1.4) /* ArmorModVsCold */
     , (450285,  17,     0.7) /* ArmorModVsFire */
     , (450285,  18,     1.2) /* ArmorModVsAcid */
     , (450285,  19,     1.4) /* ArmorModVsElectric */
     , (450285, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450285,   1, 'Shou-jen Shozoku Mask') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450285,   1, 0x02001620) /* Setup */
     , (450285,   3, 0x20000014) /* SoundTable */
     , (450285,   7, 0x1000069C) /* ClothingBase */
     , (450285,   8, 0x060064CD) /* Icon */
     , (450285,  22, 0x3400002B) /* PhysicsEffectTable */;

