DELETE FROM `weenie` WHERE `class_Id` = 450117;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450117, 'capstockingtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450117,   1,          4) /* ItemType - Clothing */
     , (450117,   3,          8) /* PaletteTemplate - Green */
     , (450117,   4,      16384) /* ClothingPriority - Head */
     , (450117,   5,         0) /* EncumbranceVal */
     , (450117,   8,         15) /* Mass */
     , (450117,   9,          1) /* ValidLocations - HeadWear */
     , (450117,  16,          1) /* ItemUseable - No */
     , (450117,  19,        20) /* Value */
     , (450117,  27,          1) /* ArmorType - Cloth */
     , (450117,  28,          0) /* ArmorLevel */
     , (450117,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450117, 150,        103) /* HookPlacement - Hook */
     , (450117, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450117,  22, True ) /* Inscribable */
     , (450117, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450117,   5,  -0.017) /* ManaRate */
     , (450117,  12,     0.8) /* Shade */
     , (450117,  13,     0.8) /* ArmorModVsSlash */
     , (450117,  14,     0.8) /* ArmorModVsPierce */
     , (450117,  15,       1) /* ArmorModVsBludgeon */
     , (450117,  16,     0.5) /* ArmorModVsCold */
     , (450117,  17,     0.2) /* ArmorModVsFire */
     , (450117,  18,     0.1) /* ArmorModVsAcid */
     , (450117,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450117,   1, 'Stocking Cap') /* Name */
     , (450117,  15, 'A warm stocking cap of cold protection.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450117,   1, 0x02000E83) /* Setup */
     , (450117,   3, 0x20000014) /* SoundTable */
     , (450117,   6, 0x0400007E) /* PaletteBase */
     , (450117,   7, 0x10000435) /* ClothingBase */
     , (450117,   8, 0x06002976) /* Icon */
     , (450117,  22, 0x3400002B) /* PhysicsEffectTable */;

