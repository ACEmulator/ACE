DELETE FROM `weenie` WHERE `class_Id` = 450355;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450355, 'ace450355-eldrytchwebtassetstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450355,   1,          2) /* ItemType - Armor */
     , (450355,   4,        256) /* ClothingPriority - OuterwearUpperLegs */
     , (450355,   5,        0) /* EncumbranceVal */
     , (450355,   9,       8192) /* ValidLocations - UpperLegArmor */
     , (450355,  16,          1) /* ItemUseable - No */
     , (450355,  18,          1) /* UiEffects - Magical */
     , (450355,  19,       20) /* Value */
     , (450355,  27,         32) /* ArmorType - Metal */
     , (450355,  28,        0) /* ArmorLevel */
     , (450355,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450355, 150,        103) /* HookPlacement - Hook */
     , (450355, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450355,  22, True ) /* Inscribable */
     , (450355, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450355,   5,  -0.067) /* ManaRate */
     , (450355,  13,     1.3) /* ArmorModVsSlash */
     , (450355,  14,       1) /* ArmorModVsPierce */
     , (450355,  15,       1) /* ArmorModVsBludgeon */
     , (450355,  16,     0.4) /* ArmorModVsCold */
     , (450355,  17,     0.4) /* ArmorModVsFire */
     , (450355,  18,     0.6) /* ArmorModVsAcid */
     , (450355,  19,     0.4) /* ArmorModVsElectric */
     , (450355,  39,    1.33) /* DefaultScale */
     , (450355, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450355,   1, 'Eldrytch Web Tassets') /* Name */
     , (450355,  16, 'Eldrytch Web Tassets') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450355,   1, 0x020000E0) /* Setup */
     , (450355,   3, 0x20000014) /* SoundTable */
     , (450355,   7, 0x10000755) /* ClothingBase */
     , (450355,   8, 0x0600694B) /* Icon */
     , (450355,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450355,  36, 0x0E000012) /* MutateFilter */;
