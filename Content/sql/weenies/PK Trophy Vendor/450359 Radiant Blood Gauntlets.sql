DELETE FROM `weenie` WHERE `class_Id` = 450359;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450359, 'ace450359-radiantbloodgauntletstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450359,   1,          2) /* ItemType - Armor */
     , (450359,   4,      32768) /* ClothingPriority - Hands */
     , (450359,   5,        0) /* EncumbranceVal */
     , (450359,   9,         32) /* ValidLocations - HandWear */
     , (450359,  16,          1) /* ItemUseable - No */
     , (450359,  18,          1) /* UiEffects - Magical */
     , (450359,  19,       20) /* Value */
     , (450359,  27,         32) /* ArmorType - Metal */
     , (450359,  28,        0) /* ArmorLevel */
     , (450359,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450359, 150,        103) /* HookPlacement - Hook */
     , (450359, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450359,  22, True ) /* Inscribable */
     , (450359, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450359,   5,  -0.067) /* ManaRate */
     , (450359,  13,     1.3) /* ArmorModVsSlash */
     , (450359,  14,       1) /* ArmorModVsPierce */
     , (450359,  15,       1) /* ArmorModVsBludgeon */
     , (450359,  16,     0.4) /* ArmorModVsCold */
     , (450359,  17,     0.4) /* ArmorModVsFire */
     , (450359,  18,     0.6) /* ArmorModVsAcid */
     , (450359,  19,     0.4) /* ArmorModVsElectric */
     , (450359, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450359,   1, 'Radiant Blood Gauntlets') /* Name */
     , (450359,  16, 'Radiant Blood Gauntlets') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450359,   1, 0x020000D8) /* Setup */
     , (450359,   3, 0x20000014) /* SoundTable */
     , (450359,   7, 0x10000746) /* ClothingBase */
     , (450359,   8, 0x060061E3) /* Icon */
     , (450359,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450359,  36, 0x0E000012) /* MutateFilter */;
