DELETE FROM `weenie` WHERE `class_Id` = 4200101;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200101, 'helmetgromniewingedtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200101,   1,          2) /* ItemType - Armor */
     , (4200101,   3,          1) /* PaletteTemplate - AquaBlue */
     , (4200101,   4,      16384) /* ClothingPriority - Head */
     , (4200101,   5,          1) /* EncumbranceVal */
     , (4200101,   8,        125) /* Mass */
     , (4200101,   9,          1) /* ValidLocations - HeadWear */
     , (4200101,  16,          1) /* ItemUseable - No */
     , (4200101,  19,         20) /* Value */
     , (4200101,  27,          4) /* ArmorType - StuddedLeather */
     , (4200101,  28,          1) /* ArmorLevel */
     , (4200101,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200101,  22, True ) /* Inscribable */
     , (4200101, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200101,   5,  -0.033) /* ManaRate */
     , (4200101,  12,     0.5) /* Shade */
     , (4200101,  13,       1) /* ArmorModVsSlash */
     , (4200101,  14,       1) /* ArmorModVsPierce */
     , (4200101,  15,       2) /* ArmorModVsBludgeon */
     , (4200101,  16,       1) /* ArmorModVsCold */
     , (4200101,  17,       1) /* ArmorModVsFire */
     , (4200101,  18,       2) /* ArmorModVsAcid */
     , (4200101,  19,       1) /* ArmorModVsElectric */
     , (4200101, 110,       1) /* BulkMod */
     , (4200101, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200101,   1, 'Turquoise Winged Helmet') /* Name */
     , (4200101,  16, 'A winged helmet crafted from the hide of an adolescent azure gromnie.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200101,   1, 0x02001131) /* Setup */
     , (4200101,   3, 0x20000014) /* SoundTable */
     , (4200101,   6, 0x0400007E) /* PaletteBase */
     , (4200101,   7, 0x10000574) /* ClothingBase */
     , (4200101,   8, 0x06001353) /* Icon */
     , (4200101,  22, 0x3400002B) /* PhysicsEffectTable */;
