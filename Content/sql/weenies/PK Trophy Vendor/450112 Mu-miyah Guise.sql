DELETE FROM `weenie` WHERE `class_Id` = 450112;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450112, 'costumemummytailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450112,   1,          4) /* ItemType - Clothing */
     , (450112,   3,         46) /* PaletteTemplate - Tan */
     , (450112,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450112,   5,       0) /* EncumbranceVal */
     , (450112,   8,        150) /* Mass */
     , (450112,   9,      512) /* ValidLocations - Armor */
     , (450112,  16,          1) /* ItemUseable - No */
     , (450112,  19,         20) /* Value */
     , (450112,  27,          1) /* ArmorType - Cloth */
     , (450112,  28,         0) /* ArmorLevel */
     , (450112,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450112, 150,        103) /* HookPlacement - Hook */
     , (450112, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450112,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450112,  12,       0) /* Shade */
     , (450112,  13,     0.5) /* ArmorModVsSlash */
     , (450112,  14,     0.5) /* ArmorModVsPierce */
     , (450112,  15,    0.75) /* ArmorModVsBludgeon */
     , (450112,  16,    0.65) /* ArmorModVsCold */
     , (450112,  17,    0.55) /* ArmorModVsFire */
     , (450112,  18,    0.55) /* ArmorModVsAcid */
     , (450112,  19,    0.65) /* ArmorModVsElectric */
     , (450112,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450112,   1, 'Mu-miyah Guise') /* Name */
     , (450112,  15, 'A mu-miyah costume.') /* ShortDesc */
     , (450112,  16, 'A finely crafted mu-miyah costume that is only missing the head. The smell of mold and old dirt lingers despite the glues used to hold the costume together. There is a thin line of padding that has been added to the interior to protect the wearer from touching the aged bandages.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450112,   1, 0x02000E01) /* Setup */
     , (450112,   3, 0x20000014) /* SoundTable */
     , (450112,   6, 0x0400007E) /* PaletteBase */
     , (450112,   7, 0x100003F7) /* ClothingBase */
     , (450112,   8, 0x060028B4) /* Icon */
     , (450112,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450112,  36, 0x0E000016) /* MutateFilter */;
