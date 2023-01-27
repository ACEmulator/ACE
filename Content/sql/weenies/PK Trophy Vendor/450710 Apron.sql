DELETE FROM `weenie` WHERE `class_Id` = 450710;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450710, 'aprontailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450710,   1,          4) /* ItemType - Clothing */
     , (450710,   3,          4) /* PaletteTemplate - Brown */
     , (450710,   4,       1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms */
     , (450710,   5,         10) /* EncumbranceVal */
     , (450710,   8,         10) /* Mass */
     , (450710,   9,       3584) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor */
     , (450710,  16,          1) /* ItemUseable - No */
     , (450710,  19,         20) /* Value */
     , (450710,  27,          0) /* ArmorType - Cloth */
     , (450710,  28,          0) /* ArmorLevel */
     , (450710,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450710,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450710,  12,     0.8) /* Shade */
     , (450710,  13,     0.8) /* ArmorModVsSlash */
     , (450710,  14,     0.8) /* ArmorModVsPierce */
     , (450710,  15,       1) /* ArmorModVsBludgeon */
     , (450710,  16,     0.2) /* ArmorModVsCold */
     , (450710,  17,     0.2) /* ArmorModVsFire */
     , (450710,  18,     0.1) /* ArmorModVsAcid */
     , (450710,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450710,   1, 'Apron') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450710,   1, 0x020001A6) /* Setup */
     , (450710,   3, 0x20000014) /* SoundTable */
     , (450710,   6, 0x0400007E) /* PaletteBase */
     , (450710,   7, 0x10000059) /* ClothingBase */
     , (450710,   8, 0x06000FF0) /* Icon */
     , (450710,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450710,  36, 0x0E000016) /* MutateFilter */;
