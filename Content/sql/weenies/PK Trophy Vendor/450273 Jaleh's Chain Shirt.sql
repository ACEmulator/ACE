DELETE FROM `weenie` WHERE `class_Id` = 450273;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450273, 'shirtchainjalehtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450273,   1,          2) /* ItemType - Armor */
     , (450273,   3,          2) /* PaletteTemplate - Blue */
     , (450273,   4,       1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms */
     , (450273,   5,        0) /* EncumbranceVal */
     , (450273,   8,        680) /* Mass */
     , (450273,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor */
     , (450273,  16,          1) /* ItemUseable - No */
     , (450273,  19,      20) /* Value */
     , (450273,  27,         16) /* ArmorType - Chainmail */
     , (450273,  28,        0) /* ArmorLevel */
     , (450273,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450273,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450273,   5,  -0.033) /* ManaRate */
     , (450273,  12,       1) /* Shade */
     , (450273,  13,     1.1) /* ArmorModVsSlash */
     , (450273,  14,     0.8) /* ArmorModVsPierce */
     , (450273,  15,     0.9) /* ArmorModVsBludgeon */
     , (450273,  16,    0.75) /* ArmorModVsCold */
     , (450273,  17,    0.75) /* ArmorModVsFire */
     , (450273,  18,     0.4) /* ArmorModVsAcid */
     , (450273,  19,     0.4) /* ArmorModVsElectric */
     , (450273, 110,    1.33) /* BulkMod */
     , (450273, 111,       4) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450273,   1, 'Jaleh''s Chain Shirt') /* Name */
     , (450273,  15, 'This chain mail shirt has been modified with a silken lining. It seems to breath better and offer better protection from heat and cold') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450273,   1, 0x020001C3) /* Setup */
     , (450273,   3, 0x20000014) /* SoundTable */
     , (450273,   6, 0x0400007E) /* PaletteBase */
     , (450273,   7, 0x10000472) /* ClothingBase */
     , (450273,   8, 0x06000FC7) /* Icon */
     , (450273,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450273,  36, 0x0E000012) /* MutateFilter */;


