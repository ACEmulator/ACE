DELETE FROM `weenie` WHERE `class_Id` = 450060;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450060, 'ace450060-royalknightcloaktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450060,   1,          4) /* ItemType - Clothing */
     , (450060,   4,     131072) /* ClothingPriority - 131072 */
     , (450060,   5,         75) /* EncumbranceVal */
     , (450060,   9,  134217728) /* ValidLocations - Cloak */
     , (450060,  16,          1) /* ItemUseable - No */
     , (450060,  18,          1) /* UiEffects - Magical */
     , (450060,  19,       20) /* Value */
     , (450060,  28,          0) /* ArmorLevel */
     , (450060,  36,       9999) /* ResistMagic */
     , (450060,  65,        101) /* Placement - Resting */
     , (450060,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450060, 105,          8) /* ItemWorkmanship */
     , (450060, 131,          6) /* MaterialType - Silk */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450060,   1, False) /* Stuck */
     , (450060,  11, True ) /* IgnoreCollisions */
     , (450060,  13, True ) /* Ethereal */
     , (450060,  14, True ) /* GravityStatus */
     , (450060,  19, True ) /* Attackable */
     , (450060,  22, True ) /* Inscribable */
     , (450060, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450060,  13,     0.8) /* ArmorModVsSlash */
     , (450060,  14,     0.8) /* ArmorModVsPierce */
     , (450060,  15,       1) /* ArmorModVsBludgeon */
     , (450060,  16,     0.2) /* ArmorModVsCold */
     , (450060,  17,     0.2) /* ArmorModVsFire */
     , (450060,  18,     0.1) /* ArmorModVsAcid */
     , (450060,  19,     0.2) /* ArmorModVsElectric */
     , (450060, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450060,   1, 'Royal Knight Cloak') /* Name */
     , (450060,  16, 'Cloak of Borelean') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450060,   1, 0x02001B2A) /* Setup */
     , (450060,   3, 0x20000014) /* SoundTable */
     , (450060,   7, 0x100007E9) /* ClothingBase */
     , (450060,   8, 0x06007090) /* Icon */
     , (450060,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450060,  50, 0x06006C36) /* IconOverlay */;
