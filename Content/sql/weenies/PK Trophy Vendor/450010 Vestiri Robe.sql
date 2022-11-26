DELETE FROM `weenie` WHERE `class_Id` = 450010;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450010, 'ace450010-vestirirobetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450010,   1,          4) /* ItemType - Clothing */
     , (450010,   3,         85) /* PaletteTemplate - DyeDarkRed */
     , (450010,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450010,   5,        10) /* EncumbranceVal */
     , (450010,   9,      512) /* ValidLocations - Armor */
     , (450010,  16,          1) /* ItemUseable - No */
     , (450010,  19,         20) /* Value */
     , (450010,  27,          1) /* ArmorType - Cloth */
     , (450010,  28,          0) /* ArmorLevel */
     , (450010,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450010, 169,  201328144) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450010,  11, True ) /* IgnoreCollisions */
     , (450010,  13, True ) /* Ethereal */
     , (450010,  14, True ) /* GravityStatus */
     , (450010,  19, True ) /* Attackable */
     , (450010,  22, True ) /* Inscribable */
     , (450010, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450010,  12,     0.5) /* Shade */
     , (450010,  13,     0.8) /* ArmorModVsSlash */
     , (450010,  14,     0.8) /* ArmorModVsPierce */
     , (450010,  15,       1) /* ArmorModVsBludgeon */
     , (450010,  16,     0.2) /* ArmorModVsCold */
     , (450010,  17,     0.2) /* ArmorModVsFire */
     , (450010,  18,     0.1) /* ArmorModVsAcid */
     , (450010,  19,     0.2) /* ArmorModVsElectric */
     , (450010, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450010,   1, 'Vestiri Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450010,   1, 0x02001313) /* Setup */
     , (450010,   3, 0x20000014) /* SoundTable */
     , (450010,   6, 0x0400007E) /* PaletteBase */
     , (450010,   7, 0x100005BB) /* ClothingBase */
     , (450010,   8, 0x06005884) /* Icon */
     , (450010,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450010,  36, 0x0E000016) /* MutateFilter */;
