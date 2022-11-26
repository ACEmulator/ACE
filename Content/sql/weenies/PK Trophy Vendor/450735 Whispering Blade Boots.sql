DELETE FROM `weenie` WHERE `class_Id` = 450735;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450735, 'ace450735-whisperingbladebootstailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450735,   1,          2) /* ItemType - Armor */
     , (450735,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450735,   4,      65536) /* ClothingPriority - Feet */
     , (450735,   5,        0) /* EncumbranceVal */
     , (450735,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450735,  16,          1) /* ItemUseable - No */
     , (450735,  18,          1) /* UiEffects - Magical */
     , (450735,  19,       20) /* Value */
     , (450735,  28,        0) /* ArmorLevel */
     , (450735,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450735, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450735,  11, True ) /* IgnoreCollisions */
     , (450735,  13, True ) /* Ethereal */
     , (450735,  14, True ) /* GravityStatus */
     , (450735,  19, True ) /* Attackable */
     , (450735,  22, True ) /* Inscribable */
     , (450735,  69, True ) /* IsSellable */
     , (450735, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450735,   5,   -0.05) /* ManaRate */
     , (450735,  12,       0) /* Shade */
     , (450735,  13,     1.1) /* ArmorModVsSlash */
     , (450735,  14,     1.1) /* ArmorModVsPierce */
     , (450735,  15,     1.1) /* ArmorModVsBludgeon */
     , (450735,  16,     0.8) /* ArmorModVsCold */
     , (450735,  17,     0.7) /* ArmorModVsFire */
     , (450735,  18,     0.8) /* ArmorModVsAcid */
     , (450735,  19,     0.7) /* ArmorModVsElectric */
     , (450735, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450735,   1, 'Whispering Blade Boots') /* Name */
     , (450735,  16, 'Well-crafted armored boots, known to be worn by members of the mysterious Whispering Blade.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450735,   1, 0x0200151A) /* Setup */
     , (450735,   3, 0x20000014) /* SoundTable */
     , (450735,   7, 0x10000669) /* ClothingBase */
     , (450735,   8, 0x060062CF) /* Icon */
     , (450735,  22, 0x3400002B) /* PhysicsEffectTable */;

