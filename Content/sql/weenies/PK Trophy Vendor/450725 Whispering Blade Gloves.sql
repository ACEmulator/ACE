DELETE FROM `weenie` WHERE `class_Id` = 450725;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450725, 'ace450725-whisperingbladeglovestailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450725,   1,          2) /* ItemType - Armor */
     , (450725,   4,      32768) /* ClothingPriority - Hands */
     , (450725,   5,        0) /* EncumbranceVal */
     , (450725,   9,         32) /* ValidLocations - HandWear */
     , (450725,  16,          1) /* ItemUseable - No */
     , (450725,  19,       20) /* Value */
     , (450725,  28,        0) /* ArmorLevel */
     , (450725,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450725,  11, True ) /* IgnoreCollisions */
     , (450725,  13, True ) /* Ethereal */
     , (450725,  14, True ) /* GravityStatus */
     , (450725,  19, True ) /* Attackable */
     , (450725,  22, True ) /* Inscribable */
     , (450725, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450725,   5,   -0.05) /* ManaRate */
     , (450725,  13,     0.9) /* ArmorModVsSlash */
     , (450725,  14,       1) /* ArmorModVsPierce */
     , (450725,  15,     0.8) /* ArmorModVsBludgeon */
     , (450725,  16,     1.1) /* ArmorModVsCold */
     , (450725,  17,       1) /* ArmorModVsFire */
     , (450725,  18,       1) /* ArmorModVsAcid */
     , (450725,  19,     1.1) /* ArmorModVsElectric */
     , (450725, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450725,   1, 'Whispering Blade Gloves') /* Name */
     , (450725,  16, 'These enchanted gloves bear the blade sigil of the enigmatic Whispering Blade.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450725,   1, 0x02001514) /* Setup */
     , (450725,   3, 0x20000014) /* SoundTable */
     , (450725,   7, 0x10000667) /* ClothingBase */
     , (450725,   8, 0x060062BA) /* Icon */
     , (450725,  22, 0x3400002B) /* PhysicsEffectTable */;

