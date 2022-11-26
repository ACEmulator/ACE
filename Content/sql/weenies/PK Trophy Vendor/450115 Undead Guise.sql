DELETE FROM `weenie` WHERE `class_Id` = 450115;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450115, 'costumeundeadtailor', 2, '2005-02-09 10:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450115,   1,          4) /* ItemType - Clothing */
     , (450115,   3,          4) /* PaletteTemplate - Brown */
     , (450115,   4,      1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Feet */
     , (450115,   5,       0) /* EncumbranceVal */
     , (450115,   8,        150) /* Mass */
     , (450115,   9,      512) /* ValidLocations - Armor */
     , (450115,  16,          1) /* ItemUseable - No */
     , (450115,  19,       20) /* Value */
     , (450115,  27,          1) /* ArmorType - Cloth */
     , (450115,  28,          0) /* ArmorLevel */
     , (450115,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450115, 150,        103) /* HookPlacement - Hook */
     , (450115, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450115,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450115,  12,       0) /* Shade */
     , (450115,  13,     0.5) /* ArmorModVsSlash */
     , (450115,  14,     0.5) /* ArmorModVsPierce */
     , (450115,  15,    0.75) /* ArmorModVsBludgeon */
     , (450115,  16,    0.55) /* ArmorModVsCold */
     , (450115,  17,     0.3) /* ArmorModVsFire */
     , (450115,  18,     0.3) /* ArmorModVsAcid */
     , (450115,  19,    0.55) /* ArmorModVsElectric */
     , (450115,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450115,   1, 'Undead Guise') /* Name */
     , (450115,  15, 'An undead costume.') /* ShortDesc */
     , (450115,  16, 'A finely crafted undead costume that is only missing the head. Thankfully the smell of the previous owner is masked by the scent of the various glues used in its crafting.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450115,   1, 0x02000DFD) /* Setup */
     , (450115,   3, 0x20000014) /* SoundTable */
     , (450115,   6, 0x0400007E) /* PaletteBase */
     , (450115,   7, 0x100003F9) /* ClothingBase */
     , (450115,   8, 0x060028B7) /* Icon */
     , (450115,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450115,  36, 0x0E000016) /* MutateFilter */;
