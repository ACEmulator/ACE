DELETE FROM `weenie` WHERE `class_Id` = 450293;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450293, 'ace450293-reinforcedshoujenshozokugauntletstailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450293,   1,          2) /* ItemType - Armor */
     , (450293,   3,          9) /* PaletteTemplate - Grey */
     , (450293,   4,      32768) /* ClothingPriority - Hands */
     , (450293,   5,        0) /* EncumbranceVal */
     , (450293,   9,         32) /* ValidLocations - HandWear */
     , (450293,  16,          1) /* ItemUseable - No */
     , (450293,  18,          1) /* UiEffects - Magical */
     , (450293,  19,      20) /* Value */
     , (450293,  28,        0) /* ArmorLevel */
     , (450293,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450293,  11, True ) /* IgnoreCollisions */
     , (450293,  13, True ) /* Ethereal */
     , (450293,  14, True ) /* GravityStatus */
     , (450293,  19, True ) /* Attackable */
     , (450293,  22, True ) /* Inscribable */
     , (450293, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450293,   5,  -0.017) /* ManaRate */
     , (450293,  12,       0) /* Shade */
     , (450293,  13,     0.6) /* ArmorModVsSlash */
     , (450293,  14,     0.6) /* ArmorModVsPierce */
     , (450293,  15,     0.6) /* ArmorModVsBludgeon */
     , (450293,  16,     1.4) /* ArmorModVsCold */
     , (450293,  17,     0.7) /* ArmorModVsFire */
     , (450293,  18,     1.2) /* ArmorModVsAcid */
     , (450293,  19,     1.4) /* ArmorModVsElectric */
     , (450293, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450293,   1, 'Reinforced Shou-jen Shozoku Gauntlets') /* Name */
     , (450293,  33, 'HoshinoFortArmorPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450293,   1, 0x020000D8) /* Setup */
     , (450293,   3, 0x20000014) /* SoundTable */
     , (450293,   6, 0x0400007E) /* PaletteBase */
     , (450293,   7, 0x10000836) /* ClothingBase */
     , (450293,   8, 0x06002E8C) /* Icon */
     , (450293,  22, 0x3400002B) /* PhysicsEffectTable */;


