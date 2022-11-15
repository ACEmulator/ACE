DELETE FROM `weenie` WHERE `class_Id` = 450295;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450295, 'helmenvoytailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450295,   1,          2) /* ItemType - Armor */
     , (450295,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450295,   4,      16384) /* ClothingPriority - Head */
     , (450295,   5,          1) /* EncumbranceVal */
     , (450295,   8,          5) /* Mass */
     , (450295,   9,          1) /* ValidLocations - HeadWear */
     , (450295,  16,          1) /* ItemUseable - No */
     , (450295,  19,       20) /* Value */
     , (450295,  27,         32) /* ArmorType - Metal */
     , (450295,  28,        0) /* ArmorLevel */
     , (450295,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450295, 150,        103) /* HookPlacement - Hook */
     , (450295, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450295,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450295,  12,    0.66) /* Shade */
     , (450295,  13,     1.3) /* ArmorModVsSlash */
     , (450295,  14,       1) /* ArmorModVsPierce */
     , (450295,  15,       1) /* ArmorModVsBludgeon */
     , (450295,  16,     0.4) /* ArmorModVsCold */
     , (450295,  17,     0.4) /* ArmorModVsFire */
     , (450295,  18,     0.6) /* ArmorModVsAcid */
     , (450295,  19,     0.4) /* ArmorModVsElectric */
     , (450295, 110,     0.8) /* BulkMod */
     , (450295, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450295,   1, 'Envoy''s Heaume') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450295,   1, 0x02000978) /* Setup */
     , (450295,   3, 0x20000014) /* SoundTable */
     , (450295,   6, 0x0400007E) /* PaletteBase */
     , (450295,   7, 0x10000533) /* ClothingBase */
     , (450295,   8, 0x06000FCF) /* Icon */
     , (450295,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450295,  36, 0x0E000012) /* MutateFilter */
     , (450295,  46, 0x38000032) /* TsysMutationFilter */;
