DELETE FROM `weenie` WHERE `class_Id` = 450154;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450154, 'bootshizkri3tailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450154,   1,          2) /* ItemType - Armor */
     , (450154,   3,         14) /* PaletteTemplate - Red */
     , (450154,   4,      65536) /* ClothingPriority - Feet */
     , (450154,   5,        0) /* EncumbranceVal */
     , (450154,   8,        360) /* Mass */
     , (450154,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450154,  16,          1) /* ItemUseable - No */
     , (450154,  18,          1) /* UiEffects - Magical */
     , (450154,  19,      20) /* Value */
     , (450154,  27,          4) /* ArmorType - StuddedLeather */
     , (450154,  28,        0) /* ArmorLevel */
     , (450154,  44,         18) /* Damage */
     , (450154,  45,          4) /* DamageType - Bludgeon */
     , (450154,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450154, 150,        103) /* HookPlacement - Hook */
     , (450154, 151,          9) /* HookType - Floor, Yard */
;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450154,  22, True ) /* Inscribable */
     , (450154,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450154,   5,  -0.033) /* ManaRate */
     , (450154,  12,    0.66) /* Shade */
     , (450154,  13,     1.3) /* ArmorModVsSlash */
     , (450154,  14,     0.8) /* ArmorModVsPierce */
     , (450154,  15,     1.3) /* ArmorModVsBludgeon */
     , (450154,  16,       1) /* ArmorModVsCold */
     , (450154,  17,       1) /* ArmorModVsFire */
     , (450154,  18,     1.1) /* ArmorModVsAcid */
     , (450154,  19,     0.5) /* ArmorModVsElectric */
     , (450154,  22,    0.75) /* DamageVariance */
     , (450154, 110,       1) /* BulkMod */
     , (450154, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450154,   1, 'Ancient Armored Long Boots') /* Name */
     , (450154,  16, 'These armored boots appear to have been an ornamental piece. Obviously this is only one part of a complete suit of armor.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450154,   1, 0x020008CB) /* Setup */
     , (450154,   3, 0x20000014) /* SoundTable */
     , (450154,   6, 0x0400007E) /* PaletteBase */
     , (450154,   7, 0x1000055D) /* ClothingBase */
     , (450154,   8, 0x060033C7) /* Icon */
     , (450154,  22, 0x3400002B) /* PhysicsEffectTable */;

