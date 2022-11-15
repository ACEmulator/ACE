DELETE FROM `weenie` WHERE `class_Id` = 450017;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450017, 'ace450017-vestirioverrobetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450017,   1,          4) /* ItemType - Armor */
     , (450017,   3,         13) /* PaletteTemplate - Purple */
     , (450017,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450017,   5,        0) /* EncumbranceVal */
     , (450017,   9,        512) /* ValidLocations - ChestArmor */
     , (450017,  16,          1) /* ItemUseable - No */
     , (450017,  19,        20) /* Value */
     , (450017,  27,          2) /* ArmorType - Leather */
     , (450017,  28,         0) /* ArmorLevel */
     , (450017,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450017, 124,          3) /* Version */
     , (450017, 169,  118161678) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450017,  22, True ) /* Inscribable */
     , (450017, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450017,  13,     1.2) /* ArmorModVsSlash */
     , (450017,  14,     0.8) /* ArmorModVsPierce */
     , (450017,  15,       1) /* ArmorModVsBludgeon */
     , (450017,  16,     0.5) /* ArmorModVsCold */
     , (450017,  17,     1.1) /* ArmorModVsFire */
     , (450017,  18,     0.7) /* ArmorModVsAcid */
     , (450017,  19,     0.8) /* ArmorModVsElectric */
     , (450017, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450017,   1, 'Vestiri Over-robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450017,   1, 0x020001A6) /* Setup */
     , (450017,   3, 0x20000014) /* SoundTable */
     , (450017,   6, 0x0400007E) /* PaletteBase */
     , (450017,   7, 0x100007E6) /* ClothingBase */
     , (450017,   8, 0x0600587D) /* Icon */
     , (450017,  22, 0x3400002B) /* PhysicsEffectTable */;
