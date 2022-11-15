DELETE FROM `weenie` WHERE `class_Id` = 450065;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450065, 'ace450065-creepingblightcloaktailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450065,   1,          4) /* ItemType - Clothing */
     , (450065,   4,     131072) /* ClothingPriority - 131072 */
     , (450065,   5,         0) /* EncumbranceVal */
     , (450065,   9,  134217728) /* ValidLocations - Cloak */
     , (450065,  16,          1) /* ItemUseable - No */
     , (450065,  19,         20) /* Value */
     , (450065,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450065,  19, True ) /* Attackable */
     , (450065,  22, True ) /* Inscribable */
     , (450065, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450065,  13,     0.8) /* ArmorModVsSlash */
     , (450065,  14,     0.8) /* ArmorModVsPierce */
     , (450065,  15,       1) /* ArmorModVsBludgeon */
     , (450065,  16,     0.2) /* ArmorModVsCold */
     , (450065,  17,     0.2) /* ArmorModVsFire */
     , (450065,  18,     0.1) /* ArmorModVsAcid */
     , (450065,  19,     0.2) /* ArmorModVsElectric */
     , (450065, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450065,   1, 'Creeping Blight Cloak') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450065,   1, 0x02001B2A) /* Setup */
     , (450065,   3, 0x20000014) /* SoundTable */
     , (450065,   7, 0x100007F6) /* ClothingBase */
     , (450065,   8, 0x060070A6) /* Icon */
     , (450065,  22, 0x3400002B) /* PhysicsEffectTable */;
