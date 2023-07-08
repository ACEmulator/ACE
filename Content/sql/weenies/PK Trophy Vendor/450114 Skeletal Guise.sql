DELETE FROM `weenie` WHERE `class_Id` = 450114;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450114, 'costumeskeletontailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450114,   1,          4) /* ItemType - Clothing */
     , (450114,   3,          4) /* PaletteTemplate - Brown */
     , (450114,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450114,   5,       0) /* EncumbranceVal */
     , (450114,   8,        150) /* Mass */
     , (450114,   9,      512) /* ValidLocations - Armor */
     , (450114,  16,          1) /* ItemUseable - No */
     , (450114,  19,       20) /* Value */
     , (450114,  27,          1) /* ArmorType - Cloth */
     , (450114,  28,         0) /* ArmorLevel */
     , (450114,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450114, 150,        103) /* HookPlacement - Hook */
     , (450114, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450114,  22, True ) /* Inscribable */
     , (450114,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450114,  12,       0) /* Shade */
     , (450114,  13,     0.5) /* ArmorModVsSlash */
     , (450114,  14,     0.5) /* ArmorModVsPierce */
     , (450114,  15,    0.75) /* ArmorModVsBludgeon */
     , (450114,  16,    0.65) /* ArmorModVsCold */
     , (450114,  17,    0.55) /* ArmorModVsFire */
     , (450114,  18,    0.55) /* ArmorModVsAcid */
     , (450114,  19,    0.65) /* ArmorModVsElectric */
     , (450114,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450114,   1, 'Skeletal Guise') /* Name */
     , (450114,  15, 'A skeleton costume.') /* ShortDesc */
     , (450114,  16, 'A finely crafted skeleton costume that is only missing the head.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450114,   1, 0x02000DF8) /* Setup */
     , (450114,   3, 0x20000014) /* SoundTable */
     , (450114,   6, 0x0400007E) /* PaletteBase */
     , (450114,   7, 0x100003F6) /* ClothingBase */
     , (450114,   8, 0x060028B6) /* Icon */
     , (450114,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450114,  36, 0x0E000016) /* MutateFilter */;
