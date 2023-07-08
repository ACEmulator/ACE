DELETE FROM `weenie` WHERE `class_Id` = 480576;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480576, 'rainmentweddingpk', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480576,   1,          4) /* ItemType - Clothing */
     , (480576,   3,          9) /* PaletteTemplate - Grey */
     , (480576,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (480576,   5,        0) /* EncumbranceVal */
     , (480576,   8,        175) /* Mass */
     , (480576,   9,      512) /* ValidLocations - Armor */
     , (480576,  16,          1) /* ItemUseable - No */
     , (480576,  19,      20) /* Value */
     , (480576,  27,          1) /* ArmorType - Cloth */
     , (480576,  28,          0) /* ArmorLevel */
     , (480576,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480576,  22, True ) /* Inscribable */
     , (480576, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480576,  12,    0.48) /* Shade */
     , (480576,  13,       1) /* ArmorModVsSlash */
     , (480576,  14,     0.7) /* ArmorModVsPierce */
     , (480576,  15,     0.4) /* ArmorModVsBludgeon */
     , (480576,  16,     0.2) /* ArmorModVsCold */
     , (480576,  17,     0.2) /* ArmorModVsFire */
     , (480576,  18,     0.3) /* ArmorModVsAcid */
     , (480576,  19,     0.4) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480576,   1, 'Wedding Raiment') /* Name */
     , (480576,  15, 'The perfect outfit for wedding party members. This rainment is dyeable.') /* ShortDesc */
     , (480576,  16, 'The perfect outfit for wedding party members. This rainment is dyeable.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480576,   1, 0x020001A6) /* Setup */
     , (480576,   3, 0x20000014) /* SoundTable */
     , (480576,   6, 0x0400007E) /* PaletteBase */
     , (480576,   7, 0x10000385) /* ClothingBase */
     , (480576,   8, 0x060024D6) /* Icon */
     , (480576,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480576,  36, 0x0E000016) /* MutateFilter */;
