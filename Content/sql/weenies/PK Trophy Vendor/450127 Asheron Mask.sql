DELETE FROM `weenie` WHERE `class_Id` = 450127;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450127, 'ace450127-asheronmasktailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450127,   1,          4) /* ItemType - Armor */
     , (450127,   4,      16384) /* ClothingPriority - Head */
     , (450127,   5,        0) /* EncumbranceVal */
     , (450127,   9,          1) /* ValidLocations - HeadWear */
     , (450127,  16,          1) /* ItemUseable - No */
     , (450127,  19,        20) /* Value */
     , (450127,  28,         0) /* ArmorLevel */
     , (450127,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450127, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450127,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450127,  13,     0.5) /* ArmorModVsSlash */
     , (450127,  14,     0.4) /* ArmorModVsPierce */
     , (450127,  15,     0.4) /* ArmorModVsBludgeon */
     , (450127,  16,     0.6) /* ArmorModVsCold */
     , (450127,  17,     0.2) /* ArmorModVsFire */
     , (450127,  18,    0.75) /* ArmorModVsAcid */
     , (450127,  19,    0.35) /* ArmorModVsElectric */
     , (450127, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450127,   1, 'Asheron Mask') /* Name */
     , (450127,  16, 'A mask, made out of the labels of thousands of stout bottles, painted to be an eerily accurate likeness of Asheron.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450127,   1, 0x0200173F) /* Setup */
     , (450127,   3, 0x20000014) /* SoundTable */
     , (450127,   7, 0x1000065E) /* ClothingBase */
     , (450127,   8, 0x060066D6) /* Icon */
     , (450127,  22, 0x3400002B) /* PhysicsEffectTable */;
