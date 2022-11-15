DELETE FROM `weenie` WHERE `class_Id` = 450288;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450288, 'ace450288-shoujenshozokusleevegauntletstailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450288,   1,          2) /* ItemType - Armor */
     , (450288,   3,          9) /* PaletteTemplate - Grey */
     , (450288,   4,      32768) /* ClothingPriority - Hands */
     , (450288,   5,        0) /* EncumbranceVal */
     , (450288,   9,         32) /* ValidLocations - HandWear */
     , (450288,  16,          1) /* ItemUseable - No */
     , (450288,  18,          1) /* UiEffects - Magical */
     , (450288,  19,      20) /* Value */
     , (450288,  28,        0) /* ArmorLevel */
     , (450288,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450288,  11, True ) /* IgnoreCollisions */
     , (450288,  13, True ) /* Ethereal */
     , (450288,  14, True ) /* GravityStatus */
     , (450288,  19, True ) /* Attackable */
     , (450288,  22, True ) /* Inscribable */
     , (450288,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450288,   5,  -0.017) /* ManaRate */
     , (450288,  13,     0.6) /* ArmorModVsSlash */
     , (450288,  14,     0.6) /* ArmorModVsPierce */
     , (450288,  15,     0.6) /* ArmorModVsBludgeon */
     , (450288,  16,     1.4) /* ArmorModVsCold */
     , (450288,  17,     0.7) /* ArmorModVsFire */
     , (450288,  18,     1.2) /* ArmorModVsAcid */
     , (450288,  19,     1.4) /* ArmorModVsElectric */
     , (450288, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450288,   1, 'Shou-jen Shozoku Sleeve Gauntlets') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450288,   1, 0x020000D8) /* Setup */
     , (450288,   3, 0x20000014) /* SoundTable */
     , (450288,   6, 0x0400007E) /* PaletteBase */
     , (450288,   7, 0x1000069A) /* ClothingBase */
     , (450288,   8, 0x06002E7D) /* Icon */
     , (450288,  22, 0x3400002B) /* PhysicsEffectTable */;
