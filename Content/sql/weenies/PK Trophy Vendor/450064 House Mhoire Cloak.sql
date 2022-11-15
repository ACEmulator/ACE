DELETE FROM `weenie` WHERE `class_Id` = 450064;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450064, 'ace450064-housemhoirecloaktailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450064,   1,          4) /* ItemType - Clothing */
     , (450064,   4,     131072) /* ClothingPriority - 131072 */
     , (450064,   5,         0) /* EncumbranceVal */
     , (450064,   9,  134217728) /* ValidLocations - Cloak */
     , (450064,  16,          1) /* ItemUseable - No */
     , (450064,  19,         20) /* Value */
     , (450064,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450064,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450064,   1, 'House Mhoire Cloak') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450064,   1, 0x02001B2A) /* Setup */
     , (450064,   3, 0x20000014) /* SoundTable */
     , (450064,   7, 0x100007E8) /* ClothingBase */
     , (450064,   8, 0x06007099) /* Icon */
     , (450064,  22, 0x3400002B) /* PhysicsEffectTable */;
