DELETE FROM `weenie` WHERE `class_Id` = 450783;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450783, 'breastplateleathernewbiequestPK', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450783,   1,          2) /* ItemType - Armor */
     , (450783,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450783,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450783,   5,        0) /* EncumbranceVal */
     , (450783,   8,        140) /* Mass */
     , (450783,   9,        512) /* ValidLocations - ChestArmor */
     , (450783,  16,          1) /* ItemUseable - No */
     , (450783,  18,          1) /* UiEffects - Magical */
     , (450783,  19,          20) /* Value */
     , (450783,  27,          2) /* ArmorType - Leather */
     , (450783,  28,        0) /* ArmorLevel */
     , (450783,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450783,  22, True ) /* Inscribable */
     , (450783, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450783,   5,  -0.025) /* ManaRate */
     , (450783,  12,     0.3) /* Shade */
     , (450783,  13,       1) /* ArmorModVsSlash */
     , (450783,  14,       1) /* ArmorModVsPierce */
     , (450783,  15,       1) /* ArmorModVsBludgeon */
     , (450783,  16,     0.6) /* ArmorModVsCold */
     , (450783,  17,     0.6) /* ArmorModVsFire */
     , (450783,  18,     0.6) /* ArmorModVsAcid */
     , (450783,  19,     0.6) /* ArmorModVsElectric */
     , (450783, 110,       1) /* BulkMod */
     , (450783, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450783,   1, 'A Society Leather Breastplate') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450783,   1, 0x020000D2) /* Setup */
     , (450783,   3, 0x20000014) /* SoundTable */
     , (450783,   6, 0x0400007E) /* PaletteBase */
     , (450783,   7, 0x10000055) /* ClothingBase */
     , (450783,   8, 0x06000FD6) /* Icon */
     , (450783,  22, 0x3400002B) /* PhysicsEffectTable */;


