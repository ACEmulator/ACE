DELETE FROM `weenie` WHERE `class_Id` = 450348;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450348, 'ace450348-celestialhandsolleretstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450348,   1,          2) /* ItemType - Armor */
     , (450348,   4,      65536) /* ClothingPriority - Feet */
     , (450348,   5,        0) /* EncumbranceVal */
     , (450348,   9,        256) /* ValidLocations - FootWear */
     , (450348,  16,          1) /* ItemUseable - No */
     , (450348,  18,          1) /* UiEffects - Magical */
     , (450348,  19,       20) /* Value */
     , (450348,  27,         32) /* ArmorType - Metal */
     , (450348,  28,        0) /* ArmorLevel */
     , (450348,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450348, 150,        103) /* HookPlacement - Hook */
     , (450348, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450348,  22, True ) /* Inscribable */
     , (450348, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450348,   5,  -0.067) /* ManaRate */
     , (450348,  13,     1.3) /* ArmorModVsSlash */
     , (450348,  14,       1) /* ArmorModVsPierce */
     , (450348,  15,       1) /* ArmorModVsBludgeon */
     , (450348,  16,     0.4) /* ArmorModVsCold */
     , (450348,  17,     0.4) /* ArmorModVsFire */
     , (450348,  18,     0.6) /* ArmorModVsAcid */
     , (450348,  19,     0.4) /* ArmorModVsElectric */
     , (450348, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450348,   1, 'Celestial Hand Sollerets') /* Name */
     , (450348,  16, 'Celestial Hand Sollerets') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450348,   1, 0x020000DE) /* Setup */
     , (450348,   3, 0x20000014) /* SoundTable */
     , (450348,   7, 0x10000742) /* ClothingBase */
     , (450348,   8, 0x060068F9) /* Icon */
     , (450348,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450348,  36, 0x0E000012) /* MutateFilter */;
