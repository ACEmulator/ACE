DELETE FROM `weenie` WHERE `class_Id` = 450478;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450478, 'maskchitticktailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450478,   1,          4) /* ItemType - Armor */
     , (450478,   3,          4) /* PaletteTemplate - Brown */
     , (450478,   4,      16384) /* ClothingPriority - Head */
     , (450478,   5,        0) /* EncumbranceVal */
     , (450478,   8,         75) /* Mass */
     , (450478,   9,          1) /* ValidLocations - HeadWear */
     , (450478,  16,          1) /* ItemUseable - No */
     , (450478,  19,        20) /* Value */
     , (450478,  27,          2) /* ArmorType - Leather */
     , (450478,  28,         0) /* ArmorLevel */
     , (450478,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450478, 150,        101) /* HookPlacement - Resting */
     , (450478, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450478,  22, True ) /* Inscribable */
     , (450478,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450478,  12,    0.66) /* Shade */
     , (450478,  13,     0.5) /* ArmorModVsSlash */
     , (450478,  14,     0.4) /* ArmorModVsPierce */
     , (450478,  15,     0.4) /* ArmorModVsBludgeon */
     , (450478,  16,     0.6) /* ArmorModVsCold */
     , (450478,  17,     0.2) /* ArmorModVsFire */
     , (450478,  18,    0.75) /* ArmorModVsAcid */
     , (450478,  19,    0.35) /* ArmorModVsElectric */
     , (450478,  39,       1) /* DefaultScale */
     , (450478, 110,       1) /* BulkMod */
     , (450478, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450478,   1, 'Chittick Mask') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450478,   1, 0x020011E6) /* Setup */
     , (450478,   3, 0x20000014) /* SoundTable */
     , (450478,   6, 0x0400007E) /* PaletteBase */
     , (450478,   7, 0x10000583) /* ClothingBase */
     , (450478,   8, 0x060035EE) /* Icon */
     , (450478,  22, 0x3400002B) /* PhysicsEffectTable */;
