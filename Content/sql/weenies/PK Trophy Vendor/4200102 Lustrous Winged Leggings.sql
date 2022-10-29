DELETE FROM `weenie` WHERE `class_Id` = 4200102;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200102, 'leggingsgromniewingedtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200102,   1,          2) /* ItemType - Armor */
     , (4200102,   3,         20) /* PaletteTemplate - Silver */
     , (4200102,   4,        256) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (4200102,   5,          1) /* EncumbranceVal */
     , (4200102,   8,       1275) /* Mass */
     , (4200102,   9,       8192) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (4200102,  16,          1) /* ItemUseable - No */
     , (4200102,  19,         20) /* Value */
     , (4200102,  27,          2) /* ArmorType - Leather */
     , (4200102,  28,          1) /* ArmorLevel */
     , (4200102,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200102,  22, True ) /* Inscribable */
     , (4200102, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200102,   5,  -0.033) /* ManaRate */
     , (4200102,  12,     0.5) /* Shade */
     , (4200102,  13,       2) /* ArmorModVsSlash */
     , (4200102,  14,       1) /* ArmorModVsPierce */
     , (4200102,  15,       1) /* ArmorModVsBludgeon */
     , (4200102,  16,       1) /* ArmorModVsCold */
     , (4200102,  17,       2) /* ArmorModVsFire */
     , (4200102,  18,       1) /* ArmorModVsAcid */
     , (4200102,  19,       1) /* ArmorModVsElectric */
     , (4200102, 110,     1.1) /* BulkMod */
     , (4200102, 111,     1.5) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200102,   1, 'Lustrous Winged Leggings') /* Name */
     , (4200102,  16, 'A pair of winged leggings crafted from the hide of an adolescent ivory gromnie.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200102,   1, 0x020001A8) /* Setup */
     , (4200102,   3, 0x20000014) /* SoundTable */
     , (4200102,   6, 0x0400007E) /* PaletteBase */
     , (4200102,   7, 0x10000570) /* ClothingBase */
     , (4200102,   8, 0x06001BEB) /* Icon */
     , (4200102,  22, 0x3400002B) /* PhysicsEffectTable */;
