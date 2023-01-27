DELETE FROM `weenie` WHERE `class_Id` = 450344;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450344, 'ace450344-celestialhandhelmtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450344,   1,          2) /* ItemType - Armor */
     , (450344,   4,      16384) /* ClothingPriority - Head */
     , (450344,   5,        0) /* EncumbranceVal */
     , (450344,   9,          1) /* ValidLocations - HeadWear */
     , (450344,  16,          1) /* ItemUseable - No */
     , (450344,  18,          1) /* UiEffects - Magical */
     , (450344,  19,       20) /* Value */
     , (450344,  27,         32) /* ArmorType - Metal */
     , (450344,  28,        0) /* ArmorLevel */
     , (450344,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450344, 150,        103) /* HookPlacement - Hook */
     , (450344, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450344,  22, True ) /* Inscribable */
     , (450344, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450344,   5,  -0.067) /* ManaRate */
     , (450344,  13,     1.3) /* ArmorModVsSlash */
     , (450344,  14,       1) /* ArmorModVsPierce */
     , (450344,  15,       1) /* ArmorModVsBludgeon */
     , (450344,  16,     0.4) /* ArmorModVsCold */
     , (450344,  17,     0.4) /* ArmorModVsFire */
     , (450344,  18,     0.6) /* ArmorModVsAcid */
     , (450344,  19,     0.4) /* ArmorModVsElectric */
     , (450344, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450344,   1, 'Celestial Hand Helm') /* Name */
     , (450344,  16, 'Celestial Hand Helm') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450344,   1, 0x02001630) /* Setup */
     , (450344,   3, 0x20000014) /* SoundTable */
     , (450344,   7, 0x10000740) /* ClothingBase */
     , (450344,   8, 0x060068F7) /* Icon */
     , (450344,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450344,  36, 0x0E000012) /* MutateFilter */;
