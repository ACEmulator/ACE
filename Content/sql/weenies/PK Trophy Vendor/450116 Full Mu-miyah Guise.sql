DELETE FROM `weenie` WHERE `class_Id` = 450116;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450116, 'costumemummyheadtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450116,   1,          4) /* ItemType - Armor */
     , (450116,   3,         46) /* PaletteTemplate - Tan */
     , (450116,   4,     1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Hands, Feet */
     , (450116,   5,       0) /* EncumbranceVal */
     , (450116,   8,         75) /* Mass */
     , (450116,   9,      512) /* ValidLocations - HeadWear, HandWear, Armor */
     , (450116,  16,          1) /* ItemUseable - No */
     , (450116,  19,         20) /* Value */
     , (450116,  27,          2) /* ArmorType - Leather */
     , (450116,  28,         0) /* ArmorLevel */
     , (450116,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450116, 150,        103) /* HookPlacement - Hook */
     , (450116, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450116,  22, True ) /* Inscribable */
     , (450116,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450116,  12,    0.66) /* Shade */
     , (450116,  13,     0.5) /* ArmorModVsSlash */
     , (450116,  14,     0.5) /* ArmorModVsPierce */
     , (450116,  15,    0.75) /* ArmorModVsBludgeon */
     , (450116,  16,    0.65) /* ArmorModVsCold */
     , (450116,  17,    0.55) /* ArmorModVsFire */
     , (450116,  18,    0.55) /* ArmorModVsAcid */
     , (450116,  19,    0.65) /* ArmorModVsElectric */
     , (450116,  39,     0.8) /* DefaultScale */
     , (450116, 110,       1) /* BulkMod */
     , (450116, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450116,   1, 'Full Mu-miyah Guise') /* Name */
     , (450116,  16, 'A finely crafted mu-miyah costume complete with head. The smell of mold and old dirt lingers despite the glues used to hold the costume together. There is a thin line of padding that has been added to the interior to protect the wearer from touching the aged bandages.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450116,   1, 0x02000E06) /* Setup */
     , (450116,   3, 0x20000014) /* SoundTable */
     , (450116,   6, 0x0400007E) /* PaletteBase */
     , (450116,   7, 0x100003FC) /* ClothingBase */
     , (450116,   8, 0x060028B3) /* Icon */
     , (450116,  22, 0x3400002B) /* PhysicsEffectTable */;
