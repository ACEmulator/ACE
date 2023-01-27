DELETE FROM `weenie` WHERE `class_Id` = 450354;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450354, 'ace450354-eldrytchwebpauldronstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450354,   1,          2) /* ItemType - Armor */
     , (450354,   4,       4096) /* ClothingPriority - OuterwearUpperArms */
     , (450354,   5,        0) /* EncumbranceVal */
     , (450354,   9,       2048) /* ValidLocations - UpperArmArmor */
     , (450354,  16,          1) /* ItemUseable - No */
     , (450354,  18,          1) /* UiEffects - Magical */
     , (450354,  19,       20) /* Value */
     , (450354,  27,         32) /* ArmorType - Metal */
     , (450354,  28,        0) /* ArmorLevel */
     , (450354,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450354, 150,        103) /* HookPlacement - Hook */
     , (450354, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450354,  22, True ) /* Inscribable */
     , (450354, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450354,   5,  -0.067) /* ManaRate */
     , (450354,  13,     1.3) /* ArmorModVsSlash */
     , (450354,  14,       1) /* ArmorModVsPierce */
     , (450354,  15,       1) /* ArmorModVsBludgeon */
     , (450354,  16,     0.4) /* ArmorModVsCold */
     , (450354,  17,     0.4) /* ArmorModVsFire */
     , (450354,  18,     0.6) /* ArmorModVsAcid */
     , (450354,  19,     0.4) /* ArmorModVsElectric */
     , (450354,  39,     1.1) /* DefaultScale */
     , (450354, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450354,   1, 'Eldrytch Web Pauldrons') /* Name */
     , (450354,  16, 'Eldrytch Web Pauldrons') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450354,   1, 0x020000D1) /* Setup */
     , (450354,   3, 0x20000014) /* SoundTable */
     , (450354,   7, 0x10000753) /* ClothingBase */
     , (450354,   8, 0x06006949) /* Icon */
     , (450354,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450354,  36, 0x0E000012) /* MutateFilter */;
