DELETE FROM `weenie` WHERE `class_Id` = 4200098;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200098, 'ace4200098-enhancedrobeofthetundratailorr', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200098,   1,          2) /* ItemType - Clothing */
     , (4200098,   3,          9) /* PaletteTemplate - Grey */
     , (4200098,   4,       1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (4200098,   5,          1) /* EncumbranceVal */
	 , (4200098,   8,        340) /* Mass */
     , (4200098,   9,        512) /* ValidLocations - HeadWear, Armor */
     , (4200098,  16,          1) /* ItemUseable - No */
     , (4200098,  19,         20) /* Value */
     , (4200098,  27,          1) /* ArmorType - Cloth */
     , (4200098,  28,          1) /* ArmorLevel */
     , (4200098,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200098, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200098,  22, True ) /* Inscribable */
     , (4200098,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200098,   5,  -0.025) /* ManaRate */
     , (4200098,  12,    0.81) /* Shade */
     , (4200098,  13,       1) /* ArmorModVsSlash */
     , (4200098,  14,       1) /* ArmorModVsPierce */
     , (4200098,  15,       1) /* ArmorModVsBludgeon */
     , (4200098,  16,       2) /* ArmorModVsCold */
     , (4200098,  17,     0.5) /* ArmorModVsFire */
     , (4200098,  18,     0.5) /* ArmorModVsAcid */
     , (4200098,  19,     0.5) /* ArmorModVsElectric */
     , (4200098, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200098,   1, 'Enhanced Robe of the Tundra') /* Name */
     , (4200098,  16, 'A robe crafted from a mattekar hide.  It has some natural padding in it that makes it stronger and more resistant to damage. This robe has been enhanced by Belinda du Loc.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200098,   1, 0x020001A6) /* Setup */
     , (4200098,   3, 0x20000014) /* SoundTable */
     , (4200098,   6, 0x0400007E) /* PaletteBase */
     , (4200098,   7, 0x10000327) /* ClothingBase */
     , (4200098,   8, 0x060022E6) /* Icon */
     , (4200098,  22, 0x3400002B) /* PhysicsEffectTable */;
