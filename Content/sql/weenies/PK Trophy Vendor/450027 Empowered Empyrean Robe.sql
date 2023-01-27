DELETE FROM `weenie` WHERE `class_Id` = 450027;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450027, 'ace450027-empoweredempyreanrobetailor', 2, '2022-01-08 18:29:57') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450027,   1,          4) /* ItemType - Clothing */
     , (450027,   3,          2) /* PaletteTemplate - Blue */
     , (450027,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Feet */
     , (450027,   5,        0) /* EncumbranceVal */
     , (450027,   9,      512) /* ValidLocations - HeadWear, Armor */
     , (450027,  16,          1) /* ItemUseable - No */
     , (450027,  19,     20) /* Value */
     , (450027,  28,        0) /* ArmorLevel */
     , (450027,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450027,  22, True ) /* Inscribable */
     , (450027,  23, True ) /* DestroyOnSell */
     , (450027,  69, False) /* IsSellable */
     , (450027,  99, True ) /* Ivoryable */
     , (450027, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450027,   5,    -0.5) /* ManaRate */
     , (450027,  12,     0.5) /* Shade */
     , (450027,  13,     0.6) /* ArmorModVsSlash */
     , (450027,  14,     0.6) /* ArmorModVsPierce */
     , (450027,  15,     0.6) /* ArmorModVsBludgeon */
     , (450027,  16,     0.6) /* ArmorModVsCold */
     , (450027,  17,     0.6) /* ArmorModVsFire */
     , (450027,  18,     0.6) /* ArmorModVsAcid */
     , (450027,  19,     0.6) /* ArmorModVsElectric */
     , (450027, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450027,   1, 'Empowered Empyrean Robe') /* Name */
     , (450027,  16, 'A blue Empyrean robe, like the one worn by Asheron.  Embedded in the fabric are small threads of Thaumaturgic Crystal which radiate an almost palpable power.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450027,   1, 0x020001A6) /* Setup */
     , (450027,   3, 0x20000014) /* SoundTable */
     , (450027,   6, 0x0400007E) /* PaletteBase */
     , (450027,   7, 0x100006BA) /* ClothingBase */
     , (450027,   8, 0x060065D2) /* Icon */
     , (450027,  22, 0x3400002B) /* PhysicsEffectTable */;

