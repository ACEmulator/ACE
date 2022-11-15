DELETE FROM `weenie` WHERE `class_Id` = 450517;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450517, 'helmolthoitailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450517,   1,          2) /* ItemType - Armor */
     , (450517,   3,         13) /* PaletteTemplate - Purple */
     , (450517,   4,      16384) /* ClothingPriority - Head */
     , (450517,   5,        0) /* EncumbranceVal */
     , (450517,   8,        150) /* Mass */
     , (450517,   9,          1) /* ValidLocations - HeadWear */
     , (450517,  16,          1) /* ItemUseable - No */
     , (450517,  19,       20) /* Value */
     , (450517,  27,         32) /* ArmorType - Metal */
     , (450517,  28,        0) /* ArmorLevel */
     , (450517,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450517, 150,        103) /* HookPlacement - Hook */
     , (450517, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450517,  22, True ) /* Inscribable */
     , (450517, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450517,  12,    0.66) /* Shade */
     , (450517,  13,       1) /* ArmorModVsSlash */
     , (450517,  14,     0.8) /* ArmorModVsPierce */
     , (450517,  15,     0.6) /* ArmorModVsBludgeon */
     , (450517,  16,     0.6) /* ArmorModVsCold */
     , (450517,  17,       1) /* ArmorModVsFire */
     , (450517,  18,     0.8) /* ArmorModVsAcid */
     , (450517,  19,       1) /* ArmorModVsElectric */
     , (450517, 110,       1) /* BulkMod */
     , (450517, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450517,   1, 'Olthoi Helm') /* Name */
     , (450517,  15, 'Helm crafted from the carapace of an Olthoi. This item can be dyed.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450517,   1, 0x020000DA) /* Setup */
     , (450517,   3, 0x20000014) /* SoundTable */
     , (450517,   6, 0x0400007E) /* PaletteBase */
     , (450517,   7, 0x1000002C) /* ClothingBase */
     , (450517,   8, 0x06000FCF) /* Icon */
     , (450517,  22, 0x3400002B) /* PhysicsEffectTable */;
