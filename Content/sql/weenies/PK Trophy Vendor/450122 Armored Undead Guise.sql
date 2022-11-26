DELETE FROM `weenie` WHERE `class_Id` = 450122;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450122, 'costumearmoredundeadtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450122,   1,          4) /* ItemType - Clothing */
     , (450122,   3,          4) /* PaletteTemplate - Brown */
     , (450122,   4,     1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Hands, Feet */
     , (450122,   5,       0) /* EncumbranceVal */
     , (450122,   8,        150) /* Mass */
     , (450122,   9,      512) /* ValidLocations - HeadWear, HandWear, Armor */
     , (450122,  16,          1) /* ItemUseable - No */
     , (450122,  19,       20) /* Value */
     , (450122,  27,          1) /* ArmorType - Cloth */
     , (450122,  28,         0) /* ArmorLevel */
     , (450122,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450122, 150,        103) /* HookPlacement - Hook */
     , (450122, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450122,  22, True ) /* Inscribable */
     , (450122,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450122,  12,       0) /* Shade */
     , (450122,  13,     0.5) /* ArmorModVsSlash */
     , (450122,  14,     0.5) /* ArmorModVsPierce */
     , (450122,  15,    0.75) /* ArmorModVsBludgeon */
     , (450122,  16,    0.65) /* ArmorModVsCold */
     , (450122,  17,    0.55) /* ArmorModVsFire */
     , (450122,  18,    0.55) /* ArmorModVsAcid */
     , (450122,  19,    0.65) /* ArmorModVsElectric */
     , (450122,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450122,   1, 'Armored Undead Guise') /* Name */
     , (450122,  16, 'A finely-built armored undead costume.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450122,   1, 0x020011DE) /* Setup */
     , (450122,   3, 0x20000014) /* SoundTable */
     , (450122,   6, 0x0400007E) /* PaletteBase */
     , (450122,   7, 0x10000580) /* ClothingBase */
     , (450122,   8, 0x060035DD) /* Icon */
     , (450122,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450122,  36, 0x0E000016) /* MutateFilter */;
