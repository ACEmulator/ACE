DELETE FROM `weenie` WHERE `class_Id` = 450727;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450727, 'bootsgromniewingedtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450727,   1,          2) /* ItemType - Armor */
     , (450727,   3,         14) /* PaletteTemplate - Red */
     , (450727,   4,      65536) /* ClothingPriority - Feet */
     , (450727,   5,        0) /* EncumbranceVal */
     , (450727,   8,        140) /* Mass */
     , (450727,   9,        256) /* ValidLocations - FootWear */
     , (450727,  16,          1) /* ItemUseable - No */
     , (450727,  19,       20) /* Value */
     , (450727,  27,         32) /* ArmorType - Metal */
     , (450727,  28,        0) /* ArmorLevel */
     , (450727,  44,          3) /* Damage */
     , (450727,  45,          4) /* DamageType - Bludgeon */
     , (450727,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450727,  22, True ) /* Inscribable */
     , (450727, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450727,   5,  -0.033) /* ManaRate */
     , (450727,  12,     0.1) /* Shade */
     , (450727,  13,       1) /* ArmorModVsSlash */
     , (450727,  14,       2) /* ArmorModVsPierce */
     , (450727,  15,       1) /* ArmorModVsBludgeon */
     , (450727,  16,       2) /* ArmorModVsCold */
     , (450727,  17,       1) /* ArmorModVsFire */
     , (450727,  18,       1) /* ArmorModVsAcid */
     , (450727,  19,       1) /* ArmorModVsElectric */
     , (450727,  22,    0.75) /* DamageVariance */
     , (450727, 110,       1) /* BulkMod */
     , (450727, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450727,   1, 'Ruddy Winged Boots') /* Name */
     , (450727,  16, 'A pair of winged boots crafted from the hide of an adolescent rust gromnie.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450727,   1, 0x020000DE) /* Setup */
     , (450727,   3, 0x20000014) /* SoundTable */
     , (450727,   6, 0x0400007E) /* PaletteBase */
     , (450727,   7, 0x10000576) /* ClothingBase */
     , (450727,   8, 0x06000FAE) /* Icon */
     , (450727,  22, 0x3400002B) /* PhysicsEffectTable */;

