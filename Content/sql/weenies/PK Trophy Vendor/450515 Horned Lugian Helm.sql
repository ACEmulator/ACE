DELETE FROM `weenie` WHERE `class_Id` = 450515;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450515, 'helmrenegadegeneraltailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450515,   1,          2) /* ItemType - Armor */
     , (450515,   3,         14) /* PaletteTemplate - Red */
     , (450515,   4,      16384) /* ClothingPriority - Head */
     , (450515,   5,        0) /* EncumbranceVal */
     , (450515,   8,        125) /* Mass */
     , (450515,   9,          1) /* ValidLocations - HeadWear */
     , (450515,  16,          1) /* ItemUseable - No */
     , (450515,  18,          1) /* UiEffects - Magical */
     , (450515,  19,       20) /* Value */
     , (450515,  27,         32) /* ArmorType - Metal */
     , (450515,  28,        0) /* ArmorLevel */
     , (450515,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450515,  22, True ) /* Inscribable */
     , (450515,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450515,   5,   -0.05) /* ManaRate */
     , (450515,  12,    0.66) /* Shade */
     , (450515,  13,     0.8) /* ArmorModVsSlash */
     , (450515,  14,     0.8) /* ArmorModVsPierce */
     , (450515,  15,       1) /* ArmorModVsBludgeon */
     , (450515,  16,     0.4) /* ArmorModVsCold */
     , (450515,  17,     0.4) /* ArmorModVsFire */
     , (450515,  18,     0.6) /* ArmorModVsAcid */
     , (450515,  19,     0.2) /* ArmorModVsElectric */
     , (450515, 110,       1) /* BulkMod */
     , (450515, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450515,   1, 'Horned Lugian Helm') /* Name */
     , (450515,  15, 'A horned helm taken from the Renegade Lugian, General Fostok.') /* ShortDesc */
     , (450515,  33, 'RenegadeHelmGeneral') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450515,   1, 0x020010D6) /* Setup */
     , (450515,   3, 0x20000014) /* SoundTable */
     , (450515,   6, 0x0400007E) /* PaletteBase */
     , (450515,   7, 0x10000558) /* ClothingBase */
     , (450515,   8, 0x06003388) /* Icon */
     , (450515,  22, 0x3400002B) /* PhysicsEffectTable */;


