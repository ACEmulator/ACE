DELETE FROM `weenie` WHERE `class_Id` = 450788;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450788, 'bootsleathernewbiequestpk', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450788,   1,          2) /* ItemType - Armor */
     , (450788,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450788,   4,      65536) /* ClothingPriority - Feet */
     , (450788,   5,        0) /* EncumbranceVal */
     , (450788,   8,        140) /* Mass */
     , (450788,   9,        384) /* ValidLocations - LowerLegWear, FootWear */
     , (450788,  16,          1) /* ItemUseable - No */
     , (450788,  18,          1) /* UiEffects - Magical */
     , (450788,  19,          20) /* Value */
     , (450788,  27,          2) /* ArmorType - Leather */
     , (450788,  28,        0) /* ArmorLevel */
     , (450788,  44,          1) /* Damage */
     , (450788,  45,          4) /* DamageType - Bludgeon */
     , (450788,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450788,  22, True ) /* Inscribable */
     , (450788, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450788,   5,  -0.025) /* ManaRate */
     , (450788,  12,     0.3) /* Shade */
     , (450788,  13,       1) /* ArmorModVsSlash */
     , (450788,  14,       1) /* ArmorModVsPierce */
     , (450788,  15,       1) /* ArmorModVsBludgeon */
     , (450788,  16,     0.6) /* ArmorModVsCold */
     , (450788,  17,     0.6) /* ArmorModVsFire */
     , (450788,  18,     0.6) /* ArmorModVsAcid */
     , (450788,  19,     0.6) /* ArmorModVsElectric */
     , (450788,  22,    0.75) /* DamageVariance */
     , (450788, 110,       1) /* BulkMod */
     , (450788, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450788,   1, 'A Pair Of Society Leather Boots') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450788,   1, 0x020000D0) /* Setup */
     , (450788,   3, 0x20000014) /* SoundTable */
     , (450788,   6, 0x0400007E) /* PaletteBase */
     , (450788,   7, 0x10000007) /* ClothingBase */
     , (450788,   8, 0x06000FAE) /* Icon */
     , (450788,  22, 0x3400002B) /* PhysicsEffectTable */;

