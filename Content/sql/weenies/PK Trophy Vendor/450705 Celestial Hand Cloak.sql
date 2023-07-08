DELETE FROM `weenie` WHERE `class_Id` = 450705;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450705, 'ace450705-cloaktailorwhand', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450705,   1,          4) /* ItemType - Clothing */
     , (450705,   3,          33) /* PaletteTemplate - AquaBlue */
     , (450705,   4,     131072) /* ClothingPriority - 131072 */
     , (450705,   5,         75) /* EncumbranceVal */
     , (450705,   9,  134217728) /* ValidLocations - Cloak */
     , (450705,  16,          1) /* ItemUseable - No */
     , (450705,  18,          1) /* UiEffects - Magical */
     , (450705,  19,         20) /* Value */
     , (450705,  28,          0) /* ArmorLevel */
     , (450705,  36,       9999) /* ResistMagic */
     , (450705,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450705, 169,         16) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450705,  22, True ) /* Inscribable */
     , (450705,  84, True ) /* IgnoreCloIcons */
     , (450705, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450705,  13,     0.8) /* ArmorModVsSlash */
     , (450705,  14,     0.8) /* ArmorModVsPierce */
     , (450705,  15,       1) /* ArmorModVsBludgeon */
     , (450705,  16,     0.2) /* ArmorModVsCold */
     , (450705,  17,     0.2) /* ArmorModVsFire */
     , (450705,  18,     0.1) /* ArmorModVsAcid */
     , (450705,  19,     0.2) /* ArmorModVsElectric */
     , (450705, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450705,   1, 'Cloak') /* Name */
     , (450705,  16, 'Cloak') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450705,   1, 0x02001B2A) /* Setup */
     , (450705,   3, 0x20000014) /* SoundTable */
     , (450705,   7, 0x100007F5) /* ClothingBase */
     , (450705,   8, 0x060070A5) /* Icon */
     , (450705,  22, 0x3400002B) /* PhysicsEffectTable */;

