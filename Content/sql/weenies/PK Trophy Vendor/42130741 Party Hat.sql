DELETE FROM `weenie` WHERE `class_Id` = 42130741;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (42130741, 'newyearsgiftpartyhattailor', 4, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (42130741,   1,          4) /* ItemType - Armor */
     , (42130741,   3,         13) /* PaletteTemplate - Purple */
     , (42130741,   4,      16384) /* ClothingPriority - Head */
     , (42130741,   5,        100) /* EncumbranceVal */
     , (42130741,   8,        100) /* Mass */
     , (42130741,   9,          1) /* ValidLocations - HeadWear */
     , (42130741,  16,          1) /* ItemUseable - No */
     , (42130741,  19,         20) /* Value */
     , (42130741,  27,         32) /* ArmorType - Metal */
     , (42130741,  28,         10) /* ArmorLevel */
     , (42130741,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (42130741, 150,        103) /* HookPlacement - Hook */
     , (42130741, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (42130741,  22, True ) /* Inscribable */
     , (42130741, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (42130741,  12,    0.66) /* Shade */
     , (42130741,  13,     0.6) /* ArmorModVsSlash */
     , (42130741,  14,     0.6) /* ArmorModVsPierce */
     , (42130741,  15,     0.6) /* ArmorModVsBludgeon */
     , (42130741,  16,     0.6) /* ArmorModVsCold */
     , (42130741,  17,     0.6) /* ArmorModVsFire */
     , (42130741,  18,     0.6) /* ArmorModVsAcid */
     , (42130741,  19,     0.6) /* ArmorModVsElectric */
     , (42130741, 110,       1) /* BulkMod */
     , (42130741, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (42130741,   1, 'Party Hat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (42130741,   1, 0x020012B8) /* Setup */
     , (42130741,   3, 0x20000014) /* SoundTable */
     , (42130741,   6, 0x0400007E) /* PaletteBase */
     , (42130741,   7, 0x100005A5) /* ClothingBase */
     , (42130741,   8, 0x06000FCF) /* Icon */
     , (42130741,  22, 0x3400002B) /* PhysicsEffectTable */;
