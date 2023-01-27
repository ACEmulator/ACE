DELETE FROM `weenie` WHERE `class_Id` = 450341;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450341, 'ace450341-celestialhandgauntletstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450341,   1,          2) /* ItemType - Armor */
     , (450341,   4,      32768) /* ClothingPriority - Hands */
     , (450341,   5,        0) /* EncumbranceVal */
     , (450341,   9,         32) /* ValidLocations - HandWear */
     , (450341,  16,          1) /* ItemUseable - No */
     , (450341,  18,          1) /* UiEffects - Magical */
     , (450341,  19,       20) /* Value */
     , (450341,  27,         32) /* ArmorType - Metal */
     , (450341,  28,        0) /* ArmorLevel */
     , (450341,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450341, 150,        103) /* HookPlacement - Hook */
     , (450341, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450341,  22, True ) /* Inscribable */
     , (450341, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450341,   5,  -0.067) /* ManaRate */
     , (450341,  13,     1.3) /* ArmorModVsSlash */
     , (450341,  14,       1) /* ArmorModVsPierce */
     , (450341,  15,       1) /* ArmorModVsBludgeon */
     , (450341,  16,     0.4) /* ArmorModVsCold */
     , (450341,  17,     0.4) /* ArmorModVsFire */
     , (450341,  18,     0.6) /* ArmorModVsAcid */
     , (450341,  19,     0.4) /* ArmorModVsElectric */
     , (450341, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450341,   1, 'Celestial Hand Gauntlets') /* Name */
     , (450341,  16, 'Celestial Hand Gauntlets') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450341,   1, 0x020000D8) /* Setup */
     , (450341,   3, 0x20000014) /* SoundTable */
     , (450341,   7, 0x1000073D) /* ClothingBase */
     , (450341,   8, 0x060061E0) /* Icon */
     , (450341,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450341,  36, 0x0E000012) /* MutateFilter */;
