DELETE FROM `weenie` WHERE `class_Id` = 450787;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450787, 'leggingsleathernewbiequestpk', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450787,   1,          2) /* ItemType - Armor */
     , (450787,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450787,   4,        768) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs */
     , (450787,   5,        0) /* EncumbranceVal */
     , (450787,   8,        320) /* Mass */
     , (450787,   9,      24576) /* ValidLocations - UpperLegArmor, LowerLegArmor */
     , (450787,  16,          1) /* ItemUseable - No */
     , (450787,  18,          1) /* UiEffects - Magical */
     , (450787,  19,          20) /* Value */
     , (450787,  27,          2) /* ArmorType - Leather */
     , (450787,  28,        0) /* ArmorLevel */
     , (450787,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450787,  22, True ) /* Inscribable */
	 , (450787, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450787,   5,  -0.025) /* ManaRate */
     , (450787,  12,     0.3) /* Shade */
     , (450787,  13,       1) /* ArmorModVsSlash */
     , (450787,  14,       1) /* ArmorModVsPierce */
     , (450787,  15,       1) /* ArmorModVsBludgeon */
     , (450787,  16,     0.6) /* ArmorModVsCold */
     , (450787,  17,     0.6) /* ArmorModVsFire */
     , (450787,  18,     0.6) /* ArmorModVsAcid */
     , (450787,  19,     0.6) /* ArmorModVsElectric */
     , (450787, 110,       1) /* BulkMod */
     , (450787, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450787,   1, 'A Pair Of Society Leather Leggings') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450787,   1, 0x020001A8) /* Setup */
     , (450787,   3, 0x20000014) /* SoundTable */
     , (450787,   6, 0x0400007E) /* PaletteBase */
     , (450787,   7, 0x1000004D) /* ClothingBase */
     , (450787,   8, 0x06001838) /* Icon */
     , (450787,  22, 0x3400002B) /* PhysicsEffectTable */;

