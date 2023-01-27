DELETE FROM `weenie` WHERE `class_Id` = 450346;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450346, 'ace450346-celestialhandtassetstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450346,   1,          2) /* ItemType - Armor */
     , (450346,   4,        256) /* ClothingPriority - OuterwearUpperLegs */
     , (450346,   5,        0) /* EncumbranceVal */
     , (450346,   9,       8192) /* ValidLocations - UpperLegArmor */
     , (450346,  16,          1) /* ItemUseable - No */
     , (450346,  18,          1) /* UiEffects - Magical */
     , (450346,  19,       20) /* Value */
     , (450346,  27,         32) /* ArmorType - Metal */
     , (450346,  28,        0) /* ArmorLevel */
     , (450346,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450346, 150,        103) /* HookPlacement - Hook */
     , (450346, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450346,  22, True ) /* Inscribable */
     , (450346, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450346,   5,  -0.067) /* ManaRate */
     , (450346,  13,     1.3) /* ArmorModVsSlash */
     , (450346,  14,       1) /* ArmorModVsPierce */
     , (450346,  15,       1) /* ArmorModVsBludgeon */
     , (450346,  16,     0.4) /* ArmorModVsCold */
     , (450346,  17,     0.4) /* ArmorModVsFire */
     , (450346,  18,     0.6) /* ArmorModVsAcid */
     , (450346,  19,     0.4) /* ArmorModVsElectric */
     , (450346,  39,    1.33) /* DefaultScale */
     , (450346, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450346,   1, 'Celestial Hand Tassets') /* Name */
     , (450346,  16, 'Celestial Hand Tassets') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450346,   1, 0x020000E0) /* Setup */
     , (450346,   3, 0x20000014) /* SoundTable */
     , (450346,   7, 0x10000743) /* ClothingBase */
     , (450346,   8, 0x060068FA) /* Icon */
     , (450346,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450346,  36, 0x0E000012) /* MutateFilter */;
