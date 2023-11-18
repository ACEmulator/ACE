DELETE FROM `weenie` WHERE `class_Id` = 450761;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450761, 'shroudvirinditailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450761,   1,          4) /* ItemType - Clothing */
     , (450761,   3,          3) /* PaletteTemplate - BluePurple */
     , (450761,   4,      81664) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450761,   5,        0) /* EncumbranceVal */
     , (450761,   8,        150) /* Mass */
     , (450761,   9,      32512) /* ValidLocations - Armor */
     , (450761,  16,          1) /* ItemUseable - No */
     , (450761,  19,       20) /* Value */
     , (450761,  27,          1) /* ArmorType - Cloth */
     , (450761,  28,          0) /* ArmorLevel */
     , (450761,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450761,  22, True ) /* Inscribable */
     , (450761,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450761,   5,   -0.05) /* ManaRate */
     , (450761,  12,     0.9) /* Shade */
     , (450761,  13,       1) /* ArmorModVsSlash */
     , (450761,  14,     0.7) /* ArmorModVsPierce */
     , (450761,  15,     0.7) /* ArmorModVsBludgeon */
     , (450761,  16,     0.7) /* ArmorModVsCold */
     , (450761,  17,     0.3) /* ArmorModVsFire */
     , (450761,  18,     0.3) /* ArmorModVsAcid */
     , (450761,  19,     0.5) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450761,   1, 'Virindi Shroud') /* Name */
     , (450761,  15, 'The wrappings of a powerful Virindi Director.') /* ShortDesc */
     , (450761,  16, 'The wrappings of a powerful Virindi Director') /* LongDesc */
     , (450761,  33, 'ShroudVirindiOct01') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450761,   1, 0x020001A6) /* Setup */
     , (450761,   3, 0x20000014) /* SoundTable */
     , (450761,   6, 0x0400007E) /* PaletteBase */
     , (450761,   7, 0x10000335) /* ClothingBase */
     , (450761,   8, 0x060022C2) /* Icon */
     , (450761,  22, 0x3400002B) /* PhysicsEffectTable */;
