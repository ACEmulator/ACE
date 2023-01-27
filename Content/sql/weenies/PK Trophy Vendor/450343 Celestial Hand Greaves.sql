DELETE FROM `weenie` WHERE `class_Id` = 450343;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450343, 'ace450343-celestialhandgreavestailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450343,   1,          2) /* ItemType - Armor */
     , (450343,   4,        512) /* ClothingPriority - OuterwearLowerLegs */
     , (450343,   5,        0) /* EncumbranceVal */
     , (450343,   9,      16384) /* ValidLocations - LowerLegArmor */
     , (450343,  16,          1) /* ItemUseable - No */
     , (450343,  18,          1) /* UiEffects - Magical */
     , (450343,  19,       20) /* Value */
     , (450343,  27,         32) /* ArmorType - Metal */
     , (450343,  28,        0) /* ArmorLevel */
     , (450343,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450343, 150,        103) /* HookPlacement - Hook */
     , (450343, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450343,  22, True ) /* Inscribable */
     , (450343, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450343,   5,  -0.067) /* ManaRate */
     , (450343,  13,     1.3) /* ArmorModVsSlash */
     , (450343,  14,       1) /* ArmorModVsPierce */
     , (450343,  15,       1) /* ArmorModVsBludgeon */
     , (450343,  16,     0.4) /* ArmorModVsCold */
     , (450343,  17,     0.4) /* ArmorModVsFire */
     , (450343,  18,     0.6) /* ArmorModVsAcid */
     , (450343,  19,     0.4) /* ArmorModVsElectric */
     , (450343,  39,    1.33) /* DefaultScale */
     , (450343, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450343,   1, 'Celestial Hand Greaves') /* Name */
     , (450343,  16, 'Celestial Hand Greaves') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450343,   1, 0x020000D1) /* Setup */
     , (450343,   3, 0x20000014) /* SoundTable */
     , (450343,   7, 0x1000073F) /* ClothingBase */
     , (450343,   8, 0x060068F6) /* Icon */
     , (450343,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450343,  36, 0x0E000012) /* MutateFilter */;
