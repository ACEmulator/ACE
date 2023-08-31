DELETE FROM `weenie` WHERE `class_Id` = 480057;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480057, 'ace480057-scrollofcloudedsouliipk', 34, '2021-11-01 00:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480057,   1,       8192) /* ItemType - Writable */
     , (480057,   5,         30) /* EncumbranceVal */
     , (480057,  16,          8) /* ItemUseable - Contained */
	 , (480057,  33,         1) /* Bonded - Slippery */
	 , (480057, 114,          1) /* Attuned - Attuned */
     , (480057,  19,        100) /* Value */
     , (480057,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480057,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480057,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480057,   1, 'Scroll of Clouded Soul II') /* Name */
     , (480057,  14, 'Use this item to attempt to learn its spell.') /* Use */
     , (480057,  16, 'Inscribed spell: Clouded Soul II
Shoots eight waves of nether outward from the caster. Each wave does 109-172 points of nether damage to the first thing it hits.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480057,   1, 0x0200018A) /* Setup */
     , (480057,   8, 0x06006E74) /* Icon */
     , (480057,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480057,  28,       6195) /* Spell - Clouded Soul II */;
