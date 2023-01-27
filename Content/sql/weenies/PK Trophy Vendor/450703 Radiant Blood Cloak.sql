DELETE FROM `weenie` WHERE `class_Id` = 450703;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450703, 'ace450703-cloaktailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450703,   1,          4) /* ItemType - Clothing */
     , (450703,   3,          33) /* PaletteTemplate - AquaBlue */
     , (450703,   4,     131072) /* ClothingPriority - 131072 */
     , (450703,   5,         75) /* EncumbranceVal */
     , (450703,   9,  134217728) /* ValidLocations - Cloak */
     , (450703,  16,          1) /* ItemUseable - No */
     , (450703,  18,          1) /* UiEffects - Magical */
     , (450703,  19,         20) /* Value */
     , (450703,  28,          0) /* ArmorLevel */
     , (450703,  36,       9999) /* ResistMagic */
     , (450703,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450703, 169,         16) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450703,  22, True ) /* Inscribable */
     , (450703,  84, True ) /* IgnoreCloIcons */
     , (450703, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450703,  13,     0.8) /* ArmorModVsSlash */
     , (450703,  14,     0.8) /* ArmorModVsPierce */
     , (450703,  15,       1) /* ArmorModVsBludgeon */
     , (450703,  16,     0.2) /* ArmorModVsCold */
     , (450703,  17,     0.2) /* ArmorModVsFire */
     , (450703,  18,     0.1) /* ArmorModVsAcid */
     , (450703,  19,     0.2) /* ArmorModVsElectric */
     , (450703, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450703,   1, 'Cloak') /* Name */
     , (450703,  16, 'Cloak') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450703,   1, 0x02001B2A) /* Setup */
     , (450703,   3, 0x20000014) /* SoundTable */
     , (450703,   7, 0x100007F8) /* ClothingBase */
     , (450703,   8, 0x060070A8) /* Icon */
     , (450703,  22, 0x3400002B) /* PhysicsEffectTable */;
