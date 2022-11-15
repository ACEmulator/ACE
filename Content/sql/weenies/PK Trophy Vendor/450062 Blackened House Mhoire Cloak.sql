DELETE FROM `weenie` WHERE `class_Id` = 450062;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450062, 'ace450062-blackenedhousemhoirecloaktailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450062,   1,          4) /* ItemType - Clothing */
     , (450062,   4,     131072) /* ClothingPriority - 131072 */
     , (450062,   5,         0) /* EncumbranceVal */
     , (450062,   9,  134217728) /* ValidLocations - Cloak */
     , (450062,  16,          1) /* ItemUseable - No */
     , (450062,  19,         20) /* Value */
     , (450062,  36,       9999) /* ResistMagic */
     , (450062,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450062,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450062,  12,       0) /* Shade */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450062,   1, 'Blackened House Mhoire Cloak') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450062,   1, 0x02001B2A) /* Setup */
     , (450062,   3, 0x20000014) /* SoundTable */
     , (450062,   7, 0x1000081E) /* ClothingBase */
     , (450062,   8, 0x0600712D) /* Icon */
     , (450062,  22, 0x3400002B) /* PhysicsEffectTable */;
