DELETE FROM `weenie` WHERE `class_Id` = 450040;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450040, 'robebathulgrimtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450040,   1,          4) /* ItemType - Clothing */
     , (450040,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450040,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450040,   5,        0) /* EncumbranceVal */
     , (450040,   8,        150) /* Mass */
     , (450040,   9,      512) /* ValidLocations - Armor */
     , (450040,  16,          1) /* ItemUseable - No */
     , (450040,  19,       20) /* Value */
     , (450040,  27,          1) /* ArmorType - Cloth */
     , (450040,  28,         0) /* ArmorLevel */
     , (450040,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450040, 150,        103) /* HookPlacement - Hook */
     , (450040, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450040,  22, True ) /* Inscribable */
     , (450040, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450040,  12,     0.5) /* Shade */
     , (450040,  13,     0.8) /* ArmorModVsSlash */
     , (450040,  14,     0.5) /* ArmorModVsPierce */
     , (450040,  15,       1) /* ArmorModVsBludgeon */
     , (450040,  16,     1.5) /* ArmorModVsCold */
     , (450040,  17,       0) /* ArmorModVsFire */
     , (450040,  18,       0) /* ArmorModVsAcid */
     , (450040,  19,     0.3) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450040,   1, 'Bathrobe') /* Name */
     , (450040,   7, 'Property of the Ravenous Mattekar Inn') /* Inscription */
     , (450040,   8, 'Odvik') /* ScribeName */
     , (450040,  16, 'A plush and comfy bathrobe. There is a small label on the inside of the robe.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450040,   1, 0x020001A6) /* Setup */
     , (450040,   3, 0x20000014) /* SoundTable */
     , (450040,   6, 0x0400007E) /* PaletteBase */
     , (450040,   7, 0x1000053A) /* ClothingBase */
     , (450040,   8, 0x060030E3) /* Icon */
     , (450040,  22, 0x3400002B) /* PhysicsEffectTable */;
