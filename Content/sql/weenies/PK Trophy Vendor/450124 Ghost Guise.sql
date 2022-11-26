DELETE FROM `weenie` WHERE `class_Id` = 450124;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450124, 'ace450124-ghostguisetailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450124,   1,          4) /* ItemType - Clothing */
     , (450124,   3,          4) /* PaletteTemplate - Brown */
     , (450124,   4,     1024) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearChest, OuterwearAbdomen, OuterwearUpperArms, OuterwearLowerArms, Head, Hands, Feet */
     , (450124,   5,       0) /* EncumbranceVal */
     , (450124,   9,      512) /* ValidLocations - HeadWear, HandWear, Armor */
     , (450124,  16,          1) /* ItemUseable - No */
     , (450124,  19,       20) /* Value */
     , (450124,  28,         0) /* ArmorLevel */
     , (450124,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450124, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450124,  11, True ) /* IgnoreCollisions */
     , (450124,  13, True ) /* Ethereal */
     , (450124,  14, True ) /* GravityStatus */
     , (450124,  19, True ) /* Attackable */
     , (450124,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450124,  13,     0.5) /* ArmorModVsSlash */
     , (450124,  14,     0.5) /* ArmorModVsPierce */
     , (450124,  15,    0.75) /* ArmorModVsBludgeon */
     , (450124,  16,    0.65) /* ArmorModVsCold */
     , (450124,  17,    0.55) /* ArmorModVsFire */
     , (450124,  18,    0.55) /* ArmorModVsAcid */
     , (450124,  19,    0.65) /* ArmorModVsElectric */
     , (450124, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450124,   1, 'Ghost Guise') /* Name */
     , (450124,  16, 'A gauzy robe that resembles a ghost.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450124,   1, 0x02001628) /* Setup */
     , (450124,   3, 0x20000014) /* SoundTable */
     , (450124,   7, 0x100006A1) /* ClothingBase */
     , (450124,   8, 0x06005F58) /* Icon */
     , (450124,  22, 0x3400002B) /* PhysicsEffectTable */;
