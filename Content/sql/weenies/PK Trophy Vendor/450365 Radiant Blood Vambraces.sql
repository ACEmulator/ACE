DELETE FROM `weenie` WHERE `class_Id` = 450365;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450365, 'ace450365-radiantbloodvambracestailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450365,   1,          2) /* ItemType - Armor */
     , (450365,   4,       8192) /* ClothingPriority - OuterwearLowerArms */
     , (450365,   5,        0) /* EncumbranceVal */
     , (450365,   9,       4096) /* ValidLocations - LowerArmArmor */
     , (450365,  16,          1) /* ItemUseable - No */
     , (450365,  18,          1) /* UiEffects - Magical */
     , (450365,  19,       20) /* Value */
     , (450365,  27,         32) /* ArmorType - Metal */
     , (450365,  28,        0) /* ArmorLevel */
     , (450365,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450365, 150,        103) /* HookPlacement - Hook */
     , (450365, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450365,  22, True ) /* Inscribable */
     , (450365, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450365,   5,  -0.067) /* ManaRate */
     , (450365,  13,     1.3) /* ArmorModVsSlash */
     , (450365,  14,       1) /* ArmorModVsPierce */
     , (450365,  15,       1) /* ArmorModVsBludgeon */
     , (450365,  16,     0.4) /* ArmorModVsCold */
     , (450365,  17,     0.4) /* ArmorModVsFire */
     , (450365,  18,     0.6) /* ArmorModVsAcid */
     , (450365,  19,     0.4) /* ArmorModVsElectric */
     , (450365, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450365,   1, 'Radiant Blood Vambraces') /* Name */
     , (450365,  16, 'Radiant Blood Vambraces') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450365,   1, 0x020000D1) /* Setup */
     , (450365,   3, 0x20000014) /* SoundTable */
     , (450365,   7, 0x10000744) /* ClothingBase */
     , (450365,   8, 0x0600692E) /* Icon */
     , (450365,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450365,  36, 0x0E000012) /* MutateFilter */;
