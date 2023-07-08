DELETE FROM `weenie` WHERE `class_Id` = 450049;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450049, 'robegaerlanredtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450049,   1,          4) /* ItemType - Clothing */
     , (450049,   3,         14) /* PaletteTemplate - Red */
     , (450049,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450049,   5,        0) /* EncumbranceVal */
     , (450049,   8,        450) /* Mass */
     , (450049,   9,      512) /* ValidLocations - HeadWear, Armor */
     , (450049,  16,          1) /* ItemUseable - No */
     , (450049,  18,          1) /* UiEffects - Magical */
     , (450049,  19,       20) /* Value */
     , (450049,  27,          1) /* ArmorType - Cloth */
     , (450049,  28,         0) /* ArmorLevel */
     , (450049,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450049, 150,        103) /* HookPlacement - Hook */
     , (450049, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450049,  22, True ) /* Inscribable */
     , (450049,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450049,   5,    -0.5) /* ManaRate */
     , (450049,  12,     0.5) /* Shade */
     , (450049,  13,    0.75) /* ArmorModVsSlash */
     , (450049,  14,    0.75) /* ArmorModVsPierce */
     , (450049,  15,    0.75) /* ArmorModVsBludgeon */
     , (450049,  16,       2) /* ArmorModVsCold */
     , (450049,  17,       2) /* ArmorModVsFire */
     , (450049,  18,       2) /* ArmorModVsAcid */
     , (450049,  19,       2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450049,   1, 'Elemental Master Robe') /* Name */
     , (450049,  15, 'A red robe trimmed in black, it is lined with an unknown material but feels like the perfect insulator.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450049,   1, 0x020001A6) /* Setup */
     , (450049,   3, 0x20000014) /* SoundTable */
     , (450049,   6, 0x0400007E) /* PaletteBase */
     , (450049,   7, 0x100003EC) /* ClothingBase */
     , (450049,   8, 0x060027BF) /* Icon */
     , (450049,  22, 0x3400002B) /* PhysicsEffectTable */;

