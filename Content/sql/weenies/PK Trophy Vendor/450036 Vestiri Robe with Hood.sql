DELETE FROM `weenie` WHERE `class_Id` = 450036;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450036, 'robeviamontianhoodtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450036,   1,          4) /* ItemType - Clothing */
     , (450036,   3,          4) /* PaletteTemplate - Brown */
     , (450036,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450036,   5,        0) /* EncumbranceVal */
     , (450036,   9,      512) /* ValidLocations - HeadWear, Armor */
     , (450036,  16,          1) /* ItemUseable - No */
     , (450036,  19,         20) /* Value */
     , (450036,  27,          1) /* ArmorType - Cloth */
     , (450036,  28,          0) /* ArmorLevel */
     , (450036,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450036, 169,  201328144) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450036,  11, True ) /* IgnoreCollisions */
     , (450036,  13, True ) /* Ethereal */
     , (450036,  14, True ) /* GravityStatus */
     , (450036,  19, True ) /* Attackable */
     , (450036,  22, True ) /* Inscribable */
     , (450036, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450036,  13,     0.8) /* ArmorModVsSlash */
     , (450036,  14,     0.8) /* ArmorModVsPierce */
     , (450036,  15,       1) /* ArmorModVsBludgeon */
     , (450036,  16,     0.2) /* ArmorModVsCold */
     , (450036,  17,     0.2) /* ArmorModVsFire */
     , (450036,  18,     0.1) /* ArmorModVsAcid */
     , (450036,  19,     0.2) /* ArmorModVsElectric */
     , (450036, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450036,   1, 'Vestiri Robe with Hood') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450036,   1, 0x02001313) /* Setup */
     , (450036,   3, 0x20000014) /* SoundTable */
     , (450036,   6, 0x0400007E) /* PaletteBase */
     , (450036,   7, 0x100005BA) /* ClothingBase */
     , (450036,   8, 0x06005E4A) /* Icon */
     , (450036,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450036,  36, 0x0E000016) /* MutateFilter */;
