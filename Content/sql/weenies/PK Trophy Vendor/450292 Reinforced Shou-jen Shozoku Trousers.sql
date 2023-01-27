DELETE FROM `weenie` WHERE `class_Id` = 450292;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450292, 'ace450292-reinforcedshoujenshozokutrousers', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450292,   1,          2) /* ItemType - Armor */
     , (450292,   3,          9) /* PaletteTemplate - Grey */
     , (450292,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (450292,   5,        0) /* EncumbranceVal */
     , (450292,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (450292,  16,          1) /* ItemUseable - No */
     , (450292,  18,          1) /* UiEffects - Magical */
     , (450292,  19,      20) /* Value */
     , (450292,  28,        0) /* ArmorLevel */
     , (450292,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450292,  11, True ) /* IgnoreCollisions */
     , (450292,  13, True ) /* Ethereal */
     , (450292,  14, True ) /* GravityStatus */
     , (450292,  19, True ) /* Attackable */
     , (450292,  22, True ) /* Inscribable */
     , (450292,  69, False) /* IsSellable */
     , (450292, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450292,   5,  -0.017) /* ManaRate */
     , (450292,  12,       0) /* Shade */
     , (450292,  13,     0.6) /* ArmorModVsSlash */
     , (450292,  14,     0.6) /* ArmorModVsPierce */
     , (450292,  15,     0.6) /* ArmorModVsBludgeon */
     , (450292,  16,     1.4) /* ArmorModVsCold */
     , (450292,  17,     0.7) /* ArmorModVsFire */
     , (450292,  18,     1.2) /* ArmorModVsAcid */
     , (450292,  19,     1.4) /* ArmorModVsElectric */
     , (450292, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450292,   1, 'Reinforced Shou-jen Shozoku Trousers') /* Name */
     , (450292,  33, 'HoshinoFortArmorPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450292,   1, 0x020000DD) /* Setup */
     , (450292,   3, 0x20000014) /* SoundTable */
     , (450292,   6, 0x0400007E) /* PaletteBase */
     , (450292,   7, 0x10000837) /* ClothingBase */
     , (450292,   8, 0x0600308B) /* Icon */
     , (450292,  22, 0x3400002B) /* PhysicsEffectTable */;
