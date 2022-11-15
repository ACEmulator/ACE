DELETE FROM `weenie` WHERE `class_Id` = 450118;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450118, 'capstocking2tailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450118,   1,          4) /* ItemType - Clothing */
     , (450118,   3,         13) /* PaletteTemplate - Purple */
     , (450118,   4,      16384) /* ClothingPriority - Head */
     , (450118,   5,         0) /* EncumbranceVal */
     , (450118,   8,         15) /* Mass */
     , (450118,   9,          1) /* ValidLocations - HeadWear */
     , (450118,  16,          1) /* ItemUseable - No */
     , (450118,  19,        20) /* Value */
     , (450118,  27,          1) /* ArmorType - Cloth */
     , (450118,  28,          0) /* ArmorLevel */
     , (450118,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450118, 150,        103) /* HookPlacement - Hook */
     , (450118, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450118,  22, True ) /* Inscribable */
     , (450118, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450118,   5,  -0.017) /* ManaRate */
     , (450118,  12,     0.8) /* Shade */
     , (450118,  13,     0.8) /* ArmorModVsSlash */
     , (450118,  14,     0.8) /* ArmorModVsPierce */
     , (450118,  15,       1) /* ArmorModVsBludgeon */
     , (450118,  16,     0.5) /* ArmorModVsCold */
     , (450118,  17,     0.2) /* ArmorModVsFire */
     , (450118,  18,     0.1) /* ArmorModVsAcid */
     , (450118,  19,     0.2) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450118,   1, 'Tall Stocking Cap') /* Name */
     , (450118,  16, 'A silly, warm stocking cap of cold protection.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450118,   1, 0x02000E84) /* Setup */
     , (450118,   3, 0x20000014) /* SoundTable */
     , (450118,   6, 0x0400007E) /* PaletteBase */
     , (450118,   7, 0x10000436) /* ClothingBase */
     , (450118,   8, 0x06002977) /* Icon */
     , (450118,  22, 0x3400002B) /* PhysicsEffectTable */;
