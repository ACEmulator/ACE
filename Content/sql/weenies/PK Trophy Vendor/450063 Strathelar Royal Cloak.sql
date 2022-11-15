DELETE FROM `weenie` WHERE `class_Id` = 450063;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450063, 'ace450063-strathelarroyalcloaktailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450063,   1,          4) /* ItemType - Clothing */
     , (450063,   4,     131072) /* ClothingPriority - 131072 */
     , (450063,   5,         0) /* EncumbranceVal */
     , (450063,   9,  134217728) /* ValidLocations - Cloak */
     , (450063,  16,          1) /* ItemUseable - No */
     , (450063,  19,         20) /* Value */
     , (450063,  36,       9999) /* ResistMagic */
     , (450063,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450063,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450063,   1, 'Strathelar Royal Cloak') /* Name */
     , (450063,  16, 'A fine cloak bearing the heraldry of Elysa Strathelar''s Royal House.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450063,   1, 0x02001B2A) /* Setup */
     , (450063,   3, 0x20000014) /* SoundTable */
     , (450063,   7, 0x100007EA) /* ClothingBase */
     , (450063,   8, 0x0600709B) /* Icon */
     , (450063,  22, 0x3400002B) /* PhysicsEffectTable */;
