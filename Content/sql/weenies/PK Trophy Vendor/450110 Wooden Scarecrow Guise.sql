DELETE FROM `weenie` WHERE `class_Id` = 450110;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450110, 'ace450110-woodenscarecrowguisetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450110,   1,          4) /* ItemType - Clothing */
     , (450110,   4,     1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Hands, Feet */
     , (450110,   5,       0) /* EncumbranceVal */
     , (450110,   9,      512) /* ValidLocations - HeadWear, HandWear, Armor */
     , (450110,  16,          1) /* ItemUseable - No */
     , (450110,  19,       20) /* Value */
     , (450110,  28,         0) /* ArmorLevel */
     , (450110,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450110, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450110,  11, True ) /* IgnoreCollisions */
     , (450110,  13, True ) /* Ethereal */
     , (450110,  14, True ) /* GravityStatus */
     , (450110,  19, True ) /* Attackable */
     , (450110,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450110,  13,    0.75) /* ArmorModVsSlash */
     , (450110,  14,    0.75) /* ArmorModVsPierce */
     , (450110,  15,     0.5) /* ArmorModVsBludgeon */
     , (450110,  16,     0.5) /* ArmorModVsCold */
     , (450110,  17,     0.3) /* ArmorModVsFire */
     , (450110,  18,     0.3) /* ArmorModVsAcid */
     , (450110,  19,     0.5) /* ArmorModVsElectric */
     , (450110, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450110,   1, 'Wooden Scarecrow Guise') /* Name */
     , (450110,  16, 'A finely-built scarecrow costume, using the latest in wood golem enchantments. The pumpkin head feels a bit restrictive, and you have to look out of two very tiny eye holes.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450110,   1, 0x02001A24) /* Setup */
     , (450110,   3, 0x20000014) /* SoundTable */
     , (450110,   6, 0x0400007E) /* PaletteBase */
     , (450110,   7, 0x100007BC) /* ClothingBase */
     , (450110,   8, 0x060035DC) /* Icon */
     , (450110,  22, 0x3400002B) /* PhysicsEffectTable */;
