DELETE FROM `weenie` WHERE `class_Id` = 480065;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480065, 'ace480065-scrollofhalooffrostiipk', 34, '2021-11-01 00:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480065,   1,       8192) /* ItemType - Writable */
     , (480065,   5,         30) /* EncumbranceVal */
     , (480065,  16,          8) /* ItemUseable - Contained */
	 , (480065,  33,         1) /* Bonded - Slippery */
	 , (480065, 114,          1) /* Attuned - Attuned */
     , (480065,  19,        100) /* Value */
     , (480065,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480065,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480065,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480065,   1, 'Scroll of Halo of Frost II') /* Name */
     , (480065,  14, 'Use this item to attempt to learn its spell.') /* Use */
     , (480065,  16, 'Inscribed spell: Halo of Frost II
Shoots eight waves of frost outward from the caster. Each wave does 49-98 points of cold damage to the first thing it hits.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480065,   1, 0x0200018A) /* Setup */
     , (480065,   8, 0x06003597) /* Icon */
     , (480065,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480065,  28,       6193) /* Spell - Halo of Frost II */;
