DELETE FROM `weenie` WHERE `class_Id` = 450184;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450184, 'peglegstailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450184,   1,          4) /* ItemType - Armor */
     , (450184,   3,          4) /* PaletteTemplate - Brown */
     , (450184,   4,      65536) /* ClothingPriority - Feet */
     , (450184,   5,        0) /* EncumbranceVal */
     , (450184,   8,        140) /* Mass */
     , (450184,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450184,  16,          1) /* ItemUseable - No */
     , (450184,  19,        20) /* Value */
     , (450184,  27,          2) /* ArmorType - Leather */
     , (450184,  28,         0) /* ArmorLevel */
     , (450184,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450184, 150,        103) /* HookPlacement - Hook */
     , (450184, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450184,  22, True ) /* Inscribable */
     , (450184,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450184,  12,     0.1) /* Shade */
     , (450184,  13,       1) /* ArmorModVsSlash */
     , (450184,  14,     0.8) /* ArmorModVsPierce */
     , (450184,  15,       1) /* ArmorModVsBludgeon */
     , (450184,  16,     0.5) /* ArmorModVsCold */
     , (450184,  17,     0.5) /* ArmorModVsFire */
     , (450184,  18,     0.3) /* ArmorModVsAcid */
     , (450184,  19,     0.6) /* ArmorModVsElectric */
     , (450184, 110,    1.67) /* BulkMod */
     , (450184, 111,       2) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450184,   1, 'Peg Legs') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450184,   1, 0x020011E8) /* Setup */
     , (450184,   3, 0x20000014) /* SoundTable */
     , (450184,   6, 0x0400007E) /* PaletteBase */
     , (450184,   7, 0x10000584) /* ClothingBase */
     , (450184,   8, 0x060035F0) /* Icon */
     , (450184,  22, 0x3400002B) /* PhysicsEffectTable */;
