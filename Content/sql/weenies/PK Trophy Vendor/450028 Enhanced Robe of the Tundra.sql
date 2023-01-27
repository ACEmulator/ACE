DELETE FROM `weenie` WHERE `class_Id` = 450028;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450028, 'ace450028-enhancedrobeofthetundratailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450028,   1,          4) /* ItemType - Clothing */
     , (450028,   3,          9) /* PaletteTemplate - Grey */
     , (450028,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450028,   5,        0) /* EncumbranceVal */
     , (450028,   9,      32513) /* ValidLocations - HeadWear, Armor */
     , (450028,  16,          1) /* ItemUseable - No */
     , (450028,  19,       20) /* Value */
     , (450028,  27,          1) /* ArmorType - Cloth */
     , (450028,  28,        0) /* ArmorLevel */
     , (450028,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450028, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450028,  22, True ) /* Inscribable */
     , (450028,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450028,   5,  -0.025) /* ManaRate */
     , (450028,  12,    0.81) /* Shade */
     , (450028,  13,       1) /* ArmorModVsSlash */
     , (450028,  14,       1) /* ArmorModVsPierce */
     , (450028,  15,       1) /* ArmorModVsBludgeon */
     , (450028,  16,       2) /* ArmorModVsCold */
     , (450028,  17,     0.5) /* ArmorModVsFire */
     , (450028,  18,     0.5) /* ArmorModVsAcid */
     , (450028,  19,     0.5) /* ArmorModVsElectric */
     , (450028, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450028,   1, 'Enhanced Robe of the Tundra') /* Name */
     , (450028,  16, 'A robe crafted from a mattekar hide.  It has some natural padding in it that makes it stronger and more resistant to damage. This robe has been enhanced by Belinda du Loc.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450028,   1, 0x020001A6) /* Setup */
     , (450028,   3, 0x20000014) /* SoundTable */
     , (450028,   6, 0x0400007E) /* PaletteBase */
     , (450028,   7, 0x10000327) /* ClothingBase */
     , (450028,   8, 0x060022E6) /* Icon */
     , (450028,  22, 0x3400002B) /* PhysicsEffectTable */;

