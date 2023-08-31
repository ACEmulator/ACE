DELETE FROM `weenie` WHERE `class_Id` = 480064;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480064, 'ace480064-scrollofeyeofthestormiipk', 34, '2021-11-01 00:00:00') /* Scroll */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480064,   1,       8192) /* ItemType - Writable */
     , (480064,   5,         30) /* EncumbranceVal */
     , (480064,  16,          8) /* ItemUseable - Contained */
	 , (480064,  33,         1) /* Bonded - Slippery */
	 , (480064, 114,          1) /* Attuned - Attuned */
     , (480064,  19,        100) /* Value */
     , (480064,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480064,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480064,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480064,   1, 'Scroll of Eye of the Storm II') /* Name */
     , (480064,  14, 'Use this item to attempt to learn its spell.') /* Use */
     , (480064,  16, 'Inscribed spell: Eye of the Storm II
Shoots eight waves of flame outward from the caster. Each wave does 49-98 points of fire damage to the first thing it hits.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480064,   1, 0x0200018A) /* Setup */
     , (480064,   8, 0x06003594) /* Icon */
     , (480064,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480064,  28,       6194) /* Spell - Eye of the Storm II */;
