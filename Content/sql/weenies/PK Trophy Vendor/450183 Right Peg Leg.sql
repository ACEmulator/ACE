DELETE FROM `weenie` WHERE `class_Id` = 450183;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450183, 'peglegrighttailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450183,   1,          4) /* ItemType - Armor */
     , (450183,   3,          4) /* PaletteTemplate - Brown */
     , (450183,   4,      65536) /* ClothingPriority - Feet */
     , (450183,   5,        0) /* EncumbranceVal */
     , (450183,   8,        140) /* Mass */
     , (450183,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450183,  16,          1) /* ItemUseable - No */
     , (450183,  19,        20) /* Value */
     , (450183,  27,          2) /* ArmorType - Leather */
     , (450183,  28,         0) /* ArmorLevel */
     , (450183,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450183, 150,        103) /* HookPlacement - Hook */
     , (450183, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450183,  22, True ) /* Inscribable */
     , (450183,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450183,  12,     0.1) /* Shade */
     , (450183,  13,       1) /* ArmorModVsSlash */
     , (450183,  14,     0.8) /* ArmorModVsPierce */
     , (450183,  15,       1) /* ArmorModVsBludgeon */
     , (450183,  16,     0.5) /* ArmorModVsCold */
     , (450183,  17,     0.5) /* ArmorModVsFire */
     , (450183,  18,     0.3) /* ArmorModVsAcid */
     , (450183,  19,     0.6) /* ArmorModVsElectric */
     , (450183, 110,    1.67) /* BulkMod */
     , (450183, 111,       2) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450183,   1, 'Right Peg Leg') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450183,   1, 0x020011E7) /* Setup */
     , (450183,   3, 0x20000014) /* SoundTable */
     , (450183,   6, 0x0400007E) /* PaletteBase */
     , (450183,   7, 0x10000586) /* ClothingBase */
     , (450183,   8, 0x060035F1) /* Icon */
     , (450183,  22, 0x3400002B) /* PhysicsEffectTable */;
