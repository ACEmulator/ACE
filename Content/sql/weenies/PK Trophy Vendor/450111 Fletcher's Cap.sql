DELETE FROM `weenie` WHERE `class_Id` = 450111;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450111, 'hatfletchertailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450111,   1,          4) /* ItemType - Clothing */
     , (450111,   3,          2) /* PaletteTemplate - Blue */
     , (450111,   4,      16384) /* ClothingPriority - Head */
     , (450111,   5,         0) /* EncumbranceVal */
     , (450111,   8,         15) /* Mass */
     , (450111,   9,          1) /* ValidLocations - HeadWear */
     , (450111,  16,          1) /* ItemUseable - No */
     , (450111,  19,          20) /* Value */
     , (450111,  27,          1) /* ArmorType - Cloth */
     , (450111,  28,          0) /* ArmorLevel */
     , (450111,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450111, 150,        103) /* HookPlacement - Hook */
     , (450111, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450111,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450111,  12,    0.66) /* Shade */
     , (450111,  13,     0.8) /* ArmorModVsSlash */
     , (450111,  14,     0.8) /* ArmorModVsPierce */
     , (450111,  15,       1) /* ArmorModVsBludgeon */
     , (450111,  16,     0.2) /* ArmorModVsCold */
     , (450111,  17,     0.2) /* ArmorModVsFire */
     , (450111,  18,     0.1) /* ArmorModVsAcid */
     , (450111,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450111,   1, 'Fletcher''s Cap') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450111,   1, 0x02000A2B) /* Setup */
     , (450111,   3, 0x20000014) /* SoundTable */
     , (450111,   6, 0x0400007E) /* PaletteBase */
     , (450111,   7, 0x100002D5) /* ClothingBase */
     , (450111,   8, 0x0600208C) /* Icon */
     , (450111,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450111,  36, 0x0E000016) /* MutateFilter */;
