DELETE FROM `weenie` WHERE `class_Id` = 450121;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450121, 'costumearmoredskeletontailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450121,   1,          4) /* ItemType - Clothing */
     , (450121,   3,          4) /* PaletteTemplate - Brown */
     , (450121,   4,    1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Hands, Feet */
     , (450121,   5,       0) /* EncumbranceVal */
     , (450121,   8,        150) /* Mass */
     , (450121,   9,      512) /* ValidLocations - HeadWear, HandWear, Armor */
     , (450121,  16,          1) /* ItemUseable - No */
     , (450121,  19,       20) /* Value */
     , (450121,  27,          1) /* ArmorType - Cloth */
     , (450121,  28,         0) /* ArmorLevel */
     , (450121,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450121, 150,        103) /* HookPlacement - Hook */
     , (450121, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450121,  22, True ) /* Inscribable */
     , (450121,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450121,  12,       0) /* Shade */
     , (450121,  13,     0.5) /* ArmorModVsSlash */
     , (450121,  14,     0.5) /* ArmorModVsPierce */
     , (450121,  15,    0.75) /* ArmorModVsBludgeon */
     , (450121,  16,    0.65) /* ArmorModVsCold */
     , (450121,  17,    0.55) /* ArmorModVsFire */
     , (450121,  18,    0.55) /* ArmorModVsAcid */
     , (450121,  19,    0.65) /* ArmorModVsElectric */
     , (450121,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450121,   1, 'Armored Skeleton Guise') /* Name */
     , (450121,  16, 'A finely-built armored skeleton costume.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450121,   1, 0x020011E4) /* Setup */
     , (450121,   3, 0x20000014) /* SoundTable */
     , (450121,   6, 0x0400007E) /* PaletteBase */
     , (450121,   7, 0x10000581) /* ClothingBase */
     , (450121,   8, 0x060035DE) /* Icon */
     , (450121,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450121,  36, 0x0E000016) /* MutateFilter */;
