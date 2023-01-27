DELETE FROM `weenie` WHERE `class_Id` = 450050;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450050, 'robegaerlanblacktailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450050,   1,          4) /* ItemType - Clothing */
     , (450050,   3,         39) /* PaletteTemplate - Black */
     , (450050,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450050,   5,        0) /* EncumbranceVal */
     , (450050,   8,        450) /* Mass */
     , (450050,   9,      512) /* ValidLocations - HeadWear, Armor */
     , (450050,  16,          1) /* ItemUseable - No */
     , (450050,  18,          1) /* UiEffects - Magical */
     , (450050,  19,       20) /* Value */
     , (450050,  27,          1) /* ArmorType - Cloth */
     , (450050,  28,         0) /* ArmorLevel */
     , (450050,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450050, 150,        103) /* HookPlacement - Hook */
     , (450050, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450050,  22, True ) /* Inscribable */
     , (450050,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450050,   5,    -0.5) /* ManaRate */
     , (450050,  12,     0.5) /* Shade */
     , (450050,  13,    0.75) /* ArmorModVsSlash */
     , (450050,  14,    0.75) /* ArmorModVsPierce */
     , (450050,  15,    0.75) /* ArmorModVsBludgeon */
     , (450050,  16,       2) /* ArmorModVsCold */
     , (450050,  17,       2) /* ArmorModVsFire */
     , (450050,  18,       2) /* ArmorModVsAcid */
     , (450050,  19,       2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450050,   1, 'Elemental Master Robe') /* Name */
     , (450050,  15, 'A black robe trimmed in red, it is lined with an unknown material but feels like the perfect insulator.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450050,   1, 0x020001A6) /* Setup */
     , (450050,   3, 0x20000014) /* SoundTable */
     , (450050,   6, 0x0400007E) /* PaletteBase */
     , (450050,   7, 0x100003EC) /* ClothingBase */
     , (450050,   8, 0x060027BE) /* Icon */
     , (450050,  22, 0x3400002B) /* PhysicsEffectTable */;

