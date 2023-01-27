DELETE FROM `weenie` WHERE `class_Id` = 450270;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450270, 'hauberkbastiontailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450270,   1,          2) /* ItemType - Armor */
     , (450270,   3,         13) /* PaletteTemplate - Purple */
     , (450270,   4,      15360) /* ClothingPriority - OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms */
     , (450270,   5,       0) /* EncumbranceVal */
     , (450270,   8,       1100) /* Mass */
     , (450270,   9,       512) /* ValidLocations - ChestArmor, AbdomenArmor, UpperArmArmor, LowerArmArmor */
     , (450270,  16,          1) /* ItemUseable - No */
     , (450270,  19,       20) /* Value */
     , (450270,  27,         32) /* ArmorType - Metal */
     , (450270,  28,        0) /* ArmorLevel */
     , (450270,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450270,  22, True ) /* Inscribable */
     , (450270,  23, True ) /* DestroyOnSell */
     , (450270, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450270,  12,    0.66) /* Shade */
     , (450270,  13,     1.3) /* ArmorModVsSlash */
     , (450270,  14,       1) /* ArmorModVsPierce */
     , (450270,  15,       1) /* ArmorModVsBludgeon */
     , (450270,  16,     0.7) /* ArmorModVsCold */
     , (450270,  17,     0.7) /* ArmorModVsFire */
     , (450270,  18,     0.5) /* ArmorModVsAcid */
     , (450270,  19,     0.3) /* ArmorModVsElectric */
     , (450270, 110,       1) /* BulkMod */
     , (450270, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450270,   1, 'Bastion of Tukal') /* Name */
     , (450270,  15, 'A chestplate decorated with a large carved seal on the chest.') /* ShortDesc */
     , (450270,  16, 'A chestplate with the seal of Linvak Tukal on the chest.  The armor is elegant yet simple, and sturdily crafted.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450270,   1, 0x020000D4) /* Setup */
     , (450270,   3, 0x20000014) /* SoundTable */
     , (450270,   6, 0x0400007E) /* PaletteBase */
     , (450270,   7, 0x100002C7) /* ClothingBase */
     , (450270,   8, 0x0600200D) /* Icon */
     , (450270,  22, 0x3400002B) /* PhysicsEffectTable */;
