DELETE FROM `weenie` WHERE `class_Id` = 450364;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450364, 'ace450364-radiantbloodtassetstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450364,   1,          2) /* ItemType - Armor */
     , (450364,   4,        256) /* ClothingPriority - OuterwearUpperLegs */
     , (450364,   5,        0) /* EncumbranceVal */
     , (450364,   9,       8192) /* ValidLocations - UpperLegArmor */
     , (450364,  16,          1) /* ItemUseable - No */
     , (450364,  18,          1) /* UiEffects - Magical */
     , (450364,  19,       20) /* Value */
     , (450364,  27,         32) /* ArmorType - Metal */
     , (450364,  28,        0) /* ArmorLevel */
     , (450364,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450364, 150,        103) /* HookPlacement - Hook */
     , (450364, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450364,  22, True ) /* Inscribable */
     , (450364, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450364,   5,  -0.067) /* ManaRate */
     , (450364,  13,     1.3) /* ArmorModVsSlash */
     , (450364,  14,       1) /* ArmorModVsPierce */
     , (450364,  15,       1) /* ArmorModVsBludgeon */
     , (450364,  16,     0.4) /* ArmorModVsCold */
     , (450364,  17,     0.4) /* ArmorModVsFire */
     , (450364,  18,     0.6) /* ArmorModVsAcid */
     , (450364,  19,     0.4) /* ArmorModVsElectric */
     , (450364,  39,    1.33) /* DefaultScale */
     , (450364, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450364,   1, 'Radiant Blood Tassets') /* Name */
     , (450364,  16, 'Radiant Blood Tassets') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450364,   1, 0x020000E0) /* Setup */
     , (450364,   3, 0x20000014) /* SoundTable */
     , (450364,   7, 0x1000074C) /* ClothingBase */
     , (450364,   8, 0x06006935) /* Icon */
     , (450364,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450364,  36, 0x0E000012) /* MutateFilter */;
