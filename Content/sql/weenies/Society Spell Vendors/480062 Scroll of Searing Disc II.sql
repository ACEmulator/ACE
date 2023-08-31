DELETE FROM `weenie` WHERE `class_Id` = 480062;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480062, 'ace480062-scrollofsearingdisciipk', 34, '2021-11-01 00:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480062,   1,       8192) /* ItemType - Writable */
     , (480062,   5,         30) /* EncumbranceVal */
     , (480062,  16,          8) /* ItemUseable - Contained */
	 , (480062,  33,         1) /* Bonded - Slippery */
	 , (480062, 114,          1) /* Attuned - Attuned */
     , (480062,  19,        100) /* Value */
     , (480062,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480062,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480062,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480062,   1, 'Scroll of Searing Disc II') /* Name */
     , (480062,  14, 'Use this item to attempt to learn its spell.') /* Use */
     , (480062,  16, 'Inscribed spell: Searing Disc II
Shoots eight waves of acid outward from the caster. Each wave does 49-96 points of acid damage to the first thing it hits.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480062,   1, 0x0200018A) /* Setup */
     , (480062,   8, 0x060035A3) /* Icon */
     , (480062,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480062,  28,       6189) /* Spell - Searing Disc II */;
