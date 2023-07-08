DELETE FROM `weenie` WHERE `class_Id` = 450045;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450045, 'robemattekarbossnewtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450045,   1,          4) /* ItemType - Clothing */
     , (450045,   3,         46) /* PaletteTemplate - Tan */
     , (450045,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450045,   5,        0) /* EncumbranceVal */
     , (450045,   8,        150) /* Mass */
     , (450045,   9,      512) /* ValidLocations - HeadWear, Armor */
     , (450045,  16,          1) /* ItemUseable - No */
     , (450045,  19,       20) /* Value */
     , (450045,  27,          1) /* ArmorType - Cloth */
     , (450045,  28,        0) /* ArmorLevel */
     , (450045,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450045, 150,        103) /* HookPlacement - Hook */
     , (450045, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450045,  22, True ) /* Inscribable */
     , (450045,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450045,   5,  -0.025) /* ManaRate */
     , (450045,  12,    0.81) /* Shade */
     , (450045,  13,       1) /* ArmorModVsSlash */
     , (450045,  14,       1) /* ArmorModVsPierce */
     , (450045,  15,       1) /* ArmorModVsBludgeon */
     , (450045,  16,     0.5) /* ArmorModVsCold */
     , (450045,  17,     0.5) /* ArmorModVsFire */
     , (450045,  18,     0.5) /* ArmorModVsAcid */
     , (450045,  19,     0.5) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450045,   1, 'Robe of the Tundra') /* Name */
     , (450045,  16, 'A robe crafted from a mattekar hide.  It has some natural padding in it that makes it stronger and more resistant to damage.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450045,   1, 0x020001A6) /* Setup */
     , (450045,   3, 0x20000014) /* SoundTable */
     , (450045,   6, 0x0400007E) /* PaletteBase */
     , (450045,   7, 0x10000327) /* ClothingBase */
     , (450045,   8, 0x06002A34) /* Icon */
     , (450045,  22, 0x3400002B) /* PhysicsEffectTable */;


