DELETE FROM `weenie` WHERE `class_Id` = 450357;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450357, 'ace450357-eldrytchwebsolleretstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450357,   1,          2) /* ItemType - Armor */
     , (450357,   4,      65536) /* ClothingPriority - Feet */
     , (450357,   5,        0) /* EncumbranceVal */
     , (450357,   9,        256) /* ValidLocations - FootWear */
     , (450357,  16,          1) /* ItemUseable - No */
     , (450357,  18,          1) /* UiEffects - Magical */
     , (450357,  19,       20) /* Value */
     , (450357,  27,         32) /* ArmorType - Metal */
     , (450357,  28,        0) /* ArmorLevel */
     , (450357,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450357, 150,        103) /* HookPlacement - Hook */
     , (450357, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450357,  22, True ) /* Inscribable */
     , (450357, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450357,   5,  -0.067) /* ManaRate */
     , (450357,  13,     1.3) /* ArmorModVsSlash */
     , (450357,  14,       1) /* ArmorModVsPierce */
     , (450357,  15,       1) /* ArmorModVsBludgeon */
     , (450357,  16,     0.4) /* ArmorModVsCold */
     , (450357,  17,     0.4) /* ArmorModVsFire */
     , (450357,  18,     0.6) /* ArmorModVsAcid */
     , (450357,  19,     0.4) /* ArmorModVsElectric */
     , (450357, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450357,   1, 'Eldrytch Web Sollerets') /* Name */
     , (450357,  16, 'Eldrytch Web Sollerets') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450357,   1, 0x020000DE) /* Setup */
     , (450357,   3, 0x20000014) /* SoundTable */
     , (450357,   7, 0x10000754) /* ClothingBase */
     , (450357,   8, 0x0600694A) /* Icon */
     , (450357,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450357,  36, 0x0E000012) /* MutateFilter */;
