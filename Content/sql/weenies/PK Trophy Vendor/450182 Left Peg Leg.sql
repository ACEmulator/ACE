DELETE FROM `weenie` WHERE `class_Id` = 450182;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450182, 'pegleglefttailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450182,   1,          4) /* ItemType - Armor */
     , (450182,   3,          4) /* PaletteTemplate - Brown */
     , (450182,   4,      65536) /* ClothingPriority - Feet */
     , (450182,   5,        0) /* EncumbranceVal */
     , (450182,   8,        140) /* Mass */
     , (450182,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450182,  16,          1) /* ItemUseable - No */
     , (450182,  19,        20) /* Value */
     , (450182,  27,          2) /* ArmorType - Leather */
     , (450182,  28,         0) /* ArmorLevel */
     , (450182,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450182, 150,        103) /* HookPlacement - Hook */
     , (450182, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450182,  22, True ) /* Inscribable */
     , (450182,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450182,  12,     0.1) /* Shade */
     , (450182,  13,       1) /* ArmorModVsSlash */
     , (450182,  14,     0.8) /* ArmorModVsPierce */
     , (450182,  15,       1) /* ArmorModVsBludgeon */
     , (450182,  16,     0.5) /* ArmorModVsCold */
     , (450182,  17,     0.5) /* ArmorModVsFire */
     , (450182,  18,     0.3) /* ArmorModVsAcid */
     , (450182,  19,     0.6) /* ArmorModVsElectric */
     , (450182, 110,    1.67) /* BulkMod */
     , (450182, 111,       2) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450182,   1, 'Left Peg Leg') /* Name */
     , (450182,  16, 'Onda Nakoza in MacNiall''s Freehold will modify this left peg leg so it can be coupled with the right peg leg for a double peg leg set!') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450182,   1, 0x020011E7) /* Setup */
     , (450182,   3, 0x20000014) /* SoundTable */
     , (450182,   6, 0x0400007E) /* PaletteBase */
     , (450182,   7, 0x10000585) /* ClothingBase */
     , (450182,   8, 0x060035F1) /* Icon */
     , (450182,  22, 0x3400002B) /* PhysicsEffectTable */;
