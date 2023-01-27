DELETE FROM `weenie` WHERE `class_Id` = 450287;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450287, 'ace450287-shoujenshozokutrouserstailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450287,   1,          2) /* ItemType - Armor */
     , (450287,   3,          9) /* PaletteTemplate - Grey */
     , (450287,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (450287,   5,        0) /* EncumbranceVal */
     , (450287,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (450287,  16,          1) /* ItemUseable - No */
     , (450287,  18,          1) /* UiEffects - Magical */
     , (450287,  19,      20) /* Value */
     , (450287,  28,        0) /* ArmorLevel */
     , (450287,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450287,  11, True ) /* IgnoreCollisions */
     , (450287,  13, True ) /* Ethereal */
     , (450287,  14, True ) /* GravityStatus */
     , (450287,  19, True ) /* Attackable */
     , (450287,  22, True ) /* Inscribable */
     , (450287,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450287,   5,  -0.017) /* ManaRate */
     , (450287,  13,     0.6) /* ArmorModVsSlash */
     , (450287,  14,     0.6) /* ArmorModVsPierce */
     , (450287,  15,     0.6) /* ArmorModVsBludgeon */
     , (450287,  16,     1.4) /* ArmorModVsCold */
     , (450287,  17,     0.7) /* ArmorModVsFire */
     , (450287,  18,     1.2) /* ArmorModVsAcid */
     , (450287,  19,     1.4) /* ArmorModVsElectric */
     , (450287, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450287,   1, 'Shou-jen Shozoku Trousers') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450287,   1, 0x020000DD) /* Setup */
     , (450287,   3, 0x20000014) /* SoundTable */
     , (450287,   7, 0x1000069B) /* ClothingBase */
     , (450287,   8, 0x0600308B) /* Icon */
     , (450287,  22, 0x3400002B) /* PhysicsEffectTable */;


