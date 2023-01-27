DELETE FROM `weenie` WHERE `class_Id` = 450350;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450350, 'ace450350-eldrytchwebgauntletstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450350,   1,          2) /* ItemType - Armor */
     , (450350,   4,      32768) /* ClothingPriority - Hands */
     , (450350,   5,        0) /* EncumbranceVal */
     , (450350,   9,         32) /* ValidLocations - HandWear */
     , (450350,  16,          1) /* ItemUseable - No */
     , (450350,  18,          1) /* UiEffects - Magical */
     , (450350,  19,       20) /* Value */
     , (450350,  27,         32) /* ArmorType - Metal */
     , (450350,  28,        0) /* ArmorLevel */
     , (450350,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450350, 150,        103) /* HookPlacement - Hook */
     , (450350, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450350,  22, True ) /* Inscribable */
     , (450350, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450350,   5,  -0.067) /* ManaRate */
     , (450350,  13,     1.3) /* ArmorModVsSlash */
     , (450350,  14,       1) /* ArmorModVsPierce */
     , (450350,  15,       1) /* ArmorModVsBludgeon */
     , (450350,  16,     0.4) /* ArmorModVsCold */
     , (450350,  17,     0.4) /* ArmorModVsFire */
     , (450350,  18,     0.6) /* ArmorModVsAcid */
     , (450350,  19,     0.4) /* ArmorModVsElectric */
     , (450350, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450350,   1, 'Eldrytch Web Gauntlets') /* Name */
     , (450350,  16, 'Eldrytch Web Gauntlets') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450350,   1, 0x020000D8) /* Setup */
     , (450350,   3, 0x20000014) /* SoundTable */
     , (450350,   7, 0x1000074F) /* ClothingBase */
     , (450350,   8, 0x060061E1) /* Icon */
     , (450350,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450350,  36, 0x0E000012) /* MutateFilter */;
