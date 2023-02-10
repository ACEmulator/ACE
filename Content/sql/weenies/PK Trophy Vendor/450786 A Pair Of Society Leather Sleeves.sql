DELETE FROM `weenie` WHERE `class_Id` = 450786;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450786, 'sleevesleathernewbiequestPK', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450786,   1,          2) /* ItemType - Armor */
     , (450786,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450786,   4,      12288) /* ClothingPriority - OuterwearUpperArms, OuterwearLowerArms */
     , (450786,   5,        0) /* EncumbranceVal */
     , (450786,   8,        180) /* Mass */
     , (450786,   9,       6144) /* ValidLocations - UpperArmArmor, LowerArmArmor */
     , (450786,  16,          1) /* ItemUseable - No */
     , (450786,  18,          1) /* UiEffects - Magical */
     , (450786,  19,          20) /* Value */
     , (450786,  27,          2) /* ArmorType - Leather */
     , (450786,  28,        0) /* ArmorLevel */
     , (450786,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450786,  22, True ) /* Inscribable */
     , (450786, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450786,   5,  -0.025) /* ManaRate */
     , (450786,  12,     0.3) /* Shade */
     , (450786,  13,       1) /* ArmorModVsSlash */
     , (450786,  14,       1) /* ArmorModVsPierce */
     , (450786,  15,       1) /* ArmorModVsBludgeon */
     , (450786,  16,     0.6) /* ArmorModVsCold */
     , (450786,  17,     0.6) /* ArmorModVsFire */
     , (450786,  18,     0.6) /* ArmorModVsAcid */
     , (450786,  19,     0.6) /* ArmorModVsElectric */
     , (450786, 110,       1) /* BulkMod */
     , (450786, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450786,   1, 'A Pair Of Society Leather Sleeves') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450786,   1, 0x020000DF) /* Setup */
     , (450786,   3, 0x20000014) /* SoundTable */
     , (450786,   6, 0x0400007E) /* PaletteBase */
     , (450786,   7, 0x1000002E) /* ClothingBase */
     , (450786,   8, 0x060013FC) /* Icon */
     , (450786,  22, 0x3400002B) /* PhysicsEffectTable */;

