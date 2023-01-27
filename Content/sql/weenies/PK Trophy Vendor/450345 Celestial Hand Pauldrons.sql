DELETE FROM `weenie` WHERE `class_Id` = 450345;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450345, 'ace450345-celestialhandpauldronstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450345,   1,          2) /* ItemType - Armor */
     , (450345,   4,       4096) /* ClothingPriority - OuterwearUpperArms */
     , (450345,   5,        0) /* EncumbranceVal */
     , (450345,   9,       2048) /* ValidLocations - UpperArmArmor */
     , (450345,  16,          1) /* ItemUseable - No */
     , (450345,  18,          1) /* UiEffects - Magical */
     , (450345,  19,       20) /* Value */
     , (450345,  27,         32) /* ArmorType - Metal */
     , (450345,  28,        0) /* ArmorLevel */
     , (450345,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450345, 150,        103) /* HookPlacement - Hook */
     , (450345, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450345,  22, True ) /* Inscribable */
     , (450345, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450345,   5,  -0.067) /* ManaRate */
     , (450345,  13,     1.3) /* ArmorModVsSlash */
     , (450345,  14,       1) /* ArmorModVsPierce */
     , (450345,  15,       1) /* ArmorModVsBludgeon */
     , (450345,  16,     0.4) /* ArmorModVsCold */
     , (450345,  17,     0.4) /* ArmorModVsFire */
     , (450345,  18,     0.6) /* ArmorModVsAcid */
     , (450345,  19,     0.4) /* ArmorModVsElectric */
     , (450345,  39,     1.1) /* DefaultScale */
     , (450345, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450345,   1, 'Celestial Hand Pauldrons') /* Name */
     , (450345,  16, 'Celestial Hand Pauldrons') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450345,   1, 0x020000D1) /* Setup */
     , (450345,   3, 0x20000014) /* SoundTable */
     , (450345,   7, 0x10000741) /* ClothingBase */
     , (450345,   8, 0x060068F8) /* Icon */
     , (450345,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450345,  36, 0x0E000012) /* MutateFilter */;
