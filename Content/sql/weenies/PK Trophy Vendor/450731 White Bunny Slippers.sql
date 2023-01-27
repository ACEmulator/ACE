DELETE FROM `weenie` WHERE `class_Id` = 450731;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450731, 'slippersbunnywhitetailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450731,   1,          4) /* ItemType - Clothing */
     , (450731,   3,         61) /* PaletteTemplate - White */
     , (450731,   4,      65536) /* ClothingPriority - Feet */
     , (450731,   5,        0) /* EncumbranceVal */
     , (450731,   8,        350) /* Mass */
     , (450731,   9,        256) /* ValidLocations - FootWear */
     , (450731,  16,          1) /* ItemUseable - No */
     , (450731,  19,          20) /* Value */
     , (450731,  27,          2) /* ArmorType - Leather */
     , (450731,  28,         0) /* ArmorLevel */
     , (450731,  44,         10) /* Damage */
     , (450731,  45,          4) /* DamageType - Bludgeon */
     , (450731,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450731, 150,        103) /* HookPlacement - Hook */
     , (450731, 151,          1) /* HookType - Floor */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450731,  22, True ) /* Inscribable */
     , (450731,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450731,   5,  -0.056) /* ManaRate */
     , (450731,  12,     0.1) /* Shade */
     , (450731,  13,     0.4) /* ArmorModVsSlash */
     , (450731,  14,     0.4) /* ArmorModVsPierce */
     , (450731,  15,     0.4) /* ArmorModVsBludgeon */
     , (450731,  16,     1.3) /* ArmorModVsCold */
     , (450731,  17,     0.4) /* ArmorModVsFire */
     , (450731,  18,     0.4) /* ArmorModVsAcid */
     , (450731,  19,     0.4) /* ArmorModVsElectric */
     , (450731,  22,    0.75) /* DamageVariance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450731,   1, 'White Bunny Slippers') /* Name */
     , (450731,  15, 'A pair of white bunny slippers. This item can be used on an item hook.') /* ShortDesc */
     , (450731,  16, 'A pair of white bunny slippers. This item can be used on an item hook.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450731,   1, 0x02000BBB) /* Setup */
     , (450731,   6, 0x0400007E) /* PaletteBase */
     , (450731,   7, 0x10000353) /* ClothingBase */
     , (450731,   8, 0x06002389) /* Icon */
     , (450731,  22, 0x3400002B) /* PhysicsEffectTable */;

