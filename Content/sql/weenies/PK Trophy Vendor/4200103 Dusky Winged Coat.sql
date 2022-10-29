DELETE FROM `weenie` WHERE `class_Id` = 4200103;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200103, 'coatgromniewingedtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200103,   1,          2) /* ItemType - Armor */
     , (4200103,   3,          9) /* PaletteTemplate - Grey */
     , (4200103,   4,       1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (4200103,   5,          1) /* EncumbranceVal */
     , (4200103,   8,       1000) /* Mass */
     , (4200103,   9,        512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (4200103,  16,          1) /* ItemUseable - No */
     , (4200103,  19,         20) /* Value */
     , (4200103,  27,          8) /* ArmorType - Scalemail */
     , (4200103,  28,          1) /* ArmorLevel */
     , (4200103,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200103,  22, True ) /* Inscribable */
     , (4200103, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200103,   5,  -0.033) /* ManaRate */
     , (4200103,  12,    0.66) /* Shade */
     , (4200103,  13,       1) /* ArmorModVsSlash */
     , (4200103,  14,       2) /* ArmorModVsPierce */
     , (4200103,  15,       1) /* ArmorModVsBludgeon */
     , (4200103,  16,       1) /* ArmorModVsCold */
     , (4200103,  17,       1) /* ArmorModVsFire */
     , (4200103,  18,       1) /* ArmorModVsAcid */
     , (4200103,  19,       2) /* ArmorModVsElectric */
     , (4200103, 110,     1.1) /* BulkMod */
     , (4200103, 111,     1.5) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200103,   1, 'Dusky Winged Coat') /* Name */
     , (4200103,  16, 'A winged coat crafted from the hide of an adolescent ash gromnie.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200103,   1, 0x020001A6) /* Setup */
     , (4200103,   3, 0x20000014) /* SoundTable */
     , (4200103,   6, 0x0400007E) /* PaletteBase */
     , (4200103,   7, 0x10000572) /* ClothingBase */
     , (4200103,   8, 0x06001BE3) /* Icon */
     , (4200103,  22, 0x3400002B) /* PhysicsEffectTable */;

