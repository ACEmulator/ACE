DELETE FROM `weenie` WHERE `class_Id` = 450531;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450531, 'robegeliditetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450531,   1,          4) /* ItemType - Clothing */
     , (450531,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450531,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450531,   5,        0) /* EncumbranceVal */
     , (450531,   8,        150) /* Mass */
     , (450531,   9,      512) /* ValidLocations - HeadWear, Armor */
     , (450531,  16,          1) /* ItemUseable - No */
     , (450531,  18,          1) /* UiEffects - Magical */
     , (450531,  19,       20) /* Value */
     , (450531,  27,          1) /* ArmorType - Cloth */
     , (450531,  28,          0) /* ArmorLevel */
     , (450531,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450531, 150,        103) /* HookPlacement - Hook */
     , (450531, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450531,  22, True ) /* Inscribable */
     , (450531,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450531,   5,    -0.1) /* ManaRate */
     , (450531,  12,     0.1) /* Shade */
     , (450531,  13,     0.8) /* ArmorModVsSlash */
     , (450531,  14,     0.8) /* ArmorModVsPierce */
     , (450531,  15,       1) /* ArmorModVsBludgeon */
     , (450531,  16,     0.2) /* ArmorModVsCold */
     , (450531,  17,     0.2) /* ArmorModVsFire */
     , (450531,  18,     0.1) /* ArmorModVsAcid */
     , (450531,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450531,   1, 'Gelidite Robe') /* Name */
     , (450531,  15, 'An icy blue robe.') /* ShortDesc */
     , (450531,  16, 'An icy blue robe, worn by the Gelidites of Frore when they walked the living world. This artifact is several millennia old.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450531,   1, 0x020001A6) /* Setup */
     , (450531,   3, 0x20000014) /* SoundTable */
     , (450531,   6, 0x0400007E) /* PaletteBase */
     , (450531,   7, 0x1000018E) /* ClothingBase */
     , (450531,   8, 0x06001B90) /* Icon */
     , (450531,  22, 0x3400002B) /* PhysicsEffectTable */;

