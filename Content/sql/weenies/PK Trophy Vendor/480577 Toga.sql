DELETE FROM `weenie` WHERE `class_Id` = 480577;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480577, 'robetogapk', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480577,   1,          4) /* ItemType - Clothing */
     , (480577,   3,          1) /* PaletteTemplate - AquaBlue */
     , (480577,   4,       1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearChest, OuterwearAbdomen */
     , (480577,   5,        0) /* EncumbranceVal */
     , (480577,   8,        150) /* Mass */
     , (480577,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperLegArmor */
     , (480577,  16,          1) /* ItemUseable - No */
     , (480577,  19,         20) /* Value */
     , (480577,  27,          1) /* ArmorType - Cloth */
     , (480577,  28,          0) /* ArmorLevel */
     , (480577,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480577, 169,  201328144) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480577,  11, True ) /* IgnoreCollisions */
     , (480577,  13, True ) /* Ethereal */
     , (480577,  14, True ) /* GravityStatus */
     , (480577,  19, True ) /* Attackable */
     , (480577,  22, True ) /* Inscribable */
     , (480577, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480577,  13,     0.8) /* ArmorModVsSlash */
     , (480577,  14,     0.8) /* ArmorModVsPierce */
     , (480577,  15,       1) /* ArmorModVsBludgeon */
     , (480577,  16,     0.2) /* ArmorModVsCold */
     , (480577,  17,     0.2) /* ArmorModVsFire */
     , (480577,  18,     0.1) /* ArmorModVsAcid */
     , (480577,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480577,   1, 'Toga') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480577,   1, 0x020001A6) /* Setup */
     , (480577,   3, 0x20000014) /* SoundTable */
     , (480577,   6, 0x0400007E) /* PaletteBase */
     , (480577,   7, 0x100005BC) /* ClothingBase */
     , (480577,   8, 0x0600589D) /* Icon */
     , (480577,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480577,  36, 0x0E000016) /* MutateFilter */;
