DELETE FROM `weenie` WHERE `class_Id` = 450340;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450340, 'ace450340-celestialhandbreastplatetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450340,   1,          2) /* ItemType - Armor */
     , (450340,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450340,   5,       0) /* EncumbranceVal */
     , (450340,   9,        512) /* ValidLocations - ChestArmor */
     , (450340,  16,          1) /* ItemUseable - No */
     , (450340,  18,          1) /* UiEffects - Magical */
     , (450340,  19,      20) /* Value */
     , (450340,  27,         32) /* ArmorType - Metal */
     , (450340,  28,        0) /* ArmorLevel */
     , (450340,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450340, 150,        103) /* HookPlacement - Hook */
     , (450340, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450340,  22, True ) /* Inscribable */
     , (450340, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450340,   5,  -0.067) /* ManaRate */
     , (450340,  13,     1.3) /* ArmorModVsSlash */
     , (450340,  14,       1) /* ArmorModVsPierce */
     , (450340,  15,       1) /* ArmorModVsBludgeon */
     , (450340,  16,     0.4) /* ArmorModVsCold */
     , (450340,  17,     0.4) /* ArmorModVsFire */
     , (450340,  18,     0.6) /* ArmorModVsAcid */
     , (450340,  19,     0.4) /* ArmorModVsElectric */
     , (450340, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450340,   1, 'Celestial Hand Breastplate') /* Name */
     , (450340,  16, 'Celestial Hand Breastplate') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450340,   1, 0x020000D2) /* Setup */
     , (450340,   3, 0x20000014) /* SoundTable */
     , (450340,   7, 0x1000073C) /* ClothingBase */
     , (450340,   8, 0x060068F4) /* Icon */
     , (450340,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450340,  36, 0x0E000012) /* MutateFilter */;
