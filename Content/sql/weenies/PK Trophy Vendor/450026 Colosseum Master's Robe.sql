DELETE FROM `weenie` WHERE `class_Id` = 450026;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450026, 'ace450026-colosseummastersrobetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450026,   1,          4) /* ItemType - Clothing */
     , (450026,   3,         39) /* PaletteTemplate - Black */
     , (450026,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450026,   5,        0) /* EncumbranceVal */
     , (450026,   9,      512) /* ValidLocations - Armor */
     , (450026,  16,          1) /* ItemUseable - No */
     , (450026,  19,          20) /* Value */
     , (450026,  28,        0) /* ArmorLevel */
     , (450026,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450026,  22, True ) /* Inscribable */
     , (450026,  99, True ) /* Ivoryable */
     , (450026, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450026,   5,   -0.05) /* ManaRate */
     , (450026,  12,   0.917) /* Shade */
     , (450026,  13,       1) /* ArmorModVsSlash */
     , (450026,  14,       1) /* ArmorModVsPierce */
     , (450026,  15,       1) /* ArmorModVsBludgeon */
     , (450026,  16,     0.8) /* ArmorModVsCold */
     , (450026,  17,     0.8) /* ArmorModVsFire */
     , (450026,  18,     0.7) /* ArmorModVsAcid */
     , (450026,  19,     0.7) /* ArmorModVsElectric */
     , (450026, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450026,   1, 'Colosseum Master''s Robe') /* Name */
     , (450026,  16, 'The elegant silken robes that once belonged to the fearsome Master of the Colosseum.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450026,   1, 0x020001A6) /* Setup */
     , (450026,   3, 0x20000014) /* SoundTable */
     , (450026,   6, 0x0400007E) /* PaletteBase */
     , (450026,   7, 0x10000198) /* ClothingBase */
     , (450026,   8, 0x060023C2) /* Icon */
     , (450026,  22, 0x3400002B) /* PhysicsEffectTable */;


