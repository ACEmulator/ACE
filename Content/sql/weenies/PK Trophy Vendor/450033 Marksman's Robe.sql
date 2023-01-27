DELETE FROM `weenie` WHERE `class_Id` = 450033;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450033, 'robenoblemissiletailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450033,   1,          4) /* ItemType - Armor */
     , (450033,   3,         21) /* PaletteTemplate - Gold */
     , (450033,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450033,   5,        0) /* EncumbranceVal */
     , (450033,   8,        450) /* Mass */
     , (450033,   9,      512) /* ValidLocations - Armor */
     , (450033,  16,          1) /* ItemUseable - No */
     , (450033,  19,       20) /* Value */
     , (450033,  27,          1) /* ArmorType - Cloth */
     , (450033,  28,        0) /* ArmorLevel */
     , (450033,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450033,  22, True ) /* Inscribable */
     , (450033, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450033,   5,   -0.01) /* ManaRate */
     , (450033,  12,       1) /* Shade */
     , (450033,  13,     0.4) /* ArmorModVsSlash */
     , (450033,  14,     0.2) /* ArmorModVsPierce */
     , (450033,  15,     0.4) /* ArmorModVsBludgeon */
     , (450033,  16,     1.1) /* ArmorModVsCold */
     , (450033,  17,     0.4) /* ArmorModVsFire */
     , (450033,  18,     0.4) /* ArmorModVsAcid */
     , (450033,  19,     1.1) /* ArmorModVsElectric */
     , (450033, 110,       1) /* BulkMod */
     , (450033, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450033,   1, 'Marksman''s Robe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450033,   1, 0x020001A6) /* Setup */
     , (450033,   3, 0x20000014) /* SoundTable */
     , (450033,   6, 0x0400007E) /* PaletteBase */
     , (450033,   7, 0x10000591) /* ClothingBase */
     , (450033,   8, 0x0600301D) /* Icon */
     , (450033,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450033,  36, 0x0E000016) /* MutateFilter */;

