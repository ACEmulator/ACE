DELETE FROM `weenie` WHERE `class_Id` = 450730;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450730, 'bunnyslipperstailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450730,   1,          4) /* ItemType - Clothing */
     , (450730,   3,          4) /* PaletteTemplate - Brown */
     , (450730,   4,      65536) /* ClothingPriority - Feet */
     , (450730,   5,        0) /* EncumbranceVal */
     , (450730,   8,        350) /* Mass */
     , (450730,   9,        256) /* ValidLocations - FootWear */
     , (450730,  16,          1) /* ItemUseable - No */
     , (450730,  19,          20) /* Value */
     , (450730,  27,          2) /* ArmorType - Leather */
     , (450730,  28,         0) /* ArmorLevel */
     , (450730,  44,          3) /* Damage */
     , (450730,  45,          4) /* DamageType - Bludgeon */
     , (450730,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450730, 150,        103) /* HookPlacement - Hook */
     , (450730, 151,          1) /* HookType - Floor */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450730,  22, True ) /* Inscribable */
     , (450730,  69, False) /* IsSellable */
     , (450730, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450730,   5,  -0.033) /* ManaRate */
     , (450730,  12,     0.1) /* Shade */
     , (450730,  13,     0.3) /* ArmorModVsSlash */
     , (450730,  14,     0.3) /* ArmorModVsPierce */
     , (450730,  15,     0.3) /* ArmorModVsBludgeon */
     , (450730,  16,     1.3) /* ArmorModVsCold */
     , (450730,  17,     0.3) /* ArmorModVsFire */
     , (450730,  18,     0.3) /* ArmorModVsAcid */
     , (450730,  19,     0.3) /* ArmorModVsElectric */
     , (450730,  22,    0.75) /* DamageVariance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450730,   1, 'Bunny Slippers') /* Name */
     , (450730,  15, 'A pair of bunny slippers.') /* ShortDesc */
     , (450730,  16, 'A pair of bunny slippers.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450730,   1, 0x02000BBB) /* Setup */
     , (450730,   6, 0x0400007E) /* PaletteBase */
     , (450730,   7, 0x10000353) /* ClothingBase */
     , (450730,   8, 0x0600237A) /* Icon */
     , (450730,  22, 0x3400002B) /* PhysicsEffectTable */;

