DELETE FROM `weenie` WHERE `class_Id` = 480063;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480063, 'ace480063-scrollofcassiusringoffireiipk', 34, '2021-11-01 00:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480063,   1,       8192) /* ItemType - Writable */
     , (480063,   5,         30) /* EncumbranceVal */
     , (480063,  16,          8) /* ItemUseable - Contained */
	 , (480063,  33,         1) /* Bonded - Slippery */
	 , (480063, 114,          1) /* Attuned - Attuned */
     , (480063,  19,        100) /* Value */
     , (480063,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480063,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480063,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480063,   1, 'Scroll of Cassius'' Ring of Fire II') /* Name */
     , (480063,  14, 'Use this item to attempt to learn its spell.') /* Use */
     , (480063,  16, 'Inscribed spell: Cassius'' Ring of Fire II
Shoots eight waves of flame outward from the caster. Each wave does 49-98 points of fire damage to the first thing it hits.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480063,   1, 0x0200018A) /* Setup */
     , (480063,   8, 0x0600359D) /* Icon */
     , (480063,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480063,  28,       6191) /* Spell - Cassius' Ring of Fire II */;
