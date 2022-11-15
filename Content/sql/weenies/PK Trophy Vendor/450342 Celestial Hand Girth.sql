DELETE FROM `weenie` WHERE `class_Id` = 450342;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450342, 'ace450342-celestialhandgirthtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450342,   1,          2) /* ItemType - Armor */
     , (450342,   4,       2048) /* ClothingPriority - OuterwearAbdomen */
     , (450342,   5,        0) /* EncumbranceVal */
     , (450342,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450342,  16,          1) /* ItemUseable - No */
     , (450342,  18,          1) /* UiEffects - Magical */
     , (450342,  19,       20) /* Value */
     , (450342,  27,         32) /* ArmorType - Metal */
     , (450342,  28,        0) /* ArmorLevel */
     , (450342,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450342, 150,        103) /* HookPlacement - Hook */
     , (450342, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450342,  22, True ) /* Inscribable */
     , (450342, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450342,   5,  -0.067) /* ManaRate */
     , (450342,  13,     1.3) /* ArmorModVsSlash */
     , (450342,  14,       1) /* ArmorModVsPierce */
     , (450342,  15,       1) /* ArmorModVsBludgeon */
     , (450342,  16,     0.4) /* ArmorModVsCold */
     , (450342,  17,     0.4) /* ArmorModVsFire */
     , (450342,  18,     0.6) /* ArmorModVsAcid */
     , (450342,  19,     0.4) /* ArmorModVsElectric */
     , (450342, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450342,   1, 'Celestial Hand Girth') /* Name */
     , (450342,  16, 'Celestial Hand Girth') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450342,   1, 0x020000D7) /* Setup */
     , (450342,   3, 0x20000014) /* SoundTable */
     , (450342,   7, 0x1000073E) /* ClothingBase */
     , (450342,   8, 0x060068F5) /* Icon */
     , (450342,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450342,  36, 0x0E000012) /* MutateFilter */;
