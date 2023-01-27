DELETE FROM `weenie` WHERE `class_Id` = 450519;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450519, 'helmbalancetestubertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450519,   1,          2) /* ItemType - Armor */
     , (450519,   3,         20) /* PaletteTemplate - Silver */
     , (450519,   4,      16384) /* ClothingPriority - Head */
     , (450519,   5,        0) /* EncumbranceVal */
     , (450519,   8,        125) /* Mass */
     , (450519,   9,          1) /* ValidLocations - HeadWear */
     , (450519,  16,          1) /* ItemUseable - No */
     , (450519,  19,       20) /* Value */
     , (450519,  27,         32) /* ArmorType - Metal */
     , (450519,  28,        0) /* ArmorLevel */
     , (450519,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450519,  22, True ) /* Inscribable */
     , (450519,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450519,   5,       0) /* ManaRate */
     , (450519,  12,    0.66) /* Shade */
     , (450519,  13,       1) /* ArmorModVsSlash */
     , (450519,  14,       1) /* ArmorModVsPierce */
     , (450519,  15,       1) /* ArmorModVsBludgeon */
     , (450519,  16,     0.8) /* ArmorModVsCold */
     , (450519,  17,     0.8) /* ArmorModVsFire */
     , (450519,  18,     0.8) /* ArmorModVsAcid */
     , (450519,  19,     0.8) /* ArmorModVsElectric */
     , (450519, 110,       1) /* BulkMod */
     , (450519, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450519,   1, 'Uber Balance Testing Helm') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450519,   1, 0x02000EFA) /* Setup */
     , (450519,   3, 0x20000014) /* SoundTable */
     , (450519,   6, 0x0400007E) /* PaletteBase */
     , (450519,   7, 0x10000451) /* ClothingBase */
     , (450519,   8, 0x06002A58) /* Icon */
     , (450519,  22, 0x3400002B) /* PhysicsEffectTable */;


