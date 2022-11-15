DELETE FROM `weenie` WHERE `class_Id` = 450356;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450356, 'ace450356-eldrytchwebvambracestailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450356,   1,          2) /* ItemType - Armor */
     , (450356,   4,       8192) /* ClothingPriority - OuterwearLowerArms */
     , (450356,   5,        0) /* EncumbranceVal */
     , (450356,   9,       4096) /* ValidLocations - LowerArmArmor */
     , (450356,  16,          1) /* ItemUseable - No */
     , (450356,  18,          1) /* UiEffects - Magical */
     , (450356,  19,       20) /* Value */
     , (450356,  27,         32) /* ArmorType - Metal */
     , (450356,  28,        0) /* ArmorLevel */
     , (450356,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450356, 150,        103) /* HookPlacement - Hook */
     , (450356, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450356,  22, True ) /* Inscribable */
     , (450356, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450356,   5,  -0.067) /* ManaRate */
     , (450356,  13,     1.3) /* ArmorModVsSlash */
     , (450356,  14,       1) /* ArmorModVsPierce */
     , (450356,  15,       1) /* ArmorModVsBludgeon */
     , (450356,  16,     0.4) /* ArmorModVsCold */
     , (450356,  17,     0.4) /* ArmorModVsFire */
     , (450356,  18,     0.6) /* ArmorModVsAcid */
     , (450356,  19,     0.4) /* ArmorModVsElectric */
     , (450356, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450356,   1, 'Eldrytch Web Vambraces') /* Name */
     , (450356,  16, 'Eldrytch Web Vambraces') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450356,   1, 0x020000D1) /* Setup */
     , (450356,   3, 0x20000014) /* SoundTable */
     , (450356,   7, 0x1000074D) /* ClothingBase */
     , (450356,   8, 0x06006944) /* Icon */
     , (450356,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450356,  36, 0x0E000012) /* MutateFilter */;
