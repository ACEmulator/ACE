DELETE FROM `weenie` WHERE `class_Id` = 450732;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450732, 'ace450732-hulkingbunnyslipperstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450732,   1,          4) /* ItemType - Clothing */
     , (450732,   3,          4) /* PaletteTemplate - Brown */
     , (450732,   4,      65536) /* ClothingPriority - Feet */
     , (450732,   5,        0) /* EncumbranceVal */
     , (450732,   9,        256) /* ValidLocations - FootWear */
     , (450732,  16,          1) /* ItemUseable - No */
     , (450732,  19,          20) /* Value */
     , (450732,  27,          2) /* ArmorType - Leather */
     , (450732,  28,         0) /* ArmorLevel */
     , (450732,  44,          5) /* Damage */
     , (450732,  45,          4) /* DamageType - Bludgeon */
     , (450732,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450732, 150,        103) /* HookPlacement - Hook */
     , (450732, 151,          1) /* HookType - Floor */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450732,  22, True ) /* Inscribable */
     , (450732,  69, False) /* IsSellable */
     , (450732, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450732,   5,  -0.033) /* ManaRate */
     , (450732,  13,     0.5) /* ArmorModVsSlash */
     , (450732,  14,     0.5) /* ArmorModVsPierce */
     , (450732,  15,     0.5) /* ArmorModVsBludgeon */
     , (450732,  16,     1.3) /* ArmorModVsCold */
     , (450732,  17,     0.4) /* ArmorModVsFire */
     , (450732,  18,     0.4) /* ArmorModVsAcid */
     , (450732,  19,     0.4) /* ArmorModVsElectric */
     , (450732,  39,       2) /* DefaultScale */
     , (450732, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450732,   1, 'Hulking Bunny Slippers') /* Name */
     , (450732,  16, 'A pair of hulking bunny slippers.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450732,   1, 0x02000BBB) /* Setup */
     , (450732,   6, 0x0400007E) /* PaletteBase */
     , (450732,   7, 0x100006D2) /* ClothingBase */
     , (450732,   8, 0x0600237A) /* Icon */
     , (450732,  22, 0x3400002B) /* PhysicsEffectTable */;
