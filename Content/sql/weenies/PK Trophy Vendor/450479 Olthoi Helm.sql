DELETE FROM `weenie` WHERE `class_Id` = 450479;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450479, 'helmolthoinewtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450479,   1,          2) /* ItemType - Armor */
     , (450479,   3,         39) /* PaletteTemplate - Black */
     , (450479,   4,      16384) /* ClothingPriority - Head */
     , (450479,   5,        0) /* EncumbranceVal */
     , (450479,   8,        150) /* Mass */
     , (450479,   9,          1) /* ValidLocations - HeadWear */
     , (450479,  16,          1) /* ItemUseable - No */
     , (450479,  19,       20) /* Value */
     , (450479,  27,         32) /* ArmorType - Metal */
     , (450479,  28,        0) /* ArmorLevel */
     , (450479,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450479, 150,        103) /* HookPlacement - Hook */
     , (450479, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450479,  22, True ) /* Inscribable */
     , (450479, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450479,  12,     0.3) /* Shade */
     , (450479,  13,       1) /* ArmorModVsSlash */
     , (450479,  14,     0.8) /* ArmorModVsPierce */
     , (450479,  15,     0.6) /* ArmorModVsBludgeon */
     , (450479,  16,     0.6) /* ArmorModVsCold */
     , (450479,  17,       1) /* ArmorModVsFire */
     , (450479,  18,     0.8) /* ArmorModVsAcid */
     , (450479,  19,       1) /* ArmorModVsElectric */
     , (450479, 110,       1) /* BulkMod */
     , (450479, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450479,   1, 'Olthoi Helm') /* Name */
     , (450479,  15, 'Helm crafted from the carapace of an Olthoi. This item can be dyed.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450479,   1, 0x02000977) /* Setup */
     , (450479,   3, 0x20000014) /* SoundTable */
     , (450479,   6, 0x0400007E) /* PaletteBase */
     , (450479,   7, 0x1000026A) /* ClothingBase */
     , (450479,   8, 0x06001E9A) /* Icon */
     , (450479,  22, 0x3400002B) /* PhysicsEffectTable */;
