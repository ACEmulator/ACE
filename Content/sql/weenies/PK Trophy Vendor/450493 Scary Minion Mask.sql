DELETE FROM `weenie` WHERE `class_Id` = 450493;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450493, 'maskscareminiontailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450493,   1,          4) /* ItemType - Armor */
     , (450493,   3,          4) /* PaletteTemplate - Brown */
     , (450493,   4,      16384) /* ClothingPriority - Head */
     , (450493,   5,         0) /* EncumbranceVal */
     , (450493,   8,         75) /* Mass */
     , (450493,   9,          1) /* ValidLocations - HeadWear */
     , (450493,  16,          1) /* ItemUseable - No */
     , (450493,  19,       20) /* Value */
     , (450493,  27,          2) /* ArmorType - Leather */
     , (450493,  28,         0) /* ArmorLevel */
     , (450493,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450493, 150,        103) /* HookPlacement - Hook */
     , (450493, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450493,  22, True ) /* Inscribable */
     , (450493,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450493,  12,    0.66) /* Shade */
     , (450493,  13,    0.45) /* ArmorModVsSlash */
     , (450493,  14,     0.5) /* ArmorModVsPierce */
     , (450493,  15,       1) /* ArmorModVsBludgeon */
     , (450493,  16,    0.45) /* ArmorModVsCold */
     , (450493,  17,    0.35) /* ArmorModVsFire */
     , (450493,  18,     0.5) /* ArmorModVsAcid */
     , (450493,  19,     0.3) /* ArmorModVsElectric */
     , (450493, 110,       1) /* BulkMod */
     , (450493, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450493,   1, 'Scary Minion Mask') /* Name */
     , (450493,  16, 'A cross between a Scarecrow Mask and a Hollow Minion''s visage.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450493,   1, 0x02000957) /* Setup */
     , (450493,   3, 0x20000014) /* SoundTable */
     , (450493,   6, 0x0400007E) /* PaletteBase */
     , (450493,   7, 0x100004CD) /* ClothingBase */
     , (450493,   8, 0x06002D84) /* Icon */
     , (450493,  22, 0x3400002B) /* PhysicsEffectTable */;
