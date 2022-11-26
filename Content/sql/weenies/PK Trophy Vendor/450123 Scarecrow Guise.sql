DELETE FROM `weenie` WHERE `class_Id` = 450123;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450123, 'costumescarecrowtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450123,   1,          4) /* ItemType - Clothing */
     , (450123,   3,          4) /* PaletteTemplate - Brown */
     , (450123,   4,     1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Hands, Feet */
     , (450123,   5,       0) /* EncumbranceVal */
     , (450123,   8,        150) /* Mass */
     , (450123,   9,      512) /* ValidLocations - HeadWear, HandWear, Armor */
     , (450123,  16,          1) /* ItemUseable - No */
     , (450123,  19,       20) /* Value */
     , (450123,  27,          1) /* ArmorType - Cloth */
     , (450123,  28,         0) /* ArmorLevel */
     , (450123,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450123, 150,        103) /* HookPlacement - Hook */
     , (450123, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450123,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450123,  12,       0) /* Shade */
     , (450123,  13,    0.75) /* ArmorModVsSlash */
     , (450123,  14,    0.75) /* ArmorModVsPierce */
     , (450123,  15,     0.5) /* ArmorModVsBludgeon */
     , (450123,  16,     0.5) /* ArmorModVsCold */
     , (450123,  17,     0.3) /* ArmorModVsFire */
     , (450123,  18,     0.3) /* ArmorModVsAcid */
     , (450123,  19,     0.5) /* ArmorModVsElectric */
     , (450123,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450123,   1, 'Scarecrow Guise') /* Name */
     , (450123,  16, 'A finely-built scarecrow costume. The pumpkin head feels a bit breezy, as thought it might not offer any real protection.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450123,   1, 0x020011D9) /* Setup */
     , (450123,   3, 0x20000014) /* SoundTable */
     , (450123,   6, 0x0400007E) /* PaletteBase */
     , (450123,   7, 0x10000582) /* ClothingBase */
     , (450123,   8, 0x060035DC) /* Icon */
     , (450123,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450123,  36, 0x0E000016) /* MutateFilter */;
