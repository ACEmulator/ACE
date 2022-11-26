DELETE FROM `weenie` WHERE `class_Id` = 450717;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450717, 'cowlsandtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450717,   1,          2) /* ItemType - Armor */
     , (450717,   3,         17) /* PaletteTemplate - Yellow */
     , (450717,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, Head */
     , (450717,   5,        0) /* EncumbranceVal */
     , (450717,   8,        270) /* Mass */
     , (450717,   9,       2561) /* ValidLocations - HeadWear, ChestArmor, UpperArmArmor */
     , (450717,  16,          1) /* ItemUseable - No */
     , (450717,  19,       20) /* Value */
     , (450717,  27,          2) /* ArmorType - Leather */
     , (450717,  28,        0) /* ArmorLevel */
     , (450717,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;
	 
INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450717,  22, True ) /* Inscribable */
     , (450717,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450717,   5,  -0.033) /* ManaRate */
     , (450717,  12,    0.66) /* Shade */
     , (450717,  13,     1.2) /* ArmorModVsSlash */
     , (450717,  14,     0.8) /* ArmorModVsPierce */
     , (450717,  15,     0.8) /* ArmorModVsBludgeon */
     , (450717,  16,     1.2) /* ArmorModVsCold */
     , (450717,  17,     1.2) /* ArmorModVsFire */
     , (450717,  18,     0.6) /* ArmorModVsAcid */
     , (450717,  19,     0.6) /* ArmorModVsElectric */
     , (450717, 110,       1) /* BulkMod */
     , (450717, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450717,   1, 'Cowl of the Sand') /* Name */
     , (450717,  16, 'This Cowl was used by a member of the Shagar Zharala to protect himself from the sandstorms of the A''mun Desert.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450717,   1, 0x020001C3) /* Setup */
     , (450717,   3, 0x20000014) /* SoundTable */
     , (450717,   6, 0x0400007E) /* PaletteBase */
     , (450717,   7, 0x10000529) /* ClothingBase */
     , (450717,   8, 0x06003065) /* Icon */
     , (450717,  22, 0x3400002B) /* PhysicsEffectTable */;

