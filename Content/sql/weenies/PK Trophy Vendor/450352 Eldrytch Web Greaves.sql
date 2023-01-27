DELETE FROM `weenie` WHERE `class_Id` = 450352;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450352, 'ace450352-eldrytchwebgreavestailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450352,   1,          2) /* ItemType - Armor */
     , (450352,   4,        512) /* ClothingPriority - OuterwearLowerLegs */
     , (450352,   5,        0) /* EncumbranceVal */
     , (450352,   9,      16384) /* ValidLocations - LowerLegArmor */
     , (450352,  16,          1) /* ItemUseable - No */
     , (450352,  18,          1) /* UiEffects - Magical */
     , (450352,  19,       20) /* Value */
     , (450352,  27,         32) /* ArmorType - Metal */
     , (450352,  28,        0) /* ArmorLevel */
     , (450352,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450352, 150,        103) /* HookPlacement - Hook */
     , (450352, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450352,  22, True ) /* Inscribable */
     , (450352, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450352,   5,  -0.067) /* ManaRate */
     , (450352,  13,     1.3) /* ArmorModVsSlash */
     , (450352,  14,       1) /* ArmorModVsPierce */
     , (450352,  15,       1) /* ArmorModVsBludgeon */
     , (450352,  16,     0.4) /* ArmorModVsCold */
     , (450352,  17,     0.4) /* ArmorModVsFire */
     , (450352,  18,     0.6) /* ArmorModVsAcid */
     , (450352,  19,     0.4) /* ArmorModVsElectric */
     , (450352,  39,    1.33) /* DefaultScale */
     , (450352, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450352,   1, 'Eldrytch Web Greaves') /* Name */
     , (450352,  16, 'Eldrytch Web Greaves') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450352,   1, 0x020000D1) /* Setup */
     , (450352,   3, 0x20000014) /* SoundTable */
     , (450352,   7, 0x10000751) /* ClothingBase */
     , (450352,   8, 0x06006947) /* Icon */
     , (450352,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450352,  36, 0x0E000012) /* MutateFilter */;
