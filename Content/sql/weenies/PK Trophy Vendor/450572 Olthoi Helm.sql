DELETE FROM `weenie` WHERE `class_Id` = 450572;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450572, 'helmolthoitailor2', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450572,   1,          2) /* ItemType - Armor */
     , (450572,   3,         13) /* PaletteTemplate - Purple */
     , (450572,   4,      16384) /* ClothingPriority - Head */
     , (450572,   5,        0) /* EncumbranceVal */
     , (450572,   8,        150) /* Mass */
     , (450572,   9,          1) /* ValidLocations - HeadWear */
     , (450572,  16,          1) /* ItemUseable - No */
     , (450572,  19,       20) /* Value */
     , (450572,  27,         32) /* ArmorType - Metal */
     , (450572,  28,        0) /* ArmorLevel */
     , (450572,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450572, 150,        103) /* HookPlacement - Hook */
     , (450572, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450572,  22, True ) /* Inscribable */
     , (450572, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450572,  12,    0.66) /* Shade */
     , (450572,  13,       1) /* ArmorModVsSlash */
     , (450572,  14,     0.8) /* ArmorModVsPierce */
     , (450572,  15,     0.6) /* ArmorModVsBludgeon */
     , (450572,  16,     0.6) /* ArmorModVsCold */
     , (450572,  17,       1) /* ArmorModVsFire */
     , (450572,  18,     0.8) /* ArmorModVsAcid */
     , (450572,  19,       1) /* ArmorModVsElectric */
     , (450572, 110,       1) /* BulkMod */
     , (450572, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450572,   1, 'Olthoi Helm') /* Name */
     , (450572,  15, 'Helm crafted from the carapace of an Olthoi. This item can be dyed.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450572,   1, 0x020000DA) /* Setup */
     , (450572,   3, 0x20000014) /* SoundTable */
     , (450572,   6, 0x0400007E) /* PaletteBase */
     , (450572,   7, 0x1000002C) /* ClothingBase */
     , (450572,   8, 0x06000FCF) /* Icon */
     , (450572,  22, 0x3400002B) /* PhysicsEffectTable */;
