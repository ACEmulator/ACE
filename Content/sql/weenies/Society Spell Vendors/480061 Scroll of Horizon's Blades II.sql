DELETE FROM `weenie` WHERE `class_Id` = 480061;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480061, 'ace480061-scrollofhorizonsbladesiipk', 34, '2021-11-01 00:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480061,   1,       8192) /* ItemType - Writable */
     , (480061,   5,         30) /* EncumbranceVal */
     , (480061,  16,          8) /* ItemUseable - Contained */
	 , (480061,  33,         1) /* Bonded - Slippery */
	 , (480061, 114,          1) /* Attuned - Attuned */
     , (480061,  19,        100) /* Value */
     , (480061,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480061,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480061,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480061,   1, 'Scroll of Horizon''s Blades II') /* Name */
     , (480061,  14, 'Use this item to attempt to learn its spell.') /* Use */
     , (480061,  16, 'Inscribed spell: Horizon''s Blades II
Shoots eight blades outward from the caster. Each blade does 49-98 points of slashing damage to the first thing it hits.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480061,   1, 0x0200018A) /* Setup */
     , (480061,   8, 0x060035A0) /* Icon */
     , (480061,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480061,  28,       6190) /* Spell - Horizon's Blades II */;
