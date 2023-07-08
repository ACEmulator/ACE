DELETE FROM `weenie` WHERE `class_Id` = 450736;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450736, 'ace450736-rossumortabootstailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450736,   1,          2) /* ItemType - Armor */
     , (450736,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450736,   4,      65536) /* ClothingPriority - Feet */
     , (450736,   5,        0) /* EncumbranceVal */
     , (450736,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450736,  16,          1) /* ItemUseable - No */
     , (450736,  18,          1) /* UiEffects - Magical */
     , (450736,  19,       20) /* Value */
     , (450736,  28,        0) /* ArmorLevel */
     , (450736,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450736, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450736,  11, True ) /* IgnoreCollisions */
     , (450736,  13, True ) /* Ethereal */
     , (450736,  14, True ) /* GravityStatus */
     , (450736,  19, True ) /* Attackable */
     , (450736,  22, True ) /* Inscribable */
     , (450736,  69, True ) /* IsSellable */
     , (450736, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450736,   5,   -0.05) /* ManaRate */
     , (450736,  12,       0) /* Shade */
     , (450736,  13,     1.1) /* ArmorModVsSlash */
     , (450736,  14,     1.1) /* ArmorModVsPierce */
     , (450736,  15,     1.1) /* ArmorModVsBludgeon */
     , (450736,  16,     0.8) /* ArmorModVsCold */
     , (450736,  17,     0.7) /* ArmorModVsFire */
     , (450736,  18,     0.8) /* ArmorModVsAcid */
     , (450736,  19,     0.7) /* ArmorModVsElectric */
     , (450736, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450736,   1, 'Rossu Morta Boots') /* Name */
     , (450736,  16, 'Well-crafted boots worn by the fearsome Ordina Rossu Morta of Viamont.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450736,   1, 0x0200151B) /* Setup */
     , (450736,   3, 0x20000014) /* SoundTable */
     , (450736,   7, 0x1000066A) /* ClothingBase */
     , (450736,   8, 0x060062D1) /* Icon */
     , (450736,  22, 0x3400002B) /* PhysicsEffectTable */;

