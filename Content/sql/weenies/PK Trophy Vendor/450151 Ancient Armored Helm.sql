DELETE FROM `weenie` WHERE `class_Id` = 450151;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450151, 'helmqinxikit3tailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450151,   1,          2) /* ItemType - Armor */
     , (450151,   3,          4) /* PaletteTemplate - Brown */
     , (450151,   4,      16384) /* ClothingPriority - Head */
     , (450151,   5,        0) /* EncumbranceVal */
     , (450151,   8,        350) /* Mass */
     , (450151,   9,          1) /* ValidLocations - HeadWear */
     , (450151,  16,          1) /* ItemUseable - No */
     , (450151,  19,      20) /* Value */
     , (450151,  27,         32) /* ArmorType - Metal */
     , (450151,  28,        0) /* ArmorLevel */
     , (450151,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450151,  22, True ) /* Inscribable */
     , (450151,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450151,   5,  -0.033) /* ManaRate */
     , (450151,  12,    0.66) /* Shade */
     , (450151,  13,     1.3) /* ArmorModVsSlash */
     , (450151,  14,     0.8) /* ArmorModVsPierce */
     , (450151,  15,     1.3) /* ArmorModVsBludgeon */
     , (450151,  16,       1) /* ArmorModVsCold */
     , (450151,  17,       1) /* ArmorModVsFire */
     , (450151,  18,     1.1) /* ArmorModVsAcid */
     , (450151,  19,     0.5) /* ArmorModVsElectric */
     , (450151, 110,     1.2) /* BulkMod */
     , (450151, 111,       4) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450151,   1, 'Ancient Armored Helm') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450151,   1, 0x0200122A) /* Setup */
     , (450151,   3, 0x20000014) /* SoundTable */
     , (450151,   6, 0x0400007E) /* PaletteBase */
     , (450151,   7, 0x10000593) /* ClothingBase */
     , (450151,   8, 0x0600369D) /* Icon */
     , (450151,  22, 0x3400002B) /* PhysicsEffectTable */;


