DELETE FROM `weenie` WHERE `class_Id` = 450704;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450704, 'ace450704-cloaktailorwhand', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450704,   1,          4) /* ItemType - Clothing */
     , (450704,   3,          33) /* PaletteTemplate - AquaBlue */
     , (450704,   4,     131072) /* ClothingPriority - 131072 */
     , (450704,   5,         75) /* EncumbranceVal */
     , (450704,   9,  134217728) /* ValidLocations - Cloak */
     , (450704,  16,          1) /* ItemUseable - No */
     , (450704,  18,          1) /* UiEffects - Magical */
     , (450704,  19,         20) /* Value */
     , (450704,  28,          0) /* ArmorLevel */
     , (450704,  36,       9999) /* ResistMagic */
     , (450704,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450704, 169,         16) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450704,  22, True ) /* Inscribable */
     , (450704,  84, True ) /* IgnoreCloIcons */
     , (450704, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450704,  13,     0.8) /* ArmorModVsSlash */
     , (450704,  14,     0.8) /* ArmorModVsPierce */
     , (450704,  15,       1) /* ArmorModVsBludgeon */
     , (450704,  16,     0.2) /* ArmorModVsCold */
     , (450704,  17,     0.2) /* ArmorModVsFire */
     , (450704,  18,     0.1) /* ArmorModVsAcid */
     , (450704,  19,     0.2) /* ArmorModVsElectric */
     , (450704, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450704,   1, 'Cloak') /* Name */
     , (450704,  16, 'Cloak') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450704,   1, 0x02001B2A) /* Setup */
     , (450704,   3, 0x20000014) /* SoundTable */
     , (450704,   7, 0x100007F7) /* ClothingBase */
     , (450704,   8, 0x060070A7) /* Icon */
     , (450704,  22, 0x3400002B) /* PhysicsEffectTable */;

