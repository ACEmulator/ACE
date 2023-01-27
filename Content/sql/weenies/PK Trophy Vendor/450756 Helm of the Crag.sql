DELETE FROM `weenie` WHERE `class_Id` = 450756;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450756, 'helmcragtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450756,   1,          2) /* ItemType - Armor */
     , (450756,   3,          2) /* PaletteTemplate - Blue */
     , (450756,   4,      16384) /* ClothingPriority - Head */
     , (450756,   5,        0) /* EncumbranceVal */
     , (450756,   8,        300) /* Mass */
     , (450756,   9,          1) /* ValidLocations - HeadWear */
     , (450756,  16,          1) /* ItemUseable - No */
     , (450756,  19,       20) /* Value */
     , (450756,  27,         0) /* ArmorType - Metal */
     , (450756,  28,        240) /* ArmorLevel */
     , (450756,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450756, 150,        103) /* HookPlacement - Hook */
     , (450756, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450756,  22, True ) /* Inscribable */
     , (450756,  23, True ) /* DestroyOnSell */
     , (450756, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450756,  12,    0.25) /* Shade */
     , (450756,  13,     0.8) /* ArmorModVsSlash */
     , (450756,  14,     0.5) /* ArmorModVsPierce */
     , (450756,  15,     1.1) /* ArmorModVsBludgeon */
     , (450756,  16,       0) /* ArmorModVsCold */
     , (450756,  17,       0) /* ArmorModVsFire */
     , (450756,  18,     0.6) /* ArmorModVsAcid */
     , (450756,  19,       0) /* ArmorModVsElectric */
     , (450756, 110,       1) /* BulkMod */
     , (450756, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450756,   1, 'Helm of the Crag') /* Name */
     , (450756,  15, 'A large horned helm.') /* ShortDesc */
     , (450756,  16, 'A large horned helm with the horns of a large mattekar.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450756,   1, 0x02000A0A) /* Setup */
     , (450756,   3, 0x20000014) /* SoundTable */
     , (450756,   6, 0x0400007E) /* PaletteBase */
     , (450756,   7, 0x100002B7) /* ClothingBase */
     , (450756,   8, 0x06000FD5) /* Icon */
     , (450756,  22, 0x3400002B) /* PhysicsEffectTable */;
