DELETE FROM `weenie` WHERE `class_Id` = 450018;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450018, 'ace450018-suikanoverrobetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450018,   1,          4) /* ItemType - Armor */
     , (450018,   3,          7) /* PaletteTemplate - DeepGreen */
     , (450018,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450018,   5,        0) /* EncumbranceVal */
     , (450018,   9,        512) /* ValidLocations - ChestArmor */
     , (450018,  16,          1) /* ItemUseable - No */
     , (450018,  19,        20) /* Value */
     , (450018,  27,          2) /* ArmorType - Leather */
     , (450018,  28,         0) /* ArmorLevel */
     , (450018,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450018, 124,          3) /* Version */
     , (450018, 169,  118161678) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450018,  22, True ) /* Inscribable */
     , (450018, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450018,  13,     1.2) /* ArmorModVsSlash */
     , (450018,  14,     0.8) /* ArmorModVsPierce */
     , (450018,  15,       1) /* ArmorModVsBludgeon */
     , (450018,  16,     0.5) /* ArmorModVsCold */
     , (450018,  17,     0.5) /* ArmorModVsFire */
     , (450018,  18,     0.7) /* ArmorModVsAcid */
     , (450018,  19,     0.8) /* ArmorModVsElectric */
     , (450018, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450018,   1, 'Suikan Over-robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450018,   1, 0x020001A6) /* Setup */
     , (450018,   3, 0x20000014) /* SoundTable */
     , (450018,   6, 0x0400007E) /* PaletteBase */
     , (450018,   7, 0x100007E5) /* ClothingBase */
     , (450018,   8, 0x06001BAA) /* Icon */
     , (450018,  22, 0x3400002B) /* PhysicsEffectTable */;
