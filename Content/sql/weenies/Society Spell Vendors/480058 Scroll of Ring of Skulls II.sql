DELETE FROM `weenie` WHERE `class_Id` = 480058;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480058, 'ace480058-scrollofringofskullsiipk', 34, '2021-11-01 00:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480058,   1,       8192) /* ItemType - Writable */
     , (480058,   5,         30) /* EncumbranceVal */
     , (480058,  16,          8) /* ItemUseable - Contained */
	 , (480058,  33,         1) /* Bonded - Slippery */
	 , (480058, 114,          1) /* Attuned - Attuned */
     , (480058,  19,        100) /* Value */
     , (480058,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480058,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480058,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480058,   1, 'Scroll of Ring of Skulls II') /* Name */
     , (480058,  14, 'Use this item to attempt to learn its spell.') /* Use */
     , (480058,  16, 'Inscribed spell: Ring of Skulls II
Shoots eight nether skulls outward from the caster. Each skull does 109-172 points of nether damage to the first thing it hits.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480058,   1, 0x0200018A) /* Setup */
     , (480058,   8, 0x06006E74) /* Icon */
     , (480058,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480058,  28,       6320) /* Spell - Ring of Skulls II */;
