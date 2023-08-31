DELETE FROM `weenie` WHERE `class_Id` = 480054;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480054, 'ace480054-scrollofspectralflamepk', 34, '2021-11-01 00:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480054,   1,       8192) /* ItemType - Writable */
     , (480054,   5,         30) /* EncumbranceVal */
     , (480054,  16,          8) /* ItemUseable - Contained */
     , (480054,  19,       250) /* Value */
     , (480054,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480054,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480054,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480054,   1, 'Scroll of Spectral Flame') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480054,   1, 0x0200018A) /* Setup */
     , (480054,   8, 0x0600670F) /* Icon */
     , (480054,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480054,  28,       4208) /* Spell - Spectral Flame */
     , (480054,  52, 0x06001FBB) /* IconUnderlay */;
