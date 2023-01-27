DELETE FROM `weenie` WHERE `class_Id` = 450347;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450347, 'ace450347-celestialhandvambracestailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450347,   1,          2) /* ItemType - Armor */
     , (450347,   4,       8192) /* ClothingPriority - OuterwearLowerArms */
     , (450347,   5,        0) /* EncumbranceVal */
     , (450347,   9,       4096) /* ValidLocations - LowerArmArmor */
     , (450347,  16,          1) /* ItemUseable - No */
     , (450347,  18,          1) /* UiEffects - Magical */
     , (450347,  19,       20) /* Value */
     , (450347,  27,         32) /* ArmorType - Metal */
     , (450347,  28,        0) /* ArmorLevel */
     , (450347,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450347, 150,        103) /* HookPlacement - Hook */
     , (450347, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450347,  22, True ) /* Inscribable */
     , (450347, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450347,   5,  -0.067) /* ManaRate */
     , (450347,  13,     1.3) /* ArmorModVsSlash */
     , (450347,  14,       1) /* ArmorModVsPierce */
     , (450347,  15,       1) /* ArmorModVsBludgeon */
     , (450347,  16,     0.4) /* ArmorModVsCold */
     , (450347,  17,     0.4) /* ArmorModVsFire */
     , (450347,  18,     0.6) /* ArmorModVsAcid */
     , (450347,  19,     0.4) /* ArmorModVsElectric */
     , (450347, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450347,   1, 'Celestial Hand Vambraces') /* Name */
     , (450347,  16, 'Celestial Hand Vambraces') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450347,   1, 0x020000D1) /* Setup */
     , (450347,   3, 0x20000014) /* SoundTable */
     , (450347,   7, 0x1000073B) /* ClothingBase */
     , (450347,   8, 0x060068F3) /* Icon */
     , (450347,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450347,  36, 0x0E000012) /* MutateFilter */;
