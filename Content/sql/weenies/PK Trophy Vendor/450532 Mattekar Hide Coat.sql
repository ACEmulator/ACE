DELETE FROM `weenie` WHERE `class_Id` = 450532;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450532, 'coatmattekarhidetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450532,   1,          2) /* ItemType - Armor */
     , (450532,   3,          9) /* PaletteTemplate - Grey */
     , (450532,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450532,   5,        0) /* EncumbranceVal */
     , (450532,   8,        270) /* Mass */
     , (450532,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450532,  16,          1) /* ItemUseable - No */
     , (450532,  19,        20) /* Value */
     , (450532,  27,          2) /* ArmorType - Leather */
     , (450532,  28,        0) /* ArmorLevel */
     , (450532,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450532, 150,        103) /* HookPlacement - Hook */
     , (450532, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450532,  22, True ) /* Inscribable */
     , (450532, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450532,  12,    0.66) /* Shade */
     , (450532,  13,     1.2) /* ArmorModVsSlash */
     , (450532,  14,     0.9) /* ArmorModVsPierce */
     , (450532,  15,     0.9) /* ArmorModVsBludgeon */
     , (450532,  16,       2) /* ArmorModVsCold */
     , (450532,  17,     0.7) /* ArmorModVsFire */
     , (450532,  18,       1) /* ArmorModVsAcid */
     , (450532,  19,       2) /* ArmorModVsElectric */
     , (450532, 110,       1) /* BulkMod */
     , (450532, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450532,   1, 'Mattekar Hide Coat') /* Name */
     , (450532,  15, 'Coat crafted from the hide of a Mattekar. This item can be dyed.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450532,   1, 0x020000D4) /* Setup */
     , (450532,   3, 0x20000014) /* SoundTable */
     , (450532,   6, 0x0400007E) /* PaletteBase */
     , (450532,   7, 0x10000413) /* ClothingBase */
     , (450532,   8, 0x06000FF1) /* Icon */
     , (450532,  22, 0x3400002B) /* PhysicsEffectTable */;
