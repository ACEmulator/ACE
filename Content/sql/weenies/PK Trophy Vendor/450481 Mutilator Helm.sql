DELETE FROM `weenie` WHERE `class_Id` = 450481;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450481, 'helmolthoimutilatortailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450481,   1,          2) /* ItemType - Armor */
     , (450481,   3,         13) /* PaletteTemplate - Purple */
     , (450481,   4,      16384) /* ClothingPriority - Head */
     , (450481,   5,        0) /* EncumbranceVal */
     , (450481,   8,        150) /* Mass */
     , (450481,   9,          1) /* ValidLocations - HeadWear */
     , (450481,  16,          1) /* ItemUseable - No */
     , (450481,  19,       20) /* Value */
     , (450481,  27,         32) /* ArmorType - Metal */
     , (450481,  28,        0) /* ArmorLevel */
     , (450481,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450481, 150,        103) /* HookPlacement - Hook */
     , (450481, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450481,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450481,  12,     0.3) /* Shade */
     , (450481,  13,    1.25) /* ArmorModVsSlash */
     , (450481,  14,    0.75) /* ArmorModVsPierce */
     , (450481,  15,    0.75) /* ArmorModVsBludgeon */
     , (450481,  16,     1.1) /* ArmorModVsCold */
     , (450481,  17,    1.25) /* ArmorModVsFire */
     , (450481,  18,     1.9) /* ArmorModVsAcid */
     , (450481,  19,     1.6) /* ArmorModVsElectric */
     , (450481, 110,       1) /* BulkMod */
     , (450481, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450481,   1, 'Mutilator Helm') /* Name */
     , (450481,  15, 'This helm was crafted from the hollowed out head of an Olthoi Mutilator. The thick chitin of the mutilator provides good protection.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450481,   1, 0x02000FDD) /* Setup */
     , (450481,   3, 0x20000014) /* SoundTable */
     , (450481,   6, 0x0400007E) /* PaletteBase */
     , (450481,   7, 0x100004CE) /* ClothingBase */
     , (450481,   8, 0x06002D87) /* Icon */
     , (450481,  22, 0x3400002B) /* PhysicsEffectTable */;
