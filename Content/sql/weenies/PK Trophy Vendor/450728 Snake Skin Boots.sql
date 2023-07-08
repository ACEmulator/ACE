DELETE FROM `weenie` WHERE `class_Id` = 450728;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450728, 'bootssnakeskintailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450728,   1,          2) /* ItemType - Armor */
     , (450728,   3,          8) /* PaletteTemplate - Green */
     , (450728,   4,      65536) /* ClothingPriority - Feet */
     , (450728,   5,        0) /* EncumbranceVal */
     , (450728,   8,        230) /* Mass */
     , (450728,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450728,  16,          1) /* ItemUseable - No */
     , (450728,  19,       20) /* Value */
     , (450728,  27,          4) /* ArmorType - StuddedLeather */
     , (450728,  28,        0) /* ArmorLevel */
     , (450728,  44,          3) /* Damage */
     , (450728,  45,          4) /* DamageType - Bludgeon */
     , (450728,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450728,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450728,  12,     0.1) /* Shade */
     , (450728,  13,     1.4) /* ArmorModVsSlash */
     , (450728,  14,       1) /* ArmorModVsPierce */
     , (450728,  15,     0.7) /* ArmorModVsBludgeon */
     , (450728,  16,     1.4) /* ArmorModVsCold */
     , (450728,  17,       1) /* ArmorModVsFire */
     , (450728,  18,     0.4) /* ArmorModVsAcid */
     , (450728,  19,     0.4) /* ArmorModVsElectric */
     , (450728,  22,    0.75) /* DamageVariance */
     , (450728, 110,       1) /* BulkMod */
     , (450728, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450728,   1, 'Snake Skin Boots') /* Name */
     , (450728,  16, 'Boots made out of the hide of a Sclavus.  They are faintly iridescent, and shimmer when you walk.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450728,   1, 0x020008CB) /* Setup */
     , (450728,   3, 0x20000014) /* SoundTable */
     , (450728,   6, 0x0400007E) /* PaletteBase */
     , (450728,   7, 0x100002B2) /* ClothingBase */
     , (450728,   8, 0x06001311) /* Icon */
     , (450728,  22, 0x3400002B) /* PhysicsEffectTable */;
