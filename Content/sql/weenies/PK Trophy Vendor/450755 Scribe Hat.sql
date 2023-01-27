DELETE FROM `weenie` WHERE `class_Id` = 450755;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450755, 'hatscribetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450755,   1,          4) /* ItemType - Clothing */
     , (450755,   3,          2) /* PaletteTemplate - Blue */
     , (450755,   4,      16384) /* ClothingPriority - Head */
     , (450755,   5,         0) /* EncumbranceVal */
     , (450755,   8,         15) /* Mass */
     , (450755,   9,          1) /* ValidLocations - HeadWear */
     , (450755,  16,          1) /* ItemUseable - No */
     , (450755,  19,          20) /* Value */
     , (450755,  27,          1) /* ArmorType - Cloth */
     , (450755,  28,          0) /* ArmorLevel */
     , (450755,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450755, 150,        103) /* HookPlacement - Hook */
     , (450755, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450755,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450755,  12,    0.66) /* Shade */
     , (450755,  13,     0.8) /* ArmorModVsSlash */
     , (450755,  14,     0.8) /* ArmorModVsPierce */
     , (450755,  15,       1) /* ArmorModVsBludgeon */
     , (450755,  16,     0.2) /* ArmorModVsCold */
     , (450755,  17,     0.2) /* ArmorModVsFire */
     , (450755,  18,     0.1) /* ArmorModVsAcid */
     , (450755,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450755,   1, 'Scribe Hat') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450755,   1, 0x020006D6) /* Setup */
     , (450755,   3, 0x20000014) /* SoundTable */
     , (450755,   6, 0x0400007E) /* PaletteBase */
     , (450755,   7, 0x1000017F) /* ClothingBase */
     , (450755,   8, 0x06001357) /* Icon */
     , (450755,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450755,  36, 0x0E000016) /* MutateFilter */;
