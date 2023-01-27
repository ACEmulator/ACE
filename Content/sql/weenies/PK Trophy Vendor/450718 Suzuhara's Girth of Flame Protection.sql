DELETE FROM `weenie` WHERE `class_Id` = 450718;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450718, 'girthflameprotectiontailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450718,   1,          2) /* ItemType - Armor */
     , (450718,   3,         14) /* PaletteTemplate - Red */
     , (450718,   4,       2304) /* ClothingPriority - OuterwearUpperLegs, OuterwearAbdomen */
     , (450718,   5,        0) /* EncumbranceVal */
     , (450718,   8,         90) /* Mass */
     , (450718,   9,       1024) /* ValidLocations - AbdomenArmor */
     , (450718,  16,          1) /* ItemUseable - No */
     , (450718,  19,       20) /* Value */
     , (450718,  27,          2) /* ArmorType - Leather */
     , (450718,  28,        0) /* ArmorLevel */
     , (450718,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450718, 150,        103) /* HookPlacement - Hook */
     , (450718, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450718,  22, True ) /* Inscribable */
     , (450718, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450718,   5,  -0.033) /* ManaRate */
     , (450718,  12,    0.66) /* Shade */
     , (450718,  13,     0.6) /* ArmorModVsSlash */
     , (450718,  14,     0.6) /* ArmorModVsPierce */
     , (450718,  15,     0.8) /* ArmorModVsBludgeon */
     , (450718,  16,     0.8) /* ArmorModVsCold */
     , (450718,  17,     1.4) /* ArmorModVsFire */
     , (450718,  18,     0.7) /* ArmorModVsAcid */
     , (450718,  19,     0.8) /* ArmorModVsElectric */
     , (450718, 110,       1) /* BulkMod */
     , (450718, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450718,   1, 'Suzuhara''s Girth of Flame Protection') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450718,   1, 0x02000210) /* Setup */
     , (450718,   3, 0x20000014) /* SoundTable */
     , (450718,   6, 0x0400007E) /* PaletteBase */
     , (450718,   7, 0x10000597) /* ClothingBase */
     , (450718,   8, 0x060012EE) /* Icon */
     , (450718,  22, 0x3400002B) /* PhysicsEffectTable */;

