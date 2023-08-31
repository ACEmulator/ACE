DELETE FROM `weenie` WHERE `class_Id` = 480055;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480055, 'scrollulgrimrecallpk', 34, '2005-02-09 10:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480055,   1,       8192) /* ItemType - Writable */
     , (480055,   5,         30) /* EncumbranceVal */
     , (480055,   8,         90) /* Mass */
     , (480055,   9,          0) /* ValidLocations - None */
     , (480055,  16,          8) /* ItemUseable - Contained */
     , (480055,  19,         100) /* Value */
     , (480055,  33,          1) /* Bonded - Bonded */
     , (480055,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480055, 150,        103) /* HookPlacement - Hook */
     , (480055, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480055,  22, True ) /* Inscribable */
     , (480055,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480055,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480055,   1, 'Ulgrim''s Recall Scroll') /* Name */
     , (480055,  15, 'A foul smelling spell scroll that has been glued back together. Part of the scroll is barely legible, but you can just about make out the spell.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480055,   1, 0x0200018A) /* Setup */
     , (480055,   8, 0x0600295C) /* Icon */
     , (480055,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480055,  28,       2941) /* Spell - Ulgrim's Recall */;
