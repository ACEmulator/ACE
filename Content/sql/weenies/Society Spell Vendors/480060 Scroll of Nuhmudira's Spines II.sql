DELETE FROM `weenie` WHERE `class_Id` = 480060;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480060, 'ace480060-scrollofnuhmudirasspinesiipk', 34, '2021-11-01 00:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480060,   1,       8192) /* ItemType - Writable */
     , (480060,   5,         30) /* EncumbranceVal */
     , (480060,  16,          8) /* ItemUseable - Contained */
	 , (480060,  33,         1) /* Bonded - Slippery */
	 , (480060, 114,          1) /* Attuned - Attuned */
     , (480060,  19,        100) /* Value */
     , (480060,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480060,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480060,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480060,   1, 'Scroll of Nuhmudira''s Spines II') /* Name */
     , (480060,  14, 'Use this item to attempt to learn its spell.') /* Use */
     , (480060,  16, 'Inscribed spell: Nuhmudira''s Spines II
Shoots eight waves of force outward from the caster. Each wave does 49-98 points of piercing damage to the first thing it hits.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480060,   1, 0x0200018A) /* Setup */
     , (480060,   8, 0x0600359A) /* Icon */
     , (480060,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480060,  28,       6192) /* Spell - Nuhmudira's Spines II */;
